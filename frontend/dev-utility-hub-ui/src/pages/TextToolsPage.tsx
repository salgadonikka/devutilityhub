import { useState } from "react";
import TerminalPane from "../components/common/TerminalPane";
import TextArea from "../components/common/TextArea";
import Button from "../components/common/Button";
import Loader from "../components/common/Loader";
import { useApi } from "../hooks/useApi";
import { transformText } from "../api/textApi";
import type { TextTransformResponse } from "../types/api.types";

type Operation = {
  id: string;
  label: string;
  group: "CASE" | "EDIT";
};

const operations: Operation[] = [
  { id: "uppercase", label: "UPPER-CASE", group: "CASE" },
  { id: "lowercase", label: "LOWER-CASE", group: "CASE" },
  { id: "titlecase", label: "TITLE-CASE", group: "CASE" },
  { id: "camelcase", label: "CAMEL-CASE", group: "CASE" },
  { id: "snakecase", label: "SNAKE-CASE", group: "CASE" },
  { id: "kebabcase", label: "KEBAB-CASE", group: "CASE" },
  { id: "trim", label: "TRIM", group: "EDIT" },
  { id: "sort", label: "SORT-LINES", group: "EDIT" },
  { id: "dedup", label: "DEDUP", group: "EDIT" },
  { id: "reverse", label: "REVERSE", group: "EDIT" },
  { id: "count", label: "COUNT-LINES", group: "EDIT" },
];

const groups = ["CASE", "EDIT"] as const;

export default function TextToolsPage() {
  const [input, setInput] = useState("");
  const [selectedOps, setSelectedOps] = useState<string[]>([]);
  const { data, loading, error, call, reset } = useApi<TextTransformResponse>();

  const caseOpIds = operations
    .filter((o) => o.group === "CASE")
    .map((o) => o.id);

  const toggleOp = (id: string) => {
    setSelectedOps((prev) => {
      // Deselect if already active
      if (prev.includes(id)) return prev.filter((o) => o !== id);

      // count is solo, deselects everything
      if (id === "count") return ["count"];

      // Selecting anything deselects count
      const withoutCount = prev.filter((o) => o !== "count");

      // CASE ops are mutually exclusive — deselect any other CASE op
      if (caseOpIds.includes(id)) {
        return [...withoutCount.filter((o) => !caseOpIds.includes(o)), id];
      }

      return [...withoutCount, id];
    });
  };

  const opCount = selectedOps.length;

  const outputBadge = data?.isValid
    ? `[${data.appliedOperations.length} ops applied]`
    : "[READY]";

  const pipelineStatus = data?.isValid
    ? `> pipeline: ${data.appliedOperations.join(" | ")}`
    : opCount > 0
      ? `> pipeline: ${selectedOps.join(" | ")}`
      : undefined;

  const renderOutput = () => {
    if (loading) return <Loader />;
    if (error) return <span className="text-(--t-error) text-xs">{error}</span>;
    if (data && !data.isValid)
      return (
        <span className="text-(--t-error) text-xs">
          {data.errorMessage ?? "Something went wrong"}
        </span>
      );
    if (data?.isValid) return data.output;
    return <span className="cursor-block">select operations and apply</span>;
  };

  return (
    <div className="flex flex-col gap-5 animate-fade-in">
      {/* ── Shell heading ──────────────────────────────────────── */}
      <div>
        <div className="flex items-center gap-1.5 text-xs mb-3">
          <span className="text-(--t-secondary)">user@toolkit</span>
          <span className="text-(--t-text-dim)">:</span>
          <span className="text-(--t-primary) text-glow-sm">~/text-tools</span>
          <span className="text-(--t-text-dim)">$</span>
          <span className="text-(--t-text-dim) ml-2 italic">
            // case conversion, cleanup, line operations
          </span>
        </div>
        <div className="h-px bg-(--t-border)" />
      </div>

      {/* ── Input pane ─────────────────────────────────────────── */}
      <TerminalPane
        title="INPUT"
        badge={`${input.length} chars`}
        bodyClassName="p-3 flex flex-col gap-2"
      >
        <div className="flex items-center gap-1 text-[10px] text-(--t-text-dim)">
          <span className="text-(--t-secondary)">user@toolkit</span>
          <span>:</span>
          <span className="text-(--t-primary)">~/text-tools</span>
          <span>$</span>
        </div>
        <TextArea
          value={input}
          onChange={(e) => setInput(e.target.value)}
          placeholder="> paste or type text here..."
          rows={8}
        />
      </TerminalPane>

      {/* ── Operations ─────────────────────────────────────────── */}
      <TerminalPane
        title="OPERATIONS"
        badge={opCount > 0 ? `${opCount} selected` : undefined}
        bodyClassName="p-3 flex flex-col gap-4"
        statusLeft="> click to toggle operations, then apply"
      >
        {groups.map((group) => (
          <div key={group} className="flex flex-col gap-2">
            <span className="text-[10px] text-(--t-text-dim) uppercase tracking-widest">
              {group}:
            </span>
            <div className="flex flex-wrap gap-2">
              {operations
                .filter((op) => op.group === group)
                .map((op) => {
                  const active = selectedOps.includes(op.id);
                  return (
                    <Button
                      key={op.id}
                      size="sm"
                      variant={active ? "primary" : "secondary"}
                      onClick={() => toggleOp(op.id)}
                    >
                      {op.label}
                    </Button>
                  );
                })}
            </div>
          </div>
        ))}

        {/* ── Clear selections ──────────────────────────────────── */}
        <div className="h-px bg-(--t-border)" />
        <div className="flex justify-end">
          <Button
            size="sm"
            variant="ghost"
            disabled={opCount === 0}
            onClick={() => setSelectedOps([])}
          >
            CLEAR SELECTIONS
          </Button>
        </div>
      </TerminalPane>

      {/* ── Execute + Output ───────────────────────────────────── */}
      <div className="flex items-center gap-2 flex-wrap">
        <Button
          variant="primary"
          disabled={!input || opCount === 0 || loading}
          onClick={() =>
            call(() => transformText({ input, operations: selectedOps }))
          }
        >
          APPLY
        </Button>
        <Button
          variant="ghost"
          disabled={!input}
          onClick={() => {
            setInput("");
            setSelectedOps([]);
            reset();
          }}
        >
          CLEAR
        </Button>
        <div className="flex-1" />
        <Button
          variant="ghost"
          disabled={!data?.isValid || !data?.output}
          onClick={() => {
            if (data?.output) navigator.clipboard.writeText(data.output);
          }}
        >
          COPY OUTPUT
        </Button>
      </div>

      <TerminalPane
        title="OUTPUT"
        badge={outputBadge}
        bodyClassName="p-3"
        statusLeft={pipelineStatus}
      >
        <div
          className={[
            "min-h-32 text-xs leading-relaxed text-(--t-text-dim)",
            "border border-(--t-border) bg-(--t-surface-2) p-3",
            "whitespace-pre-wrap",
          ].join(" ")}
        >
          {renderOutput()}
        </div>
      </TerminalPane>
    </div>
  );
}
