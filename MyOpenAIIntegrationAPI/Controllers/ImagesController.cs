using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MyOpenAIIntegrationAPI.Models;
using MyOpenAIIntegrationAPI.TemplateClass;
using MongoDB.Bson;
using System.IO;
using MyOpenAIIntegrationAPI.Database;

namespace MyOpenAIIntegrationAPI.Controllers;

[Route("v1/images")]
[ApiController]
public class ImagesController : ControllerTemplate
{
    private readonly ImageRepository _imageRepository;

    public ImagesController(HttpClient httpClient, ImageRepository imageRepository) : base(httpClient)
    {
        _imageRepository = imageRepository;
    }

    [HttpPost("generations")]
    public async Task<IActionResult> GenerateImage([FromBody] GenerateImageRequest request)
    {
        var response = await _httpClient.PostAsJsonAsync($"{this.BaseUrl}/images/generations", request);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            var imageResponse = JsonSerializer.Deserialize<GenerateImageResponse>(content);
            var imageUrl = imageResponse?.data[0].url;
            var createdTimestamp = imageResponse?.created;
            var revisedPrompt = imageResponse?.data[0].revised_prompt;
            
            using (var imageClient = new HttpClient())
            {
                await using (var imageResponseStream = await imageClient.GetStreamAsync(imageUrl))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await imageResponseStream.CopyToAsync(memoryStream);
                        var imageData = memoryStream.ToArray();
                        
                        var imageModel = new ImageModel
                        {
                            Url = imageUrl,
                            Created = createdTimestamp ?? 0,
                            Data = imageData,
                            RevisedPrompt = revisedPrompt
                        };
                        await _imageRepository.CreateAsync(imageModel);
                    }
                }
            }
        }

        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }

    [HttpGet()]
    public async Task<IActionResult> ListImages()
    {
        var images = await _imageRepository.GetAllAsync();
        var response = images.Select(image => new
        {
            Id = image.Created,
            RevisedPrompt = image.RevisedPrompt
        }).ToList();
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetImageById(long id)
    {
        Console.WriteLine(id);
        var image = await _imageRepository.GetByTimestampAsync(id);
        if (image == null)
        {
            return NotFound("Image not found.");
        }

        return File(image.Data, "image/jpeg");
    }
}
