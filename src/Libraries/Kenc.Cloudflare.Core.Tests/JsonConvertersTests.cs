namespace Kenc.Cloudflare.Core.Tests
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;
    using FluentAssertions;
    using Kenc.Cloudflare.Core.JsonConverters;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class JsonConvertersTests
    {
        private static JsonSerializerOptions OptionsWith(params JsonConverter[] converters)
        {
            var options = new JsonSerializerOptions();
            foreach (var converter in converters)
            {
                options.Converters.Add(converter);
            }

            return options;
        }

        [TestMethod]
        public void DateTimeConverter_RoundTripsToUtc()
        {
            var options = OptionsWith(new DateTimeConverter());
            var value = new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc);

            var json = JsonSerializer.Serialize(value, options);
            var result = JsonSerializer.Deserialize<DateTime>(json, options);

            json.Should().Be("\"2024-01-15T10:30:00Z\"");
            result.Should().Be(value);
        }

        [TestMethod]
        public void DateTimeOffsetConverter_RoundTripsToUtc()
        {
            var options = OptionsWith(new DateTimeOffsetConverter());
            var value = new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero);

            var json = JsonSerializer.Serialize(value, options);
            var result = JsonSerializer.Deserialize<DateTimeOffset>(json, options);

            json.Should().Be("\"2024-01-15T10:30:00Z\"");
            result.Should().Be(value);
        }

        [TestMethod]
        public void NullableDateTimeConverter_ReadsNullWithoutThrowing()
        {
            var options = OptionsWith(new NullableDateTimeConverter());

            var result = JsonSerializer.Deserialize<DateTime?>("null", options);
            result.Should().BeNull();
        }

        [TestMethod]
        public void NullableDateTimeConverter_ReadsValue()
        {
            var options = OptionsWith(new NullableDateTimeConverter());

            var result = JsonSerializer.Deserialize<DateTime?>("\"2024-01-15T10:30:00Z\"", options);
            result.Should().Be(new DateTime(2024, 1, 15, 10, 30, 0, DateTimeKind.Utc));
        }

        [TestMethod]
        public void NullableDateTimeOffsetConverter_ReadsNullWithoutThrowing()
        {
            var options = OptionsWith(new NullableDateTimeOffsetConverter());

            var result = JsonSerializer.Deserialize<DateTimeOffset?>("null", options);
            result.Should().BeNull();
        }

        [TestMethod]
        public void NullableDateTimeOffsetConverter_ReadsValue()
        {
            var options = OptionsWith(new NullableDateTimeOffsetConverter());

            var result = JsonSerializer.Deserialize<DateTimeOffset?>("\"2024-01-15T10:30:00Z\"", options);
            result.Should().Be(new DateTimeOffset(2024, 1, 15, 10, 30, 0, TimeSpan.Zero));
        }

        [TestMethod]
        public void FlexibleStringConverter_ReadsNumericToken()
        {
            var options = OptionsWith(new FlexibleStringConverter());

            var result = JsonSerializer.Deserialize<string>("1003", options);
            result.Should().Be("1003");
        }

        [TestMethod]
        public void FlexibleStringConverter_ReadsStringToken()
        {
            var options = OptionsWith(new FlexibleStringConverter());

            var result = JsonSerializer.Deserialize<string>("\"1003\"", options);
            result.Should().Be("1003");
        }

        [TestMethod]
        public void CloudflareApiError_DeserializesNumericErrorCode()
        {
            // Cloudflare's real API returns error codes as JSON numbers, not strings.
            var json = "{\"code\":1003,\"message\":\"Invalid or missing zone id.\"}";

            var error = JsonSerializer.Deserialize<Exceptions.CloudflareApiError>(json);

            error!.Code.Should().Be("1003");
            error.Message.Should().Be("Invalid or missing zone id.");
        }
    }
}
