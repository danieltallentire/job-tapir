using MongoDB.Driver;
public static class IListStringTagExtensions
{
    public static async Task<bool> AllTagsExist(this IList<string> list, IMongoCollection<Tag> tags)
    {
        if (list == null || list.Count == 0) return true;

        Console.WriteLine(string.Join(",", list));

        var filter = Builders<Tag>.Filter.In("Id", list);
        var matchCount = await tags.CountDocumentsAsync(filter);

        return matchCount == list.Count;
    }
}