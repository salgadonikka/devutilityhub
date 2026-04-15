import { useState } from 'react'
import TerminalPane from '../components/common/TerminalPane'
import TextArea from '../components/common/TextArea'
import Button from '../components/common/Button'

type FormatType = 'json' | 'xml'

export default function FormatterPage() {
  const [formatType, setFormatType] = useState<FormatType>('json')
  const [input, setInput] = useState('')

  const charCount = input.length

  return (
    <div className="flex flex-col gap-5 animate-fade-in">

      {/* ── Shell heading ──────────────────────────────────────── */}
      <div>
        <div className="flex items-center gap-1.5 text-xs mb-3">
          <span className="text-(--t-secondary)">user@toolkit</span>
          <span className="text-(--t-text-dim)">:</span>
          <span className="text-(--t-primary) text-glow-sm">~/formatter</span>
          <span className="text-(--t-text-dim)">$</span>
          <span className="text-(--t-text-dim) ml-2 italic">
            // universal formatter -- json / xml
          </span>
        </div>
        <div className="h-px bg-(--t-border)" />
      </div>

      {/* ── Format type selector ───────────────────────────────── */}
      <div className="flex items-center gap-3">
        <span className="text-[10px] uppercase tracking-widest text-(--t-text-dim)">
          FORMAT
        </span>
        <div className="flex gap-2">
          {(['json', 'xml'] as FormatType[]).map((type) => (
            <Button
              key={type}
              size="sm"
              variant={formatType === type ? 'primary' : 'secondary'}
              onClick={() => setFormatType(type)}
            >
              {type.toUpperCase()}
            </Button>
          ))}
        </div>
      </div>

      {/* ── Input / Output panes ───────────────────────────────── */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">

        {/* Input */}
        <TerminalPane
          title="INPUT"
          badge={`${charCount} chars`}
          bodyClassName="p-3 flex flex-col gap-2"
        >
          <div className="flex items-center gap-1 text-[10px] text-(--t-text-dim)">
            <span className="text-(--t-secondary)">user@toolkit</span>
            <span>:</span>
            <span className="text-(--t-primary)">~/formatter</span>
            <span>$</span>
          </div>
          <TextArea
            value={input}
            onChange={(e) => setInput(e.target.value)}
            placeholder={`> paste ${formatType.toUpperCase()} here...`}
            rows={12}
            className="flex-1"
          />
        </TerminalPane>

        {/* Output */}
        <TerminalPane
          title="OUTPUT"
          badge="[READY]"
          bodyClassName="p-3"
        >
          <div className="text-[10px] text-(--t-text-dim) mb-2 flex items-center gap-1">
            <span className="text-(--t-secondary)">user@toolkit</span>
            <span>:</span>
            <span className="text-(--t-primary)">~/formatter</span>
            <span>$</span>
          </div>
          <div
            className={[
              'min-h-48 text-xs leading-relaxed text-(--t-text-dim)',
              'border border-(--t-border) bg-(--t-surface-2) p-3',
              'whitespace-pre-wrap',
            ].join(' ')}
          >
            <span className="cursor-block">awaiting input</span>
          </div>
        </TerminalPane>
      </div>

      {/* ── Actions ────────────────────────────────────────────── */}
      <TerminalPane
        title="ACTIONS"
        bodyClassName="p-3 flex items-center gap-2 flex-wrap"
        statusLeft={`> run a command on the ${formatType.toUpperCase()} in the input pane`}
      >
        <Button size="sm" variant="secondary" disabled={!input}>
          PRETTIFY
        </Button>
        <Button size="sm" variant="secondary" disabled={!input}>
          MINIFY
        </Button>
        <Button size="sm" variant="secondary" disabled={!input}>
          VALIDATE
        </Button>
        <div className="flex-1" />
        <Button size="sm" variant="ghost" disabled={!input}>
          COPY
        </Button>
        <Button size="sm" variant="ghost" disabled={!input} onClick={() => setInput('')}>
          CLEAR
        </Button>
      </TerminalPane>

    </div>
  )
}
