using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;

using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

Console.WriteLine("Hello, World!");

string jsonStr = await File.ReadAllTextAsync("data.json");
var model = JsonConvert.DeserializeObject<LatHtaukBayDin>(jsonStr);

Console.WriteLine(jsonStr);

foreach (var question in model.Questions)
{
    Console.WriteLine(question.QuestionNo);
}

Console.ReadLine();

// static int MmNoToEngNo(string num)
// {
//     num = num.Replace("၀", "0");
//     num = num.Replace("၁", "1");
//     num = num.Replace("၂", "2");
//     num = num.Replace("၃", "3");
//     num = num.Replace("၄", "4");
//     num = num.Replace("၅", "5");
//     num = num.Replace("၆", "6");
//     num = num.Replace("၇", "7");
//     num = num.Replace("၈", "8");
//     num = num.Replace("၉", "9");

//     return Convert.ToInt32(num);
// }



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
    public static string ToJson(this LatHtaukBayDin self) => JsonConvert.SerializeObject(self, Converter.Settings);
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