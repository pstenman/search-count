import { useState } from "react";

type SearchBoxProps = {
	onSearch: (query: string) => Promise<void> | void;
	isLoading?: boolean;
};

export const SearchBox = ({ onSearch, isLoading = false }: SearchBoxProps) => {
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
				disabled={isLoading}
			/>
			<button type="submit" disabled={isLoading}>
				Search
			</button>
		</form>
	);
};
