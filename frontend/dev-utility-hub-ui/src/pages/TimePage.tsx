import { useState } from 'react'
import TerminalPane from '../components/common/TerminalPane'
import Button from '../components/common/Button'

type ConvertMode = 'toHuman' | 'toUnix'

const resultFields = [
  { key: 'seconds',  label: 'UNIX SECONDS  ' },
  { key: 'ms',       label: 'UNIX MILLIS   ' },
  { key: 'utc',      label: 'UTC           ' },
  { key: 'iso',      label: 'ISO 8601      ' },
  { key: 'relative', label: 'RELATIVE      ' },
]

export default function TimePage() {
  const [mode,       setMode]       = useState<ConvertMode>('toHuman')
  const [unixInput,  setUnixInput]  = useState('')
  const [humanInput, setHumanInput] = useState('')

  const canConvert = mode === 'toHuman'
    ? unixInput.trim().length > 0
    : humanInput.trim().length > 0

  return (
    <div className="flex flex-col gap-5 animate-fade-in">

      {/* ── Shell heading ──────────────────────────────────────── */}
      <div>
        <div className="flex items-center gap-1.5 text-xs mb-3">
          <span className="text-(--t-secondary)">user@toolkit</span>
          <span className="text-(--t-text-dim)">:</span>
          <span className="text-(--t-primary) text-glow-sm">~/timestamp</span>
          <span className="text-(--t-text-dim)">$</span>
          <span className="text-(--t-text-dim) ml-2 italic">
            // unix ↔ human-readable -- seconds and milliseconds
          </span>
        </div>
        <div className="h-px bg-(--t-border)" />
      </div>

      {/* ── Mode selector ──────────────────────────────────────── */}
      <div className="flex items-center gap-3">
        <span className="text-[10px] uppercase tracking-widest text-(--t-text-dim)">
          MODE
        </span>
        <div className="flex gap-2">
          <Button
            size="sm"
            variant={mode === 'toHuman' ? 'primary' : 'secondary'}
            onClick={() => setMode('toHuman')}
          >
            UNIX → HUMAN
          </Button>
          <Button
            size="sm"
            variant={mode === 'toUnix' ? 'primary' : 'secondary'}
            onClick={() => setMode('toUnix')}
          >
            HUMAN → UNIX
          </Button>
        </div>
      </div>

      {/* ── Input panes ────────────────────────────────────────── */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">

        {/* Unix timestamp input */}
        <TerminalPane
          title="UNIX TIMESTAMP"
          badge={mode === 'toHuman' ? '[ACTIVE]' : undefined}
          bodyClassName="p-3 flex flex-col gap-2"
        >
          <div className="text-[10px] text-(--t-text-dim) flex items-center gap-1">
            <span className="text-(--t-secondary)">user@toolkit</span>
            <span>:</span>
            <span className="text-(--t-primary)">~/timestamp</span>
            <span>$</span>
          </div>
          <input
            type="text"
            inputMode="numeric"
            value={unixInput}
            onChange={(e) => setUnixInput(e.target.value)}
            placeholder="> e.g. 1700000000 or 1700000000000"
            disabled={mode === 'toUnix'}
            className={[
              'w-full bg-(--t-surface) text-(--t-text)',
              'border border-(--t-border) rounded-none',
              'text-xs px-3 py-2.5',
              'placeholder:text-(--t-text-dim) placeholder:opacity-40',
              'focus:outline-none focus:border-(--t-border-active)',
              'disabled:opacity-40 disabled:cursor-not-allowed',
              'transition-colors duration-100',
            ].join(' ')}
          />
          <p className="text-[10px] text-(--t-text-dim) italic">
            // accepts seconds (10 digits) or milliseconds (13 digits)
          </p>
        </TerminalPane>

        {/* Human-readable input */}
        <TerminalPane
          title="HUMAN READABLE"
          badge={mode === 'toUnix' ? '[ACTIVE]' : undefined}
          bodyClassName="p-3 flex flex-col gap-2"
        >
          <div className="text-[10px] text-(--t-text-dim) flex items-center gap-1">
            <span className="text-(--t-secondary)">user@toolkit</span>
            <span>:</span>
            <span className="text-(--t-primary)">~/timestamp</span>
            <span>$</span>
          </div>
          <input
            type="text"
            value={humanInput}
            onChange={(e) => setHumanInput(e.target.value)}
            placeholder="> e.g. 2024-11-14T22:13:20Z"
            disabled={mode === 'toHuman'}
            className={[
              'w-full bg-(--t-surface) text-(--t-text)',
              'border border-(--t-border) rounded-none',
              'text-xs px-3 py-2.5',
              'placeholder:text-(--t-text-dim) placeholder:opacity-40',
              'focus:outline-none focus:border-(--t-border-active)',
              'disabled:opacity-40 disabled:cursor-not-allowed',
              'transition-colors duration-100',
            ].join(' ')}
          />
          <p className="text-[10px] text-(--t-text-dim) italic">
            // ISO 8601, RFC 2822, or natural language
          </p>
        </TerminalPane>
      </div>

      {/* ── Convert ────────────────────────────────────────────── */}
      <div className="flex items-center gap-2">
        <Button variant="primary" disabled={!canConvert}>
          CONVERT
        </Button>
        <Button
          variant="ghost"
          disabled={!unixInput && !humanInput}
          onClick={() => { setUnixInput(''); setHumanInput('') }}
        >
          CLEAR
        </Button>
      </div>

      {/* ── Result pane ────────────────────────────────────────── */}
      <TerminalPane
        title="RESULT"
        badge="[READY]"
        bodyClassName="p-3"
        statusLeft="> enter a timestamp and convert"
      >
        <div className="border border-(--t-border) bg-(--t-surface-2) p-3">
          <table className="w-full text-xs border-collapse">
            <tbody>
              {resultFields.map(({ key, label }, i) => (
                <tr
                  key={key}
                  className={i < resultFields.length - 1 ? 'border-b border-(--t-border)' : ''}
                >
                  <td className="py-2 pr-4 text-(--t-text-dim) uppercase tracking-wider whitespace-nowrap w-40">
                    {label}
                  </td>
                  <td className="py-2 text-(--t-border) select-none" aria-hidden>
                    :
                  </td>
                  <td className="py-2 pl-4 text-(--t-text) font-mono cursor-block">
                    --
                  </td>
                </tr>
              ))}
            </tbody>
          </table>
        </div>
      </TerminalPane>

    </div>
  )
}
