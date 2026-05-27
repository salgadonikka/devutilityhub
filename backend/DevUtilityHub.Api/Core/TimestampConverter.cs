using DevUtilityHub.Api.Models.Responses;
using Microsoft.Recognizers.Text;
using Microsoft.Recognizers.Text.DateTime;
using System.Globalization;
using System.Text.RegularExpressions;

namespace DevUtilityHub.Api.Core
{
    public partial class TimestampConverter
    {
        private static readonly Dictionary<string, Func<DateTimeOffset>> Shorthands =
            new(StringComparer.OrdinalIgnoreCase)
            {
                ["now"]       = () => DateTimeOffset.UtcNow,
                ["today"]     = () => new DateTimeOffset(DateTimeOffset.UtcNow.Date, TimeSpan.Zero),
                ["yesterday"] = () => new DateTimeOffset(DateTimeOffset.UtcNow.AddDays(-1).Date, TimeSpan.Zero),
                ["tomorrow"]  = () => new DateTimeOffset(DateTimeOffset.UtcNow.AddDays(1).Date, TimeSpan.Zero),
            };

        [GeneratedRegex(@"(Z|[+-]\d{2}:\d{2}|[+-]\d{4})$", RegexOptions.IgnoreCase)]
        private static partial Regex ExplicitOffsetPattern();

        public DateTimeOffset FromUnix(long value, bool isMilliseconds) =>
            isMilliseconds
                ? DateTimeOffset.FromUnixTimeMilliseconds(value)
                : DateTimeOffset.FromUnixTimeSeconds(value);

        public DateTimeOffset? FromHuman(string input, string? timeZoneId = null)
        {
            var trimmed = input.Trim();
            if (string.IsNullOrEmpty(trimmed)) return null;

            // 1. Shorthand keywords — always UTC-anchored
            if (Shorthands.TryGetValue(trimmed, out var factory))
                return factory();

            // 2. Structured parsing
            bool hasExplicitOffset = ExplicitOffsetPattern().IsMatch(trimmed);

            if (hasExplicitOffset)
            {
                // Explicit offset present — parse and normalise to UTC
                if (DateTimeOffset.TryParse(trimmed, CultureInfo.InvariantCulture,
                        DateTimeStyles.AdjustToUniversal, out var dto))
                    return dto;
            }
            else
            {
                // Ambiguous — apply caller's timezone if provided, else assume UTC
                if (DateTime.TryParse(trimmed, CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out var dt))
                    return ApplyTimezone(dt, timeZoneId);
            }

            // 3. Natural language via Microsoft.Recognizers.Text
            try
            {
                var results = DateTimeRecognizer.RecognizeDateTime(
                    trimmed, Culture.English, refTime: DateTime.UtcNow);

                foreach (var result in results)
                {
                    if (result.Resolution.TryGetValue("values", out var rawValues) &&
                        rawValues is List<Dictionary<string, string>> values)
                    {
                        foreach (var val in values)
                        {
                            if (val.TryGetValue("value", out var dateStr) &&
                                DateTime.TryParse(dateStr, CultureInfo.InvariantCulture,
                                    DateTimeStyles.None, out var dt))
                                return ApplyTimezone(dt, timeZoneId);
                        }
                    }
                }
            }
            catch { /* recognizer failure is non-fatal */ }

            return null;
        }

        public TimeResponse ToResponse(DateTimeOffset dt, string? timeZoneId = null)
        {
            string? local = null;
            if (timeZoneId is not null)
            {
                try
                {
                    var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                    var localDt = TimeZoneInfo.ConvertTime(dt, tz);
                    local = localDt.ToString("yyyy-MM-ddTHH:mm:sszzz");
                }
                catch { /* invalid or unknown timezone — omit local */ }
            }

            return new()
            {
                Seconds = dt.ToUnixTimeSeconds(),
                Ms      = dt.ToUnixTimeMilliseconds(),
                Utc     = dt.UtcDateTime.ToString("R"),
                Iso     = dt.UtcDateTime.ToString("yyyy-MM-ddTHH:mm:ss") + "Z",
                Local   = local,
                IsValid = true,
            };
        }

        private static DateTimeOffset ApplyTimezone(DateTime dt, string? timeZoneId)
        {
            if (timeZoneId is not null)
            {
                try
                {
                    var tz = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
                    var utc = TimeZoneInfo.ConvertTimeToUtc(
                        DateTime.SpecifyKind(dt, DateTimeKind.Unspecified), tz);
                    return new DateTimeOffset(utc, TimeSpan.Zero);
                }
                catch { /* unknown timezone — fall through to UTC assumption */ }
            }
            return new DateTimeOffset(dt, TimeSpan.Zero);
        }
    }
}
