using DevUtilityHub.Api.Core;
using DevUtilityHub.Api.Models.Requests;
using DevUtilityHub.Api.Models.Responses;
using DevUtilityHub.Api.Services.Interfaces;

namespace DevUtilityHub.Api.Services
{
    public class TimestampService : ITimestampService
    {
        private readonly TimestampConverter _converter = new();

        public TimeResponse Convert(TimeRequest request)
        {
            if (request.Direction == "toHuman")
            {
                if (request.UnixValue is null)
                    return Invalid("Unix timestamp value is required.");

                var dt = _converter.FromUnix(request.UnixValue.Value, request.IsMilliseconds);
                return _converter.ToResponse(dt, request.TimeZoneId);
            }

            if (request.Direction == "toUnix")
            {
                if (string.IsNullOrWhiteSpace(request.HumanValue))
                    return Invalid("A date/time string is required.");

                var dt = _converter.FromHuman(request.HumanValue, request.TimeZoneId);
                if (dt is null)
                    return Invalid($"Could not parse '{request.HumanValue}' as a date or time.");

                return _converter.ToResponse(dt.Value, request.TimeZoneId);
            }

            return Invalid("Direction must be 'toHuman' or 'toUnix'.");
        }

        private static TimeResponse Invalid(string message) =>
            new() { IsValid = false, ErrorMessage = message };
    }
}
