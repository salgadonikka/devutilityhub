namespace DevUtilityHub.Api.Core.Transformers
{
	public class CleanupTransformer
	{
		public string Trim(string input) =>
			string.Join("\n", input.Split('\n').Select(line => line.Trim()));
	}
}
