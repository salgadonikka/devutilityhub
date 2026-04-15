using System.Text.Json;
using DevUtilityHub.Api.Core.Formatters;

namespace DevUtilityHub.Tests.Core.Formatters;

public class JsonFormatterTests
{
    private readonly JsonFormatter _formatter = new();

    // -------------------------------------------------------------------------
    // Format (prettify)
    // -------------------------------------------------------------------------

    [Fact]
    public void Format_CompactJsonObject_ReturnsIndentedOutput()
    {
        var result = _formatter.Format("""{"name":"Alice","age":30}""");

        // Verify indentation is present
        Assert.Contains("\n", result);
        Assert.Contains("  ", result);

        // Verify the output is still valid JSON with the same values
        var doc = JsonDocument.Parse(result);
        Assert.Equal("Alice", doc.RootElement.GetProperty("name").GetString());
        Assert.Equal(30, doc.RootElement.GetProperty("age").GetInt32());
    }

    [Fact]
    public void Format_JsonArray_ReturnsIndentedOutput()
    {
        var result = _formatter.Format("[1,2,3]");

        Assert.Contains("\n", result);
        Assert.Contains("1", result);
        Assert.Contains("2", result);
        Assert.Contains("3", result);
    }

    [Fact]
    public void Format_AlreadyPrettifiedJson_ReturnsIndentedOutput()
    {
        var pretty = """
            {
              "key": "value"
            }
            """;
        var result = _formatter.Format(pretty);

        Assert.Contains("\n", result);
        Assert.Contains("key", result);
        Assert.Contains("value", result);
    }

    [Fact]
    public void Format_NestedJsonObject_ReturnsIndentedOutput()
    {
        var result = _formatter.Format("""{"outer":{"inner":"val"}}""");

        Assert.Contains("\n", result);
        Assert.Contains("outer", result);
        Assert.Contains("inner", result);
    }

    [Fact]
    public void Format_InvalidJson_ThrowsJsonException()
    {
        Assert.ThrowsAny<JsonException>(() => _formatter.Format("{invalid}"));
    }

    [Fact]
    public void Format_EmptyString_ThrowsJsonException()
    {
        Assert.ThrowsAny<JsonException>(() => _formatter.Format(string.Empty));
    }

    // -------------------------------------------------------------------------
    // Minify
    // -------------------------------------------------------------------------

    [Fact]
    public void Minify_PrettifiedJsonObject_ReturnsCompactOutput()
    {
        var pretty = """
            {
              "name": "Alice",
              "age": 30
            }
            """;
        var result = _formatter.Minify(pretty);

        Assert.DoesNotContain("\n", result);
        Assert.DoesNotContain("  ", result);
        Assert.Contains("name", result);
        Assert.Contains("Alice", result);
    }

    [Fact]
    public void Minify_PrettifiedJsonArray_ReturnsCompactOutput()
    {
        var pretty = """
            [
              1,
              2,
              3
            ]
            """;
        var result = _formatter.Minify(pretty);

        Assert.DoesNotContain("\n", result);
        Assert.Contains("1", result);
        Assert.Contains("2", result);
        Assert.Contains("3", result);
    }

    [Fact]
    public void Minify_AlreadyCompactJson_ReturnsSameCompactForm()
    {
        const string compact = """{"key":"value"}""";
        var result = _formatter.Minify(compact);

        Assert.DoesNotContain("\n", result);
        Assert.Contains("key", result);
        Assert.Contains("value", result);
    }

    [Fact]
    public void Minify_InvalidJson_ThrowsJsonException()
    {
        Assert.ThrowsAny<JsonException>(() => _formatter.Minify("{invalid}"));
    }

    [Fact]
    public void Minify_EmptyString_ThrowsJsonException()
    {
        Assert.ThrowsAny<JsonException>(() => _formatter.Minify(string.Empty));
    }

    // -------------------------------------------------------------------------
    // Validate
    // -------------------------------------------------------------------------

    [Fact]
    public void Validate_ValidJsonObject_ReturnsIsValidTrueAndNullError()
    {
        var (isValid, error) = _formatter.Validate("""{"key": "value"}""");

        Assert.True(isValid);
        Assert.Null(error);
    }

    [Fact]
    public void Validate_ValidJsonArray_ReturnsIsValidTrueAndNullError()
    {
        var (isValid, error) = _formatter.Validate("[1, 2, 3]");

        Assert.True(isValid);
        Assert.Null(error);
    }

    [Fact]
    public void Validate_ValidJsonBoolean_ReturnsIsValidTrueAndNullError()
    {
        var (isValid, error) = _formatter.Validate("true");

        Assert.True(isValid);
        Assert.Null(error);
    }

    [Fact]
    public void Validate_InvalidJsonMissingQuotes_ReturnsIsValidFalseWithMessage()
    {
        var (isValid, error) = _formatter.Validate("{key: value}");

        Assert.False(isValid);
        Assert.NotNull(error);
        Assert.NotEmpty(error);
    }

    [Fact]
    public void Validate_InvalidJsonTrailingComma_ReturnsIsValidFalseWithMessage()
    {
        var (isValid, error) = _formatter.Validate("""{"key": "value",}""");

        Assert.False(isValid);
        Assert.NotNull(error);
        Assert.NotEmpty(error);
    }

    [Fact]
    public void Validate_EmptyString_ReturnsIsValidFalseWithMessage()
    {
        var (isValid, error) = _formatter.Validate(string.Empty);

        Assert.False(isValid);
        Assert.NotNull(error);
        Assert.NotEmpty(error);
    }

    [Fact]
    public void Validate_PlainText_ReturnsIsValidFalseWithMessage()
    {
        var (isValid, error) = _formatter.Validate("this is not json");

        Assert.False(isValid);
        Assert.NotNull(error);
        Assert.NotEmpty(error);
    }

    [Fact]
    public void Validate_XmlInput_ReturnsIsValidFalseWithMessage()
    {
        var (isValid, error) = _formatter.Validate("<root/>");

        Assert.False(isValid);
        Assert.NotNull(error);
        Assert.NotEmpty(error);
    }
}
