﻿using AuxLabs.Twitch.Rest.Api;
using AuxLabs.Twitch.Rest.Models;
using AuxLabs.Twitch.Rest.Requests;

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

        public static async Task<User> RequestUserAsync(TwitchRestApiClient rest, string message)
        {
            User? user = null;
            while (user == null)
            {
                var userName = RequestValue(message);

                var userResponse = await rest.GetUsersAsync(new GetUsersArgs(GetUsersMode.Name, userName));
                if (!userResponse.Data.Any())
                {
                    Console.WriteLine($"> {userName} is not a valid user name");
                    continue;
                }

                user = userResponse.Data.SingleOrDefault();
            }

            return user;
        }
    }
}
