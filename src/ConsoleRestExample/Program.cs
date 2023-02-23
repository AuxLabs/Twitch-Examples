using AuxLabs.SimpleTwitch.Rest;
using System.Diagnostics;

var token = Environment.GetEnvironmentVariable("TWITCH_TOKEN", EnvironmentVariableTarget.User);

Console.WriteLine("> Initializing rest client...");
if (token == null)
{
    Console.WriteLine("> Please enter your oauth token: ");
    token = Console.ReadLine();
}

// Create the client and authenticate with the provided token
var rest = new TwitchRestApiClient();
var identity = await rest.ValidateAsync(token);

Console.WriteLine($"> Verfied token for {identity.UserName} ({identity.UserId})");

// Just a loop so you can request multiple users
while (true)                                            
{
    Console.WriteLine("> Please enter the username you want to get info for: ");
    var userName = Console.ReadLine();

    // Get the user by name
    var userResponse = await rest.GetUsersAsync(new GetUsersArgs(GetUsersMode.Name, userName));

    // Method returns an empty data collection if the user doesn't exist
    if (!userResponse.Data.Any())
    {
        Console.WriteLine($"{userName} does not exist!");
    } else
    {
        // Get the user model out of the response
        var user = userResponse.Data.First();

        // Get the user's channel
        var channelResponse = await rest.GetChannelsAsync(new GetChannelsArgs(user.Id));

        // Get the user's stream
        var broadcastResponse = await rest.GetBroadcastsAsync(new GetBroadcastsArgs(user.Id));

        // Output their user info
        Console.WriteLine($"{user.DisplayName ?? user.Name} ({user.Id})");
        
        if (!string.IsNullOrWhiteSpace(user.Description))
            Console.WriteLine($"{user.Description}");
        
        Console.WriteLine($"They joined at {user.CreatedAt}");
        
        if (user.BroadcasterType != BroadcasterType.None)
            Console.WriteLine($"They currently have {user.BroadcasterType} status");
        
        if (user.Type != UserType.None)
            Console.WriteLine($"They are a {user.Type}");

        // Output their channel info
        var channel = channelResponse.Data.First();
        
        if (channel.GameId != null)
            Console.WriteLine($"They were last playing {channel.GameName} ({channel.GameId})");
        
        if (channel.Tags.Any())
            Console.WriteLine($"With the tags: {string.Join(", ", channel.Tags)}");

        // Output their stream info
        var broadcast = broadcastResponse.Data.FirstOrDefault();
        if (broadcast == null)
        {
            Console.WriteLine("They are not currently streaming");
        } else
        {
            Console.WriteLine($"They're playing {broadcast.GameName} for {broadcast.ViewerCount} viewer(s)!");
            Console.WriteLine($"Title: {broadcast.Title}");

            var uptime = DateTime.UtcNow - broadcast.StartedAt;
            Console.WriteLine($"They've been live for {uptime:h'h 'm'm 's's'}");
        }

        // Wait before looping
        Console.ReadKey(true);
    }
}
