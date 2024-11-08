using Microsoft.AspNetCore.Mvc;
using MyOpenAIIntegrationAPI.Models;
using MyOpenAIIntegrationAPI.TemplateClass;

namespace MyOpenAIIntegrationAPI.Controllers;

[Route("v1/chat")]
[ApiController]
public class ChatController(HttpClient httpClient) : ControllerTemplate(httpClient)
{
    [HttpPost("completions")]
    public async Task<IActionResult> GenerateText([FromBody] ChatRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{this.BaseUrl}/chat/completions", request);
        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStreamAsync());
    }
}