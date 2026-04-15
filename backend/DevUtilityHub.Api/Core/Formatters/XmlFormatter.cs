using System.Text;
using System.Xml;

namespace DevUtilityHub.Api.Core.Formatters
{
	public class XmlFormatter
	{
		public string Format(string input)
		{
			var doc = new XmlDocument();
			doc.LoadXml(input);
			var sb = new StringBuilder();
			var settings = new XmlWriterSettings { Indent = true, IndentChars = "  ", OmitXmlDeclaration = false };
			using var writer = XmlWriter.Create(sb, settings);
			doc.WriteTo(writer);
			writer.Flush();
			return sb.ToString();
		}

		public string Minify(string input)
		{
			var doc = new XmlDocument();
			doc.LoadXml(input);
			var sb = new StringBuilder();
			var settings = new XmlWriterSettings { Indent = false, OmitXmlDeclaration = false };
			using var writer = XmlWriter.Create(sb, settings);
			doc.WriteTo(writer);
			writer.Flush();
			return sb.ToString();
		}

		public (bool IsValid, string? Error) Validate(string input)
		{
			try
			{
				var doc = new XmlDocument();
				doc.LoadXml(input);
				return (true, null);
			}
			catch (XmlException ex)
			{
				return (false, ex.Message);
			}
		}
	}
}
