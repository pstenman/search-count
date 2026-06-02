export type ProviderCount = {
	provider: string;
	count: number;
};

export type SearchResponse = {
	query: string;
	results: ProviderCount[];
	totalHits: number;
};
