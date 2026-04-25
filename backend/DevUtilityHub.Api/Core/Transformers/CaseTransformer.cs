using System.Globalization;
using System.Text.RegularExpressions;

namespace DevUtilityHub.Api.Core.Transformers
{
	public class CaseTransformer
	{
		// Splits on whitespace/underscores/hyphens and camelCase boundaries
		private static readonly Regex WordSplitter =
			new(@"(?<=[a-z])(?=[A-Z])|(?<=[A-Z])(?=[A-Z][a-z])|[\s_\-]+", RegexOptions.Compiled);

		public string ToUpperCase(string input) =>
			ApplyPerLine(input, line => line.ToUpperInvariant());

		public string ToLowerCase(string input) =>
			ApplyPerLine(input, line => line.ToLowerInvariant());

		public string ToTitleCase(string input) =>
			ApplyPerLine(input, line =>
			{
				if (string.IsNullOrEmpty(line)) return line;
				return string.Join(" ", line.Split(' ').Select(word =>
					word.Length == 0 ? word : char.ToUpperInvariant(word[0]) + word[1..].ToLowerInvariant()));
			});

		public string ToCamelCase(string input) =>
			ApplyPerLine(input, line =>
			{
				var words = SplitWords(line);
				if (words.Length == 0) return line;
				return words[0].ToLowerInvariant() +
					string.Concat(words[1..].Select(w => w.Length == 0 ? w : char.ToUpperInvariant(w[0]) + w[1..].ToLowerInvariant()));
			});

		public string ToSnakeCase(string input) =>
			ApplyPerLine(input, line =>
				string.Join("_", SplitWords(line).Select(w => w.ToLowerInvariant())));

		public string ToKebabCase(string input) =>
			ApplyPerLine(input, line =>
				string.Join("-", SplitWords(line).Select(w => w.ToLowerInvariant())));

		private static string[] SplitWords(string line) =>
			WordSplitter.Split(line).Where(w => w.Length > 0).ToArray();

		private static string ApplyPerLine(string input, Func<string, string> transform) =>
			string.Join("\n", input.Split('\n').Select(transform));
	}
}
