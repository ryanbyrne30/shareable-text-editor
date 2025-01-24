import { Endpoint } from '@/usecases/common/server';
import { handleGetCurrentUserDocuments } from '@/usecases/getCurrentUserDocuments/server/handleGetCurrentUserDocuments';

export const GET = Endpoint.new(handleGetCurrentUserDocuments);
