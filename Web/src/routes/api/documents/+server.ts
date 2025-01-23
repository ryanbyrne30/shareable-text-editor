import { Endpoint } from '@/usecases/common/server';
import { handleCreateDocument } from '@/usecases/createDocument/server';

export const POST = Endpoint.new(handleCreateDocument);
