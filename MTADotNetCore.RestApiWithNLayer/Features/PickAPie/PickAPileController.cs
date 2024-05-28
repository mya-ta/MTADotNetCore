using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MTADotNetCore.RestApiWithNLayer.Features.PickAPie;

[Route("api/[controller]")]
[ApiController]
public class PickAPileController : ControllerBase
{
    private async Task<PickAPile> GetDataAsync()
    {
        string jsonStr = await System.IO.File.ReadAllTextAsync("PickAPile.json");
        var model = JsonConvert.DeserializeObject<PickAPile>(jsonStr);
        return model;
    }

    [HttpGet("questions")]
    public async Task<IActionResult> Questions()
    {
        var model = await GetDataAsync();
        return Ok(model.Questions);
    }
}

public partial class PickAPile
{
    [JsonProperty("Questions")]
    public Question[] Questions { get; set; }

    [JsonProperty("Answers")]
    public Answer[] Answers { get; set; }
}

public partial class Answer
{
    [JsonProperty("AnswerId")]
    public long AnswerId { get; set; }

    [JsonProperty("AnswerImageUrl")]
    public string AnswerImageUrl { get; set; }

    [JsonProperty("AnswerName")]
    public AnswerName AnswerName { get; set; }

    [JsonProperty("AnswerDesp")]
    public string AnswerDesp { get; set; }

    [JsonProperty("QuestionId")]
    public long QuestionId { get; set; }
}

public partial class Question
{
    [JsonProperty("QuestionId")]
    public long QuestionId { get; set; }

    [JsonProperty("QuestionName")]
    public string QuestionName { get; set; }

    [JsonProperty("QuestionDesp")]
    public string QuestionDesp { get; set; }
}

public enum AnswerName { Pile1, Pile2, Pile3, Pile4 };

public partial class PickAPile
{
    public static PickAPile FromJson(string json) => JsonConvert.DeserializeObject<PickAPile>(json, Converter.Settings);
}

public static class Serialize
{
    public static string ToJson(this PickAPile self) => JsonConvert.SerializeObject(self, PickAPie.Converter.Settings);
}

internal static class Converter
{
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters =
            {
                AnswerNameConverter.Singleton,
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
    };
}

internal class AnswerNameConverter : JsonConverter
{
    public override bool CanConvert(Type t) => t == typeof(AnswerName) || t == typeof(AnswerName?);

    public override object ReadJson(JsonReader reader, Type t, object existingValue, JsonSerializer serializer)
    {
        if (reader.TokenType == JsonToken.Null) return null;
        var value = serializer.Deserialize<string>(reader);
        switch (value)
        {
            case "Pile-1 ":
                return AnswerName.Pile1;
            case "Pile-2":
                return AnswerName.Pile2;
            case "Pile-3":
                return AnswerName.Pile3;
            case "Pile-4":
                return AnswerName.Pile4;
        }
        throw new Exception("Cannot unmarshal type AnswerName");
    }

    public override void WriteJson(JsonWriter writer, object untypedValue, JsonSerializer serializer)
    {
        if (untypedValue == null)
        {
            serializer.Serialize(writer, null);
            return;
        }
        var value = (AnswerName)untypedValue;
        switch (value)
        {
            case AnswerName.Pile1:
                serializer.Serialize(writer, "Pile-1 ");
                return;
            case AnswerName.Pile2:
                serializer.Serialize(writer, "Pile-2");
                return;
            case AnswerName.Pile3:
                serializer.Serialize(writer, "Pile-3");
                return;
            case AnswerName.Pile4:
                serializer.Serialize(writer, "Pile-4");
                return;
        }
        throw new Exception("Cannot marshal type AnswerName");
    }

    public static readonly AnswerNameConverter Singleton = new AnswerNameConverter();
}