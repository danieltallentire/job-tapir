using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

public partial class Role
{

    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = string.Empty;

    [BsonIgnoreIfNull]
    public Company? Company{ get; set; } = null;

    [BsonRepresentation(BsonType.ObjectId)]
    public string CompanyId {get; set;} = string.Empty;

    public Source? Source { get; set; } = null;

    public string Title { get; set; } = string.Empty;

    public string URL { get; set; } = string.Empty;

    public bool HasAdvertisedSalary {get; set;} = false;

    public int AdvertisedSalaryFloor { get; set; } = 0;

    public int AdvertisedSalaryCeiling {get; set; } = 0;

    public RemoteWorking RemoteWorking {get; set; } = new RemoteWorking();

    [JsonConverter(typeof(StringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public ApplicationState State { get; set;} = ApplicationState.NotApplied;

    public DateTime AddedAt = DateTime.MinValue;

    public DateTime AppliedAt = DateTime.MinValue;

    public DateTime LastResponseAt = DateTime.MinValue;

    public int NumStages { get; set; } = 0;

    public int CurrentStage { get; set; } = 0;

    [JsonConverter(typeof(StringEnumConverter))]
    [BsonRepresentation(BsonType.String)]
    public ApplicationStage Stage { get; set; } = ApplicationStage.None;

    public IList<string> Tags{ get; set; } = new List<string>();

    public IList<string> Contacts {get;set; } = new List<string>();

    public string Notes { get; set; } = string.Empty;

    public enum ApplicationStage {
        None = 0,
        Screening = 1,
        Technical = 2,
        Final = 3,
    }

    public enum ApplicationState {
        NotApplied = 0,
        Applied = 1,
        Withdrawn = 2,
        Rejected = 3,
    }
}