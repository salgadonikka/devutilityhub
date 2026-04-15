using DevUtilityHub.Api.Core.Detectors;
using DevUtilityHub.Api.Core.Formatters;
using DevUtilityHub.Api.Models.Requests;
using DevUtilityHub.Api.Models.Responses;
using DevUtilityHub.Api.Services.Interfaces;

namespace DevUtilityHub.Api.Services
{
	public class FormatService : IFormatService
	{
		private readonly JsonFormatter _json = new();
		private readonly XmlFormatter _xml = new();

		public FormatResponse Process(FormatRequest request)
		{
			var type = string.IsNullOrWhiteSpace(request.OverrideType)
				? InputTypeDetector.Detect(request.Input)
				: request.OverrideType.ToLowerInvariant();

			if (type == "plain")
				return new FormatResponse
				{
					IsValid = false,
					ErrorMessage = "Unsupported format: plain text",
					DetectedType = "plain"
				};

			try
			{
				if (type == "json")
					return ProcessJson(request.Input, request.Action);

				if (type == "xml")
					return ProcessXml(request.Input, request.Action);

				return new FormatResponse
				{
					IsValid = false,
					ErrorMessage = $"Unknown format type: {type}",
					DetectedType = type
				};
			}
			catch (Exception ex)
			{
				return new FormatResponse
				{
					IsValid = false,
					ErrorMessage = ex.Message,
					DetectedType = type
				};
			}
		}

		private FormatResponse ProcessJson(string input, string action)
		{
			if (action == "validate")
			{
				var (isValid, error) = _json.Validate(input);
				return new FormatResponse { DetectedType = "json", IsValid = isValid, ErrorMessage = error };
			}

			if (action == "minify")
			{
				var output = _json.Minify(input);
				return new FormatResponse { DetectedType = "json", IsValid = true, Output = output };
			}

			// default: format / prettify
			var formatted = _json.Format(input);
			return new FormatResponse { DetectedType = "json", IsValid = true, Output = formatted };
		}

		private FormatResponse ProcessXml(string input, string action)
		{
			if (action == "validate")
			{
				var (isValid, error) = _xml.Validate(input);
				return new FormatResponse { DetectedType = "xml", IsValid = isValid, ErrorMessage = error };
			}

			if (action == "minify")
			{
				var output = _xml.Minify(input);
				return new FormatResponse { DetectedType = "xml", IsValid = true, Output = output };
			}

			// default: format / prettify
			var formatted = _xml.Format(input);
			return new FormatResponse { DetectedType = "xml", IsValid = true, Output = formatted };
		}
	}
}
