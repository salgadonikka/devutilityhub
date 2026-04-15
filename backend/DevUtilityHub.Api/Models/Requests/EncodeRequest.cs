namespace DevUtilityHub.Api.Models.Requests
{
	public class EncodeRequest
	{
		public string Input { get; set; } = string.Empty;
		public string Type { get; set; } = "base64";  // base64 | url | html
		public string Direction { get; set; } = "encode"; // encode | decode
	}
}
