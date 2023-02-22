using AuxLabs.SimpleTwitch;
using AuxLabs.SimpleTwitch.Chat;

var user = Environment.GetEnvironmentVariable("TWITCH_USER", EnvironmentVariableTarget.User);
var token = Environment.GetEnvironmentVariable("TWITCH_TOKEN", EnvironmentVariableTarget.User);

Console.WriteLine("> Initializing chat client...");
if (user == null)
{
    Console.WriteLine("> Please enter your username: ");
    user = Console.ReadLine();
}
if (token == null)
{
    Console.WriteLine("> Please enter your oauth token: ");
    token = Console.ReadLine();
}

Console.WriteLine("> Please enter the channel name to join: ");
var channelName = Console.ReadLine();

Console.WriteLine("> Connecting...");

// Create an instance of the chat client and set identity
var twitch = new TwitchChatApiClient();
twitch.SetIdentity(user, token);

twitch.Connected += OnConnected;
twitch.MessageReceived += OnMessageReceived;

// Run the connection loop, the app will lock here until the client is disposed.
await twitch.RunAsync();

// After connection is confirmed, join a channel
void OnConnected()                     
{
    Console.WriteLine("> Connected");
    twitch.Send(new JoinChannelRequest(channelName));
}

// Handle when a message is received
void OnMessageReceived(MessageEventArgs args)
{
    // Check if the message is replying to another
    if (!string.IsNullOrWhiteSpace(args.Tags.ReplyParentMessageId))     
        Console.WriteLine($"#{args.ChannelName} {args.Tags.DisplayName} -> {args.Tags.ReplyParentUserName}: {args.Message}");
    else
        Console.WriteLine($"#{args.ChannelName} {args.Tags.DisplayName}: {args.Message}");
}
