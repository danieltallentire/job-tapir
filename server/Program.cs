using job_tapir.Data;
using Microsoft.AspNetCore.Http.HttpResults;
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
job_tapir.Api.Endpoints.MapRoutes(app);
job_tapir.Api.Roles.MapRoutes(app);


app.Run();

//this is for tests
public partial class Program { }