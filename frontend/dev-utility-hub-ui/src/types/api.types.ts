//mirrors backend models

export interface FormatRequest { input: string; action: "format" | "minify" | "validate"; overrideType?: string; }
export interface FormatResponse { output: string; detectedType: string; isValid: boolean; errorMessage?: string; }

export interface EncodeRequest { input: string; type: "base64" | "url" | "html"; direction: "encode" | "decode"; }
export interface EncodeResponse { output: string; success: boolean; errorMessage?: string; }

export interface TextTransformRequest { input: string; operations: string[]; }
export interface TextTransformResponse { output: string; appliedOperations: string[]; isValid: boolean; errorMessage?: string; }

export interface DiffRequest { textA: string; textB: string; }
export interface DiffLine { type: "added" | "removed" | "unchanged"; content: string; lineNumber: number; }
export interface DiffResponse { lines: DiffLine[]; addedCount: number; removedCount: number; isValid: boolean; errorMessage?: string; }

export interface TimeRequest { direction: "toHuman" | "toUnix"; unixValue?: number; isMilliseconds?: boolean; humanValue?: string; }
export interface TimeResponse { humanReadable: string; unixSeconds: number; unixMilliseconds: number; utc: string; }