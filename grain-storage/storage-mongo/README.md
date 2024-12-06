# Grain Storage with MongoDB

This sample provides minimal configuration for using Azure Storage in your Orleans application with Aspire.

> **NOTE**: Currently, this sample does not use Aspire to configure the mongo provider as the SDK is missing a Provider Builder to register the MongoDB Storage Provider. Instead, it uses Aspire to generate & pass the connection string, which is then used to manually configure MongoDB Grain Storage.

The included Bruno and HTTP files allow users to reach these endpoints to ensure the cluster is working appropriately.

## Retrieving Account Balance

```
@Silo_HostAddress = https://localhost:7272

GET {{Silo_HostAddress}}/account/1234
Accept: application/json
```

## Depositing Money

```
@Silo_HostAddress = https://localhost:7272

POST {{Silo_HostAddress}}/account/1234/deposit
Accept: application/json
Content-Type: application/json

{
  "amount": 54321.24
}
```

## Withdrawing Money

```
@Silo_HostAddress = https://localhost:7272

POST {{Silo_HostAddress}}/account/1234/withdraw
Accept: application/json
Content-Type: application/json

{
  "amount": 1000.23
}
```