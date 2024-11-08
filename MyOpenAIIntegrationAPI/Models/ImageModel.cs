namespace MyOpenAIIntegrationAPI.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class ImageModel
{
    [BsonId]
    public ObjectId Id { get; set; }
    public string Url { get; set; }
    public long Created { get; set; }
    public byte[] Data { get; set; }
    public string RevisedPrompt { get; set; }
}