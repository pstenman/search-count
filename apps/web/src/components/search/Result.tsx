import { formatHits } from "@/lib/utils";
import { Text } from "../ui";
import { EmptyState } from "./EmptyState";
import { ResultSkeleton } from "./ResultSkeleton";

type ResultProps = {
  loading: boolean;
  error: string | null;
  hits: number | null;
};

export function Result({ loading, error, hits }: ResultProps) {
  if (loading) return <ResultSkeleton />;

  if (error) return <Text className="text-red-500">{error}</Text>;

  if (hits === null) return <EmptyState />;

  return (
    <div className="space-y-2">
      <Text className="text-muted-foreground" size="base">
        Total hits
      </Text>
      <Text className="font-semibold tracking-tight tabular-nums" size="xl">
        {formatHits(hits)}
      </Text>
    </div>
  );
}
