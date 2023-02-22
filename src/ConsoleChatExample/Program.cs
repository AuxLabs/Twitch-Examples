using AuxLabs.SimpleTwitch.Chat;
using AuxLabs.SimpleTwitch.Rest;

var user = Environment.GetEnvironmentVariable("TWITCH_USER", EnvironmentVariableTarget.User);
var token = Environment.GetEnvironmentVariable("TWITCH_TOKEN", EnvironmentVariableTarget.User);

Console.WriteLine("> Initializing chat client...");
if (token == null)
{
    Console.WriteLine("> Please enter your oauth token: ");
    token = Console.ReadLine();
}

Console.WriteLine("> Please enter the channel name to join: ");
var channelName = Console.ReadLine();

Console.WriteLine("> Connecting...");

// Create an instance of the rest client and validate provided token
var rest = new TwitchRestApiClient();
var identity = await rest.ValidateAsync(token);

// Create an instance of the chat client and set identity
var chat = new TwitchChatApiClient();
chat.SetIdentity(identity.UserName, token);

chat.Connected += OnConnected;
chat.MessageReceived += OnMessageReceived;

// Run the connection loop, the app will lock here until the client is disposed.
await chat.RunAsync();

// After connection is confirmed, join a channel
void OnConnected()                     
{
    Console.WriteLine("> Connected");
    chat.Send(new JoinChannelRequest(channelName));
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
