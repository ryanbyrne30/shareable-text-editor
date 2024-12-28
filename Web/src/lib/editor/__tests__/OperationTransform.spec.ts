import { describe, expect, it } from 'vitest';
import { newTextAction, type TextAction } from '../TextAction';
import { OperationalTransform } from '../OperationalTransform';

function assertAction(received: TextAction, expected: TextAction) {
	expect(received.revision).toBe(expected.revision);
	expect(received.pos).toBe(expected.pos);
	expect(received.insert).toBe(expected.insert);
	expect(received.delete).toBe(expected.delete);
}

function assertTexts(received: string, expected: string) {
	expect(received).toBe(expected);
}

describe('OperationTransform', () => {
	describe('reverseUpdateOverUpdate', () => {
		function runTest(
			doc: string,
			remoteAction: TextAction,
			localAction: TextAction,
			expected: TextAction,
			expectedString: string
		) {
			const received = OperationalTransform.reverseUpdateOverUpdate(localAction, remoteAction);
			assertAction(received, expected);

			const docAfterLocal = OperationalTransform.applyAction(doc, localAction);
			const docAfterRemote = OperationalTransform.applyAction(docAfterLocal, received);
			assertTexts(docAfterRemote, expectedString);
		}

		it('remoteAction insert comes way before localAction insert -> remoteAction unchanged', () => {
			// arrange
			const doc = '0123456';
			const remoteAction = newTextAction(0, 1, 'aaa', 0);
			const localAction = newTextAction(0, 5, 'bbb', 0);
			const expected = newTextAction(0, remoteAction.pos, 'aaa', 0);
			const expectedString = '0aaa1234bbb56';

			// act & assert
			runTest(doc, remoteAction, localAction, expected, expectedString);
		});

		it('remoteAction delete comes way before localAction insert -> remoteAction unchanged', () => {
			// arrange
			const doc = '0123456';
			const remoteAction = newTextAction(0, 1, '', 2);
			const localAction = newTextAction(0, 5, '', 1);
			const expected = newTextAction(0, remoteAction.pos, '', 2);
			const expectedString = '0346';

			// act & assert
			runTest(doc, remoteAction, localAction, expected, expectedString);
		});

		it('remoteAction update comes way before localAction update -> remoteAction unchanged', () => {
			// arrange
			const doc = '0123456';
			const localAction = newTextAction(0, 5, 'bbb', 2);
			const remoteAction = newTextAction(0, 1, 'aaa', 1);
			const expected = newTextAction(0, remoteAction.pos, 'aaa', 1);
			const expectedString = '0aaa234bbb';

			// act & assert
			runTest(doc, remoteAction, localAction, expected, expectedString);
		});

		it('remoteAction big update comes just before localAction update -> remoteAction readjusted to encapsulate localAction', () => {
			// arrange
			const doc = '0123456';
			const localAction = newTextAction(0, 2, 'bbb', 2);
			const remoteAction = newTextAction(0, 1, 'aaa', 5);
			const expected = newTextAction(0, remoteAction.pos, 'aaabbb', 6);
			const expectedString = '0aaabbb6';

			// act & assert
			runTest(doc, remoteAction, localAction, expected, expectedString);
		});

		it('remoteAction small update comes just before localAction update -> remoteAction readjusted to encapsulate localAction', () => {
			// arrange
			const doc = '0123456';
			const localAction = newTextAction(0, 2, 'bbb', 3);
			const remoteAction = newTextAction(0, 1, 'aaa', 2);
			const expected = newTextAction(0, remoteAction.pos, 'aaabbb', 4);
			const expectedString = '0aaabbb56';

			// act & assert
			runTest(doc, remoteAction, localAction, expected, expectedString);
		});

		it('remoteAction update comes way after localAction update -> remoteAction adjusted position', () => {
			// arrange
			const doc = '0123456';
			const localAction = newTextAction(0, 1, 'bbb', 3);
			const remoteAction = newTextAction(0, 5, 'aaa', 1);
			const expected = newTextAction(0, remoteAction.pos, 'aaa', 1);
			const expectedString = '0bbb4aaa6';

			// act & assert
			runTest(doc, remoteAction, localAction, expected, expectedString);
		});

		it('(2) remoteAction update comes way after localAction update -> remoteAction adjusted position', () => {
			// arrange
			const doc = '0123456';
			const localAction = newTextAction(0, 1, 'b', 3);
			const remoteAction = newTextAction(0, 5, 'aaa', 1);
			const expected = newTextAction(0, remoteAction.pos - 2, 'aaa', 1);
			const expectedString = '0b4aaa6';

			// act & assert
			runTest(doc, remoteAction, localAction, expected, expectedString);
		});

		it('(3) remoteAction update comes way after localAction update -> remoteAction adjusted position', () => {
			// arrange
			const doc = '0123456';
			const localAction = newTextAction(0, 1, 'bbb', 1);
			const remoteAction = newTextAction(0, 5, 'aaa', 1);
			const expected = newTextAction(0, remoteAction.pos + 2, 'aaa', 1);
			const expectedString = '0bbb234aaa6';

			// act & assert
			runTest(doc, remoteAction, localAction, expected, expectedString);
		});

		it('big remoteAction update comes just after small localAction update -> remoteAction adjusted with localAction changes', () => {
			// arrange
			const doc = '0123456';
			const localAction = newTextAction(0, 1, 'bbb', 2);
			const remoteAction = newTextAction(0, 2, 'aaa', 3);
			const expected = newTextAction(0, 4, 'aaa', 2);
			// '0123456';
			// '03456';
			// '0bbb3456';
			const expectedString = '0bbbaaa56';

			// act & assert
			runTest(doc, remoteAction, localAction, expected, expectedString);
		});

		it('small remoteAction update comes just after big localAction update -> remoteAction adjusted with localAction changes', () => {
			// arrange
			const doc = '0123456';
			const localAction = newTextAction(0, 1, 'bbb', 4);
			const remoteAction = newTextAction(0, 3, 'aaa', 1);
			const expected = newTextAction(0, 4, 'aaa', 0);
			const expectedString = '0bbbaaa56';

			// act & assert
			runTest(doc, remoteAction, localAction, expected, expectedString);
		});

		it('remoteAction insert comes just after big localAction update -> remoteAction adjusted with localAction changes', () => {
			// arrange
			const doc = '0123456';
			const localAction = newTextAction(0, 1, 'bbb', 3);
			const remoteAction = newTextAction(0, 3, 'aaa', undefined);
			const expected = newTextAction(0, 4, 'aaa', 0);
			const expectedString = '0bbbaaa456';

			// act & assert
			runTest(doc, remoteAction, localAction, expected, expectedString);
		});

		it('small remoteAction delete comes just after big localAction update -> remoteAction adjusted with localAction changes', () => {
			// arrange
			const doc = '0123456';
			const localAction = newTextAction(0, 1, 'bbb', 4);
			const remoteAction = newTextAction(0, 3, undefined, 1);
			const expected = newTextAction(0, 4, '', 0);
			const expectedString = '0bbb56';

			// act & assert
			runTest(doc, remoteAction, localAction, expected, expectedString);
		});

		it('remoteAction update comes at localAction update -> remoteAction comes before localAction', () => {
			// arrange
			const doc = '0123456';
			const localAction = newTextAction(0, 1, 'bbb', 1);
			const remoteAction = newTextAction(0, 1, 'aaa', 1);
			const expected = newTextAction(0, 1, 'aaabbb', 3);
			const expectedString = '0aaabbb23456';

			// act & assert
			runTest(doc, remoteAction, localAction, expected, expectedString);
		});

		it('big remoteAction update comes at localAction update -> remoteAction comes before localAction', () => {
			// arrange
			const doc = '0123456';
			const localAction = newTextAction(0, 1, 'bbb', 1);
			const remoteAction = newTextAction(0, 1, 'aaa', 3);
			const expected = newTextAction(0, 1, 'aaabbb', 5);
			const expectedString = '0aaabbb456';

			// act & assert
			runTest(doc, remoteAction, localAction, expected, expectedString);
		});

		it('remoteAction update comes at big localAction update -> remoteAction comes before localAction', () => {
			// arrange
			const doc = '0123456';
			const localAction = newTextAction(0, 1, 'bbb', 3);
			const remoteAction = newTextAction(0, 1, 'aaa', 1);
			const expected = newTextAction(0, 1, 'aaabbb', 3);
			const expectedString = '0aaabbb456';

			// act & assert
			runTest(doc, remoteAction, localAction, expected, expectedString);
		});
	});
});
