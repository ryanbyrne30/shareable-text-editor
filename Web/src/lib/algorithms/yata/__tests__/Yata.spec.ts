import { describe, expect, it } from 'vitest';
import { Block, ID, Yata } from '../Yata';

function assertBlockEqual(b1: Block, b2: Block) {
	expect(b1.id.isEqual(b2.id)).toBeTruthy();
	if (b1.originLeft === null || b2.originLeft === null) {
		expect(b2.originLeft).toBeNull();
	} else {
		expect(b1.originLeft.isEqual(b2.originLeft));
	}
	if (b1.originRight === null || b2.originRight === null) {
		expect(b2.originRight).toBeNull();
	} else {
		expect(b1.originRight.isEqual(b2.originRight));
	}
	expect(b1.value).toBe(b2.value);
}

function assertBlocksEqual(b1: Block[], b2: Block[]) {
	expect(b1.length).toBe(b2.length);
	for (let i = 0; i < b1.length; i++) {
		assertBlockEqual(b1[i], b2[i]);
	}
}

describe('Yata', () => {
	describe('merge', () => {
		it('should merge empty Yatas', () => {
			const y1 = new Yata([]);
			const y2 = new Yata([]);
			const expected: Block[] = [];

			y1.merge(y2);
			assertBlocksEqual(y1.value, expected);

			y2.merge(y1);
			assertBlocksEqual(y2.value, expected);
		});

		it('should merge insert Yatas', () => {
			const y1 = new Yata([
				new Block(new ID(0, 0), null, null, 'a'),
				new Block(new ID(1, 0), new ID(0, 0), null, 'b')
			]);
			const y2 = new Yata([
				new Block(new ID(0, 0), null, null, 'a'),
				new Block(new ID(1, 0), new ID(0, 0), null, 'b'),
				new Block(new ID(1, 1), new ID(1, 0), null, 'c')
			]);
			const expected: Block[] = [
				new Block(new ID(0, 0), null, null, 'a'),
				new Block(new ID(1, 0), new ID(0, 0), null, 'b'),
				new Block(new ID(1, 1), new ID(1, 0), null, 'c')
			];

			y1.merge(y2);
			assertBlocksEqual(y1.value, expected);

			y2.merge(y1);
			assertBlocksEqual(y2.value, expected);
		});

		it('should merge Yatas with tombstones', () => {
			const y1 = new Yata([
				new Block(new ID(0, 0), null, null, 'a'),
				new Block(new ID(1, 0), new ID(0, 0), null, null)
			]);
			const y2 = new Yata([
				new Block(new ID(0, 0), null, null, 'a'),
				new Block(new ID(1, 0), new ID(0, 0), null, 'b')
			]);
			const expected: Block[] = [
				new Block(new ID(0, 0), null, null, 'a'),
				new Block(new ID(1, 0), new ID(0, 0), null, null)
			];

			y1.merge(y2);
			assertBlocksEqual(y1.value, expected);

			y2.merge(y1);
			assertBlocksEqual(y2.value, expected);
		});
	});
});
