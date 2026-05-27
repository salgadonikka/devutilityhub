using DevUtilityHub.Api.Core;

namespace DevUtilityHub.Tests.Core
{
    public class TimestampConverterTests
    {
        private readonly TimestampConverter _converter = new();

        // ── FromUnix ──────────────────────────────────────────────────────────

        [Fact]
        public void FromUnix_Seconds_ReturnsCorrectDateTimeOffset()
        {
            var dt = _converter.FromUnix(1700000000L, isMilliseconds: false);
            Assert.Equal(1700000000L, dt.ToUnixTimeSeconds());
        }

        [Fact]
        public void FromUnix_Milliseconds_ReturnsCorrectDateTimeOffset()
        {
            var dt = _converter.FromUnix(1700000000000L, isMilliseconds: true);
            Assert.Equal(1700000000000L, dt.ToUnixTimeMilliseconds());
        }

        [Fact]
        public void FromUnix_Zero_ReturnsEpoch()
        {
            var dt = _converter.FromUnix(0L, isMilliseconds: false);
            Assert.Equal(DateTimeOffset.UnixEpoch, dt);
        }

        // ── FromHuman ─────────────────────────────────────────────────────────

        [Fact]
        public void FromHuman_Iso8601_ReturnsCorrectUnixSeconds()
        {
            var dt = _converter.FromHuman("2023-11-14T22:13:20Z");
            Assert.NotNull(dt);
            Assert.Equal(1700000000L, dt!.Value.ToUnixTimeSeconds());
        }

        [Fact]
        public void FromHuman_NowKeyword_ReturnsApproximatelyNow()
        {
            var before = DateTimeOffset.UtcNow.AddSeconds(-2);
            var dt = _converter.FromHuman("now");
            var after = DateTimeOffset.UtcNow.AddSeconds(2);

            Assert.NotNull(dt);
            Assert.InRange(dt!.Value, before, after);
        }

        [Fact]
        public void FromHuman_TodayKeyword_ReturnsMidnightUtc()
        {
            var dt = _converter.FromHuman("today");
            Assert.NotNull(dt);
            Assert.Equal(TimeSpan.Zero, dt!.Value.TimeOfDay);
            Assert.Equal(0, dt.Value.Offset.Ticks);
        }

        [Fact]
        public void FromHuman_YesterdayKeyword_ReturnsYesterdayMidnight()
        {
            var expected = new DateTimeOffset(DateTimeOffset.UtcNow.AddDays(-1).Date, TimeSpan.Zero);
            var dt = _converter.FromHuman("yesterday");
            Assert.NotNull(dt);
            Assert.Equal(expected.Date, dt!.Value.Date);
        }

        [Fact]
        public void FromHuman_TomorrowKeyword_ReturnsTomorrowMidnight()
        {
            var expected = new DateTimeOffset(DateTimeOffset.UtcNow.AddDays(1).Date, TimeSpan.Zero);
            var dt = _converter.FromHuman("tomorrow");
            Assert.NotNull(dt);
            Assert.Equal(expected.Date, dt!.Value.Date);
        }

        [Fact]
        public void FromHuman_RelativePhrase_ParsesCorrectly()
        {
            var dt = _converter.FromHuman("2 days ago");
            Assert.NotNull(dt);
            var expected = DateTimeOffset.UtcNow.AddDays(-2);
            Assert.Equal(expected.Date, dt!.Value.Date);
        }

        [Fact]
        public void FromHuman_InvalidInput_ReturnsNull()
        {
            var dt = _converter.FromHuman("not a date at all !!!@#");
            Assert.Null(dt);
        }

        [Fact]
        public void FromHuman_EmptyString_ReturnsNull()
        {
            var dt = _converter.FromHuman("   ");
            Assert.Null(dt);
        }

        // ── ToResponse ────────────────────────────────────────────────────────

        [Fact]
        public void ToResponse_PopulatesAllFields()
        {
            var dt = DateTimeOffset.FromUnixTimeSeconds(1700000000L);
            var response = _converter.ToResponse(dt);

            Assert.True(response.IsValid);
            Assert.Equal(1700000000L, response.Seconds);
            Assert.Equal(1700000000000L, response.Ms);
            Assert.NotEmpty(response.Utc);
            Assert.Contains("2023-11-14T22:13:20Z", response.Iso);
            Assert.Null(response.ErrorMessage);
        }

        [Fact]
        public void ToResponse_IsoFormat_EndsWithZ()
        {
            var dt = DateTimeOffset.UtcNow;
            var response = _converter.ToResponse(dt);
            Assert.EndsWith("Z", response.Iso);
        }

        // ── Timezone support ──────────────────────────────────────────────────

        [Fact]
        public void FromHuman_AmbiguousInput_WithTimezone_AppliesOffset()
        {
            // "2023-11-14 17:13:20" in America/New_York (UTC-5) should equal unix 1700000000
            var dt = _converter.FromHuman("2023-11-14 17:13:20", "America/New_York");
            Assert.NotNull(dt);
            Assert.Equal(1700000000L, dt!.Value.ToUnixTimeSeconds());
        }

        [Fact]
        public void FromHuman_ExplicitOffsetInput_IgnoresTimezone()
        {
            // Explicit Z suffix → always UTC regardless of timeZoneId
            var dt = _converter.FromHuman("2023-11-14T22:13:20Z", "America/New_York");
            Assert.NotNull(dt);
            Assert.Equal(1700000000L, dt!.Value.ToUnixTimeSeconds());
        }

        [Fact]
        public void FromHuman_InvalidTimezone_FallsBackToUtc()
        {
            var dt = _converter.FromHuman("2023-11-14T22:13:20", "Not/ATimezone");
            Assert.NotNull(dt);
            Assert.Equal(1700000000L, dt!.Value.ToUnixTimeSeconds());
        }

        [Fact]
        public void ToResponse_WithTimezone_LocalFieldPopulated()
        {
            var dt = DateTimeOffset.FromUnixTimeSeconds(1700000000L);
            var response = _converter.ToResponse(dt, "America/New_York");
            Assert.NotNull(response.Local);
            Assert.Contains("2023-11-14T17:13:20", response.Local);
        }

        [Fact]
        public void ToResponse_NoTimezone_LocalFieldIsNull()
        {
            var dt = DateTimeOffset.FromUnixTimeSeconds(1700000000L);
            var response = _converter.ToResponse(dt, timeZoneId: null);
            Assert.Null(response.Local);
        }

        [Fact]
        public void ToResponse_InvalidTimezone_LocalFieldIsNull()
        {
            var dt = DateTimeOffset.FromUnixTimeSeconds(1700000000L);
            var response = _converter.ToResponse(dt, "Not/ATimezone");
            Assert.Null(response.Local);
        }
    }
}
