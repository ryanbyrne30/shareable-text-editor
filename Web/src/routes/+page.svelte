<script lang="ts">
	import { userStore } from '@/usecases/common/client/stores';
	import { CreateDocumentButton } from '@/usecases/createDocument/client';
</script>

<h1>Hello {$userStore?.username ?? 'User'}!</h1>

<section class="flex w-dvw flex-col items-center justify-center p-16">
	<CreateDocumentButton>Create document</CreateDocumentButton>
</section>
