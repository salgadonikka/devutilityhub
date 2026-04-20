using DevUtilityHub.Api.Models.Responses;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;

namespace DevUtilityHub.Api.Core.Diff
{
	public class TextDiffEngine
	{
		private readonly InlineDiffBuilder _builder = new(new Differ());

		public (List<DiffLine> lines, int addedCount, int removedCount) Compute(string textA, string textB)
		{
			var diff = _builder.BuildDiffModel(textA, textB);
			var lines = new List<DiffLine>();
			int added = 0, removed = 0, lineNumber = 1;

			foreach (var piece in diff.Lines)
			{
				if (piece.Type == ChangeType.Imaginary) continue;

				var type = piece.Type switch
				{
					ChangeType.Inserted => "added",
					ChangeType.Deleted  => "removed",
					_                   => "unchanged"
				};

				lines.Add(new DiffLine { Type = type, Content = piece.Text ?? string.Empty, LineNumber = lineNumber++ });
				if (type == "added")   added++;
				if (type == "removed") removed++;
			}

			return (lines, added, removed);
		}
	}
}
