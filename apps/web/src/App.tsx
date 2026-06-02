import { useState } from "react";
import { SearchBox } from "./components/search/SearchBox";

type ProviderCount = {
	provider: string;
	count: number;
};

type SearchResponse = {
	query: string;
	results: ProviderCount[];
	totalHits: number;
};

function App() {
	const [isLoading, setIsLoading] = useState(false);
	const [error, setError] = useState("");
	const [data, setData] = useState<SearchResponse | null>(null);

	const onSearch = async (query: string) => {
		const trimmedQuery = query.trim();
		if (!trimmedQuery) {
			setError("Please enter a search query.");
			setData(null);
			return;
		}

		setIsLoading(true);
		setError("");

		try {
			const response = await fetch(
				`/api/search?q=${encodeURIComponent(trimmedQuery)}`,
			);

			if (!response.ok) {
				throw new Error(`Search request failed with status ${response.status}`);
			}

			const payload = (await response.json()) as SearchResponse;
			setData(payload);
		} catch (err) {
			const message =
				err instanceof Error ? err.message : "Something went wrong.";
			setError(message);
			setData(null);
		} finally {
			setIsLoading(false);
		}
	};

	return (
		<main>
			<h1>Search Aggregator</h1>
			<SearchBox onSearch={onSearch} isLoading={isLoading} />
			{isLoading && <p>Loading...</p>}
			{error && <p>{error}</p>}
			{data && (
				<section>
					<h2>Results</h2>
					<p>Query: {data.query}</p>
					<p>Total Hits: {data.totalHits}</p>
				</section>
			)}
		</main>
	);
}

export default App;
