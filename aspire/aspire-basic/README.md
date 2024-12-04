# Aspire Basic

The Aspire Basic sample demonstrates the minimal viable solution for running Orleans in Aspire.

In this sample, the silo also exposes API endpoints for interacting with the `IAccountGrain` implementation.

The included Bruno and HTTP files allow users to reach these endpoints to ensure the cluster is working appropriately.

## Retrieving Account Balance

```
@OrleansSamples.AspireBasic.Silo_HostAddress = https://localhost:7017

GET {{OrleansSamples.AspireBasic.Silo_HostAddress}}/account/1234
Accept: application/json
```

## Depositing Money

```
@OrleansSamples.AspireBasic.Silo_HostAddress = https://localhost:7017

POST {{OrleansSamples.AspireBasic.Silo_HostAddress}}/account/1234/deposit
Accept: application/json
Content-Type: application/json

{
  "amount": 54321.24
}
```

## Withdrawing Money

```
@OrleansSamples.AspireBasic.Silo_HostAddress = https://localhost:7017

POST {{OrleansSamples.AspireBasic.Silo_HostAddress}}/account/1234/withdraw
Accept: application/json
Content-Type: application/json

{
  "amount": 1000.23
}
```