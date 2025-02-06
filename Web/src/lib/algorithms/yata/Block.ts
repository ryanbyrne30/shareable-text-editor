import type { ID } from './ID';
import type { YataSchema } from './Yata';

export class Block {
	constructor(
		public id: ID,
		public originLeft: ID | null,
		public originRight: ID | null,
		public value: string | null
	) {}

	public isDeleted = () => {
		return this.value === null;
	};

	public toJson = (): YataSchema[number] => {
		return {
			id: this.id.toJson(),
			left: this.originLeft?.toJson() ?? null,
			right: this.originRight?.toJson() ?? null,
			val: this.value
		};
	};
}
