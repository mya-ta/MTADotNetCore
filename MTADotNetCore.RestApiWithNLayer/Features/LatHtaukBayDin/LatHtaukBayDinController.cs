using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        return Ok(model.questions);
    }

    [HttpGet("numberList")]
    public async Task<IActionResult> NumberList()
    {
        var model = await GetDataAsync();
        return Ok(model.numberList);
    }

    [HttpGet("{questionNo}/{no}")]
    public async Task<IActionResult> Answer(int questionNo, int no)
    {
        var model = await GetDataAsync();
        return Ok(model.answers.FirstOrDefault(x => x.questionNo == questionNo
        && x.answerNo == no));
    }
}

//static string ToNumber(string num)
//{
//    num = num.Replace("၀", "0");
//    num = num.Replace("၁", "1");
//    return num;
//test}

public class LatHtaukBayDin
{
    public Question[] questions { get; set; }
    public Answer[] answers { get; set; }
    public string[] numberList { get; set; }
}

public class Question
{
    public int questionNo { get; set; }
    public string questionName { get; set; }
}

public class Answer
{
    public int questionNo { get; set; }
    public int answerNo { get; set; }
    public string answerResult { get; set; }
}