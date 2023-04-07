﻿using AuxLabs.Twitch.Chat.Api;
using AuxLabs.Twitch.Rest.Api;
using EchoBotHostedExample.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureLogging(builder =>
    {
        builder.AddSimpleConsole();
    })
    .ConfigureHostConfiguration(configure =>
    {
        configure.AddEnvironmentVariables(prefix: "TWITCH_");
    })
    .ConfigureServices(services =>
    {
        services.AddSingleton<TwitchRestApiClient>();
        services.AddSingleton<TwitchChatApiClient>();
        services.AddHostedService<TwitchHostedService>();
        services.AddHostedService<EchoService>();
    })
    .Build();

await host.RunAsync();