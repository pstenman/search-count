type ResultProps = {
	loading: boolean;
	error: string | null;
	hits: number | null;
};

export function Result({ loading, error, hits }: ResultProps) {
	if (loading) return <p>Loading...</p>;

	if (error) return <p className="text-red-500">{error}</p>;

	if (hits === 0) return <p>No results</p>;

	if (hits !== null) return <p>Total hits: {hits}</p>;

	return null;
}
