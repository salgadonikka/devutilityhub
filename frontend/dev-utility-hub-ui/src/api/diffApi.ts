import { apiClient } from "./client";
import type { DiffRequest, DiffResponse } from "../types/api.types";

export async function compareDiff(req: DiffRequest): Promise<DiffResponse> {
  const { data } = await apiClient.post<DiffResponse>("/diff/compare", req);
  return data;
}
