import { useState } from 'react'
import TerminalPane from '../components/common/TerminalPane'
import Button from '../components/common/Button'
import Loader from '../components/common/Loader'
import { useApi } from '../hooks/useApi'
import { convertTimestamp } from '../api/timeApi'
import type { TimeResponse } from '../types/api.types'

type ConvertMode = 'toHuman' | 'toUnix'

const TIMEZONES = (Intl.supportedValuesOf('timeZone') as string[]).map(zone => {
  try {
    const parts = new Intl.DateTimeFormat('en-US', {
      timeZone: zone,
      timeZoneName: 'longOffset',
    }).formatToParts(new Date())
    const offset = parts.find(p => p.type === 'timeZoneName')?.value ?? 'GMT+00:00'
    const friendlyName = zone.replace(/_/g, ' ').split('/').pop()
    return { id: zone, label: `(${offset}) ${friendlyName}` }
  } catch {
    return { id: zone, label: `(GMT+00:00) ${zone}` }
  }
}).sort((a, b) => a.label.localeCompare(b.label))

const resultFields = [
  { key: 'seconds',  label: 'UNIX SECONDS  ' },
  { key: 'ms',       label: 'UNIX MILLIS   ' },
  { key: 'utc',      label: 'UTC           ' },
  { key: 'iso',      label: 'ISO 8601      ' },
  { key: 'local',    label: 'LOCAL         ' },
  { key: 'relative', label: 'RELATIVE      ' },
]

function getRelativeTime(seconds: number): string {
  const diff = seconds - Math.floor(Date.now() / 1000)
  const abs  = Math.abs(diff)
  const rtf  = new Intl.RelativeTimeFormat('en', { numeric: 'auto' })

  if (abs < 60)           return rtf.format(diff, 'second')
  if (abs < 3_600)        return rtf.format(Math.round(diff / 60), 'minute')
  if (abs < 86_400)       return rtf.format(Math.round(diff / 3_600), 'hour')
  if (abs < 86_400 * 30)  return rtf.format(Math.round(diff / 86_400), 'day')
  if (abs < 86_400 * 365) return rtf.format(Math.round(diff / (86_400 * 30)), 'month')
  return rtf.format(Math.round(diff / (86_400 * 365)), 'year')
}

function buildDisplay(data: TimeResponse) {
  return {
    seconds:  data.seconds.toString(),
    ms:       data.ms.toString(),
    utc:      data.utc,
    iso:      data.iso,
    local:    data.local ?? '--',
    relative: getRelativeTime(data.seconds),
  }
}

export default function TimePage() {
  const [mode,       setMode]       = useState<ConvertMode>('toHuman')
  const [unixInput,  setUnixInput]  = useState('')
  const [humanInput, setHumanInput] = useState('')
  const [tzId,       setTzId]       = useState(() => Intl.DateTimeFormat().resolvedOptions().timeZone)
  const { data, loading, error, call, reset } = useApi<TimeResponse>()

  const canConvert = mode === 'toHuman'
    ? unixInput.trim().length > 0
    : humanInput.trim().length > 0

  const handleConvert = () => {
    if (mode === 'toHuman') {
      const trimmed = unixInput.trim()
      const unixValue = parseInt(trimmed, 10)
      const isMilliseconds = trimmed.length === 13
      call(() => convertTimestamp({ direction: 'toHuman', unixValue, isMilliseconds, timeZoneId: tzId || undefined }))
    } else {
      call(() => convertTimestamp({ direction: 'toUnix', humanValue: humanInput.trim(), timeZoneId: tzId || undefined }))
    }
  }

  const handleClear = () => {
    setUnixInput('')
    setHumanInput('')
    reset()
  }

  const display = data?.isValid ? buildDisplay(data) : null

  const badge = data?.isValid ? '[DONE]' : '[READY]'
  const statusLeft = display
    ? `> seconds: ${display.seconds}`
    : '> enter a timestamp and convert'

  const renderResult = () => {
    if (loading) return <div className="flex items-center justify-center py-6"><Loader /></div>
    if (error)   return <span className="text-(--t-error) text-xs">{error}</span>
    if (data && !data.isValid) return <span className="text-(--t-error) text-xs">{data.errorMessage ?? 'Something went wrong'}</span>

    return (
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
              <td className="py-2 pl-4 text-(--t-text) font-mono">
                {display ? display[key as keyof typeof display] : '--'}
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    )
  }

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

      {/* ── Timezone selector ─────────────────────────────────── */}
      <div className="flex items-center gap-3">
        <span className="text-[10px] uppercase tracking-widest text-(--t-text-dim) shrink-0">
          TIMEZONE
        </span>
        <select
          value={tzId}
          onChange={(e) => setTzId(e.target.value)}
          className={[
            'bg-(--t-surface) text-(--t-text)',
            'border border-(--t-border) rounded-none',
            'text-xs px-3 py-1.5',
            'focus:outline-none focus:border-(--t-border-active)',
            'transition-colors duration-100 cursor-pointer',
          ].join(' ')}
        >
          {TIMEZONES.map(tz => (
            <option key={tz.id} value={tz.id}>{tz.label}</option>
          ))}
        </select>
        <span className="text-[10px] text-(--t-text-dim) italic">
          // affects ambiguous input parsing and LOCAL output
        </span>
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
            placeholder="> e.g. 2024-11-14T22:13:20Z or &quot;2 days ago&quot;"
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
            // ISO 8601, RFC 2822, or natural language: "now", "yesterday", "2 days ago"
          </p>
        </TerminalPane>
      </div>

      {/* ── Convert ────────────────────────────────────────────── */}
      <div className="flex items-center gap-2">
        <Button variant="primary" disabled={!canConvert || loading} onClick={handleConvert}>
          CONVERT
        </Button>
        <Button variant="ghost" disabled={!unixInput && !humanInput} onClick={handleClear}>
          CLEAR
        </Button>
      </div>

      {/* ── Result pane ────────────────────────────────────────── */}
      <TerminalPane
        title="RESULT"
        badge={badge}
        bodyClassName="p-3"
        statusLeft={statusLeft}
      >
        <div className="border border-(--t-border) bg-(--t-surface-2) p-3 min-h-24">
          {renderResult()}
        </div>
      </TerminalPane>

    </div>
  )
}
