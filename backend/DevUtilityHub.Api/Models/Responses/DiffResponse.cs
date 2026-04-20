namespace DevUtilityHub.Api.Models.Responses
{
	public class DiffLine
	{
		public string Type { get; set; } = "unchanged"; // added | removed | unchanged
		public string Content { get; set; } = string.Empty;
		public int LineNumber { get; set; }
	}

	public class DiffResponse
	{
		public List<DiffLine> Lines { get; set; } = new();
		public int AddedCount { get; set; }
		public int RemovedCount { get; set; }
		public bool IsValid { get; set; }
		public string? ErrorMessage { get; set; }
	}
}
