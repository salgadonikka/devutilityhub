using DevUtilityHub.Api.Core.Diff;

namespace DevUtilityHub.Tests.Core.Diff
{
	public class TextDiffEngineTests
	{
		private readonly TextDiffEngine _engine = new();

		// -------------------------------------------------------------------------
		// Unchanged lines
		// -------------------------------------------------------------------------

		[Fact]
		public void Compute_IdenticalSingleLine_ReturnsOneUnchangedLine()
		{
			var (lines, added, removed) = _engine.Compute("hello", "hello");

			Assert.Single(lines);
			Assert.Equal("unchanged", lines[0].Type);
			Assert.Equal("hello", lines[0].Content);
			Assert.Equal(0, added);
			Assert.Equal(0, removed);
		}

		[Fact]
		public void Compute_IdenticalMultiLineText_ReturnsAllUnchangedLines()
		{
			var text = "line one\nline two\nline three";

			var (lines, added, removed) = _engine.Compute(text, text);

			Assert.Equal(3, lines.Count);
			Assert.All(lines, l => Assert.Equal("unchanged", l.Type));
			Assert.Equal(0, added);
			Assert.Equal(0, removed);
		}

		// -------------------------------------------------------------------------
		// Added lines
		// -------------------------------------------------------------------------

		[Fact]
		public void Compute_LineAddedInB_ReturnsAddedLineWithCountOne()
		{
			var (lines, added, removed) = _engine.Compute("first", "first\nnew line");

			Assert.Equal(1, added);
			Assert.Equal(0, removed);
			Assert.Contains(lines, l => l.Type == "added" && l.Content == "new line");
		}

		[Fact]
		public void Compute_EmptyTextANonEmptyTextB_ReturnsAllLinesAsAdded()
		{
			var (lines, added, removed) = _engine.Compute("", "alpha\nbeta\ngamma");

			Assert.Equal(3, added);
			Assert.Equal(0, removed);
			Assert.All(lines, l => Assert.Equal("added", l.Type));
		}

		// -------------------------------------------------------------------------
		// Removed lines
		// -------------------------------------------------------------------------

		[Fact]
		public void Compute_LineRemovedInB_ReturnsRemovedLineWithCountOne()
		{
			var (lines, added, removed) = _engine.Compute("keep\nremove me", "keep");

			Assert.Equal(0, added);
			Assert.Equal(1, removed);
			Assert.Contains(lines, l => l.Type == "removed" && l.Content == "remove me");
		}

		[Fact]
		public void Compute_NonEmptyTextAEmptyTextB_ReturnsAllLinesAsRemoved()
		{
			var (lines, added, removed) = _engine.Compute("alpha\nbeta\ngamma", "");

			Assert.Equal(0, added);
			Assert.Equal(3, removed);
			Assert.All(lines, l => Assert.Equal("removed", l.Type));
		}

		// -------------------------------------------------------------------------
		// Mixed changes
		// -------------------------------------------------------------------------

		[Fact]
		public void Compute_OneLineChangedToAnother_ReturnsOneRemovedAndOneAdded()
		{
			var (lines, added, removed) = _engine.Compute("hello world", "hello earth");

			Assert.Equal(1, added);
			Assert.Equal(1, removed);
			Assert.Contains(lines, l => l.Type == "removed" && l.Content == "hello world");
			Assert.Contains(lines, l => l.Type == "added" && l.Content == "hello earth");
		}

		[Fact]
		public void Compute_PartialOverlapTexts_ReturnsCorrectMixOfLineTypes()
		{
			var textA = "alpha\nbeta\ngamma";
			var textB = "alpha\ndelta\ngamma";

			var (lines, added, removed) = _engine.Compute(textA, textB);

			Assert.Equal(1, added);
			Assert.Equal(1, removed);
			Assert.Contains(lines, l => l.Type == "unchanged" && l.Content == "alpha");
			Assert.Contains(lines, l => l.Type == "unchanged" && l.Content == "gamma");
			Assert.Contains(lines, l => l.Type == "removed" && l.Content == "beta");
			Assert.Contains(lines, l => l.Type == "added" && l.Content == "delta");
		}

		// -------------------------------------------------------------------------
		// Line numbers
		// -------------------------------------------------------------------------

		[Fact]
		public void Compute_MultiLineResult_LineNumbersStartAtOneAndAreSequential()
		{
			var (lines, _, _) = _engine.Compute("a\nb\nc", "a\nx\nc");

			for (int i = 0; i < lines.Count; i++)
				Assert.Equal(i + 1, lines[i].LineNumber);
		}

		[Fact]
		public void Compute_SingleLine_LineNumberIsOne()
		{
			var (lines, _, _) = _engine.Compute("only", "only");

			Assert.Equal(1, lines[0].LineNumber);
		}

		// -------------------------------------------------------------------------
		// Counts
		// -------------------------------------------------------------------------

		[Fact]
		public void Compute_MultipleAdditionsAndRemovals_CountsMatchActualLines()
		{
			var textA = "one\ntwo\nthree";
			var textB = "one\nalpha\nbeta\nthree";

			var (lines, added, removed) = _engine.Compute(textA, textB);

			Assert.Equal(added, lines.Count(l => l.Type == "added"));
			Assert.Equal(removed, lines.Count(l => l.Type == "removed"));
		}
	}
}
