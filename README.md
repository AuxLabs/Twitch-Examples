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
- [Echo bot using HostBuilder](src/main/EchoBotHostedExample)

### EventSub  
*None yet*

### PubSub  
*None yet*


## Api Clients  
Api clients are used internally by the library and, as bare minimum implementations, they require much more work to be implemented. They should only be used in advanced cases where the features provided by the main clients are unnecessary overhead in your application.

### Identity  
- [Validating tokens](src/apiclients/ValidateTokenExample)

### Rest
- [Get user info](src/apiclients/GetUserInfoExample)
- [Get stream status](src/apiclients/GetStreamStatusExample)

### Chat
- [Show chat feed in console](src/apiclients/ChatConnectionExample)
- [Show subscription messages in console](src/apiclients/ChatSubscriptionEventsExample)
- [Echo bot](src/apiclients/EchoBotExample)

### EventSub  
- [Stream status events](src/apiclients/StreamStatusEventsExample)
- [Follower events](src/apiclients/FollowerEventExample)

### PubSub  
*None yet*


## Other Examples  
Other repositories showing example applications.

*None yet*
