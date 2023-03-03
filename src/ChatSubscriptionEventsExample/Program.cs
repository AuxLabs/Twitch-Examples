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
chat.UserNoticeReceived += OnUserNoticeReceived;

// Run the connection loop, the app will lock here until the client is disposed.
await chat.RunAsync();

// After connection is confirmed, join a channel
void OnConnected()
{
    Console.WriteLine("> Connected");
    chat.SendJoin(channelName);
}

// Handle whenever a user notice event is received
// This event can contain info for bits badges, raids, rituals, and subscriptions depending on tag contents.
void OnUserNoticeReceived(UserNoticeEventArgs args)
{
    switch (args.Tags)
    {
        // Regular subs
        case SubscriptionTags sub:
            Console.WriteLine($"> {sub.DisplayName} subscribed at {sub.SubscriptionType} for {sub.TotalMonths} month(s)!");
            if (sub.IsStreakShared)
                Console.WriteLine($"> Their sub streak is {sub.StreakMonths} month(s)!");
            break;

        // Sub gifted
        case SubscriptionGiftTags subgift:
            Console.WriteLine($"> {subgift.DisplayName} just gifted a {subgift.SubscriptionType} sub to {subgift.RecipientDisplayName}!");
            break;

        // Gift sub upgraded to paid
        case SubscriptionGiftUpgradeTags subgiftUpgrade:
            Console.WriteLine($"> {subgiftUpgrade.DisplayName} just upgraded their sub gift from {subgiftUpgrade.SenderDisplayName}!");
            break;

        // Anonymous sub upgraded to paid
        case SubscriptionGiftUpgradeAnonymousTags subgiftAnon:
            Console.WriteLine($"> {subgiftAnon.DisplayName} just upgraded their sub gift from Anonymous!");
            break;
    }
}
