namespace Tempus.Utils.AspNetCore
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using Tempus.Utils;

    public class TimeSpanConverter : JsonConverter
    {
        private const string DefaultTimeFormat = @"hh\:mm";
        private string _timeFormat;
        private CultureInfo _culture;

        public CultureInfo Culture
        {
            get { return _culture ?? CultureInfo.CurrentCulture; }
            set { _culture = value; }
        }

        public string TimeFormat
        {
            get { return _timeFormat ?? string.Empty; }
            set { _timeFormat = (string.IsNullOrEmpty(value)) ? null : value; }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(TimeSpan) || objectType == typeof(TimeSpan?);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.Null)
                return null;

            bool isNullable = ReflectionUtils.IsNullableType(objectType);

            if (reader.TokenType == JsonToken.Integer)
                return TimeSpan.FromTicks((long)reader.Value);

            if (reader.TokenType != JsonToken.String)
                return new JsonSerializationException($"Unexpected token parsing date. Expected String, got {reader.TokenType}.");

            string timeText = reader.Value.ToString();

            if (string.IsNullOrEmpty(timeText) && isNullable)
                return null;

            if (!string.IsNullOrEmpty(_timeFormat))
                return TimeSpan.ParseExact(timeText, _timeFormat, Culture);
            else
                return TimeSpan.Parse(timeText, Culture);
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            string text;

            if (value is TimeSpan)
            {
                var time = (TimeSpan)value;

                text = time.ToString(_timeFormat ?? DefaultTimeFormat, Culture);
            }
            else
            {
                throw new JsonSerializationException($"Unexpected value when converting date. Expected DateTime or DateTimeOffset, got {ReflectionUtils.GetObjectType(value)}.");
            }

            writer.WriteValue(text);
        }
    }
}
