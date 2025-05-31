using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Namerd.Persistence.Context;
using Namerd.Persistence.Repository;
using Namerd.Services;
using NetCord;
using NetCord.Gateway;
using NetCord.Hosting.Gateway;
using NetCord.Hosting.Services;
using NetCord.Hosting.Services.ApplicationCommands;
using NetCord.Hosting.Services.ComponentInteractions;
using NetCord.Services.ApplicationCommands;
using NetCord.Services.ComponentInteractions;

var builder = Host.CreateApplicationBuilder(args);


builder.Configuration
    .AddEnvironmentVariables()
    .AddUserSecrets<Program>(optional: true);
var dbConnectionString = builder.Configuration["DbConnectionString"];

builder.Services.Configure<GatewayClientOptions>(builder.Configuration.GetSection("DevToken"));
builder.Services.AddDbContext<NamerdContext>(options => options.UseNpgsql(dbConnectionString));

builder.Services.AddDiscordGateway(options => { options.Intents = GatewayIntents.All; });

builder.Services.AddComponentInteractions<ButtonInteraction, ButtonInteractionContext>();
builder.Services.AddComponentInteractions<StringMenuInteraction, StringMenuInteractionContext>();
builder.Services.AddComponentInteractions<UserMenuInteraction, UserMenuInteractionContext>();
builder.Services.AddComponentInteractions<RoleMenuInteraction, RoleMenuInteractionContext>();
builder.Services.AddComponentInteractions<MentionableMenuInteraction, MentionableMenuInteractionContext>();
builder.Services.AddComponentInteractions<ChannelMenuInteraction, ChannelMenuInteractionContext>();
builder.Services.AddComponentInteractions<ModalInteraction, ModalInteractionContext>();

builder.Services.AddApplicationCommands<ApplicationCommandInteraction, ApplicationCommandContext>();

builder.Services.AddScoped<SettingsService>();
builder.Services.AddScoped<BotRepository>();
builder.Services.AddScoped<NominationService>();
builder.Services.AddScoped<NominationRepository>();

builder.Services.AddGatewayEventHandlers(typeof(Program).Assembly);

builder.Services.AddHangfire(configuration => configuration
    .UsePostgreSqlStorage(options => options
        .UseNpgsqlConnection(() => dbConnectionString)));
builder.Services.AddHangfireServer();

var host = builder.Build();


host.AddModules(typeof(Program).Assembly);

host.UseGatewayEventHandlers();

await host.RunAsync();
