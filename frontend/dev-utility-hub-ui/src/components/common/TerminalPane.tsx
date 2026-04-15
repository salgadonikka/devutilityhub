import type { ReactNode } from 'react'

interface TerminalPaneProps {
  title: string
  badge?: string
  statusLeft?: string
  statusRight?: string
  children: ReactNode
  className?: string
  bodyClassName?: string
}

export default function TerminalPane({
  title,
  badge,
  statusLeft,
  statusRight,
  children,
  className = '',
  bodyClassName = '',
}: TerminalPaneProps) {
  const hasStatus = statusLeft !== undefined || statusRight !== undefined

  return (
    <div
      className={['border border-(--t-border) flex flex-col', className].join(
        ' ',
      )}
    >
      {/* ── Title bar (inverted) ─────────────────────────────────── */}
      <div className="flex items-center justify-between px-3 py-1.5 bg-(--t-primary) text-(--t-inv-text) shrink-0">
        <span className="text-[10px] uppercase tracking-widest font-bold leading-none">
          ■ {title}
        </span>
        {badge && (
          <span className="text-[10px] opacity-75 leading-none">{badge}</span>
        )}
      </div>

      {/* ── Body ─────────────────────────────────────────────────── */}
      <div className={['flex-1', bodyClassName].join(' ')}>{children}</div>

      {/* ── Status bar ───────────────────────────────────────────── */}
      {hasStatus && (
        <div className="flex items-center justify-between px-3 py-1 border-t border-(--t-border) bg-(--t-surface) text-[10px] text-(--t-text-dim) shrink-0 leading-none">
          <span>{statusLeft ?? ''}</span>
          <span>{statusRight ?? ''}</span>
        </div>
      )}
    </div>
  )
}
