using FIAP.CloudGames.Api.Extensions;
using MongoDB.Driver;
using MongoDB.Driver.Core.Configuration;
using Serilog;
using Serilog.Events;

var builder = WebApplication.CreateBuilder(args);
BuilderExtension.AddProjectServices(builder);

var app = builder.Build();

AppExtension.UseProjectConfiguration(app);

await app.RunAsync();