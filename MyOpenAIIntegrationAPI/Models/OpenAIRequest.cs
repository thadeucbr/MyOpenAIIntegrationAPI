namespace MyOpenAIIntegrationAPI.Models;

public class OpenAIRequest
{
    public string Model { get; set; }
    public string Prompt { get; set; }
    public int MaxTokens { get; set; }
    public double Temperature { get; set; }
}