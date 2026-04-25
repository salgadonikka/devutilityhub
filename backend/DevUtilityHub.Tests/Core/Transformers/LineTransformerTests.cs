using DevUtilityHub.Api.Core.Transformers;

namespace DevUtilityHub.Tests.Core.Transformers
{
	public class LineTransformerTests
	{
		private readonly LineTransformer _transformer = new();

		// ── Sort ──────────────────────────────────────────────────────────────────

		[Fact]
		public void Sort_UnsortedLines_ReturnsSortedAlphabetically()
		{
			var result = _transformer.Sort("banana\napple\ncherry");
			Assert.Equal("apple\nbanana\ncherry", result);
		}

		[Fact]
		public void Sort_AlreadySortedLines_ReturnsSameOrder()
		{
			var result = _transformer.Sort("alpha\nbeta\ngamma");
			Assert.Equal("alpha\nbeta\ngamma", result);
		}

		[Fact]
		public void Sort_SingleLine_ReturnsSameLine()
		{
			var result = _transformer.Sort("hello");
			Assert.Equal("hello", result);
		}

		[Fact]
		public void Sort_EmptyString_ReturnsEmptyString()
		{
			Assert.Equal("", _transformer.Sort(""));
		}

		// ── Dedup ─────────────────────────────────────────────────────────────────

		[Fact]
		public void Dedup_LinesWithDuplicates_RemovesDuplicatesPreservingOrder()
		{
			var result = _transformer.Dedup("a\nb\na\nc");
			Assert.Equal("a\nb\nc", result);
		}

		[Fact]
		public void Dedup_NoDuplicates_ReturnsSameLines()
		{
			var result = _transformer.Dedup("x\ny\nz");
			Assert.Equal("x\ny\nz", result);
		}

		[Fact]
		public void Dedup_AllIdenticalLines_ReturnsSingleLine()
		{
			var result = _transformer.Dedup("foo\nfoo\nfoo");
			Assert.Equal("foo", result);
		}

		[Fact]
		public void Dedup_SingleLine_ReturnsSameLine()
		{
			var result = _transformer.Dedup("hello");
			Assert.Equal("hello", result);
		}

		// ── Reverse ───────────────────────────────────────────────────────────────

		[Fact]
		public void Reverse_MultipleLines_ReturnsLinesInReverseOrder()
		{
			var result = _transformer.Reverse("a\nb\nc");
			Assert.Equal("c\nb\na", result);
		}

		[Fact]
		public void Reverse_SingleLine_ReturnsSameLine()
		{
			var result = _transformer.Reverse("hello");
			Assert.Equal("hello", result);
		}

		[Fact]
		public void Reverse_EmptyString_ReturnsEmptyString()
		{
			Assert.Equal("", _transformer.Reverse(""));
		}

		// ── Count ─────────────────────────────────────────────────────────────────

		[Fact]
		public void Count_MultiLineInput_ReturnsCorrectLineWordCharCounts()
		{
			// "hello world\nfoo" = 2 lines, 3 words, 15 chars (including \n)
			var result = _transformer.Count("hello world\nfoo");
			Assert.Equal("Lines: 2 | Words: 3 | Characters: 15", result);
		}

		[Fact]
		public void Count_SingleLine_ReturnsCorrectCounts()
		{
			var result = _transformer.Count("hello world");
			Assert.Equal("Lines: 1 | Words: 2 | Characters: 11", result);
		}

		[Fact]
		public void Count_EmptyString_ReturnsZeroCounts()
		{
			var result = _transformer.Count("");
			Assert.Equal("Lines: 1 | Words: 0 | Characters: 0", result);
		}
	}
}
