using DevUtilityHub.Api.Models.Requests;
using DevUtilityHub.Api.Services;

namespace DevUtilityHub.Tests.Services
{
	public class DiffServiceTests
	{
		private readonly DiffService _service = new();

		// -------------------------------------------------------------------------
		// Guard clause — empty / whitespace inputs
		// -------------------------------------------------------------------------

		[Fact]
		public void Compare_EmptyTextA_ReturnsIsValidFalseWithMessage()
		{
			var request = new DiffRequest { TextA = "", TextB = "some text" };

			var response = _service.Compare(request);

			Assert.False(response.IsValid);
			Assert.NotEmpty(response.ErrorMessage!);
		}

		[Fact]
		public void Compare_EmptyTextB_ReturnsIsValidFalseWithMessage()
		{
			var request = new DiffRequest { TextA = "some text", TextB = "" };

			var response = _service.Compare(request);

			Assert.False(response.IsValid);
			Assert.NotEmpty(response.ErrorMessage!);
		}

		[Fact]
		public void Compare_WhitespaceOnlyTextA_ReturnsIsValidFalse()
		{
			var request = new DiffRequest { TextA = "   ", TextB = "some text" };

			var response = _service.Compare(request);

			Assert.False(response.IsValid);
		}

		[Fact]
		public void Compare_WhitespaceOnlyTextB_ReturnsIsValidFalse()
		{
			var request = new DiffRequest { TextA = "some text", TextB = "\t\n" };

			var response = _service.Compare(request);

			Assert.False(response.IsValid);
		}

		[Fact]
		public void Compare_BothTextsEmpty_ReturnsIsValidFalse()
		{
			var request = new DiffRequest { TextA = "", TextB = "" };

			var response = _service.Compare(request);

			Assert.False(response.IsValid);
		}

		// -------------------------------------------------------------------------
		// Valid inputs — identical texts
		// -------------------------------------------------------------------------

		[Fact]
		public void Compare_IdenticalSingleLineTexts_ReturnsIsValidTrueWithZeroCounts()
		{
			var request = new DiffRequest { TextA = "hello world", TextB = "hello world" };

			var response = _service.Compare(request);

			Assert.True(response.IsValid);
			Assert.Null(response.ErrorMessage);
			Assert.Equal(0, response.AddedCount);
			Assert.Equal(0, response.RemovedCount);
		}

		[Fact]
		public void Compare_IdenticalMultiLineTexts_ReturnsNonEmptyLinesAllUnchanged()
		{
			var text = "line one\nline two\nline three";
			var request = new DiffRequest { TextA = text, TextB = text };

			var response = _service.Compare(request);

			Assert.True(response.IsValid);
			Assert.NotEmpty(response.Lines);
			Assert.All(response.Lines, l => Assert.Equal("unchanged", l.Type));
		}

		// -------------------------------------------------------------------------
		// Valid inputs — differing texts
		// -------------------------------------------------------------------------

		[Fact]
		public void Compare_TextBHasExtraLine_ReturnsAddedCountOne()
		{
			var request = new DiffRequest
			{
				TextA = "first line",
				TextB = "first line\nsecond line"
			};

			var response = _service.Compare(request);

			Assert.True(response.IsValid);
			Assert.Equal(1, response.AddedCount);
			Assert.Equal(0, response.RemovedCount);
		}

		[Fact]
		public void Compare_TextAHasExtraLine_ReturnsRemovedCountOne()
		{
			var request = new DiffRequest
			{
				TextA = "first line\nsecond line",
				TextB = "first line"
			};

			var response = _service.Compare(request);

			Assert.True(response.IsValid);
			Assert.Equal(0, response.AddedCount);
			Assert.Equal(1, response.RemovedCount);
		}

		[Fact]
		public void Compare_CompletelyDifferentTexts_ReturnsBothAddedAndRemovedCounts()
		{
			var request = new DiffRequest { TextA = "old content", TextB = "new content" };

			var response = _service.Compare(request);

			Assert.True(response.IsValid);
			Assert.True(response.AddedCount > 0);
			Assert.True(response.RemovedCount > 0);
		}

		// -------------------------------------------------------------------------
		// Response structure
		// -------------------------------------------------------------------------

		[Fact]
		public void Compare_ValidInputs_ReturnsNonEmptyLinesList()
		{
			var request = new DiffRequest { TextA = "alpha\nbeta", TextB = "alpha\ngamma" };

			var response = _service.Compare(request);

			Assert.True(response.IsValid);
			Assert.NotEmpty(response.Lines);
		}

		[Fact]
		public void Compare_ValidInputs_AllLinesHaveValidType()
		{
			var request = new DiffRequest { TextA = "a\nb\nc", TextB = "a\nx\nc" };

			var response = _service.Compare(request);

			Assert.True(response.IsValid);
			Assert.All(response.Lines, l => Assert.Contains(l.Type, new[] { "added", "removed", "unchanged" }));
		}

		[Fact]
		public void Compare_ValidInputs_LineNumbersAreSequentialStartingAtOne()
		{
			var request = new DiffRequest { TextA = "a\nb\nc", TextB = "a\nx\nc" };

			var response = _service.Compare(request);

			Assert.True(response.IsValid);
			for (int i = 0; i < response.Lines.Count; i++)
				Assert.Equal(i + 1, response.Lines[i].LineNumber);
		}

		[Fact]
		public void Compare_ValidInputs_AddedAndRemovedCountsMatchLineTypes()
		{
			var request = new DiffRequest { TextA = "one\ntwo\nthree", TextB = "one\nalpha\nbeta\nthree" };

			var response = _service.Compare(request);

			Assert.True(response.IsValid);
			Assert.Equal(response.AddedCount, response.Lines.Count(l => l.Type == "added"));
			Assert.Equal(response.RemovedCount, response.Lines.Count(l => l.Type == "removed"));
		}
	}
}
