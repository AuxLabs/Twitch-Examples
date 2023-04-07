using AuxLabs.Twitch.Chat.Api;
using AuxLabs.Twitch.Rest.Api;
using AuxLabs.Twitch.Rest.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EchoBotHostedExample.Services
{
    public class TwitchHostedService : IHostedService
    {
        private readonly TwitchRestApiClient _rest;
        private readonly TwitchChatApiClient _chat;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public TwitchHostedService(TwitchRestApiClient rest, TwitchChatApiClient chat, IConfiguration config, ILogger<TwitchHostedService> logger)
        {
            _rest = rest;
            _chat = chat;
            _config = config;
            _logger = logger;

            _chat.Connected += OnConnected;
            _chat.ChannelJoined += (args) => _logger.LogInformation($"Joined channel #{args.ChannelName}");
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Connecting...");
            var token = _config["token"];
            var identity = await _rest.ValidateAsync(_config["token"]);
            _logger.LogInformation($"Validated token for {identity.UserName} ({identity.UserId})");

            _chat.WithIdentity(identity.UserName, token);

            _logger.LogInformation("Starting background service");
            _ = _chat.RunAsync().WaitAsync(cancellationToken);
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _chat.Dispose();
            return Task.CompletedTask;
        }

        private void OnConnected()
        {
            _logger.LogInformation($"Connected");

            var identity = (UserIdentity)_rest.Identity;
            _chat.SendJoin(identity.UserName);
        }
    }
}
