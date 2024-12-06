# Grain Storage with Azure Storage Blobs

This sample provides minimal configuration for using Azure Storage in your Orleans application with Aspire.

The included Bruno and HTTP files allow users to reach these endpoints to ensure the cluster is working appropriately.

## Retrieving Account Balance

```
@Silo_HostAddress = https://localhost:7021

GET {{OrleansSamples.AspireBasic.Silo_HostAddress}}/account/1234
Accept: application/json
```

## Depositing Money

```
@Silo_HostAddress = https://localhost:7021

POST {{OrleansSamples.AspireBasic.Silo_HostAddress}}/account/1234/deposit
Accept: application/json
Content-Type: application/json

{
  "amount": 54321.24
}
```

## Withdrawing Money

```
@Silo_HostAddress = https://localhost:7021

POST {{OrleansSamples.AspireBasic.Silo_HostAddress}}/account/1234/withdraw
Accept: application/json
Content-Type: application/json

{
  "amount": 1000.23
}
```