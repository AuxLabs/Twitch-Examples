using AuxLabs.SimpleTwitch.Rest;
using AuxLabs.Twitch.Rest;

var token = Environment.GetEnvironmentVariable("TWITCH_TOKEN", EnvironmentVariableTarget.User);

Console.WriteLine("> Initializing rest client...");
if (token == null)
{
    Console.WriteLine("> Please enter your oauth token: ");
    token = Console.ReadLine();
}

// Create an instance of the rest client
var twitch = new TwitchRestClient();

// Validate your token with the twitch servers.
await twitch.ValidateAsync(token);

// Just a loop so you can request multiple users
while (true)
{
    Console.WriteLine("> Please enter the username you want to get info for: ");
    var userName = Console.ReadLine();

    // Get the user by name
    var user = await twitch.GetUserByNameAsync(userName);

    // Method returns null if the user doesn't exist
    if (user == null)
    {
        Console.WriteLine($"{userName} does not exist!");
    } else
    {
        // Get the user's channel
        var channel = await user.GetChannelAsync();

        // Output their user info
        Console.WriteLine(user);

        if (!string.IsNullOrWhiteSpace(user.Description))
            Console.WriteLine(user.Description);

        Console.WriteLine($"They joined at {user.CreatedAt}");

        if (user.BroadcasterType != BroadcasterType.None)
            Console.WriteLine($"They currently have {user.BroadcasterType} status");

        if (user.Type != UserType.None)
            Console.WriteLine($"They are a {user.Type}");

        // Output their channel info
        if (channel.GameId != null)
            Console.WriteLine($"They were last playing {channel.GameName} ({channel.GameId})");

        if (channel.Tags.Any())
            Console.WriteLine($"With the tags: {string.Join(", ", channel.Tags)}");

        Console.ReadKey(true);
    }
}
