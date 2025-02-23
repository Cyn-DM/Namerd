using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Namerd.Persistence.Context;
using Namerd.Services;
using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Services.ApplicationCommands;
using Quartz;

var builder = Host.CreateApplicationBuilder(args);


builder.Configuration
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true);
var dbConnectionString = builder.Configuration["DbConnectionString"];
builder.Services.AddQuartz(q => q.UseMicrosoftDependencyInjectionJobFactory());
builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);

builder.Services.Configure<GatewayClientOptions>(builder.Configuration.GetSection("Token"));
builder.Services.AddDbContext<NamerdContext>(options => options.UseNpgsql(dbConnectionString));
builder.Services.AddDiscordGateway(options => { options.Intents = GatewayIntents.All; })
    .AddApplicationCommands<ApplicationCommandInteraction, ApplicationCommandContext>()
    .AddGatewayEventHandlers(typeof(Program).Assembly);


var host = builder.Build();

var schedulerFactory = host.Services.GetService<ISchedulerFactory>();
var scheduler = await schedulerFactory.GetScheduler();


host.AddModules(typeof(Program).Assembly);

host.UseGatewayEventHandlers();

await host.RunAsync();
