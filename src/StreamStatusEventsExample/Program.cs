
using AuxLabs.SimpleTwitch;
using AuxLabs.SimpleTwitch.EventSub;
using AuxLabs.SimpleTwitch.Rest;
using Examples;

var rest = new TwitchRestApiClient();
var eventsub = new TwitchEventSubApiClient();

var token = ExampleHelper.GetOrRequestToken();
var info = await rest.ValidateAsync(token);

User? user = null;
while (user == null)
{
    var channelName = ExampleHelper.RequestValue($"> Enter the channel name you want to watch stream status for:");

    var userResponse = await rest.GetUsersAsync(new GetUsersArgs(GetUsersMode.Name, channelName));
    if (!userResponse.Data.Any())
    {
        Console.WriteLine($"> {channelName} is not a valid channel name");
        continue;
    }

    user = userResponse.Data.SingleOrDefault();
}

eventsub.SessionCreated += OnSessionCreated;
eventsub.BroadcastStarted += (args, sub) => Console.WriteLine($"> {args.BroadcasterDisplayName} just started streaming!");
eventsub.BroadcastEnded += (args, sub) => Console.WriteLine($"> {args.BroadcasterDisplayName} just stopped streaming!");

await eventsub.RunAsync();

async void OnSessionCreated(Session session)
{
    Console.WriteLine($"> Session created with id {session.Id}");

    var response = await rest.PostEventSubscriptionAsync(new BroadcastStartedSubscription(user.Id, session.Id));
    Console.WriteLine($"> Subscribed to {response.Data.Single().Type.GetStringValue()}?broadcaster_id={user.Id}");

    response = await rest.PostEventSubscriptionAsync(new BroadcastEndedSubscription(user.Id, session.Id));
    Console.WriteLine($"> Subscribed to {response.Data.Single().Type.GetStringValue()}?broadcaster_id={user.Id}");
}