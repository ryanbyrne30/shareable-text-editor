<script lang="ts">
	import { userStore } from '@/usecases/common/client/stores';
</script>

<h1>Hello {$userStore?.username ?? 'User'}!</h1>
