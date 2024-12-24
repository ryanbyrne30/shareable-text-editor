import type { OnInputCallback, OnKeydownCallback, OnSelectionChangeCallback } from './types';

export interface ITextContainer {
	cleanup(): void;
	setOnKeyDown(cb: OnKeydownCallback): void;
	setOnSelectionChange(cb: OnSelectionChangeCallback): void;
	setOnInput(cb: OnInputCallback): void;
	focus(): void;
}
