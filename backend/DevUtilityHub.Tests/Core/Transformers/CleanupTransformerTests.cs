using DevUtilityHub.Api.Core.Transformers;

namespace DevUtilityHub.Tests.Core.Transformers
{
	public class CleanupTransformerTests
	{
		private readonly CleanupTransformer _transformer = new();

		[Fact]
		public void Trim_LinesWithLeadingWhitespace_RemovesLeadingWhitespace()
		{
			var result = _transformer.Trim("  hello\n  world");
			Assert.Equal("hello\nworld", result);
		}

		[Fact]
		public void Trim_LinesWithTrailingWhitespace_RemovesTrailingWhitespace()
		{
			var result = _transformer.Trim("hello  \nworld  ");
			Assert.Equal("hello\nworld", result);
		}

		[Fact]
		public void Trim_LinesWithBothLeadingAndTrailingWhitespace_RemovesBoth()
		{
			var result = _transformer.Trim("  hello  \n  world  ");
			Assert.Equal("hello\nworld", result);
		}

		[Fact]
		public void Trim_BlankLines_PreservesBlankLines()
		{
			var result = _transformer.Trim("hello\n  \nworld");
			Assert.Equal("hello\n\nworld", result);
		}

		[Fact]
		public void Trim_AlreadyTrimmedLines_ReturnsSameContent()
		{
			var result = _transformer.Trim("clean\nlines");
			Assert.Equal("clean\nlines", result);
		}

		[Fact]
		public void Trim_EmptyString_ReturnsEmptyString()
		{
			Assert.Equal("", _transformer.Trim(""));
		}

		[Fact]
		public void Trim_MultipleLines_TrimseachLineIndependently()
		{
			var result = _transformer.Trim("  a  \n  b  \n  c  ");
			Assert.Equal("a\nb\nc", result);
		}
	}
}
