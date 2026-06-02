import { useState } from "react";
import { Result } from "./components/search/Result";
import { SearchBox } from "./components/search/SearchBox";
import { logger } from "./lib/logger";
import { search } from "./services/searchApi";

function App() {
	const [hits, setHits] = useState<number | null>(null);
	const [loading, setLoading] = useState(false);
	const [error, setError] = useState<string | null>(null);

	async function handleSearch(q: string) {
		try {
			setLoading(true);
			setError(null);

			logger.debug("Search request", { query: q });

			const data = await search(q);

			logger.debug("Search response", {
				query: data.query,
				totalHits: data.totalHits,
			});

			setHits(data.totalHits);
		} catch {
			logger.error("Search request failed");
			setError("An error occurred while searching");
		} finally {
			setLoading(false);
		}
	}

	function handleClear() {
		setHits(null);
		setError(null);
	}

	return (
		<div className="mx-auto flex min-h-dvh max-w-md flex-col justify-center space-y-4 px-4">
			<SearchBox
				onSearch={handleSearch}
				onClear={handleClear}
				isLoading={loading}
			/>
			<div className="min-h-[3.75rem] py-4">
				<Result loading={loading} error={error} hits={hits} />
			</div>
		</div>
	);
}

export default App;
