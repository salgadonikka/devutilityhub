using DevUtilityHub.Api.Core.Transformers;

namespace DevUtilityHub.Tests.Core.Transformers
{
	public class CaseTransformerTests
	{
		private readonly CaseTransformer _transformer = new();

		// ── ToUpperCase ───────────────────────────────────────────────────────────

		[Fact]
		public void ToUpperCase_LowerCaseInput_ReturnsAllUpperCase()
		{
			var result = _transformer.ToUpperCase("hello world");
			Assert.Equal("HELLO WORLD", result);
		}

		[Fact]
		public void ToUpperCase_EmptyString_ReturnsEmptyString()
		{
			Assert.Equal("", _transformer.ToUpperCase(""));
		}

		[Fact]
		public void ToUpperCase_MultiLineInput_UpperCasesEachLine()
		{
			var result = _transformer.ToUpperCase("hello\nworld");
			Assert.Equal("HELLO\nWORLD", result);
		}

		// ── ToLowerCase ───────────────────────────────────────────────────────────

		[Fact]
		public void ToLowerCase_UpperCaseInput_ReturnsAllLowerCase()
		{
			var result = _transformer.ToLowerCase("HELLO WORLD");
			Assert.Equal("hello world", result);
		}

		[Fact]
		public void ToLowerCase_EmptyString_ReturnsEmptyString()
		{
			Assert.Equal("", _transformer.ToLowerCase(""));
		}

		[Fact]
		public void ToLowerCase_MultiLineInput_LowerCasesEachLine()
		{
			var result = _transformer.ToLowerCase("HELLO\nWORLD");
			Assert.Equal("hello\nworld", result);
		}

		// ── ToTitleCase ───────────────────────────────────────────────────────────

		[Fact]
		public void ToTitleCase_SpaceSeparatedWords_CapitalizesFirstLetterOfEachWord()
		{
			var result = _transformer.ToTitleCase("hello world foo");
			Assert.Equal("Hello World Foo", result);
		}

		[Fact]
		public void ToTitleCase_EmptyString_ReturnsEmptyString()
		{
			Assert.Equal("", _transformer.ToTitleCase(""));
		}

		[Fact]
		public void ToTitleCase_MultiLineInput_TitleCasesEachLine()
		{
			var result = _transformer.ToTitleCase("hello world\nfoo bar");
			Assert.Equal("Hello World\nFoo Bar", result);
		}

		// ── ToCamelCase ───────────────────────────────────────────────────────────

		[Fact]
		public void ToCamelCase_SpaceSeparatedWords_ReturnsLowerFirstUpperRest()
		{
			var result = _transformer.ToCamelCase("hello world foo");
			Assert.Equal("helloWorldFoo", result);
		}

		[Fact]
		public void ToCamelCase_SnakeCasedInput_ConvertsToCamelCase()
		{
			var result = _transformer.ToCamelCase("hello_world");
			Assert.Equal("helloWorld", result);
		}

		[Fact]
		public void ToCamelCase_EmptyString_ReturnsEmptyString()
		{
			Assert.Equal("", _transformer.ToCamelCase(""));
		}

		[Fact]
		public void ToCamelCase_MultiLineInput_CamelCasesEachLine()
		{
			var result = _transformer.ToCamelCase("hello world\nfoo bar");
			Assert.Equal("helloWorld\nfooBar", result);
		}

		// ── ToSnakeCase ───────────────────────────────────────────────────────────

		[Fact]
		public void ToSnakeCase_SpaceSeparatedWords_ReturnsLowerCaseUnderscore()
		{
			var result = _transformer.ToSnakeCase("hello world foo");
			Assert.Equal("hello_world_foo", result);
		}

		[Fact]
		public void ToSnakeCase_CamelCasedInput_ConvertsToSnakeCase()
		{
			var result = _transformer.ToSnakeCase("helloWorld");
			Assert.Equal("hello_world", result);
		}

		[Fact]
		public void ToSnakeCase_EmptyString_ReturnsEmptyString()
		{
			Assert.Equal("", _transformer.ToSnakeCase(""));
		}

		[Fact]
		public void ToSnakeCase_MultiLineInput_SnakeCasesEachLine()
		{
			var result = _transformer.ToSnakeCase("hello world\nfoo bar");
			Assert.Equal("hello_world\nfoo_bar", result);
		}

		// ── ToKebabCase ───────────────────────────────────────────────────────────

		[Fact]
		public void ToKebabCase_SpaceSeparatedWords_ReturnsLowerCaseHyphen()
		{
			var result = _transformer.ToKebabCase("hello world foo");
			Assert.Equal("hello-world-foo", result);
		}

		[Fact]
		public void ToKebabCase_CamelCasedInput_ConvertsToKebabCase()
		{
			var result = _transformer.ToKebabCase("helloWorld");
			Assert.Equal("hello-world", result);
		}

		[Fact]
		public void ToKebabCase_EmptyString_ReturnsEmptyString()
		{
			Assert.Equal("", _transformer.ToKebabCase(""));
		}

		[Fact]
		public void ToKebabCase_MultiLineInput_KebabCasesEachLine()
		{
			var result = _transformer.ToKebabCase("hello world\nfoo bar");
			Assert.Equal("hello-world\nfoo-bar", result);
		}
	}
}
