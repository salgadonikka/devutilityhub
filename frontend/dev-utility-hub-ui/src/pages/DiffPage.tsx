import { useState } from 'react'
import TerminalPane from '../components/common/TerminalPane'
import TextArea from '../components/common/TextArea'
import Button from '../components/common/Button'
import Loader from '../components/common/Loader'
import DiffViewer from '../components/diff/DiffViewer'
import { useApi } from '../hooks/useApi'
import { compareDiff } from '../api/diffApi'
import type { DiffResponse } from '../types/api.types'

export default function DiffPage() {
  const [textA, setTextA] = useState('')
  const [textB, setTextB] = useState('')
  const { data, loading, error, call } = useApi<DiffResponse>()

  const canCompare = textA.trim().length > 0 && textB.trim().length > 0

  const badge = data?.isValid ? `[${data.lines.length} lines]` : '[READY]'
  const statusLeft = data?.isValid
    ? `> added: ${data.addedCount}    removed: ${data.removedCount}    unchanged: ${data.lines.length - data.addedCount - data.removedCount}`
    : '> added: --    removed: --    unchanged: --'

  const renderResult = () => {
    if (loading) return <Loader />
    if (error) return <span className="text-(--t-error) text-xs">{error}</span>
    if (data && !data.isValid) return <span className="text-(--t-error) text-xs">{data.errorMessage ?? 'Something went wrong'}</span>
    return <DiffViewer lines={data?.lines ?? []} />
  }

  return (
    <div className="flex flex-col gap-5 animate-fade-in">

      {/* ── Shell heading ──────────────────────────────────────── */}
      <div>
        <div className="flex items-center gap-1.5 text-xs mb-3">
          <span className="text-(--t-secondary)">user@toolkit</span>
          <span className="text-(--t-text-dim)">:</span>
          <span className="text-(--t-primary) text-glow-sm">~/diff</span>
          <span className="text-(--t-text-dim)">$</span>
          <span className="text-(--t-text-dim) ml-2 italic">
            // text diff -- line-by-line comparison
          </span>
        </div>
        <div className="h-px bg-(--t-border)" />
      </div>

      {/* ── Side-by-side input panes ───────────────────────────── */}
      <div className="grid grid-cols-1 md:grid-cols-2 gap-4">

        <TerminalPane
          title="ORIGINAL (A)"
          badge={textA.length > 0 ? `${textA.split('\n').length} lines` : undefined}
          bodyClassName="p-3 flex flex-col gap-2"
        >
          <div className="flex items-center gap-1 text-[10px] text-(--t-text-dim)">
            <span className="text-(--t-secondary)">user@toolkit</span>
            <span>:</span>
            <span className="text-(--t-primary)">~/diff</span>
            <span className="text-(--t-secondary)"> [A]$</span>
          </div>
          <TextArea
            value={textA}
            onChange={(e) => setTextA(e.target.value)}
            placeholder="> paste original text here..."
            rows={12}
          />
        </TerminalPane>

        <TerminalPane
          title="MODIFIED (B)"
          badge={textB.length > 0 ? `${textB.split('\n').length} lines` : undefined}
          bodyClassName="p-3 flex flex-col gap-2"
        >
          <div className="flex items-center gap-1 text-[10px] text-(--t-text-dim)">
            <span className="text-(--t-secondary)">user@toolkit</span>
            <span>:</span>
            <span className="text-(--t-primary)">~/diff</span>
            <span className="text-(--t-secondary)"> [B]$</span>
          </div>
          <TextArea
            value={textB}
            onChange={(e) => setTextB(e.target.value)}
            placeholder="> paste modified text here..."
            rows={12}
          />
        </TerminalPane>
      </div>

      {/* ── Compare button ─────────────────────────────────────── */}
      <div className="flex items-center gap-2">
        <Button variant="primary" disabled={!canCompare || loading} onClick={() => call(() => compareDiff({ textA, textB }))}>
          COMPARE
        </Button>
        <Button
          variant="ghost"
          disabled={!textA && !textB}
          onClick={() => { setTextA(''); setTextB('') }}
        >
          CLEAR ALL
        </Button>
        <div className="flex-1" />
        <span className="text-[10px] text-(--t-text-dim) italic">
          {canCompare ? '> ready to compare' : '> fill both panes to compare'}
        </span>
      </div>

      {/* ── Diff output ────────────────────────────────────────── */}
      <TerminalPane
        title="DIFF RESULT"
        badge={badge}
        bodyClassName="p-3"
        statusLeft={statusLeft}
      >
        {/* Diff legend */}
        <div className="flex items-center gap-4 mb-3 text-[10px]">
          <span className="flex items-center gap-1.5">
            <span className="w-2 h-2 bg-(--t-secondary) inline-block" />
            <span className="text-(--t-text-dim)">ADDED</span>
          </span>
          <span className="flex items-center gap-1.5">
            <span className="w-2 h-2 bg-(--t-error) inline-block" />
            <span className="text-(--t-text-dim)">REMOVED</span>
          </span>
          <span className="flex items-center gap-1.5">
            <span className="w-2 h-2 bg-(--t-border) inline-block" />
            <span className="text-(--t-text-dim)">UNCHANGED</span>
          </span>
        </div>

        <div
          className={[
            'min-h-40 text-xs leading-relaxed font-mono',
            'border border-(--t-border) bg-(--t-surface-2) p-3',
          ].join(' ')}
        >
          {renderResult()}
        </div>
      </TerminalPane>

    </div>
  )
}
