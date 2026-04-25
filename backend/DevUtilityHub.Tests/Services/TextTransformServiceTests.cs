using DevUtilityHub.Api.Models.Requests;
using DevUtilityHub.Api.Services;

namespace DevUtilityHub.Tests.Services
{
	public class TextTransformServiceTests
	{
		private readonly TextTransformService _service = new();

		// ── Guard clauses ─────────────────────────────────────────────────────────

		[Fact]
		public void Transform_EmptyInput_ReturnsIsValidFalseWithMessage()
		{
			var request = new TextTransformRequest { Input = "", Operations = ["uppercase"] };
			var response = _service.Transform(request);
			Assert.False(response.IsValid);
			Assert.NotEmpty(response.ErrorMessage!);
		}

		[Fact]
		public void Transform_WhitespaceOnlyInput_ReturnsIsValidFalseWithMessage()
		{
			var request = new TextTransformRequest { Input = "   ", Operations = ["lowercase"] };
			var response = _service.Transform(request);
			Assert.False(response.IsValid);
			Assert.NotEmpty(response.ErrorMessage!);
		}

		// ── Single operations ─────────────────────────────────────────────────────

		[Fact]
		public void Transform_SingleOperationUppercase_ReturnsUpperCasedOutput()
		{
			var request = new TextTransformRequest { Input = "hello world", Operations = ["uppercase"] };
			var response = _service.Transform(request);
			Assert.True(response.IsValid);
			Assert.Equal("HELLO WORLD", response.Output);
		}

		[Fact]
		public void Transform_SingleOperationLowercase_ReturnsLowerCasedOutput()
		{
			var request = new TextTransformRequest { Input = "HELLO WORLD", Operations = ["lowercase"] };
			var response = _service.Transform(request);
			Assert.True(response.IsValid);
			Assert.Equal("hello world", response.Output);
		}

		[Fact]
		public void Transform_SingleOperationTrim_ReturnsTrimmedOutput()
		{
			var request = new TextTransformRequest { Input = "  hello  \n  world  ", Operations = ["trim"] };
			var response = _service.Transform(request);
			Assert.True(response.IsValid);
			Assert.Equal("hello\nworld", response.Output);
		}

		[Fact]
		public void Transform_SingleOperationSort_ReturnsSortedOutput()
		{
			var request = new TextTransformRequest { Input = "banana\napple\ncherry", Operations = ["sort"] };
			var response = _service.Transform(request);
			Assert.True(response.IsValid);
			Assert.Equal("apple\nbanana\ncherry", response.Output);
		}

		[Fact]
		public void Transform_SingleOperationDedup_ReturnsDedupedOutput()
		{
			var request = new TextTransformRequest { Input = "a\nb\na\nc", Operations = ["dedup"] };
			var response = _service.Transform(request);
			Assert.True(response.IsValid);
			Assert.Equal("a\nb\nc", response.Output);
		}

		[Fact]
		public void Transform_SingleOperationReverse_ReturnsReversedOutput()
		{
			var request = new TextTransformRequest { Input = "a\nb\nc", Operations = ["reverse"] };
			var response = _service.Transform(request);
			Assert.True(response.IsValid);
			Assert.Equal("c\nb\na", response.Output);
		}

		[Fact]
		public void Transform_SingleOperationCount_ReturnsCountSummary()
		{
			var request = new TextTransformRequest { Input = "hello world", Operations = ["count"] };
			var response = _service.Transform(request);
			Assert.True(response.IsValid);
			Assert.Contains("Lines:", response.Output);
			Assert.Contains("Words:", response.Output);
			Assert.Contains("Characters:", response.Output);
		}

		// ── Pipeline ordering ─────────────────────────────────────────────────────

		[Fact]
		public void Transform_PipelineUppercaseThenSort_AppliesInOrder()
		{
			// uppercase first → "BANANA\nAPPLE\nCHERRY", then sort → "APPLE\nBANANA\nCHERRY"
			var request = new TextTransformRequest { Input = "banana\napple\ncherry", Operations = ["uppercase", "sort"] };
			var response = _service.Transform(request);
			Assert.True(response.IsValid);
			Assert.Equal("APPLE\nBANANA\nCHERRY", response.Output);
		}

		[Fact]
		public void Transform_PipelineTrimThenUppercase_TrimsThenUppercases()
		{
			var request = new TextTransformRequest { Input = "  hello  \n  world  ", Operations = ["trim", "uppercase"] };
			var response = _service.Transform(request);
			Assert.True(response.IsValid);
			Assert.Equal("HELLO\nWORLD", response.Output);
		}

		// ── Unknown operations ────────────────────────────────────────────────────

		[Fact]
		public void Transform_UnknownOperation_IsSkippedNotAddedToAppliedOperations()
		{
			var request = new TextTransformRequest { Input = "hello", Operations = ["unknown_op"] };
			var response = _service.Transform(request);
			Assert.True(response.IsValid);
			Assert.Equal("hello", response.Output);
			Assert.Empty(response.AppliedOperations);
		}

		[Fact]
		public void Transform_MixedValidAndUnknownOperations_OnlyAppliesKnownOps()
		{
			var request = new TextTransformRequest { Input = "hello world", Operations = ["uppercase", "bogus", "reverse"] };
			var response = _service.Transform(request);
			Assert.True(response.IsValid);
			Assert.DoesNotContain("bogus", response.AppliedOperations);
			Assert.Contains("uppercase", response.AppliedOperations);
			Assert.Contains("reverse", response.AppliedOperations);
		}

		// ── AppliedOperations tracking ────────────────────────────────────────────

		[Fact]
		public void Transform_ValidInput_AppliedOperationsMatchesExecutedOps()
		{
			var request = new TextTransformRequest { Input = "hello world", Operations = ["uppercase"] };
			var response = _service.Transform(request);
			Assert.Single(response.AppliedOperations);
			Assert.Equal("uppercase", response.AppliedOperations[0]);
		}

		[Fact]
		public void Transform_TwoKnownOps_AppliedOperationsHasTwoEntries()
		{
			var request = new TextTransformRequest { Input = "hello world", Operations = ["trim", "uppercase"] };
			var response = _service.Transform(request);
			Assert.Equal(2, response.AppliedOperations.Count);
		}

		[Fact]
		public void Transform_NoOperations_ReturnsInputUnchangedWithEmptyAppliedOps()
		{
			var request = new TextTransformRequest { Input = "hello world", Operations = [] };
			var response = _service.Transform(request);
			Assert.True(response.IsValid);
			Assert.Equal("hello world", response.Output);
			Assert.Empty(response.AppliedOperations);
		}
	}
}
