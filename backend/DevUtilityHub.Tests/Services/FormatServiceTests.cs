using DevUtilityHub.Api.Models.Requests;
using DevUtilityHub.Api.Services;

namespace DevUtilityHub.Tests.Services;

public class FormatServiceTests
{
    private readonly FormatService _service = new();

    // -------------------------------------------------------------------------
    // JSON — format
    // -------------------------------------------------------------------------

    [Fact]
    public void Process_JsonInputActionFormat_ReturnsFormattedOutputWithIsValidTrue()
    {
        var request = new FormatRequest
        {
            Input = """{"name":"Alice","age":30}""",
            Action = "format"
        };

        var response = _service.Process(request);

        Assert.True(response.IsValid);
        Assert.Equal("json", response.DetectedType);
        Assert.Null(response.ErrorMessage);
        Assert.Contains("\n", response.Output);
        Assert.Contains("name", response.Output);
    }

    [Fact]
    public void Process_JsonInputActionFormatInvalidJson_ReturnsIsValidFalseWithErrorMessage()
    {
        var request = new FormatRequest
        {
            Input = "{not valid json",
            Action = "format"
        };

        var response = _service.Process(request);

        Assert.False(response.IsValid);
        Assert.Equal("json", response.DetectedType);
        Assert.NotNull(response.ErrorMessage);
        Assert.NotEmpty(response.ErrorMessage);
    }

    // -------------------------------------------------------------------------
    // JSON — minify
    // -------------------------------------------------------------------------

    [Fact]
    public void Process_JsonInputActionMinify_ReturnsCompactOutputWithIsValidTrue()
    {
        var request = new FormatRequest
        {
            Input = """
                {
                  "name": "Alice",
                  "age": 30
                }
                """,
            Action = "minify"
        };

        var response = _service.Process(request);

        Assert.True(response.IsValid);
        Assert.Equal("json", response.DetectedType);
        Assert.Null(response.ErrorMessage);
        Assert.DoesNotContain("\n", response.Output);
        Assert.Contains("name", response.Output);
    }

    [Fact]
    public void Process_JsonInputActionMinifyInvalidJson_ReturnsIsValidFalseWithErrorMessage()
    {
        var request = new FormatRequest
        {
            Input = "{invalid",
            Action = "minify"
        };

        var response = _service.Process(request);

        Assert.False(response.IsValid);
        Assert.Equal("json", response.DetectedType);
        Assert.NotNull(response.ErrorMessage);
        Assert.NotEmpty(response.ErrorMessage);
    }

    // -------------------------------------------------------------------------
    // JSON — validate
    // -------------------------------------------------------------------------

    [Fact]
    public void Process_JsonInputActionValidateValidJson_ReturnsIsValidTrue()
    {
        var request = new FormatRequest
        {
            Input = """{"key": "value"}""",
            Action = "validate"
        };

        var response = _service.Process(request);

        Assert.True(response.IsValid);
        Assert.Equal("json", response.DetectedType);
        Assert.Null(response.ErrorMessage);
    }

    [Fact]
    public void Process_JsonInputActionValidateInvalidJson_ReturnsIsValidFalseWithErrorMessage()
    {
        var request = new FormatRequest
        {
            Input = """{key: value}""",
            Action = "validate"
        };

        var response = _service.Process(request);

        Assert.False(response.IsValid);
        Assert.Equal("json", response.DetectedType);
        Assert.NotNull(response.ErrorMessage);
        Assert.NotEmpty(response.ErrorMessage);
    }

    // -------------------------------------------------------------------------
    // XML — format
    // -------------------------------------------------------------------------

    [Fact]
    public void Process_XmlInputActionFormat_ReturnsFormattedOutputWithIsValidTrue()
    {
        var request = new FormatRequest
        {
            Input = "<root><child>value</child></root>",
            Action = "format"
        };

        var response = _service.Process(request);

        Assert.True(response.IsValid);
        Assert.Equal("xml", response.DetectedType);
        Assert.Null(response.ErrorMessage);
        Assert.Contains("\n", response.Output);
        Assert.Contains("child", response.Output);
    }

    [Fact]
    public void Process_XmlInputActionFormatInvalidXml_ReturnsIsValidFalseWithErrorMessage()
    {
        var request = new FormatRequest
        {
            Input = "<root><unclosed>",
            Action = "format"
        };

        var response = _service.Process(request);

        Assert.False(response.IsValid);
        Assert.Equal("xml", response.DetectedType);
        Assert.NotNull(response.ErrorMessage);
        Assert.NotEmpty(response.ErrorMessage);
    }

    // -------------------------------------------------------------------------
    // XML — minify
    // -------------------------------------------------------------------------

    [Fact]
    public void Process_XmlInputActionMinify_ReturnsCompactOutputWithIsValidTrue()
    {
        var request = new FormatRequest
        {
            Input = """
                <root>
                  <child>value</child>
                </root>
                """,
            Action = "minify"
        };

        var response = _service.Process(request);

        Assert.True(response.IsValid);
        Assert.Equal("xml", response.DetectedType);
        Assert.Null(response.ErrorMessage);
        Assert.DoesNotContain("\n", response.Output);
        Assert.Contains("child", response.Output);
    }

    [Fact]
    public void Process_XmlInputActionMinifyInvalidXml_ReturnsIsValidFalseWithErrorMessage()
    {
        var request = new FormatRequest
        {
            Input = "<root><bad>",
            Action = "minify"
        };

        var response = _service.Process(request);

        Assert.False(response.IsValid);
        Assert.Equal("xml", response.DetectedType);
        Assert.NotNull(response.ErrorMessage);
        Assert.NotEmpty(response.ErrorMessage);
    }

    // -------------------------------------------------------------------------
    // XML — validate
    // -------------------------------------------------------------------------

    [Fact]
    public void Process_XmlInputActionValidateValidXml_ReturnsIsValidTrue()
    {
        var request = new FormatRequest
        {
            Input = "<root><child/></root>",
            Action = "validate"
        };

        var response = _service.Process(request);

        Assert.True(response.IsValid);
        Assert.Equal("xml", response.DetectedType);
        Assert.Null(response.ErrorMessage);
    }

    [Fact]
    public void Process_XmlInputActionValidateInvalidXml_ReturnsIsValidFalseWithErrorMessage()
    {
        var request = new FormatRequest
        {
            Input = "<root><mismatch></different>",
            Action = "validate"
        };

        var response = _service.Process(request);

        Assert.False(response.IsValid);
        Assert.Equal("xml", response.DetectedType);
        Assert.NotNull(response.ErrorMessage);
        Assert.NotEmpty(response.ErrorMessage);
    }

    // -------------------------------------------------------------------------
    // OverrideType
    // -------------------------------------------------------------------------

    [Fact]
    public void Process_OverrideTypeJsonOnJsonInput_UsesJsonPathRegardlessOfDetection()
    {
        var request = new FormatRequest
        {
            Input = """{"key":"value"}""",
            Action = "format",
            OverrideType = "json"
        };

        var response = _service.Process(request);

        Assert.True(response.IsValid);
        Assert.Equal("json", response.DetectedType);
    }

    [Fact]
    public void Process_OverrideTypeXmlOnXmlInput_UsesXmlPath()
    {
        var request = new FormatRequest
        {
            Input = "<root/>",
            Action = "format",
            OverrideType = "xml"
        };

        var response = _service.Process(request);

        Assert.True(response.IsValid);
        Assert.Equal("xml", response.DetectedType);
    }

    [Fact]
    public void Process_OverrideTypeCaseInsensitive_NormalizesToLowerAndDispatchesCorrectly()
    {
        var request = new FormatRequest
        {
            Input = """{"key":"value"}""",
            Action = "validate",
            OverrideType = "JSON"
        };

        var response = _service.Process(request);

        Assert.True(response.IsValid);
        Assert.Equal("json", response.DetectedType);
    }

    [Fact]
    public void Process_OverrideTypePlain_ReturnsUnsupportedFormatError()
    {
        var request = new FormatRequest
        {
            Input = """{"key":"value"}""",
            Action = "format",
            OverrideType = "plain"
        };

        var response = _service.Process(request);

        Assert.False(response.IsValid);
        Assert.Equal("plain", response.DetectedType);
        Assert.Contains("Unsupported format", response.ErrorMessage);
    }

    // -------------------------------------------------------------------------
    // Plain text auto-detection
    // -------------------------------------------------------------------------

    [Fact]
    public void Process_PlainTextInput_ReturnsUnsupportedFormatError()
    {
        var request = new FormatRequest
        {
            Input = "just some plain text",
            Action = "format"
        };

        var response = _service.Process(request);

        Assert.False(response.IsValid);
        Assert.Equal("plain", response.DetectedType);
        Assert.Equal("Unsupported format: plain text", response.ErrorMessage);
    }

    [Fact]
    public void Process_EmptyStringInput_ReturnsUnsupportedFormatError()
    {
        var request = new FormatRequest
        {
            Input = string.Empty,
            Action = "format"
        };

        var response = _service.Process(request);

        Assert.False(response.IsValid);
        Assert.Equal("plain", response.DetectedType);
        Assert.Equal("Unsupported format: plain text", response.ErrorMessage);
    }

    // -------------------------------------------------------------------------
    // Default action (no explicit Action provided defaults to "format")
    // -------------------------------------------------------------------------

    [Fact]
    public void Process_JsonInputDefaultAction_FormatsJson()
    {
        var request = new FormatRequest
        {
            Input = """{"k":"v"}"""
            // Action defaults to "format" per model initializer
        };

        var response = _service.Process(request);

        Assert.True(response.IsValid);
        Assert.Equal("json", response.DetectedType);
        Assert.Contains("\n", response.Output);
    }
}
