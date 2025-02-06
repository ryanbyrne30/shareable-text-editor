import { z } from 'zod';

export const idSchema = z.object({ id: z.number(), rid: z.number() });
export type IDSchema = z.infer<typeof idSchema>;

export class ID {
	constructor(
		public replicaId: number,
		public id: number
	) {}

	public isEqual = (i: ID): boolean => {
		return this.replicaId === i.replicaId && this.id === i.id;
	};

	public toJson = (): IDSchema => {
		return {
			rid: this.replicaId,
			id: this.id
		};
	};
}
