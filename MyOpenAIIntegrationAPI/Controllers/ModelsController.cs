using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MyOpenAIIntegrationAPI.Models;
using MyOpenAIIntegrationAPI.TemplateClass;

namespace MyOpenAIIntegrationAPI.Controllers;

[Route("v1/models")]
[ApiController]
public class ModelsController(HttpClient httpClient) : ControllerTemplate(httpClient)
{
    [HttpGet()]
    public async Task<IActionResult> GetModels()
    {
        var response = await _httpClient.GetAsync($"{this.BaseUrl}/models");
        if (response.IsSuccessStatusCode)
        {
            var modelsData = JsonSerializer.Deserialize<ModelsResponse>(await response.Content.ReadAsStringAsync());
            var models = modelsData?.data.Select(m => m.id).ToList();
            return Ok(models);
        }
        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStreamAsync());
    }
}