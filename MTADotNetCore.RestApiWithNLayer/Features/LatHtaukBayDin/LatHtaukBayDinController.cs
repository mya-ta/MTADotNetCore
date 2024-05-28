using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using Newtonsoft.Json.Converters;

namespace MTADotNetCore.RestApiWithNLayer.Features.LatHtaukBayDin;

[Route("api/[controller]")]
[ApiController]
public class LatHtaukBayDinController : ControllerBase
{
    private async Task<LatHtaukBayDin> GetDataAsync()
    {
        string jsonStr = await System.IO.File.ReadAllTextAsync("data.json");
        var model = JsonConvert.DeserializeObject<LatHtaukBayDin>(jsonStr);
        return model;
    }

    // api/LatHtaukBayDin/questions
    [HttpGet("questions")]
    public async Task<IActionResult> Questions()
    {
        var model = await GetDataAsync();
        return Ok(model.Questions);
    }

    [HttpGet("numberList")]
    public async Task<IActionResult> NumberList()
    {
        var model = await GetDataAsync();
        return Ok(model.NumberList);
    }

    [HttpGet("{questionNo}/no}")]
    public async Task<IActionResult> Answer(int questionNo, int no)
    {
        var model = await GetDataAsync();
        return Ok(model.Answers.FirstOrDefault(x => x.QuestionNo == questionNo
        && x.AnswerNo == no));
    }
}

public partial class LatHtaukBayDin
{
    [JsonProperty("questions")]
    public Question[] Questions { get; set; }

    [JsonProperty("answers")]
    public Answer[] Answers { get; set; }

    [JsonProperty("numberList")]
    public string[] NumberList { get; set; }
}

public partial class Answer
{
    [JsonProperty("questionNo")]
    public long QuestionNo { get; set; }

    [JsonProperty("answerNo")]
    public long AnswerNo { get; set; }

    [JsonProperty("answerResult")]
    public string AnswerResult { get; set; }
}

public partial class Question
{
    [JsonProperty("questionNo")]
    public long QuestionNo { get; set; }

    [JsonProperty("questionName")]
    public string QuestionName { get; set; }
}

public partial class LatHtaukBayDin
{
    public static LatHtaukBayDin FromJson(string json) => JsonConvert.DeserializeObject<LatHtaukBayDin>(json, Converter.Settings);
}

public static class Serialize
{
    public static string ToJson(this LatHtaukBayDin self) => JsonConvert.SerializeObject(self, Features.LatHtaukBayDin.Converter.Settings);
}

internal static class Converter
{
    public static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        MetadataPropertyHandling = MetadataPropertyHandling.Ignore,
        DateParseHandling = DateParseHandling.None,
        Converters =
            {
                new IsoDateTimeConverter { DateTimeStyles = DateTimeStyles.AssumeUniversal }
            },
    };
}