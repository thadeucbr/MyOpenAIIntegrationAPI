using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MyOpenAIIntegrationAPI.Models;
using MyOpenAIIntegrationAPI.TemplateClass;

namespace MyOpenAIIntegrationAPI.Controllers;

[Route("v1/models")]
[ApiController]
public class ModelsController : ControllerTemplate
{
    public ModelsController(HttpClient httpClient) : base(httpClient)
    {
    }

    [HttpGet()]
    public async Task<IActionResult> GetModels()
    {
        var response = await _httpClient.GetAsync("https://api.openai.com/v1/models");
        if (response.IsSuccessStatusCode)
        {
            var modelsData = JsonSerializer.Deserialize<ModelsResponse>(await response.Content.ReadAsStringAsync());
                var models = modelsData.data.Select(m => m.id).ToList();
            return Ok(models);
        }

        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStreamAsync());
    }
}