# Satellite Pattern

The Satellite Pattern is a design by which characteristics of CQRS are introduced into Grain data management such that the querying of the resulting data of an application are not overloading the originating Grains for that data.

For more information, please read the following blog post:
https://johnsedlak.com/2024/10/introducing-the-satellite-pattern-for-orleans/

The Satellite Pattern example is broken up into a number of variations meant to meet specific querying needs of your application.

# Basic Satellite

In the _Basic_ example, a Satellite Grain is called from the Primary (originating) Grain and informed of changes to data representing overall application state. This allows callers to query the Satellite without needing to activate the Primary grain.

![Satellite Grain](https://johnsedlak.com/wp-content/uploads/2024/10/image.png)

### Pros

1. Alleviates Primary grain of transient status queries
2. Changes made by external callers may cascade to internal grains quickly
3. Promotes support of Distributed ACID transactions by using grain calls

### Cons

1. No ability to query across multiple accounts
2. Developers can directly invoke status changes on the Satellite Grain

### API Endpoints

#### Signing In

```http
POST https://localhost:7089/api/basic/{accountId}/signin
Content-Type: application/json

{
    "status": "Playing Solitaire"
}
```

#### Signing Out

```http
POST https://localhost:7089/api/basic/{accountId}/signout
Content-Type: application/json
```

#### Getting Status

```http
GET https://localhost:7089/api/basic/{accountId}
```

# Satellite with Grain Extensions

Taking the _Basic_ sample on step further, this sample uses a Grain Extension to allow internal callers (Primary, originating grain) to affect change in the Satellite in a way that external developers may not.

![Satellite with Grain Extensions](https://johnsedlak.com/wp-content/uploads/2024/10/image-4.png)


### Pros

1. Alleviates Primary grain of transient status queries
2. Changes made by external callers may cascade to internal grains quickly
3. Promotes support of Distributed ACID transactions by using grain calls
4. Developers may not directly invoke status changes on the Satellite Grain

### Cons

1. No ability to query across multiple accounts

### API Endpoints

#### Signing In

```http
POST https://localhost:7089/api/satelliteextension/{accountId}/signin
Content-Type: application/json

{
    "status": "Playing Solitaire"
}
```

#### Signing Out

```http
POST https://localhost:7089/api/satelliteextension/{accountId}/signout
Content-Type: application/json
```

#### Getting Status

```http
GET https://localhost:7089/api/satelliteextension/{accountId}
```

# Event Driven

To support querying across multiple accounts requires us to maintain that state in a more global Satellite. There are a few options and this sample uses a global status grain to support that querying.

### Pros

1. Alleviates Primary grain of transient status queries
2. Changes made by external callers may cascade to internal grains quickly
3. Promotes support of Distributed ACID transactions by using grain calls
4. Developers may not directly invoke status changes on the Satellite Grain
5. May query across all accounts for status or online count

### Cons

1. No persistence of the data, so statuses are lost when grain is deactivated
2. No distribution of the data, so each silo will only see status of local accounts
3. Creates a choke point for querying data on a single grain

### API Endpoints

#### Signing In

```http
POST https://localhost:7089/api/eventdriven/{accountId}/signin
Content-Type: application/json

{
    "status": "Playing Solitaire"
}
```

#### Signing Out

```http
POST https://localhost:7089/api/eventdriven/{accountId}/signout
Content-Type: application/json
```

#### Getting Status

```http
GET https://localhost:7089/api/eventdriven/{accountId}
```

#### Getting All Statuses

```http
GET https://localhost:7089/api/eventdriven/
```

#### Getting Online Count

```http
GET https://localhost:7089/api/eventdriven/count
```

# Event Driven with Database

To alleviate the choke point of querying a grain and to support persistence across several silos, the _Event Driven_ sample can be extended to write statuses to a database. This database may be queried by the frontend (via an API) to support a more full fledged CQRS architecture.

### Pros

1. Alleviates Primary grain of transient status queries
2. Changes made by external callers may cascade to internal grains quickly
3. Promotes support of Distributed ACID transactions by using grain calls
4. Developers may not directly invoke status changes on the Satellite Grain
5. May query across all accounts for status or online count
6. Removes Orleans as a potential choke point for querying
7. Cross-silo statuses are supported

### Cons

1. Ties querying capacity to the underlying storage provider; additional architecture may be necessary to support low cost, large scale querying.

### API Endpoints

#### Signing In

```http
POST https://localhost:7089/api/eventdrivendatabase/{accountId}/signin
Content-Type: application/json

{
    "status": "Playing Solitaire"
}
```

#### Signing Out

```http
POST https://localhost:7089/api/eventdrivendatabase/{accountId}/signout
Content-Type: application/json
```

#### Getting Status

```http
GET https://localhost:7089/api/eventdrivendatabase/{accountId}
```

#### Getting All Statuses

```http
GET https://localhost:7089/api/eventdrivendatabase/
```

#### Getting Online Count

```http
GET https://localhost:7089/api/eventdrivendatabase/count
```