import { useState } from "react";

type SearchBoxProps = {
	onSearch: (query: string) => void;
};

export const SearchBox = ({ onSearch }: SearchBoxProps) => {
	const [query, setQuery] = useState("");

	const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
		e.preventDefault();
		onSearch(query);
	};

	return (
		<form onSubmit={handleSubmit}>
			<input
				type="text"
				value={query}
				onChange={(e) => setQuery(e.target.value)}
			/>
			<button type="submit">Search</button>
		</form>
	);
};
