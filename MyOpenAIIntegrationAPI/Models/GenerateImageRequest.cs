namespace MyOpenAIIntegrationAPI.Models;

public class GenerateImageRequest
{
    public string prompt { get; set; }
    public int n { get; set; }
    public string size { get; set; }
    public string model { get; set; }
}