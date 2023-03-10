using AuxLabs.SimpleTwitch.EventSub;
using AuxLabs.SimpleTwitch.Rest;
using Examples;

var rest = new TwitchRestApiClient();
var eventsub = new TwitchEventSubApiClient();

// Request a token from the console, and validate it
var token = ExampleHelper.GetOrRequestToken();
var info = await rest.ValidateAsync(token);

// Get the current user
var userResponse = await rest.GetUsersAsync(new GetUsersArgs(GetUsersMode.Id, info.UserId));
var user = userResponse.Data.Single();

// Register eventsub events
eventsub.SessionCreated += OnSessionCreated;
eventsub.ChannelFollow += OnChannelFollow;

// Run the connection loop, the app will lock here until the client is disposed.
await eventsub.RunAsync();

async void OnSessionCreated(Session session)
{
    Console.WriteLine($"> Session created with id `{session.Id}`");

    // Subscribe to channel.follow
    var response = await rest.PostEventSubscriptionAsync(new FollowSubscription(user.Id, user.Id, session.Id));
    Console.WriteLine($"> Subscribed to channel.follow for {user.DisplayName} ({user.Id})");
}

void OnChannelFollow(ChannelFollowEventArgs args, EventSubscription subscription)
{
    // Output event info to console
    Console.WriteLine($"> {args.UserDisplayName} ({args.UserId}) just followed {args.BroadcasterDisplayName} ({args.BroadcasterId})");
    Console.WriteLine($"> {DateTime.UtcNow - args.FollowedAt:h'h 'm'm 's's'} ago");
}