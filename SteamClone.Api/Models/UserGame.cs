using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace SteamClone.Api.Models;

public class UserGame
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]

    public string Id { get; set; } = null!;
    public string UserId { get; set; } = null!;
    public string GameId { get; set; } = null!;
    public DateTime PurchasedDate { get; set; }
}
