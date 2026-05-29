import { useContext } from 'react'
import { NavLink, Link } from 'react-router-dom'
import { ApiStatusContext, type ApiStatus } from '../../context/ApiStatusContext'

const tools = [
  { label: 'FORMATTER',  path: '/formatter', cmd: '{ }' },
  { label: 'ENCODE',     path: '/encode',    cmd: ' 64' },
  { label: 'TEXT-TOOLS', path: '/text',      cmd: ' Aa' },
  { label: 'DIFF',       path: '/diff',      cmd: '  ±' },
  { label: 'TIMESTAMP',  path: '/time',      cmd: ' ⏱' },
]

const statusConfig: Record<ApiStatus, { label: string; dotClass: string }> = {
  idle:            { label: 'UNKNOWN',  dotClass: 'text-(--t-text-dim)' },
  checking:        { label: 'CHECKING', dotClass: 'text-(--t-text-dim) animate-blink' },
  ok:              { label: 'ONLINE',   dotClass: 'text-(--t-secondary)' },
  'network-error': { label: 'OFFLINE',  dotClass: 'text-(--t-error)' },
  'server-error':  { label: 'API ERR',  dotClass: 'text-(--t-error)' },
}

interface SidebarProps {
  collapsed: boolean
  onToggle: () => void
}

export default function Sidebar({ collapsed, onToggle }: SidebarProps) {
  const { apiStatus, checkConnection } = useContext(ApiStatusContext)
  const { label, dotClass } = statusConfig[apiStatus]

  return (
    <aside
      className={[
        'shrink-0 flex flex-col border-r border-(--t-border) bg-(--t-surface)',
        'overflow-hidden transition-[width] duration-200',
        collapsed ? 'w-12' : 'w-52',
      ].join(' ')}
    >

      {/* ── Logo ─────────────────────────────────────────────────── */}
      <Link
        to="/"
        className="px-3 py-4 border-b border-(--t-border) shrink-0 flex items-center hover:bg-(--t-surface-2) transition-colors duration-100 overflow-hidden"
      >
        <span className="text-(--t-secondary) text-sm font-bold leading-none select-none shrink-0">
          &gt;
        </span>
        {!collapsed && (
          <span className="ml-2 text-xs font-bold uppercase tracking-widest text-(--t-text-bright) text-glow cursor-block leading-none whitespace-nowrap">
            DEVUTILITYHUB
          </span>
        )}
      </Link>

      {/* ── Navigation ───────────────────────────────────────────── */}
      <nav className="flex-1 py-2 overflow-y-auto overflow-x-hidden">
        {tools.map(({ label: toolLabel, path, cmd }) => (
          <NavLink key={path} to={path}>
            {({ isActive }) => (
              <div
                className={[
                  'flex items-center py-2.5 text-xs uppercase tracking-wider',
                  'transition-colors duration-100 overflow-hidden',
                  collapsed ? 'justify-center px-0' : 'gap-2.5 px-3',
                  isActive
                    ? 'bg-(--t-primary) text-(--t-inv-text)'
                    : 'text-(--t-text) hover:text-(--t-text-bright) hover:bg-(--t-surface-2)',
                ].join(' ')}
              >
                {!collapsed && (
                  <span className="w-3 shrink-0 font-bold leading-none select-none" aria-hidden>
                    {isActive ? '>' : ' '}
                  </span>
                )}
                <span className="w-6 shrink-0 text-center font-mono leading-none">{cmd}</span>
                {!collapsed && <span className="whitespace-nowrap">{toolLabel}</span>}
              </div>
            )}
          </NavLink>
        ))}
      </nav>

      {/* ── API Status ───────────────────────────────────────────── */}
      <div className="border-t border-(--t-border) shrink-0">
        {collapsed ? (
          <button
            onClick={checkConnection}
            title={`API: ${label}`}
            className="w-full flex items-center justify-center py-3 hover:bg-(--t-surface-2) transition-colors duration-100 focus:outline-none"
          >
            <span className={['text-sm leading-none', dotClass].join(' ')}>●</span>
          </button>
        ) : (
          <div className="px-3 py-3">
            <p className="text-[10px] text-(--t-text-dim) uppercase tracking-widest mb-2 leading-none">
              API STATUS
            </p>
            <div className="flex items-center justify-between gap-2">
              <span className={['text-xs flex items-center gap-1.5 leading-none', dotClass].join(' ')}>
                ● {label}
              </span>
              <button
                onClick={checkConnection}
                className={[
                  'text-[10px] uppercase tracking-wider leading-none shrink-0',
                  'border border-(--t-border) px-2 py-1',
                  'text-(--t-text-dim) transition-colors duration-100',
                  'hover:border-(--t-muted) hover:text-(--t-text)',
                  'focus:outline-none focus-visible:ring-1 focus-visible:ring-(--t-primary)',
                ].join(' ')}
              >
                CHECK
              </button>
            </div>
          </div>
        )}
      </div>

      {/* ── Footer ───────────────────────────────────────────────── */}
      {!collapsed && (
        <div className="px-3 py-3 border-t border-(--t-border) shrink-0">
          <p className="text-[10px] text-(--t-text-dim) leading-relaxed">user@toolkit:~$</p>
          <p className="text-[10px] text-(--t-text-dim) truncate leading-relaxed">
            toolkit.nikkapaola.com
          </p>
        </div>
      )}

      {/* ── Collapse toggle ──────────────────────────────────────── */}
      <button
        onClick={onToggle}
        aria-label={collapsed ? 'Expand sidebar' : 'Collapse sidebar'}
        className={[
          'w-full py-2 border-t border-(--t-border) shrink-0',
          'text-xs text-(--t-text-dim) hover:text-(--t-text)',
          'hover:bg-(--t-surface-2) transition-colors duration-100',
          'focus:outline-none focus-visible:ring-1 focus-visible:ring-(--t-primary)',
        ].join(' ')}
      >
        {collapsed ? '›' : '‹'}
      </button>

    </aside>
  )
}
