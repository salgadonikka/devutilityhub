namespace DevUtilityHub.Api.Models.Responses
{
	public class TimeResponse
	{
		public string HumanReadable { get; set; } = string.Empty;
		public long UnixSeconds { get; set; }
		public long UnixMilliseconds { get; set; }
		public string Utc { get; set; } = string.Empty;
	}
}
