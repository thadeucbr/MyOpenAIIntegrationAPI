namespace MyOpenAIIntegrationAPI.Models;

public class GenerateImageResponse
{
    public long created { get; set; }
    public List<ImageData> data { get; set; }
}

public class ImageData
{
    public string revised_prompt { get; set; }
    public string url { get; set; }
}