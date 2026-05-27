using DevUtilityHub.Api.Models.Requests;
using DevUtilityHub.Api.Services;

namespace DevUtilityHub.Tests.Services
{
    public class TimestampServiceTests
    {
        private readonly TimestampService _service = new();

        // ── Guard clauses ─────────────────────────────────────────────────────

        [Fact]
        public void Convert_MissingUnixValue_ReturnsInvalid()
        {
            var response = _service.Convert(new TimeRequest { Direction = "toHuman", UnixValue = null });
            Assert.False(response.IsValid);
            Assert.NotEmpty(response.ErrorMessage!);
        }

        [Fact]
        public void Convert_MissingHumanValue_ReturnsInvalid()
        {
            var response = _service.Convert(new TimeRequest { Direction = "toUnix", HumanValue = null });
            Assert.False(response.IsValid);
            Assert.NotEmpty(response.ErrorMessage!);
        }

        [Fact]
        public void Convert_EmptyHumanValue_ReturnsInvalid()
        {
            var response = _service.Convert(new TimeRequest { Direction = "toUnix", HumanValue = "   " });
            Assert.False(response.IsValid);
            Assert.NotEmpty(response.ErrorMessage!);
        }

        [Fact]
        public void Convert_UnknownDirection_ReturnsInvalid()
        {
            var response = _service.Convert(new TimeRequest { Direction = "invalid" });
            Assert.False(response.IsValid);
            Assert.NotEmpty(response.ErrorMessage!);
        }

        [Fact]
        public void Convert_InvalidHumanString_ReturnsInvalid()
        {
            var response = _service.Convert(new TimeRequest { Direction = "toUnix", HumanValue = "not a date !!!" });
            Assert.False(response.IsValid);
            Assert.NotEmpty(response.ErrorMessage!);
        }

        // ── Happy paths ───────────────────────────────────────────────────────

        [Fact]
        public void Convert_ValidUnixSeconds_ReturnsIsValid()
        {
            var response = _service.Convert(new TimeRequest
            {
                Direction = "toHuman",
                UnixValue = 1700000000L,
                IsMilliseconds = false,
            });
            Assert.True(response.IsValid);
            Assert.Equal(1700000000L, response.Seconds);
            Assert.Equal(1700000000000L, response.Ms);
            Assert.Contains("2023-11-14T22:13:20Z", response.Iso);
        }

        [Fact]
        public void Convert_ValidUnixMilliseconds_IsMillisecondsTrue()
        {
            var response = _service.Convert(new TimeRequest
            {
                Direction = "toHuman",
                UnixValue = 1700000000000L,
                IsMilliseconds = true,
            });
            Assert.True(response.IsValid);
            Assert.Equal(1700000000L, response.Seconds);
            Assert.Equal(1700000000000L, response.Ms);
        }

        [Fact]
        public void Convert_ValidIso8601_ReturnsIsValid()
        {
            var response = _service.Convert(new TimeRequest
            {
                Direction = "toUnix",
                HumanValue = "2023-11-14T22:13:20Z",
            });
            Assert.True(response.IsValid);
            Assert.Equal(1700000000L, response.Seconds);
        }

        [Fact]
        public void Convert_NowKeyword_ReturnsCurrentTimestamp()
        {
            var before = DateTimeOffset.UtcNow.ToUnixTimeSeconds() - 2;
            var response = _service.Convert(new TimeRequest { Direction = "toUnix", HumanValue = "now" });
            var after = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + 2;

            Assert.True(response.IsValid);
            Assert.InRange(response.Seconds, before, after);
        }

        [Fact]
        public void Convert_AllResponseFieldsPopulated()
        {
            var response = _service.Convert(new TimeRequest
            {
                Direction = "toHuman",
                UnixValue = 1700000000L,
            });
            Assert.True(response.IsValid);
            Assert.NotEmpty(response.Utc);
            Assert.NotEmpty(response.Iso);
            Assert.Null(response.ErrorMessage);
        }
    }
}
