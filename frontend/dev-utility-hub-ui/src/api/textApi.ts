import { apiClient } from "./client";
import type { TextTransformRequest, TextTransformResponse } from "../types/api.types";

export async function transformText(req: TextTransformRequest): Promise<TextTransformResponse> {
  const { data } = await apiClient.post<TextTransformResponse>("/text/transform", req);
  return data;
}
