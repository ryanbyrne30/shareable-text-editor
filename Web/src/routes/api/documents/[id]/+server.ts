import { Endpoint } from '@/usecases/common/server';
import { handleGetDocumentById } from '@/usecases/getDocumentById/server';
import { handleUpdateDocument } from '@/usecases/updateDocument/server';

export const GET = Endpoint.new(handleGetDocumentById);
export const PATCH = Endpoint.new(handleUpdateDocument);
