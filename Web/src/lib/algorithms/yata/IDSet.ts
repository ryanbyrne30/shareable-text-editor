import type { ID } from './ID';

export class IDSet {
	public value: Map<string, ID> = new Map();

	constructor(ids: ID[] = []) {
		for (let id of ids) {
			this.value.set(IDSet.key(id), id);
		}
	}

	private static key = (id: ID): string => {
		return `${id.replicaId}.${id.id}`;
	};

	public add = (id: ID) => {
		this.value.set(IDSet.key(id), id);
	};

	public contains = (id: ID): boolean => {
		return this.value.has(IDSet.key(id));
	};
}
