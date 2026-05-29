import { useState, useContext } from 'react'
import axios from 'axios'
import { ApiStatusContext } from '../context/ApiStatusContext'

export function useApi<T>() {
  const [data, setData] = useState<T | null>(null)
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState<string | null>(null)
  const { setApiStatus } = useContext(ApiStatusContext)

  const call = async (fn: () => Promise<T>) => {
    setLoading(true)
    setError(null)
    try {
      const result = await fn()
      setData(result)
      setApiStatus('ok')
    } catch (e: any) {
      if (axios.isAxiosError(e) && !e.response) {
        setError('Cannot reach API — is the server running?')
        setApiStatus('network-error')
      } else if ((e?.response?.status ?? 0) >= 500) {
        setError('Server error — try again')
        setApiStatus('server-error')
      } else {
        setError(e?.response?.data?.error ?? 'Something went wrong')
        setApiStatus('ok')
      }
    } finally {
      setLoading(false)
    }
  }

  const reset = () => { setData(null); setError(null) }

  return { data, loading, error, call, reset }
}
