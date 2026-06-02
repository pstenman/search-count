import type * as React from "react";
import { cn } from "@/lib/utils";

const sizes = {
  sm: "text-sm",
  base: "text-base",
  xl: "text-xl",
  lg: "text-lg",
} as const;

type TextProps = React.PropsWithChildren<{
  className?: string;
  size?: keyof typeof sizes;
}>;

export function Text({ children, className, size = "sm" }: TextProps) {
  return <p className={cn(sizes[size], className)}>{children}</p>;
}
