using ConvertTextInAudio;
using ConvertTextInAudio.Externsions;

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<Worker>();

BuilderExtension.AddServices(builder.Services);

var host = builder.Build();
await host.RunAsync();
