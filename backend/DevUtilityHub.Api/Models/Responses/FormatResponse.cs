namespace DevUtilityHub.Api.Models.Responses
{
	public class FormatResponse
	{
		public string Output { get; set; } = string.Empty;
		public string DetectedType { get; set; } = string.Empty;
		public bool IsValid { get; set; }
		public string? ErrorMessage { get; set; }
	}
}
