import { sveltekit } from '@sveltejs/kit/vite';
import { defineConfig } from 'vite';

export default defineConfig({
	plugins: [sveltekit()],
	legacy: { buildSsrCjsExternalHeuristics: true },
	server: {
		port: 4444
	}
});
