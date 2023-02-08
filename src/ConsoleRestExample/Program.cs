using AuxLabs.SimpleTwitch.Rest;
using System.Net.Http.Headers;

var pass = Environment.GetEnvironmentVariable("TWITCH_TOKEN", EnvironmentVariableTarget.User);
var clientId = Environment.GetEnvironmentVariable("TWITCH_CLIENTID", EnvironmentVariableTarget.User);

Console.WriteLine("> Initializing rest client...");
if (pass == null)
{
    Console.WriteLine("> Please enter your oauth token: ");
    pass = Console.ReadLine();
}
if (clientId == null)
{
    Console.WriteLine("> Please enter your client id: ");
    clientId = Console.ReadLine();
}

// Just a loop so you can request multiple users
while (true)                                            
{
    Console.WriteLine("> Please enter the username you want to get info for: ");
    var userName = Console.ReadLine();

    // Create the client with authentication headers
    var twitch = new TwitchRestApiClient                
    {
        Authorization = new AuthenticationHeaderValue("Bearer", pass),
        ClientId = clientId
    };

    // Get the user by name
    var userResponse = await twitch.GetUsersAsync(new GetUsersArgs(GetUsersMode.Name, userName));

    // Check if the requested user exists
    if (userResponse.Data.FirstOrDefault() is User user)    
    {
        // Get the user's channel
        var channelResponse = await twitch.GetChannelsAsync(new GetChannelsArgs(user.Id));

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

        Console.ReadKey(true);
    }
    else
        Console.WriteLine($"{userName} does not exist!");
}