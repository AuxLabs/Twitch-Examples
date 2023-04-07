using AuxLabs.Twitch.Rest.Api;
using AuxLabs.Twitch.Rest.Requests;
using Examples;

Console.WriteLine("> Initializing rest client...");
var token = ExampleHelper.GetOrRequestToken();

// Create the client and authenticate with the provided token
var twitch = new TwitchRestApiClient();
var identity = await twitch.ValidateAsync(token);

Console.WriteLine($"> Verfied token for {identity.UserName} ({identity.UserId})");

// Loop to request multiple users
while (true)
{
    var userName = ExampleHelper.RequestValue("> Please enter the channel you want to check status for: ");

    // Get the user's stream
    var broadcastResponse = await twitch.GetBroadcastsAsync(new GetBroadcastsArgs
    {
        UserNames = new[] { userName }
    });

    // Output their stream info
    var broadcast = broadcastResponse.Data.FirstOrDefault();
    if (broadcast == null)
    {
        Console.WriteLine($"{userName} is not currently streaming");
    }
    else
    {
        Console.WriteLine($"{broadcast.UserDisplayName} is live!");
        Console.WriteLine($"They're playing {broadcast.GameName} for {broadcast.ViewerCount} viewer(s)!");
        Console.WriteLine($"Title: {broadcast.Title}");

        var uptime = DateTime.UtcNow - broadcast.StartedAt;
        Console.WriteLine($"They've been live for {uptime:h'h 'm'm 's's'}");
    }

    // Wait before looping
    Console.ReadKey(true);
}