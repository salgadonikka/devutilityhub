export default function Loader({ label = 'PROCESSING' }: { label?: string }) {
  return (
    <div className="flex items-center gap-2 text-xs text-(--t-text-dim)">
      <span className="text-(--t-primary)">&gt;</span>
      <span className="uppercase tracking-wider">{label}</span>
      <span className="opacity-50">...</span>
      <span className="text-(--t-primary) animate-blink text-sm leading-none">
        █
      </span>
    </div>
  )
}
