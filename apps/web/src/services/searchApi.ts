import type { SearchResponse } from "@/types/search";

export async function search(query: string): Promise<SearchResponse> {
	const res = await fetch(`/api/search?q=${encodeURIComponent(query)}`);
	if (!res.ok) {
		throw new Error(`Search request failed with status ${res.status}`);
	}
	return res.json();
}
