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
var twitch = new TwitchChatApiClient()      // Create an instance of the chat client
    .SetIdentity(user, token);              // Set the login information

twitch.Connected += OnConnectedAsync;
twitch.MessageReceived += OnMessageReceived;
twitch.UserNoticeReceived += OnUserNoticeReceived;

await twitch.RunAsync();                    // Run the connection loop, the app will lock here until the client is disposed.

void OnConnectedAsync()                     // After connection is confirmed, join a channel
{
    Console.WriteLine("> Connected");
    twitch.Send(new JoinChannelRequest(channelName));
}

void OnMessageReceived(MessageEventArgs args)
{
    var name = !string.IsNullOrWhiteSpace(args.Tags.DisplayName)     // Sometimes twitch doesn't send a display name
        ? args.Tags.DisplayName 
        : args.Tags.Login;

    if (!string.IsNullOrWhiteSpace(args.Tags.ReplyParentMessageId))     // Check if the message is replying to another
        Console.WriteLine($"#{args.ChannelName} {name} -> {args.Tags.ReplyParentUserName}: {args.Message}");
    else
        Console.WriteLine($"#{args.ChannelName} {name}: {args.Message}");
}

void OnUserNoticeReceived(UserNoticeEventArgs args)
{
    var name = !string.IsNullOrWhiteSpace(args.Tags.DisplayName)     // Sometimes twitch doesn't send a display name
        ? args.Tags.DisplayName 
        : args.Tags.Login;

    switch (args.Tags)
    {
        case RaidTags tags:                 // Message indicates a raid
            Console.WriteLine($"#{args.ChannelName} just got raided by {tags.RaiderDisplayName} with {tags.RaiderViewerCount} viewers!");
            break;

        case SubscriptionGiftTags tags:     // Message indicates a gifted sub
            Console.WriteLine($"#{args.ChannelName} {name} just gifted {tags.RecipientDisplayName} a sub!");
            break;

        case SubscriptionTags tags:         // Message indicates a paid sub
            if (tags.NoticeType == UserNoticeType.Subscription)
                Console.WriteLine($"#{args.ChannelName} {name} just subscribed for {tags.TotalMonths} months: {args.Message}");
            else
                Console.WriteLine($"#{args.ChannelName} {name} just resubscribed for {tags.TotalMonths} months: {args.Message}");
            break;

        default:                            // Other non-special notices
            if (args.Tags.NoticeType == UserNoticeType.Announcement)
                Console.WriteLine($"#{args.ChannelName} {name} ANNOUNCES: {args.Message}");
            break;
    }
}
