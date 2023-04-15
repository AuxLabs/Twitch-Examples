using AuxLabs.Twitch;
using AuxLabs.Twitch.Chat.Api;
using AuxLabs.Twitch.Chat.Models;
using Examples;

Console.WriteLine("> Initializing chat client...");
var channelName = ExampleHelper.RequestValue("> Please enter the channel name to join: ");

// Create an instance of the chat client and set identity
var chat = new TwitchChatApiClient();

// To identify as anonymous, set the username and token to justinfan<numbers>
var anonymousName = TwitchConstants.AnonymousNamePrefix + 0001;
chat.WithIdentity(anonymousName, anonymousName);

chat.Connected += OnConnected;
chat.MessageReceived += OnMessageReceived;

// Run the connection loop, the app will lock here until the client is disposed.
await chat.RunAsync();
await Task.Delay(-1);

// After connection is confirmed, join a channel
void OnConnected()                     
{
    Console.WriteLine("> Connected");
    chat.SendJoin(channelName);
}

// Handle when a message is received
void OnMessageReceived(MessageEventArgs args)
{
    // Check if the message is replying to another
    if (args.Tags.ReplyAuthorId != null)     
        Console.WriteLine($"#{args.ChannelName} {args.Tags.AuthorDisplayName} -> {args.Tags.ReplyAuthorDisplayName}: {args.Message}");
    else
        Console.WriteLine($"#{args.ChannelName} {args.Tags.AuthorDisplayName}: {args.Message}");
}
