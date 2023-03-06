using AuxLabs.SimpleTwitch.Chat;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace EchoBotExample.Services
{
    public class EchoService : IHostedService
    {
        private readonly TwitchChatApiClient _chat;
        private readonly ILogger _logger;

        public EchoService(TwitchChatApiClient chat, ILogger<EchoService> logger)
        {
            _chat = chat;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _chat.MessageReceived += OnMessageReceived;
            _logger.LogInformation("Started");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _chat.MessageReceived -= OnMessageReceived;
            _logger.LogInformation("Stopped");
            return Task.CompletedTask;
        }

        private void OnMessageReceived(MessageEventArgs args)
        {
            _logger.LogInformation($"Replying to {args.UserName} with {args.Message}");
            _chat.SendChannelMessage(args.ChannelName, args.Message, args.Tags.Id);
        }
    }
}
