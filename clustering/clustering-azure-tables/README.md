# Azure Tables Clustering

This sample demonstrates clustering via Azure Tables, provided as a Azurite docker container and connection string passed via Aspire.

In this sample, the silo exposes API endpoints for interacting with the `IAccountGrain` implementation.

The included Bruno and HTTP files allow users to reach these endpoints to ensure the cluster is working appropriately.

## Retrieving Account Balance

```
@API_HostAddress = https://localhost:7214

GET {{API_HostAddress}}/account/1234
Accept: application/json
```

## Depositing Money

```
@API_HostAddress = https://localhost:7214

POST {{API_HostAddress}}/account/1234/deposit
Accept: application/json
Content-Type: application/json

{
  "amount": 54321.24
}
```

## Withdrawing Money

```
@API_HostAddress = https://localhost:7214

POST {{API_HostAddress}}/account/1234/withdraw
Accept: application/json
Content-Type: application/json

{
  "amount": 1000.23
}
```
