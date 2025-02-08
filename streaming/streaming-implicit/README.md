# Implicit Streams

This sample demonstrates the basics of how Implicit Streaming works in Orleans, using the In Memory Streaming provider.

One Grain is initiated to start counting at a regular interval, passing the current counter value to a stream that it owns. A consumer grain is implicitly registered to receive events on the particular stream namespace and is activated as necessary.

Note that the consuming grain does not pass along a stream sequence token when attaching to the stream. This avoids issues with long running streams not passing stale stream data.

To change this behavior, change the following line in `ImplicitStreamConsumerGrain` to pass a stored stream sequence token.

```csharp
await handle.ResumeAsync(this);
// await handle.ResumeAsync(this, _streamSequenceToken);
```

## Starting A Producer

Starts a reminder in the producer grain

```
@Silo_HostAddress = https://localhost:7090
@Grain_Id = test

GET {{Silo_HostAddress}}/api/producers/{{Grain_Id}}/start
Accept: application/json
```

## Getting Counter State

Retrieves the state of the counter without changing the reminder.

```
@Silo_HostAddress = https://localhost:7090
@Grain_Id = test

GET {{Silo_HostAddress}}/api/consumers/{{Grain_Id}}
Accept: application/json
```
