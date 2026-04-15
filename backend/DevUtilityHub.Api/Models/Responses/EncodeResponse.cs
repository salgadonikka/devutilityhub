namespace DevUtilityHub.Api.Models.Responses
{
	public class EncodeResponse
	{
		public string Output { get; set; } = string.Empty;
		public bool Success { get; set; }
		public string? ErrorMessage { get; set; }
	}
}
