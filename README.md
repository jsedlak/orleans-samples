The samples in this repository are meant to be a comprehensive example of how to develop with Microsoft Orleans.

In general, each sample focuses in on a particular portion of the framework while simplifying the rest. For example, while a sample is focusing on Implicit Streaming, it will utilize basic means of grain storage.

To maintain a consistent approach and the ability to quickly run any sample, Aspire is used to orchestrate any necessary resource. Where possible, local containers and emulators are used in place of hosted services.

If there is a sample you wish to see, please sound off in the Discussions section.

# Aspire

- [Basic (Minimal) Aspire](https://github.com/jsedlak/orleans-samples/tree/main/aspire/aspire-basic)
- [Aspire with API Client](https://github.com/jsedlak/orleans-samples/tree/main/aspire/aspire-with-api-client)

# Clustering

- [Azure Tables](https://github.com/jsedlak/orleans-samples/tree/main/clustering/azure-tables)
- [Redis](https://github.com/jsedlak/orleans-samples/tree/main/clustering/redis)
- [MongoDB](https://github.com/jsedlak/orleans-samples/tree/main/clustering/mongodb) - This is a WIP implementation that uses a custom Provider Builder and service registration. See Issue #4
- [ADO.NET - PostgreSQL](https://github.com/jsedlak/orleans-samples/tree/main/clustering/ado-postgres) - This is a WIP implementation that uses a custom docker container prebuilt with the necessary tables. See issue #5. Docker container courtesy of [dddanielreis](https://github.com/dddanielreis/playground/tree/main/orleans-postgres-docker)

# Storage

- [Azure Storage (Blobs)](https://github.com/jsedlak/orleans-samples/tree/main/grain-storage/blobs)
- [Redis](https://github.com/jsedlak/orleans-samples/tree/main/grain-storage/redis)

# Reminders

- [In Memory](https://github.com/jsedlak/orleans-samples/tree/main/reminders/in-memory)
- [Azure Tables](https://github.com/jsedlak/orleans-samples/tree/main/reminders/azure-tables)
- [Redis](https://github.com/jsedlak/orleans-samples/tree/main/reminders/redis)

# Patterns

- [Satellite Pattern](https://github.com/jsedlak/orleans-samples/tree/main/patterns/patterns-satellite)

# Streaming

- [Implicit Streams](https://github.com/jsedlak/orleans-samples/tree/main/streaming/streaming-implicit)
