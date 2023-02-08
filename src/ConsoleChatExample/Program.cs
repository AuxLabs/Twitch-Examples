using AuxLabs.Twitch.Chat;

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

// Create an instance of the chat client
var twitch = new TwitchChatClient();

twitch.Connected += OnConnectedAsync;
twitch.MessageReceived += OnMessageReceivedAsync;

// Validate your token with the twitch servers.
await twitch.ValidateAsync(token);

// Run the connection loop, the app will lock here until the client is disposed.
await twitch.RunAsync();

// After connection is confirmed, join a channel
async Task OnConnectedAsync()
{
    Console.WriteLine("> Connected");
    var channel =  await twitch.JoinChannelAsync(channelName);
    Console.WriteLine($"> Joined {channel.Name}");
}

// Handle when a message is received
Task OnMessageReceivedAsync(ChatMessage msg)
{
    // Check if the message is replying to another
    if (msg.Reply != null)
        Console.WriteLine($"#{msg.Channel.Name} {msg.Author.DisplayName} -> {msg.Reply.Author.DisplayName}: {msg.Content}");
    else
        Console.WriteLine($"#{msg.Channel.Name} {msg.Author.DisplayName}: {msg.Content}");
    return Task.CompletedTask;
}