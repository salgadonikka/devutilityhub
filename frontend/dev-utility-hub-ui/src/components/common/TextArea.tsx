import type { TextareaHTMLAttributes } from 'react'

interface TextAreaProps extends TextareaHTMLAttributes<HTMLTextAreaElement> {
  label?: string
}

export default function TextArea({
  label,
  className = '',
  ...props
}: TextAreaProps) {
  return (
    <div className="flex flex-col gap-1.5 w-full">
      {label && (
        <span className="text-[10px] uppercase tracking-widest text-(--t-text-dim)">
          {label}
        </span>
      )}
      <textarea
        className={[
          'w-full bg-(--t-surface) text-(--t-text)',
          'border border-(--t-border) rounded-none',
          'text-xs px-3 py-2.5 resize-y leading-relaxed',
          'placeholder:text-(--t-text-dim) placeholder:opacity-40',
          'focus:outline-none focus:border-(--t-border-active)',
          'transition-colors duration-100',
          className,
        ].join(' ')}
        {...props}
      />
    </div>
  )
}
