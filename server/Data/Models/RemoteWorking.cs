using System.Text.Json.Serialization;
using MongoDB.Bson.Serialization.Attributes;
using Reinforced.Typings.Attributes;

[Reinforced.Typings.Attributes.TsClass(IncludeNamespace = true, Namespace = "API")] 
public class RemoteWorking
{
    public enum WorkingType {
        Unknown = 0,
        OnSite = 1,
        Hybrid = 2,
        Remote = 3,
    }

    [JsonConverter(typeof(JsonStringEnumConverter<WorkingType>))]
    [BsonRepresentation(MongoDB.Bson.BsonType.String)]
    public WorkingType WorkType { get; set; } = WorkingType.Unknown;

    public IList<string> WorkRestrictions { get; set; } = new List<string>();

}