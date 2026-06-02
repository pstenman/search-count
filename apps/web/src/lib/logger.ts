export const logger = {
	debug: (...args: unknown[]) => {
		if (import.meta.env.DEV) {
			console.debug(...args);
		}
	},
	error: (...args: unknown[]) => {
		if (import.meta.env.DEV) {
			console.error(...args);
		}
	},
};
