namespace DevUtilityHub.Api.Models.Responses
{
    public class TimeResponse
    {
        public long Seconds { get; set; }
        public long Ms { get; set; }
        public string Utc { get; set; } = string.Empty;
        public string Iso { get; set; } = string.Empty;
        public string? Local { get; set; }
        public bool IsValid { get; set; }
        public string? ErrorMessage { get; set; }
    }
}
