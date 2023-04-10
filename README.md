# Twitch Examples
Example projects for the [AuxLabs.Twitch](https://github.com/AuxLabs/Twitch) library

All projects assume you have the `TWITCH_TOKEN` and `TWITCH_CLIENTID` environment variables set.

## Main Clients  
Basic example implementations to help with basic questions that might be asked when learning the library.

### Identity  
*None yet*

### Rest  
*None yet*

### Chat  
*None yet*

### EventSub  
*None yet*

### PubSub  
*None yet*


## Api Clients  
Api clients are used internally by the library and, as bare minimum implementations, they require much more work to be implemented. They should only be used in advanced cases where the features provided by the main clients are unnecessary overhead in your application.

### Identity  
- [Validating tokens](src/ValidateTokenExample)

### Rest
- [Get user info](src/GetUserInfoExample)
- [Get stream status](src/GetStreamStatusExample)

### Chat
- [Show chat feed in console](src/ChatConnectionExample)
- [Show subscription messages in console](src/ChatSubscriptionEventsExample)
- [Echo bot](src/EchoBotExample)
- [Echo bot using HostBuilder](src/EchoBotHostedExample)

### EventSub  
- [Stream status events](src/StreamStatusEventsExample)
- [Follower events](src/FollowerEventExample)

### PubSub  
*None yet*


## Other Examples  
Other repositories showing full example applications.

*None yet*
