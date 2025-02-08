# Redis Reminders

This sample demonstrates the basics of how Reminders work in Orleans, using the Redis Reminder provider.

A grain that counts utilizes a reminder to increment its state by 1 every minute (minimum reminder interval) and saves it.

The included Bruno and HTTP files allow users to reach these endpoints to ensure the cluster is working appropriately.

## Starting A Counter

Starts a reminder in the counter grain

```
@Silo_HostAddress = https://localhost:7247
@Grain_Id = test

GET {{Silo_HostAddress}}/api/counters/{{Grain_Id}}/start
Accept: application/json
```

## Stopping A Counter

Stops a reminder in the counter grain.

```
@Silo_HostAddress = https://localhost:7247
@Grain_Id = test

GET {{Silo_HostAddress}}/api/counters/{{Grain_Id}}/stop
Accept: application/json
```

## Getting Counter State

Retrieves the state of the counter without changing the reminder.

```
@Silo_HostAddress = https://localhost:7247
@Grain_Id = test

GET {{Silo_HostAddress}}/api/counters/{{Grain_Id}}
Accept: application/json
```
