using Newtonsoft.Json.Linq;

namespace Examples
{
    public static class ExampleHelper
    {
        public static string RequestValue(string message)
        {
            string? value = null;
            while (true)
            {
                Console.WriteLine(message);
                value = Console.ReadLine();
                if (!string.IsNullOrWhiteSpace(value))
                    return value;
                else
                    Console.WriteLine($"> `{value}` is not a valid entry");
            }
        }

        public static string GetOrRequestToken()
        {
            var token = Environment.GetEnvironmentVariable("TWITCH_TOKEN", EnvironmentVariableTarget.User);
            if (!string.IsNullOrWhiteSpace(token))
                return token;

            return RequestValue("> Please enter your oauth token: ");
        }

        public static string GetOrRequestClientId()
        {
            var token = Environment.GetEnvironmentVariable("TWITCH_CLIENTID", EnvironmentVariableTarget.User);
            if (!string.IsNullOrWhiteSpace(token))
                return token;

            return RequestValue("> Please enter your client id: ");
        }
    }
}
