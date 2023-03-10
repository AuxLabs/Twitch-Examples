using AuxLabs.SimpleTwitch.EventSub;
using AuxLabs.SimpleTwitch.Rest;
using Examples;

var rest = new TwitchRestApiClient();
var eventsub = new TwitchEventSubApiClient();

// Request a token from the console, and validate it
var token = ExampleHelper.GetOrRequestToken();
var info = await rest.ValidateAsync(token);

// Request a username from the console, and check if it exists
var user = await ExampleHelper.RequestUserAsync(rest, "> Enter the channel name you want to watch stream status for:");

// Register necessary events
eventsub.SessionCreated += OnSessionCreated;
eventsub.BroadcastStarted += (args, sub) => Console.WriteLine($"> {args.BroadcasterDisplayName} just started streaming!");
eventsub.BroadcastEnded += (args, sub) => Console.WriteLine($"> {args.BroadcasterDisplayName} just stopped streaming!");

// Run the connection loop, the app will lock here until the client is disposed.
await eventsub.RunAsync();

async void OnSessionCreated(Session session)
{
    Console.WriteLine($"> Session created with id `{session.Id}`");

    // Subscribe to stream.online
    var response = await rest.PostEventSubscriptionAsync(new BroadcastStartedSubscription(user.Id, session.Id));
    Console.WriteLine($"> Subscribed to stream.online for {user.DisplayName} ({user.Id})");

    // Subscribe to stream.offline
    response = await rest.PostEventSubscriptionAsync(new BroadcastEndedSubscription(user.Id, session.Id));
    Console.WriteLine($"> Subscribed to stream.offline for {user.DisplayName} ({user.Id})");
}