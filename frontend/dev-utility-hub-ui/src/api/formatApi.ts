import { apiClient } from "./client";
import type { FormatRequest, FormatResponse } from "../types/api.types";

export async function processFormat(req: FormatRequest): Promise<FormatResponse> {
  const { data } = await apiClient.post<FormatResponse>("/format/process", req);
  return data;
}
