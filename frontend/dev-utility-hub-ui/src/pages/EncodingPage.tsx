import { useState } from 'react'
import TerminalPane from '../components/common/TerminalPane'
import TextArea from '../components/common/TextArea'
import Button from '../components/common/Button'

type EncodeType = 'base64' | 'url' | 'html'
type Direction  = 'encode' | 'decode'

const typeDescriptions: Record<EncodeType, string> = {
  base64: 'base64 -- RFC 4648 standard encoding',
  url:    'url    -- percent-encoding for URI components',
  html:   'html   -- entity encoding for HTML documents',
}

export default function EncodingPage() {
  const [encodeType, setEncodeType] = useState<EncodeType>('base64')
  const [direction,  setDirection]  = useState<Direction>('encode')
  const [input,      setInput]      = useState('')

  return (
    <div className="flex flex-col gap-5 animate-fade-in">

      {/* ── Shell heading ──────────────────────────────────────── */}
      <div>
        <div className="flex items-center gap-1.5 text-xs mb-3">
          <span className="text-(--t-secondary)">user@toolkit</span>
          <span className="text-(--t-text-dim)">:</span>
          <span className="text-(--t-primary) text-glow-sm">~/encode</span>
          <span className="text-(--t-text-dim)">$</span>
          <span className="text-(--t-text-dim) ml-2 italic">
            // {typeDescriptions[encodeType]}
          </span>
        </div>
        <div className="h-px bg-(--t-border)" />
      </div>

      {/* ── Type + direction selectors ─────────────────────────── */}
      <div className="flex flex-wrap items-center gap-x-6 gap-y-3">
        <div className="flex items-center gap-3">
          <span className="text-[10px] uppercase tracking-widest text-(--t-text-dim)">
            TYPE
          </span>
          <div className="flex gap-2">
            {(['base64', 'url', 'html'] as EncodeType[]).map((type) => (
              <Button
                key={type}
                size="sm"
                variant={encodeType === type ? 'primary' : 'secondary'}
                onClick={() => setEncodeType(type)}
              >
                {type.toUpperCase()}
              </Button>
            ))}
          </div>
        </div>

        <div className="w-px h-4 bg-(--t-border) hidden md:block" aria-hidden />

        <div className="flex items-center gap-3">
          <span className="text-[10px] uppercase tracking-widest text-(--t-text-dim)">
            DIR
          </span>
          <div className="flex gap-2">
            {(['encode', 'decode'] as Direction[]).map((dir) => (
              <Button
                key={dir}
                size="sm"
                variant={direction === dir ? 'primary' : 'secondary'}
                onClick={() => setDirection(dir)}
              >
                {dir.toUpperCase()}
              </Button>
            ))}
          </div>
        </div>
      </div>

      {/* ── Input / Output panes ───────────────────────────────── */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">

        {/* Input */}
        <TerminalPane
          title="INPUT"
          badge={`${input.length} chars`}
          bodyClassName="p-3 flex flex-col gap-2"
        >
          <div className="flex items-center gap-1 text-[10px] text-(--t-text-dim)">
            <span className="text-(--t-secondary)">user@toolkit</span>
            <span>:</span>
            <span className="text-(--t-primary)">~/encode</span>
            <span>$</span>
          </div>
          <TextArea
            value={input}
            onChange={(e) => setInput(e.target.value)}
            placeholder={`> paste text to ${direction} here...`}
            rows={10}
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
            <span className="text-(--t-primary)">~/encode</span>
            <span>$</span>
          </div>
          <div
            className={[
              'min-h-40 text-xs leading-relaxed text-(--t-text-dim)',
              'border border-(--t-border) bg-(--t-surface-2) p-3',
              'whitespace-pre-wrap break-all',
            ].join(' ')}
          >
            <span className="cursor-block">awaiting input</span>
          </div>
        </TerminalPane>
      </div>

      {/* ── Run ────────────────────────────────────────────────── */}
      <TerminalPane
        title="EXECUTE"
        bodyClassName="p-3 flex items-center gap-2"
        statusLeft={`> ${direction.toUpperCase()} using ${encodeType.toUpperCase()}`}
      >
        <Button variant="primary" disabled={!input}>
          RUN
        </Button>
        <Button variant="ghost" disabled={!input} onClick={() => setInput('')}>
          CLEAR
        </Button>
        <div className="flex-1" />
        <Button variant="ghost" disabled={!input}>
          COPY OUTPUT
        </Button>
      </TerminalPane>

    </div>
  )
}
