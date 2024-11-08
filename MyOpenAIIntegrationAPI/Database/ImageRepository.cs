using MongoDB.Bson;
using MyOpenAIIntegrationAPI.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MyOpenAIIntegrationAPI.Database;

public class ImageRepository
{
    private readonly IMongoCollection<ImageModel> _images;

    public ImageRepository(IMongoClient mongoClient, string databaseName)
    {
        var database = mongoClient.GetDatabase(databaseName);
        _images = database.GetCollection<ImageModel>("images");
    }

    public async Task<List<ImageModel>> GetAllAsync() =>
        await _images.Find(new BsonDocument()).ToListAsync();

    public async Task<ImageModel> GetAsync(ObjectId id) =>
        await _images.Find(x => x.Id == id).FirstOrDefaultAsync();

    public async Task CreateAsync(ImageModel image) =>
        await _images.InsertOneAsync(image);
    
    public async Task<ImageModel> GetByTimestampAsync(long timestamp)
    {
        return await _images.Find(x => x.Created == timestamp).FirstOrDefaultAsync();
    }
}
