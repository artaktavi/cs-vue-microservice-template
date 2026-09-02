export interface ServiceStatus {
  service: string
  status: string
}

export const getServiceStatus = async (): Promise<ServiceStatus> => {
  const response = await fetch('/api/app/status')
  if (!response.ok) {
    throw new Error(`Service request failed with status ${response.status}`)
  }
  return (await response.json()) as ServiceStatus
}

