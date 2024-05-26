using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;

namespace job_tapir.Api;

public static class Roles {

  public static void MapRoutes(IEndpointRouteBuilder app)
  {

    app.MapPost("/roles", async Task<Results<Ok<Role>, ValidationProblem>>
        (IMongoCollection<Role> collection, 
        IMongoCollection<Company> companies, 
        IMongoCollection<Tag> tags, 
        Role role) => 
        {
        role.Id = string.Empty;

        if (role.Company == null && string.IsNullOrEmpty(role.CompanyId)) return TypedResults.ValidationProblem(new Dictionary<string, string[]>{{ "company", ["Missing company or company id"]}});

        if (!await role.Tags.AllTagsExist(tags)) return TypedResults.ValidationProblem(new Dictionary<string, string[]> {{ "Tags", ["Invalid tag id assigned"] }});
        
        if (string.IsNullOrEmpty(role.CompanyId) && role.Company != null )
        {
          // let us create the company
          if (!await role.Company.Tags.AllTagsExist(tags)) return TypedResults.ValidationProblem(new Dictionary<string, string[]> {{ "Tags", ["Invalid tag id assigned"] }});
          companies.InsertOne(role.Company);
          role.CompanyId = role.Company.Id;
        }

        await collection.InsertOneAsync(role);
        return TypedResults.Ok(role);
    });


  app.MapGet("/roles", async (IMongoCollection<Role> collection, 
                              IMongoCollection<Company> companies) =>
            TypedResults.Ok( await collection.Aggregate().Match(Builders<Role>.Filter.Where(_ => true)).Lookup(companies,
                r => r.CompanyId,
                c => c.Id,
                (Role r) => r.Company)
            .Unwind(r => r.Company, new AggregateUnwindOptions<Role>() { PreserveNullAndEmptyArrays = true })
            .ToListAsync()));
  }

  

}