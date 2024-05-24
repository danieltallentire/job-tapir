using job_tapir.Data;
using MongoDB.Bson;
using MongoDB.Driver;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
 
builder.Services.AddSingleton<MongoClient>(_ => new MongoClient());
builder.Services.AddSingleton<IMongoDatabase>(
    provider => provider.GetRequiredService<MongoClient>().GetDatabase("job-tapir"));
builder.Services.AddSingleton<IMongoCollection<Company>>(
    provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<Company>("companies"));
builder.Services.AddSingleton<IMongoCollection<Tag>>(
    provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<Tag>("tags"));
builder.Services.AddSingleton<IMongoCollection<Role>>(
    provider => provider.GetRequiredService<IMongoDatabase>().GetCollection<Role>("roles"));
 



//this serves our Svelte file
//app.UseDefaultFiles().UseStaticFiles();
//here's a comment to unstkick thing
var app = builder.Build();
app.UseStaticFiles();
app.MapFallbackToFile("index.html");

app.MapGet("/companies", async (IMongoCollection<Company> collection)
  => TypedResults.Ok(await collection.Find(Builders<Company>.Filter.Empty).ToListAsync()));

app.MapGet("/roles", async (IMongoCollection<Role> collection, IMongoCollection<Company> companies)
  => TypedResults.Ok( await collection.Aggregate().Match(Builders<Role>.Filter.Where(_ => true)).Lookup(companies,
                    r => r.CompanyId,
                    c => c.Id,
                    (Role r) => r.Company)
                .Unwind(r => r.Company, new AggregateUnwindOptions<Role>() { PreserveNullAndEmptyArrays = true })
                .ToListAsync()));
  
  
  
  
//   AsQueryable()                    
//                         .Join(companies.AsQueryable(),
//                             role => role.CompanyId,
//                             company => company.Id,
//                             //(role, company) => new { Role = role, Company = company } )));
//                             (role, company) => new Role(role, company) )));

app.MapGet("/tags", async (IMongoCollection<Tag> collection)
  => TypedResults.Ok(await collection.Find(Builders<Tag>.Filter.Empty).ToListAsync()));

app.MapPost("/companies", async (IMongoCollection<Company> collection, Company company)
=> {
    company.Id = string.Empty;
    await collection.InsertOneAsync(company);
    return TypedResults.Ok(company);
});

app.MapPost("/roles", async (IMongoCollection<Role> collection, Role role)
=> {
    role.Id = string.Empty;
    await collection.InsertOneAsync(role);
    return TypedResults.Ok(role);
});

app.MapPost("/tags", async (IMongoCollection<Tag> collection, Tag tag)
=> {
    tag.Id = string.Empty;
    await collection.InsertOneAsync(tag);
    return TypedResults.Ok(tag);
});



var libPath = Path.Combine(app.Environment.WebRootPath, "Content");
var contentLibrary = new ContentLibrary(libPath).Load();
//lock this down as needed
app.UseCors(builder => builder
 .AllowAnyOrigin()
 .AllowAnyMethod()
 .AllowAnyHeader()
);

//load the routes
job_tapir.Api.Content.MapRoutes(app, contentLibrary);


app.Run();

//this is for tests
public partial class Program { }