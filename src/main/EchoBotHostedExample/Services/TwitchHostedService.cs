using AuxLabs.Twitch.Chat;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EchoBotHostedExample.Services
{
    public class TwitchHostedService : IHostedService
    {
        private readonly TwitchChatClient _twitch;
        private readonly IConfiguration _config;
        private readonly ILogger _logger;

        public TwitchHostedService(TwitchChatClient twitch, IConfiguration config, ILogger<TwitchHostedService> logger)
        {
            _twitch = twitch;
            _config = config;
            _logger = logger;

            _twitch.Connected += OnConnectedAsync;
            _twitch.ChannelJoined += channelName => 
            {
                _logger.LogInformation($"Joined channel #{channelName}");
                return Task.CompletedTask;
            };
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Connecting...");

            await _twitch.ValidateAsync(_config["token"]);
            await _twitch.RunAsync();
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            //await _twitch.StopAsync();
            return Task.CompletedTask;
        }

        private async Task OnConnectedAsync()
        {
            _logger.LogInformation("Connected");
            await _twitch.JoinMyChannelAsync();
        }
    }
}
