import { SearchIcon, XIcon } from "lucide-react";
import { type SubmitEvent, useState } from "react";
import { Button, Input } from "@/components/ui";

type SearchBoxProps = {
  onSearch: (query: string) => Promise<void> | void;
  onClear?: () => void;
  isLoading?: boolean;
};

export const SearchBox = ({
  onSearch,
  onClear,
  isLoading = false,
}: SearchBoxProps) => {
  const [query, setQuery] = useState("");

  const handleSubmit = (e: SubmitEvent<HTMLFormElement>) => {
    e.preventDefault();

    const trimmed = query.trim();
    if (!trimmed) return;

    onSearch(trimmed);
  };

  const handleChange = (value: string) => {
    const wasEmpty = query.trim().length === 0;

    setQuery(value);

    const isEmpty = value.trim().length === 0;

    if (!wasEmpty && isEmpty) {
      onClear?.();
    }
  };

  const handleClear = () => {
    setQuery("");
    onClear?.();
  };

  const canClear = query.length > 0 && !isLoading;

  return (
    <form onSubmit={handleSubmit} className="flex gap-2">
      <Input
        value={query}
        onChange={(e) => handleChange(e.target.value)}
        placeholder="Search..."
      />

      <Button
        type="submit"
        disabled={isLoading || !query.trim()}
        variant="default"
        size="icon"
        aria-label="Search"
      >
        <SearchIcon />
      </Button>

      <Button
        type="button"
        onClick={handleClear}
        disabled={!canClear}
        variant="outline"
        size="icon"
        aria-label="Clear search"
      >
        <XIcon />
      </Button>
    </form>
  );
};
