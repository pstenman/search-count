import { SearchIcon } from "lucide-react";
import { type SubmitEvent, useState } from "react";
import { Button, Input } from "@/components/ui";

type SearchBoxProps = {
	onSearch: (query: string) => Promise<void> | void;
	isLoading?: boolean;
};

export const SearchBox = ({ onSearch, isLoading = false }: SearchBoxProps) => {
	const [query, setQuery] = useState("");

	const handleSubmit = (e: SubmitEvent<HTMLFormElement>) => {
		e.preventDefault();
		const trimmed = query.trim();
		if (!trimmed) return;

		onSearch(trimmed);
	};

	return (
		<form onSubmit={handleSubmit}>
			<Input
				value={query}
				onChange={(e) => setQuery(e.target.value)}
				placeholder="Search..."
			/>

			<Button type="submit" disabled={isLoading || !query.trim()}>
				<SearchIcon className="size-4" />
			</Button>
		</form>
	);
};
