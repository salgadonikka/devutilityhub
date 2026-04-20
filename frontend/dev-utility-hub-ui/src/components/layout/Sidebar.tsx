import { NavLink, Link } from 'react-router-dom'

const tools = [
  { label: 'FORMATTER',  path: '/formatter', cmd: '{ }' },
  { label: 'ENCODE',     path: '/encode',    cmd: ' 64' },
  { label: 'TEXT-TOOLS', path: '/text',      cmd: ' Aa' },
  { label: 'DIFF',       path: '/diff',      cmd: '  ±' },
  { label: 'TIMESTAMP',  path: '/time',      cmd: ' ⏱' },
]

export default function Sidebar() {
  return (
    <aside className="w-52 shrink-0 flex flex-col border-r border-(--t-border) bg-(--t-surface)">

      {/* ── Logo ─────────────────────────────────────────────────── */}
      <Link to="/" className="px-4 py-4 border-b border-(--t-border) shrink-0 block hover:bg-(--t-surface-2) transition-colors duration-100">
        <div className="flex items-center gap-2">
          <span className="text-(--t-secondary) text-sm font-bold leading-none select-none">
            &gt;
          </span>
          <span className="text-xs font-bold uppercase tracking-widest text-(--t-text-bright) text-glow cursor-block leading-none">
            DEVUTILITYHUB
          </span>
        </div>
        <p className="text-[10px] text-(--t-text-dim) mt-1.5 pl-4 leading-none">
          v1.0.0-alpha
        </p>
      </Link>

      {/* ── Navigation ───────────────────────────────────────────── */}
      <nav className="flex-1 py-2 overflow-y-auto">
        {tools.map(({ label, path, cmd }) => (
          <NavLink key={path} to={path}>
            {({ isActive }) => (
              <div
                className={[
                  'flex items-center gap-2.5 px-3 py-2.5 text-xs uppercase tracking-wider',
                  'transition-colors duration-100',
                  isActive
                    ? 'bg-(--t-primary) text-(--t-inv-text)'
                    : 'text-(--t-text) hover:text-(--t-text-bright) hover:bg-(--t-surface-2)',
                ].join(' ')}
              >
                {/* Active indicator */}
                <span
                  className="w-3 shrink-0 font-bold leading-none select-none"
                  aria-hidden
                >
                  {isActive ? '>' : ' '}
                </span>

                {/* Command symbol */}
                <span className="w-6 shrink-0 text-center font-mono leading-none">
                  {cmd}
                </span>

                {/* Label */}
                <span>{label}</span>
              </div>
            )}
          </NavLink>
        ))}
      </nav>

      {/* ── Footer ───────────────────────────────────────────────── */}
      <div className="px-4 py-3 border-t border-(--t-border) shrink-0">
        <p className="text-[10px] text-(--t-text-dim) leading-relaxed">
          user@toolkit:~$
        </p>
        <p className="text-[10px] text-(--t-text-dim) truncate leading-relaxed">
          toolkit.nikkapaola.com
        </p>
      </div>
    </aside>
  )
}
