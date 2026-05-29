import { createContext, useState, useCallback, useEffect, type ReactNode } from 'react'
import axios from 'axios'
import { checkHealth } from '../api/healthApi'

export type ApiStatus = 'idle' | 'checking' | 'ok' | 'network-error' | 'server-error'

interface ApiStatusContextValue {
  apiStatus: ApiStatus
  setApiStatus: (status: ApiStatus) => void
  checkConnection: () => Promise<void>
}

export const ApiStatusContext = createContext<ApiStatusContextValue>({
  apiStatus: 'idle',
  setApiStatus: () => {},
  checkConnection: async () => {},
})

export function ApiStatusProvider({ children }: { children: ReactNode }) {
  const [apiStatus, setApiStatus] = useState<ApiStatus>('idle')

  const checkConnection = useCallback(async () => {
    setApiStatus('checking')
    try {
      await checkHealth()
      setApiStatus('ok')
    } catch (e) {
      if (axios.isAxiosError(e) && !e.response) {
        setApiStatus('network-error')
      } else {
        setApiStatus('server-error')
      }
    }
  }, [])

  useEffect(() => { checkConnection() }, [checkConnection])

  return (
    <ApiStatusContext.Provider value={{ apiStatus, setApiStatus, checkConnection }}>
      {children}
    </ApiStatusContext.Provider>
  )
}
