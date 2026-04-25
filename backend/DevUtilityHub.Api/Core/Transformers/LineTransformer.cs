using System.Text.RegularExpressions;

namespace DevUtilityHub.Api.Core.Transformers
{
	public class LineTransformer
	{
		public string Sort(string input)
		{
			var lines = input.Split('\n');
			return string.Join("\n", lines.OrderBy(l => l, StringComparer.Ordinal));
		}

		public string Dedup(string input)
		{
			var lines = input.Split('\n');
			return string.Join("\n", lines.Distinct());
		}

		public string Reverse(string input)
		{
			var lines = input.Split('\n');
			return string.Join("\n", lines.Reverse());
		}

		public string Count(string input)
		{
			var lines = input.Split('\n');
			var wordCount = lines
				.Where(l => !string.IsNullOrWhiteSpace(l))
				.Sum(l => Regex.Split(l.Trim(), @"\s+").Length);
			return $"Lines: {lines.Length} | Words: {wordCount} | Characters: {input.Length}";
		}
	}
}
