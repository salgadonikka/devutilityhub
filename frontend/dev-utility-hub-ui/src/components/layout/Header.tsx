import { useLocation } from 'react-router-dom'
import { useTheme } from '../../context/ThemeContext'

const shellPaths: Record<string, string> = {
  '/':          '~',
  '/formatter': '~/formatter',
  '/encode':    '~/encode',
  '/text':      '~/text-tools',
  '/diff':      '~/diff',
  '/time':      '~/timestamp',
}

export default function Header() {
  const { pathname } = useLocation()
  const { theme, toggle } = useTheme()

  const shellPath = shellPaths[pathname] ?? '~'

  return (
    <header className="h-10 shrink-0 flex items-center justify-between px-4 border-b border-(--t-border) bg-(--t-surface)">
      {/* Shell prompt path */}
      <div className="flex items-center gap-1 text-xs select-none">
        <span className="text-(--t-secondary)">user@toolkit</span>
        <span className="text-(--t-text-dim)">:</span>
        <span className="text-(--t-primary) text-glow-sm">{shellPath}</span>
        <span className="text-(--t-text-dim)">$</span>
        <span className="animate-blink text-(--t-primary) ml-0.5 text-[11px] leading-none">
          █
        </span>
      </div>

      {/* Theme toggle */}
      <button
        onClick={toggle}
        aria-label={`Switch to ${theme === 'dark' ? 'light' : 'dark'} mode`}
        className={[
          'text-[10px] uppercase tracking-wider leading-none',
          'border border-(--t-border) px-2 py-1',
          'text-(--t-text-dim) transition-colors duration-100',
          'hover:border-(--t-muted) hover:text-(--t-text)',
          'focus:outline-none focus-visible:ring-1 focus-visible:ring-(--t-primary)',
        ].join(' ')}
      >
        <span aria-hidden className="opacity-50">[</span>
        {' '}
        {theme === 'dark' ? '☀ LIGHT' : '☾ DARK'}
        {' '}
        <span aria-hidden className="opacity-50">]</span>
      </button>
    </header>
  )
}
