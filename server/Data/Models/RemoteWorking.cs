using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public class RemoteWorking
{
    public enum WorkingType {
        Unknown = 0,
        OnSite = 1,
        Hybrid = 2,
        Remote = 3,
    }

    [JsonConverter(typeof(StringEnumConverter))]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public WorkingType WorkType { get; set; } = WorkingType.Unknown;

    public IList<string> WorkRestrictions { get; set; } = new List<string>();

}