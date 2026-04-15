import Button from './Button'

interface ToolbarAction {
  label: string
  onClick: () => void
  disabled?: boolean
}

interface ToolbarProps {
  actions: ToolbarAction[]
}

export default function Toolbar({ actions }: ToolbarProps) {
  return (
    <div className="flex items-center gap-2 flex-wrap">
      {actions.map(({ label, onClick, disabled }) => (
        <Button
          key={label}
          size="sm"
          variant="ghost"
          onClick={onClick}
          disabled={disabled}
        >
          {label}
        </Button>
      ))}
    </div>
  )
}
