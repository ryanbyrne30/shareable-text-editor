import { DocumentState } from './DocumentState';
import { SyncSocket } from './SyncSocket';
import { TextContainer } from './TextContainer';

export function NewEditor(docId: string, textarea: HTMLTextAreaElement) {
	const syncSocket = new SyncSocket('ws://localhost:5000/doc/' + docId);
	const document = new DocumentState(syncSocket);
	const textContainer = new TextContainer(textarea);

	syncSocket.addOnMessage(document.resolveMessage);
	textContainer.addChangeFromSelection(document.addChangeFromSelection);
	textContainer.addChangeFromNonSelection(document.addChangeFromNonSelection);
	document.addTextChangeCallback(textContainer.setText);
}
