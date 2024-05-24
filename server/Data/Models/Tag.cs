
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

public class Tag 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string Name { get; set;} = string.Empty;

    public string Category { get; set;} = string.Empty;
}