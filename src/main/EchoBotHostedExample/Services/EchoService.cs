using AuxLabs.Twitch.Chat;
using AuxLabs.Twitch.Chat.Entities;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EchoBotHostedExample.Services
{
    public class EchoService : IHostedService
    {
        private const string EchoCommand = "!echo";

        private readonly TwitchChatClient _twitch;
        private readonly ILogger _logger;

        public EchoService(TwitchChatClient twitch, ILogger<EchoService> logger)
        {
            _twitch = twitch;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _twitch.MessageReceived += OnMessageReceivedAsync;
            _logger.LogInformation("Started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _twitch.MessageReceived -= OnMessageReceivedAsync;
            _logger.LogInformation("Stopped");
            return Task.CompletedTask;
        }

        private async Task OnMessageReceivedAsync(ChatMessage msg)
        {
            if (!msg.Content.StartsWith(EchoCommand))
                return;

            var trimmedMsg = msg.Content[..EchoCommand.Length]?.Trim();
            if (string.IsNullOrWhiteSpace(trimmedMsg))
                return;

            _logger.LogInformation($"Replying to {msg.Author} with {trimmedMsg}");
            await msg.Channel.SendMessageAsync(trimmedMsg);
        }
    }
}
