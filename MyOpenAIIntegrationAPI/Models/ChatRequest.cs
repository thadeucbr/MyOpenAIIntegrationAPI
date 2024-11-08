namespace MyOpenAIIntegrationAPI.Models;

public class Content
{
    public string type { get; set; }
    public string text { get; set; }
    public string image_url { get; set; }
}

public class Messages
{
    public string role { get; set; }
    public List<Content> content { get; set; }
}
public class ChatRequest
{
    public string model { get; set; }
    public List<Messages> messages { get; set; }
    public int temperature { get; set; }
    public int top_p { get; set; }
    public int n { get; set; }
    public bool stream { get; set; }
    public int max_tokens { get; set; }
    public int presencer_penalty { get; set; }
    public int frequency_penalty { get; set; }
}