<script lang="ts">
	import { Card } from '@/lib/components/card';
	import { RegisterUserForm } from '@/usecases/registerUser/client';
</script>

<main class="flex h-dvh w-dvw flex-col items-center justify-center">
	<Card>
		<RegisterUserForm onsuccess={() => (location.href = '/auth/sign-in')} />
	</Card>
</main>
