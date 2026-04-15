using System.Xml;
using DevUtilityHub.Api.Core.Formatters;

namespace DevUtilityHub.Tests.Core.Formatters;

public class XmlFormatterTests
{
    private readonly XmlFormatter _formatter = new();

    // -------------------------------------------------------------------------
    // Format (prettify)
    // -------------------------------------------------------------------------

    [Fact]
    public void Format_CompactXml_ReturnsIndentedOutput()
    {
        var result = _formatter.Format("<root><child>value</child></root>");

        Assert.Contains("\n", result);
        Assert.Contains("  ", result);    // 2-space indent
        Assert.Contains("<child>", result);
        Assert.Contains("value", result);
    }

    [Fact]
    public void Format_SingleElementXml_ReturnsNonEmptyOutput()
    {
        var result = _formatter.Format("<root/>");

        Assert.NotEmpty(result);
        Assert.Contains("root", result);
    }

    [Fact]
    public void Format_XmlWithAttributes_PreservesAttributes()
    {
        var result = _formatter.Format("""<root id="1"><child name="x"/></root>""");

        Assert.Contains("id", result);
        Assert.Contains("name", result);
    }

    [Fact]
    public void Format_NestedXml_IndentsAllLevels()
    {
        var result = _formatter.Format("<a><b><c>deep</c></b></a>");

        Assert.Contains("\n", result);
        Assert.Contains("deep", result);
        Assert.Contains("<c>", result);
    }

    [Fact]
    public void Format_XmlWithDeclaration_IncludesDeclarationInOutput()
    {
        var result = _formatter.Format("""<?xml version="1.0" encoding="utf-8"?><root/>""");

        Assert.Contains("<?xml", result);
    }

    [Fact]
    public void Format_InvalidXmlMissingClosingTag_ThrowsXmlException()
    {
        Assert.Throws<XmlException>(() => _formatter.Format("<root><child></root>"));
    }

    [Fact]
    public void Format_EmptyString_ThrowsXmlException()
    {
        Assert.Throws<XmlException>(() => _formatter.Format(string.Empty));
    }

    [Fact]
    public void Format_JsonInput_ThrowsXmlException()
    {
        Assert.Throws<XmlException>(() => _formatter.Format("""{"key": "value"}"""));
    }

    // -------------------------------------------------------------------------
    // Minify
    // -------------------------------------------------------------------------

    [Fact]
    public void Minify_PrettifiedXml_ReturnsCompactOutput()
    {
        var pretty = """
            <root>
              <child>value</child>
            </root>
            """;
        var result = _formatter.Minify(pretty);

        Assert.DoesNotContain("\n", result);
        Assert.Contains("<child>", result);
        Assert.Contains("value", result);
    }

    [Fact]
    public void Minify_AlreadyCompactXml_ReturnsSameCompactForm()
    {
        const string compact = "<root><child>value</child></root>";
        var result = _formatter.Minify(compact);

        Assert.DoesNotContain("\n", result);
        Assert.Contains("root", result);
        Assert.Contains("child", result);
    }

    [Fact]
    public void Minify_XmlWithMultipleChildren_CompactsAllChildren()
    {
        var result = _formatter.Minify("<root><a>1</a><b>2</b><c>3</c></root>");

        Assert.DoesNotContain("\n", result);
        Assert.Contains("<a>", result);
        Assert.Contains("<b>", result);
        Assert.Contains("<c>", result);
    }

    [Fact]
    public void Minify_InvalidXml_ThrowsXmlException()
    {
        Assert.Throws<XmlException>(() => _formatter.Minify("<root><unclosed>"));
    }

    [Fact]
    public void Minify_EmptyString_ThrowsXmlException()
    {
        Assert.Throws<XmlException>(() => _formatter.Minify(string.Empty));
    }

    // -------------------------------------------------------------------------
    // Validate
    // -------------------------------------------------------------------------

    [Fact]
    public void Validate_ValidXmlElement_ReturnsIsValidTrueAndNullError()
    {
        var (isValid, error) = _formatter.Validate("<root><child/></root>");

        Assert.True(isValid);
        Assert.Null(error);
    }

    [Fact]
    public void Validate_ValidSelfClosingXml_ReturnsIsValidTrueAndNullError()
    {
        var (isValid, error) = _formatter.Validate("<root/>");

        Assert.True(isValid);
        Assert.Null(error);
    }

    [Fact]
    public void Validate_ValidXmlWithDeclaration_ReturnsIsValidTrueAndNullError()
    {
        var (isValid, error) = _formatter.Validate("""<?xml version="1.0"?><root/>""");

        Assert.True(isValid);
        Assert.Null(error);
    }

    [Fact]
    public void Validate_InvalidXmlMissingClosingTag_ReturnsIsValidFalseWithMessage()
    {
        var (isValid, error) = _formatter.Validate("<root><child>");

        Assert.False(isValid);
        Assert.NotNull(error);
        Assert.NotEmpty(error);
    }

    [Fact]
    public void Validate_InvalidXmlMismatchedTags_ReturnsIsValidFalseWithMessage()
    {
        var (isValid, error) = _formatter.Validate("<root></notroot>");

        Assert.False(isValid);
        Assert.NotNull(error);
        Assert.NotEmpty(error);
    }

    [Fact]
    public void Validate_InvalidXmlMultipleRoots_ReturnsIsValidFalseWithMessage()
    {
        var (isValid, error) = _formatter.Validate("<root/><root/>");

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
        var (isValid, error) = _formatter.Validate("this is not xml");

        Assert.False(isValid);
        Assert.NotNull(error);
        Assert.NotEmpty(error);
    }

    [Fact]
    public void Validate_JsonInput_ReturnsIsValidFalseWithMessage()
    {
        var (isValid, error) = _formatter.Validate("""{"key": "value"}""");

        Assert.False(isValid);
        Assert.NotNull(error);
        Assert.NotEmpty(error);
    }
}
