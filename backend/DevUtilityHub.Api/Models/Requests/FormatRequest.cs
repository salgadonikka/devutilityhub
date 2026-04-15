namespace DevUtilityHub.Api.Models.Requests
{
	public class FormatRequest
	{
		public string Input { get; set; } = string.Empty;
		public string Action { get; set; } = "format"; // format | minify | validate
		public string? OverrideType { get; set; }       // json | xml | plain (optional)
	}
}
