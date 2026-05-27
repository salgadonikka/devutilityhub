import { apiClient } from "./client";
import type { TimeRequest, TimeResponse } from "../types/api.types";

export async function convertTimestamp(req: TimeRequest): Promise<TimeResponse> {
  const { data } = await apiClient.post<TimeResponse>("/time/convert", req);
  return data;
}
