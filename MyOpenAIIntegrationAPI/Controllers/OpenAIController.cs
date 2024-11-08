using System.Net;
using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MyOpenAIIntegrationAPI.Models;

namespace MyOpenAIIntegrationAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OpenAiController : ControllerBase
{
    private readonly HttpClient _httpClient;

    private async Task<List<string>> GetValidModels()
    {
        var response = await _httpClient.GetAsync("https://api.openai.com/v1/models");
        if (response.IsSuccessStatusCode)
        {
            var modelsResponse = await response.Content.ReadAsStringAsync();
        
            try
            {
                var modelsData = JsonSerializer.Deserialize<ModelsResponse>(modelsResponse);
                if (modelsData?.data != null)
                {
                    return modelsData.data.Select(m => m.id).ToList();
                }
                else
                {
                    // Se modelsData.Data for nulo, retorne uma lista vazia
                    return new List<string>();
                }
            }
            catch (JsonException ex)
            {
                // Logar o erro se necessário
                Console.WriteLine($"Deserialization error: {ex.Message}");
                return new List<string>();
            }
        }
        return new List<string>(); // Retorna uma lista vazia se a requisição falhar
    }

    
    public OpenAiController(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
    }

    [HttpGet("models")]
    public async Task<IActionResult> GetModels()
    {
        var models = await GetValidModels();
        return Ok(models);
    }
    [HttpPost("completions")]
    public async Task<IActionResult> GenerateText([FromBody] OpenAICreateCompletionRequest request)
    {
        var validModels = await GetValidModels();
        if (validModels.Count < 1)
        {
            return NotFound("Models are not available, try again later.");
        }
        if (!validModels.Contains(request.Model))
        {
            return BadRequest("Invalid model specified.");
        }

        if (string.IsNullOrWhiteSpace(request.Prompt) || request.Prompt.Length > 4000)
        {
            return BadRequest("Prompt must be provided and must not exceed 4000 characters.");
        }
        
        var requestData = new
        {
            model = request.Model,
            prompt = request.Prompt,
            max_tokens = request.MaxTokens > 0 ? request.MaxTokens : 200,
            temperature = request.Temperature >= 0 ? request.Temperature : 0.7
        };
        var response = await _httpClient.PostAsJsonAsync("https://api.openai.com/v1/completions", requestData);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            return Ok(result);
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        return BadRequest(response.Content.ReadAsStringAsync());
    }
}