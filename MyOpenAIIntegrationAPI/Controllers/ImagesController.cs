using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MyOpenAIIntegrationAPI.Models;
using MyOpenAIIntegrationAPI.TemplateClass;

namespace MyOpenAIIntegrationAPI.Controllers;

[Route("v1/images")]
[ApiController]
public class ImagesController : ControllerTemplate
{
    private const string ImagesFolder = "assets/images";

    public ImagesController(HttpClient httpClient) : base(httpClient)
    {
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
            
            using (var imageClient = new HttpClient())
            {
                await using (var imageResponseStream = await imageClient.GetStreamAsync(imageUrl))
                {
                    if (!Directory.Exists(ImagesFolder))
                    {
                        Directory.CreateDirectory(ImagesFolder);
                    }

                    var fileName = $"{createdTimestamp}.jpg";
                    var filePath = Path.Combine(ImagesFolder, fileName);

                    await using (var fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write, FileShare.None))
                    {
                        await imageResponseStream.CopyToAsync(fileStream);
                    }
                }
            }
        }
        return StatusCode((int)response.StatusCode, await response.Content.ReadAsStringAsync());
    }
}
