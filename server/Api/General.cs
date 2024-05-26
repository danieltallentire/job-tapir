
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;

namespace job_tapir.Api;

public static class Endpoints {
     public static void MapRoutes(IEndpointRouteBuilder app)
  {
    app.MapGet("/companies", async (IMongoCollection<Company> collection)
  => TypedResults.Ok(await collection.Find(Builders<Company>.Filter.Empty).ToListAsync()));
 
app.MapGet("/tags", async (IMongoCollection<Tag> collection)
  => TypedResults.Ok(await collection.Find(Builders<Tag>.Filter.Empty).ToListAsync()));

app.MapPost("/companies", async Task<Results<Ok<Company>, ValidationProblem>> (IMongoCollection<Company> collection, IMongoCollection<Tag> tags, Company company)
=> {
    if (string.IsNullOrEmpty(company.Name)) return TypedResults.ValidationProblem(new Dictionary<string, string[]> {{ "Name", ["Name must be specified"] }});
    if (!await company.Tags.AllTagsExist(tags)) return TypedResults.ValidationProblem(new Dictionary<string, string[]> {{ "Tags", ["Invalid tag id assigned"] }});

    // check for duplicates

    company.Id = string.Empty;
    await collection.InsertOneAsync(company);
    return TypedResults.Ok(company);
});

app.MapPost("/tags", async (IMongoCollection<Tag> collection, Tag tag)
=> {
    // check for duplicates

    tag.Id = string.Empty;
    await collection.InsertOneAsync(tag);
    return TypedResults.Ok(tag);
});

  }
}