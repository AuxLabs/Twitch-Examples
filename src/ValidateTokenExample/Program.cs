using AuxLabs.Twitch;
using AuxLabs.Twitch.Rest.Api;
using AuxLabs.Twitch.Rest.Models;
using Examples;

Console.WriteLine("> Initializing identity client...");
var twitch = new TwitchIdentityApiClient();

while (true)
{
    var token = ExampleHelper.RequestValue("> Please enter your oauth token: ");

    // Try to validate the provided token
    AccessTokenInfo tokenInfo;
    try
    {
        tokenInfo = await twitch.ValidateAsync(token);
    } catch (TwitchRestException ex)
    {
        // Any rest exception indicates invalid token
        Console.WriteLine($"> {ex.Message}");
        Console.ReadKey();
        continue;
    };

    // Output token info
    Console.WriteLine($"> Token is valid!");
    Console.WriteLine($"Client Id: {tokenInfo.ClientId}");
    if (tokenInfo.ExpiresInSeconds.HasValue)
    {
        // Convert seconds to timespan
        var expiresIn = TimeSpan.FromSeconds(tokenInfo.ExpiresInSeconds.Value);
        Console.WriteLine($"Expires In: {expiresIn:d'd 'h'h 'm'm 's's'}");
    }

    // If user id is null, the token is app authenticated and has no scopes
    if (tokenInfo.UserId != null)
    {
        Console.WriteLine($"User: {tokenInfo.UserName} ({tokenInfo.UserId})");
        Console.WriteLine($"Scopes ({tokenInfo.Scopes.Count}): {string.Join(", ", tokenInfo.Scopes)}");
    }

    Console.ReadKey();
}