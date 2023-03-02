using AuxLabs.SimpleTwitch.Chat;
using AuxLabs.SimpleTwitch.Rest;
using Examples;

Console.WriteLine("> Initializing chat client...");
var token = ExampleHelper.GetOrRequestToken();
var channelName = ExampleHelper.RequestValue("> Please enter the channel name to join: ");

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
    chat.SendJoin(channelName);
}

// Handle when a message is received
void OnMessageReceived(MessageEventArgs args)
{
    // Check if the message is replying to another
    if (args.Tags.ReplyParentMessageId != null)     
        Console.WriteLine($"#{args.ChannelName} {args.Tags.DisplayName} -> {args.Tags.ReplyParentUserName}: {args.Message}");
    else
        Console.WriteLine($"#{args.ChannelName} {args.Tags.DisplayName}: {args.Message}");
}
