using System.Text.Json;

namespace DevUtilityHub.Api.Core.Formatters
{
	public class JsonFormatter
	{
		public string Format(string input)
		{
			var doc = JsonDocument.Parse(input);
			return JsonSerializer.Serialize(doc, new JsonSerializerOptions { WriteIndented = true });
		}

		public string Minify(string input)
		{
			var doc = JsonDocument.Parse(input);
			return JsonSerializer.Serialize(doc);
		}

		public (bool IsValid, string? Error) Validate(string input)
		{
			try
			{
				JsonDocument.Parse(input);
				return (true, null);
			}
			catch (JsonException ex)
			{
				return (false, ex.Message);
			}
		}
	}
}
