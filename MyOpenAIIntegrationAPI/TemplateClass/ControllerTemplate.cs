using System.Net.Http.Headers;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MyOpenAIIntegrationAPI.Models;

namespace MyOpenAIIntegrationAPI.TemplateClass;

public class ControllerTemplate : ControllerBase
{
    protected readonly HttpClient _httpClient;
    protected readonly string? BaseUrl = Environment.GetEnvironmentVariable("OPENAI_BASE_URL");
    public ControllerTemplate(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", Environment.GetEnvironmentVariable("OPENAI_API_KEY"));
    }

    protected async Task<List<string>> GetValidModels()
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
                    return new List<string>();
                }
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"Deserialization error: {ex.Message}");
                return new List<string>();
            }
        }
        return new List<string>();
    }
}