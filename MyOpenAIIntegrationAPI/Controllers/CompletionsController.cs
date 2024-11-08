﻿using System.Net;
using Microsoft.AspNetCore.Mvc;
using MyOpenAIIntegrationAPI.Models;
using MyOpenAIIntegrationAPI.TemplateClass;

namespace MyOpenAIIntegrationAPI.Controllers;

[Route("v1/completions")]
[ApiController]
public class CompletionsController(HttpClient httpClient) : ControllerTemplate(httpClient)
{
    [HttpPost()]
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
        var response = await _httpClient.PostAsJsonAsync($"{this.BaseUrl}/completions", requestData);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadAsStringAsync();
            return Ok(result);
        }

        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return Unauthorized();
        }
        return BadRequest(await response.Content.ReadAsStringAsync());
    }
}