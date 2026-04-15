namespace DevUtilityHub.Api.Core.Detectors
{
	public class InputTypeDetector
	{
		public static string Detect(string input)
		{
			var trimmed = input.TrimStart();
			if (trimmed.StartsWith('{') || trimmed.StartsWith('['))
				return "json";
			if (trimmed.StartsWith('<'))
				return "xml";
			return "plain";
		}
	}
}
