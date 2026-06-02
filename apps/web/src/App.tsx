import { useState } from "react";
import { Result } from "./components/search/Result";
import { SearchBox } from "./components/search/SearchBox";
import { search } from "./services/searchApi";

function App() {
	const [hits, setHits] = useState<number | null>(null);
	const [loading, setLoading] = useState(false);
	const [error, setError] = useState<string | null>(null);

	async function handleSearch(q: string) {
		try {
			setLoading(true);
			setError(null);

			const data = await search(q);

			setHits(data.totalHits);
		} catch {
			setError("Something went wrong");
		} finally {
			setLoading(false);
		}
	}

	return (
		<div className="max-w-md mx-auto mt-10 space-y-4">
			<SearchBox onSearch={handleSearch} isLoading={loading} />
			<Result loading={loading} error={error} hits={hits} />
		</div>
	);
}

export default App;
