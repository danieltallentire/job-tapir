using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Reinforced.Typings.Attributes;

[Reinforced.Typings.Attributes.TsClass(IncludeNamespace = true, Namespace = "API")] 
public class Company
{
    public Company()
    {

    }


    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;

    public string Country {get; set; }  = string.Empty;

    public string City { get; set; } = string.Empty;

    public IList<string> Contacts {get;set; } = new List<string>();

    public IList<string> Tags{ get; set; } = new List<string>();
}