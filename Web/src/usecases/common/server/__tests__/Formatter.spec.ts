import { describe, it, expect } from 'vitest';
import { Formatter } from '../Formatter';

describe('Formatter', () => {
	describe('stringToSnakeCase', () => {
		it('should return snake case of string', () => {
			const tests: [string, string][] = [
				['HelloThere', 'hello_there'],
				['abcd', 'abcd'],
				['a_b_c', 'a_b_c'],
				['aBc', 'a_bc']
			];

			for (let [input, expected] of tests) {
				const received = Formatter.stringToSnakeCase(input);
				expect(received).toBe(expected);
			}
		});
	});

	describe('isRecord', () => {
		it('should have expected results', () => {
			const tests: [unknown, boolean][] = [
				['', false],
				['a', false],
				[true, false],
				[[], false],
				[['a', 1], false],
				[-123, false],
				[{ a: 'b' }, true],
				[{ a: 'b', b: [3, 4] }, true]
			];

			for (let [input, expected] of tests) {
				const received = Formatter.isRecord(input);
				expect(received).toBe(expected);
			}
		});
	});

	describe('toSnakeCase', () => {
		it('should have expected results', () => {
			const tests: [unknown, unknown][] = [
				['', ''],
				[false, false],
				[true, true],
				[
					[1, 2, 3],
					[1, 2, 3]
				],
				[['aB'], ['aB']],
				[{ aB: 'c' }, { a_b: 'c' }],
				[{ AB: { CD: 'DE' } }, { a_b: { c_d: 'DE' } }]
			];

			for (let [input, expected] of tests) {
				const i = input;
				const received = Formatter.toSnakeCase(input);
				expect(Formatter.deepEqual(received, expected)).toBeTruthy();
			}
		});
	});
});
