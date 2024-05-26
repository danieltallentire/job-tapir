using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Reinforced.Typings.Attributes;

[Reinforced.Typings.Attributes.TsClass(IncludeNamespace = true, Namespace = "API")] 
public class Source 
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;
    public string Name { get; set;} = string.Empty;

    public string URL { get; set;} = string.Empty;
}