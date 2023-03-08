using AuxLabs.SimpleTwitch.Chat;
using AuxLabs.SimpleTwitch.Rest;
using Examples;

Console.WriteLine("> Initializing chat client...");
var token = ExampleHelper.GetOrRequestToken();

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
    chat.SendJoin(identity.UserName);
}

// Handle when a message is received
void OnMessageReceived(MessageEventArgs args)
{
    Console.WriteLine($"Replying to {args.UserName} with {args.Message}");
    chat.SendMessage(args.ChannelName, args.Message, args.Tags.Id);
}
