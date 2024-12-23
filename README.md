The samples in this repository are meant to be a comprehensive example of how to develop with Microsoft Orleans. 

In general, each sample focuses in on a particular portion of the framework while simplifying the rest. For example, while a sample is focusing on Implicit Streaming, it will utilize basic means of grain storage.

To maintain a consistent approach and the ability to quickly run any sample, Aspire is used to orchestrate any necessary resource. Where possible, local containers and emulators are used in place of hosted services.

If there is a sample you wish to see, please sound off in the Discussions section.

# Aspire

* [Basic (Minimal) Aspire](https://github.com/jsedlak/orleans-samples/tree/main/aspire/aspire-basic)

# Storage
* [Azure Storage (Blobs)](https://github.com/jsedlak/orleans-samples/tree/main/grain-storage/storage-blobs)
* [MongoDB](https://github.com/jsedlak/orleans-samples/tree/main/grain-storage/storage-mongo)

# Patterns
* [Satellite Pattern](https://github.com/jsedlak/orleans-samples/tree/main/patterns/patterns-satellite)