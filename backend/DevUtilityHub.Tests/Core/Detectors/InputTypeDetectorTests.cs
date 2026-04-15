using DevUtilityHub.Api.Core.Detectors;

namespace DevUtilityHub.Tests.Core.Detectors;

public class InputTypeDetectorTests
{
    // -------------------------------------------------------------------------
    // JSON detection
    // -------------------------------------------------------------------------

    [Fact]
    public void Detect_JsonObject_ReturnsJson()
    {
        var result = InputTypeDetector.Detect("""{"key": "value"}""");
        Assert.Equal("json", result);
    }

    [Fact]
    public void Detect_JsonArray_ReturnsJson()
    {
        var result = InputTypeDetector.Detect("""[1, 2, 3]""");
        Assert.Equal("json", result);
    }

    [Theory]
    [InlineData("  {\"key\": \"value\"}")]
    [InlineData("\t{\"key\": \"value\"}")]
    [InlineData("\n{\"key\": \"value\"}")]
    public void Detect_LeadingWhitespaceBeforeJsonObject_ReturnsJson(string input)
    {
        var result = InputTypeDetector.Detect(input);
        Assert.Equal("json", result);
    }

    [Theory]
    [InlineData("  [1, 2, 3]")]
    [InlineData("\t[1, 2, 3]")]
    [InlineData("\n[1, 2, 3]")]
    public void Detect_LeadingWhitespaceBeforeJsonArray_ReturnsJson(string input)
    {
        var result = InputTypeDetector.Detect(input);
        Assert.Equal("json", result);
    }

    [Fact]
    public void Detect_EmptyJsonObject_ReturnsJson()
    {
        var result = InputTypeDetector.Detect("{}");
        Assert.Equal("json", result);
    }

    [Fact]
    public void Detect_EmptyJsonArray_ReturnsJson()
    {
        var result = InputTypeDetector.Detect("[]");
        Assert.Equal("json", result);
    }

    // -------------------------------------------------------------------------
    // XML detection
    // -------------------------------------------------------------------------

    [Fact]
    public void Detect_XmlElement_ReturnsXml()
    {
        var result = InputTypeDetector.Detect("<root><child/></root>");
        Assert.Equal("xml", result);
    }

    [Fact]
    public void Detect_XmlWithDeclaration_ReturnsXml()
    {
        var result = InputTypeDetector.Detect("""<?xml version="1.0"?><root/>""");
        Assert.Equal("xml", result);
    }

    [Theory]
    [InlineData("  <root/>")]
    [InlineData("\t<root/>")]
    [InlineData("\n<root/>")]
    public void Detect_LeadingWhitespaceBeforeXml_ReturnsXml(string input)
    {
        var result = InputTypeDetector.Detect(input);
        Assert.Equal("xml", result);
    }

    // -------------------------------------------------------------------------
    // Plain text detection
    // -------------------------------------------------------------------------

    [Fact]
    public void Detect_PlainSentence_ReturnsPlain()
    {
        var result = InputTypeDetector.Detect("Hello, world!");
        Assert.Equal("plain", result);
    }

    [Fact]
    public void Detect_NumberString_ReturnsPlain()
    {
        var result = InputTypeDetector.Detect("12345");
        Assert.Equal("plain", result);
    }

    [Fact]
    public void Detect_EmptyString_ReturnsPlain()
    {
        var result = InputTypeDetector.Detect(string.Empty);
        Assert.Equal("plain", result);
    }

    [Fact]
    public void Detect_WhitespaceOnlyString_ReturnsPlain()
    {
        var result = InputTypeDetector.Detect("   ");
        Assert.Equal("plain", result);
    }
}
