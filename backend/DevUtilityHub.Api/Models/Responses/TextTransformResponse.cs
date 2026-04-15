namespace DevUtilityHub.Api.Models.Responses
{
	public class TextTransformResponse
	{
		public string Output { get; set; } = string.Empty;
		public List<string> AppliedOperations { get; set; } = new();
	}
}
