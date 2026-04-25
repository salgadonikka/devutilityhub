//loading / error / data state

import { useState } from "react";

export function useApi<T>() {
  const [data, setData] = useState<T | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  const call = async (fn: () => Promise<T>) => {
    setLoading(true);
    setError(null);
    try {
      const result = await fn();
      setData(result);
    } catch (e: any) {
      setError(e?.response?.data?.error ?? "Something went wrong");
    } finally {
      setLoading(false);
    }
  };

  const reset = () => { setData(null); setError(null); };

  return { data, loading, error, call, reset };
}