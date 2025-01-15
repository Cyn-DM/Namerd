using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Namerd.Services;
using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Services.ApplicationCommands;

var builder = Host.CreateApplicationBuilder(args);

builder.Configuration
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>();

builder.Services.Configure<GatewayClientOptions>(builder.Configuration.GetSection("Token"));

builder.Services.AddDiscordGateway(options => { options.Intents = GatewayIntents.All; })
    .AddApplicationCommands<ApplicationCommandInteraction, ApplicationCommandContext>()
    .AddGatewayEventHandlers(typeof(Program).Assembly);


var host = builder.Build();

host.AddModules(typeof(Program).Assembly);

host.UseGatewayEventHandlers();

await host.RunAsync();
