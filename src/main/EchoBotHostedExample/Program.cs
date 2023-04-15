using AuxLabs.Twitch.Chat;
using EchoBotHostedExample.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureHostConfiguration(configure =>
    {
        configure.AddEnvironmentVariables(prefix: "TWITCH_");
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<TwitchChatClient>();
        services.AddHostedService<TwitchHostedService>();
        services.AddHostedService<EchoService>();
    })
    .Build();

await host.RunAsync();