import { Endpoint } from '@/usecases/common/server';
import { handleDeleteDocument } from '@/usecases/deleteDocument/server';
import { handleGetDocumentById } from '@/usecases/getDocumentById/server';
import { handleUpdateDocument } from '@/usecases/updateDocument/server';

export const GET = Endpoint.new(handleGetDocumentById);
export const PATCH = Endpoint.new(handleUpdateDocument);
export const DELETE = Endpoint.new(handleDeleteDocument);
