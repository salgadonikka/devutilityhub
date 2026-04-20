import type { ButtonHTMLAttributes } from 'react'

type Variant = 'primary' | 'secondary' | 'ghost' | 'danger'
type Size = 'sm' | 'md'

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
  variant?: Variant
  size?: Size
}

const variantClasses: Record<Variant, string> = {
  primary: [
    'bg-(--t-primary) text-(--t-inv-text) border border-(--t-primary)',
    'hover:opacity-80',
  ].join(' '),
  secondary: [
    'bg-transparent text-(--t-text) border border-(--t-border)',
    'hover:bg-(--t-primary) hover:text-(--t-inv-text) hover:border-(--t-primary)',
  ].join(' '),
  ghost: [
    'bg-transparent text-(--t-text-dim) border border-transparent',
    'hover:text-(--t-text) hover:border-(--t-border)',
  ].join(' '),
  danger: [
    'bg-transparent text-(--t-error) border border-(--t-error)/40',
    'hover:bg-(--t-error) hover:text-(--t-bg) hover:border-(--t-error)',
  ].join(' '),
}

const sizeClasses: Record<Size, string> = {
  sm: 'px-2 py-0.5 text-[11px]',
  md: 'px-3 py-1 text-xs',
}

export default function Button({
  variant = 'secondary',
  size = 'md',
  className = '',
  children,
  ...props
}: ButtonProps) {
  return (
    <button
      className={[
        'inline-flex items-center uppercase tracking-wider leading-none',
        'transition-colors duration-100 cursor-pointer',
        'disabled:cursor-not-allowed disabled:opacity-40',
        'focus:outline-none focus-visible:ring-1 focus-visible:ring-(--t-primary)',
        variantClasses[variant],
        sizeClasses[size],
        className,
      ].join(' ')}
      {...props}
    >
      {/* Brackets are aria-hidden so screen readers only announce the label */}
      <span aria-hidden className="opacity-50 select-none">[</span>
      <span className="mx-1.5">{children}</span>
      <span aria-hidden className="opacity-50 select-none">]</span>
    </button>
  )
}
