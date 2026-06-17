using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace SteamClone.Api.Models;

public class Game
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public decimal Price { get; set; } 
    public string CoverImageUrl { get; set; } = null!; 
}
