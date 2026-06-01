import { SearchBox } from "./components/search/searchBox";

function App() {
	const onSearch = (query: string) => {
		console.log(query);
	};

	return (
		<main>
			<h1>Search Aggregator</h1>
			<SearchBox onSearch={onSearch} />
		</main>
	);
}

export default App;
