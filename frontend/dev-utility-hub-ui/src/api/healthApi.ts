import { apiClient } from './client'

export async function checkHealth(): Promise<void> {
  await apiClient.get('/health')
}
