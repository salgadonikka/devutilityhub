namespace DevUtilityHub.Api.Models.Requests
{
	public class TimeRequest
	{
		public string Direction { get; set; } = "toHuman"; // toHuman | toUnix
		public long? UnixValue { get; set; }
		public bool IsMilliseconds { get; set; } = false;
		public string? HumanValue { get; set; }  // ISO 8601 when direction = toUnix
	}
}
