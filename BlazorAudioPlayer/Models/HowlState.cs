using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BlazorAudioPlayer.Models
{
    [JsonConverter(typeof(HowlStateConverter))]
    public enum HowlStateEnum
    {
        Unloaded,
        Loading,
        Loaded,
        Unknown
    }

    public static class HowlStateHelper
    {
        public static string ToString(this HowlStateEnum value)
        {
            switch (value)
            {
                case HowlStateEnum.Unloaded:
                    return "unloaded";
                case HowlStateEnum.Loading:
                    return "loading";
                case HowlStateEnum.Loaded:
                    return "loaded";
                default:
                    return "unknown";
            }

            //return value switch
            //{
            //    HowlStateEnum.Unloaded => "unloaded",
            //    HowlStateEnum.Loading => "loading",
            //    HowlStateEnum.Loaded => "loaded",
            //    _ => "unknown"
            //};
        }

        public static HowlStateEnum ToEnum(this string value)
        {
            switch (value)
            {
                case "unloaded":
                    return HowlStateEnum.Unloaded;
                case "loading":
                    return HowlStateEnum.Loading;
                case "loaded":
                    return HowlStateEnum.Loaded;
                default:
                    return HowlStateEnum.Unknown;
            }

            //return value switch
            //{
            //    "unloading" => HowlStateEnum.Unloaded,
            //    "loading" => HowlStateEnum.Loading,
            //    "loaded" => HowlStateEnum.Loaded,
            //    _ => HowlStateEnum.Unknown
            //};
        }
    }

    public class HowlStateConverter : JsonConverter<HowlStateEnum>
    {
        public override HowlStateEnum Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
        {
            return reader.GetString().ToEnum();
        }

        public override void Write(Utf8JsonWriter writer, HowlStateEnum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
