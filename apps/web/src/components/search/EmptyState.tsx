import { Text } from "../ui";

export function EmptyState() {
	return (
		<Text className="text-muted-foreground py-4" size="lg">
			Search to see the amount of hits for your query
		</Text>
	);
}
