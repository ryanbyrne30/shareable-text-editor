<script>
	import { Card } from '@/lib/components/card';
	import { updateUserStore } from '@/usecases/common/client/stores';
	import { SignInUserForm } from '@/usecases/signInUser/client';

	function onsuccess() {
		updateUserStore();
		location.href = '/';
	}
</script>

<main class="flex h-dvh w-dvw flex-col items-center justify-center">
	<Card>
		<SignInUserForm {onsuccess} />
	</Card>
</main>
