import { SyncPublisher } from './SyncPublisher';
import { SyncSocket } from './SyncSocket';
import { TextContainer } from './TextContainer';

export function NewEditor(docId: string, textarea: HTMLTextAreaElement) {
	const syncSocket = new SyncSocket('ws://localhost:5000/doc/' + docId);
	const syncPublisher = new SyncPublisher(syncSocket);
	const textContainer = new TextContainer(textarea, syncPublisher);
}
