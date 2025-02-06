import { z } from 'zod';
import { ID, idSchema } from './ID';
import { Block } from './Block';
import { IDSet } from './IDSet';

export const yataSchema = z
	.object({
		id: idSchema,
		left: idSchema.nullable(),
		right: idSchema.nullable(),
		val: z.string().nullable()
	})
	.array();
export type YataSchema = z.infer<typeof yataSchema>;

export class Yata {
	constructor(public value: Block[]) {}

	public insert = (replicaId: number, index: number, value: string, sequence: number): void => {
		// get actual index in Yata (ignore deleted)
		const i = this.findIndex(index);
		if (i < 0) throw new Error('Invalid index: ' + index);

		// try get left/right neighbor ids
		const left = this.value[i - 1]?.id ?? null;
		const right = this.value[i]?.id ?? null;

		// create new block
		const id = new ID(replicaId, sequence);
		const block = new Block(id, left, right, value);

		// insert block
		this.value.splice(i, 0, block);
	};

	public delete = (index: number) => {
		const i = this.findIndex(index);
		this.value[i].value = null;
	};

	/**
	 * Maps an index to the true index within the list of blocks,
	 * taking into account tombstones
	 */
	private findIndex = (index: number): number => {
		let count = 0;
		for (let i = 0; i <= this.value.length; i++) {
			const block = this.value[i];
			if (!block) return i;
			if (count === index && !block.isDeleted()) return i;
			if (!block.isDeleted()) count++;
		}
		return -1;
	};

	public merge = (remote: Yata) => {
		// get remote tombstone ids
		const tombstones: ID[] = [];
		for (let b of remote.value) {
			if (b.isDeleted()) tombstones.push(b.id);
		}

		// mark remote tombstones locally
		for (let b of this.value) {
			if (tombstones.findIndex((id) => id.isEqual(b.id)) >= 0) b.value = null;
		}

		// IDs of blocks already existing
		const seen = new IDSet(this.value.map((b) => b.id));

		// deduplicate blocks already existing in current array
		const blocks = remote.value.filter((b) => !seen.contains(b.id));

		let remaining = blocks.length;
		const isPresent = (id: ID | null) => Yata.isBlockPresent(seen, id);

		while (remaining > 0) {
			for (let block of blocks) {
				const canInsert =
					!seen.contains(block.id) && isPresent(block.originLeft) && isPresent(block.originRight);
				if (canInsert) {
					this.integrate(block);
					seen.add(block.id);
					remaining -= 1;
				}
			}
		}
	};

	private static isBlockPresent = (seen: IDSet, id: ID | null): boolean => {
		if (id === null) return true;
		return seen.contains(id);
	};

	public integrate = (block: Block) => {
		const id = block.id;
		const last = this.lastSeqNum(id.replicaId);

		// block was sent out of order - out of scope for now - can stash for resolution later
		if (id.id !== last + 1)
			throw new Error('Block sent out of order: ' + JSON.stringify(block.toJson()));
		const left = this.findIndexOfBlock(block.originLeft);
		const right = this.findIndexOfBlock(block.originRight, this.value.length);
		const i = this.findInsertIndex(block, false, left, right, left + 1, left + 1);
		this.value.splice(i, 0, block);
	};

	private lastSeqNum = (replicaId: number) => {
		let max = -1;
		for (let b of this.value) {
			if (b.id.replicaId === replicaId && b.id.id > max) max = b.id.id;
		}
		return max;
	};

	private findIndexOfBlock = (blockId: ID | null, def: number = -1) => {
		if (blockId === null) return def;
		const idx = this.value.findIndex((b) => b.id.isEqual(blockId));
		if (idx < 0) return def;
		return idx;
	};

	/**
	 * Determines the index to insert a remote block at
	 *
	 * @param block Block to insert
	 * @param scanning If currently scanning between left and right
	 * @param left left bounds to insert into
	 * @param right right bounds to insert into
	 * @param dst current destination index
	 * @param i current index
	 * @returns index to insert block at
	 */
	private findInsertIndex = (
		block: Block,
		scanning: boolean,
		left: number,
		right: number,
		dst: number,
		i: number
	): number => {
		const d = scanning ? dst : i;
		if (i === right) return d;
		const o = this.value[i];
		const oleft = this.findIndexOfBlock(o.originLeft);
		const oright = this.findIndexOfBlock(o.originRight, this.value.length);
		const id1 = block.id.replicaId;
		const id2 = o.id.replicaId;

		if (oleft < left || (oleft === left && oright === right && id1 <= id2)) return d;
		const scan = oleft === left ? id1 <= id2 : scanning;
		return this.findInsertIndex(block, scan, oleft, oright, d, i + 1);
	};

	public toString = (): string => {
		let s = '';
		for (let b of this.value) {
			if (!b.isDeleted()) s += b.value;
		}
		return s;
	};

	public toJson = (): YataSchema => {
		return this.value.map((b) => b.toJson());
	};

	public static parseJson = (o: any): Yata => {
		const data = yataSchema.parse(o);
		const blocks = data.map((b) => {
			const id = new ID(b.id.rid, b.id.id);
			const lid = b.left ? new ID(b.left.rid, b.left.id) : null;
			const rid = b.right ? new ID(b.right.rid, b.right.id) : null;
			return new Block(id, lid, rid, b.val);
		});
		return new Yata(blocks);
	};

	public version = (): Map<number, number> => {
		const version = new Map<number, number>();

		for (let block of this.value) {
			const rid = block.id.replicaId;
			const value = version.get(rid) ?? 0;
			version.set(rid, Math.max(value, block.id.id));
		}

		return version;
	};
}
