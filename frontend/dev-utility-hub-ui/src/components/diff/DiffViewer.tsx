import type { DiffLine } from "../../types/api.types";

interface DiffViewerProps {
  lines: DiffLine[];
}

const lineStyles: Record<DiffLine["type"], string> = {
  added:     "bg-(--t-secondary)/10 text-(--t-secondary)",
  removed:   "bg-(--t-error)/10 text-(--t-error)",
  unchanged: "text-(--t-text-dim)",
};

const linePrefix: Record<DiffLine["type"], string> = {
  added:     "+ ",
  removed:   "- ",
  unchanged: "  ",
};

export default function DiffViewer({ lines }: DiffViewerProps) {
  if (lines.length === 0) {
    return (
      <span className="text-(--t-text-dim) cursor-block">
        add text to both panes and compare
      </span>
    );
  }

  return (
    <div className="font-mono text-sm leading-relaxed">
      {lines.map((line) => (
        <div
          key={line.lineNumber}
          className={["flex items-start gap-3 px-1", lineStyles[line.type]].join(" ")}
        >
          <span className="select-none shrink-0 w-8 text-right opacity-40">
            {line.lineNumber}
          </span>
          <span className="select-none shrink-0 w-4 opacity-70">
            {linePrefix[line.type]}
          </span>
          <span className="whitespace-pre-wrap break-all flex-1">
            {line.content}
          </span>
        </div>
      ))}
    </div>
  );
}
