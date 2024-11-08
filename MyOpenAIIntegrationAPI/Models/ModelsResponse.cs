namespace MyOpenAIIntegrationAPI.Models;

public class ModelsResponseData
{
    public string id { get; set; }
    public int created { get; set; }
}
public class ModelsResponse
{
    public List<ModelsResponseData> data { get; set; }
}