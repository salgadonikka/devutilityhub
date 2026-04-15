namespace DevUtilityHub.Api.Models.Requests
{
	public class TextTransformRequest
	{
		public string Input { get; set; } = string.Empty;
		// Operations applied in order (pipeline)
		public List<string> Operations { get; set; } = new();
		// e.g. ["trim", "removeExtraSpaces", "camelCase", "sortAsc"]
	}
}
