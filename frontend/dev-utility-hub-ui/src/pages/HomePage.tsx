import { useNavigate } from 'react-router-dom'
import Button from '../components/common/Button'

const tools = [
  {
    symbol: '{ }',
    label: 'FORMATTER',
    path: '/formatter',
    desc: 'Prettify, minify, and validate JSON & XML input.',
  },
  {
    symbol: 'b64',
    label: 'ENCODE',
    path: '/encode',
    desc: 'Base64 encoding and decoding with URL-safe output.',
  },
  {
    symbol: ' Aa',
    label: 'TEXT TOOLS',
    path: '/text',
    desc: 'Case conversion, whitespace cleanup, and line operations.',
  },
  {
    symbol: ' ±',
    label: 'DIFF',
    path: '/diff',
    desc: 'Line-by-line comparison between two blocks of text.',
  },
  {
    symbol: 'ts',
    label: 'TIMESTAMP',
    path: '/time',
    desc: 'Convert Unix timestamps to human-readable time and back.',
  },
]

export default function HomePage() {
  const navigate = useNavigate()

  return (
    <div className="flex flex-col gap-8 animate-fade-in">

      {/* ── Boot sequence ──────────────────────────────────────────── */}
      <div className="flex flex-col gap-1 font-mono text-sm">
        <div className="flex items-center gap-2">
          <span className="text-(--t-secondary)">user@toolkit</span>
          <span className="text-(--t-text-dim)">:~$</span>
          <span className="text-(--t-text)">./devutilityhub --init</span>
        </div>
        <div className="flex flex-col gap-0.5 pl-4 text-(--t-text-dim)">
          <span>
            &gt; DEVUTILITYHUB{' '}
            <span className="text-(--t-text)">v1.0.0-alpha</span>
          </span>
          <span>
            &gt; scanning modules
            <span className="text-(--t-text-dim)">........</span>
            {'  '}
            <span className="text-(--t-secondary)">[5/5] OK</span>
          </span>
          <span>
            &gt; environment:{' '}
            <span className="text-(--t-primary) text-glow-sm">production</span>
          </span>
          <span className="text-(--t-text) cursor-block">
            &gt; select a tool to begin
          </span>
        </div>
      </div>

      <div className="h-px bg-(--t-border)" />

      {/* ── Tool cards ─────────────────────────────────────────────── */}
      <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
        {tools.map(({ symbol, label, path, desc }) => (
          <div
            key={path}
            className="border border-(--t-border) flex flex-col hover:border-(--t-border-active) transition-colors duration-150"
          >
            {/* Title bar */}
            <div className="flex items-center px-3 py-1.5 bg-(--t-primary) text-(--t-inv-text) shrink-0">
              <span className="text-xs uppercase tracking-widest font-bold leading-none">
                ■ {label}
              </span>
            </div>

            {/* Body */}
            <div className="flex flex-col gap-5 p-5 flex-1">
              {/* Symbol */}
              <span
                className="text-5xl font-bold leading-none tracking-tight select-none text-(--t-primary) text-glow"
                aria-hidden
              >
                {symbol}
              </span>

              {/* Description */}
              <p className="text-sm text-(--t-text-dim) leading-relaxed flex-1">
                {desc}
              </p>

              {/* Launch */}
              <div>
                <Button variant="primary" onClick={() => navigate(path)}>
                  LAUNCH &rarr;
                </Button>
              </div>
            </div>
          </div>
        ))}
      </div>

    </div>
  )
}
