# Skidbladnir libraries

- [Skidbladnir libraries](#skidbladnir-libraries)
  - [Description](#description)
  - [Content of Skidbladnir](#content-of-skidbladnir)

## Description

Skidbladnir - This is the general name for a repository containing useful libraries for simplified creation of applications, implementation of clients for services, as well as just useful tools.

## Content of Skidbladnir

The repository contains the following directions and libraries:
* Repository - are an abstraction of repositories for accessing databases.
  * Repository.MongoDB - Repository implementation using MongoDB
* [Caching](src/Caching/README.md) - Temporary storage of computed data
  * [DistributedCache.MongoDB](src/Caching/Skidbladnir.Caching.Distributed.MongoDB/README.md) - Distributed cache implementation using MongoDB
* DataProtection - Abstraction of data protection asp net core subsystem for Skidbladnir libraries
  * DataProtection.MongoDB - Data protection implementation using Skidbladnir.Repository.MongoDB
* Client - Clients implementations for various services
  * Client.Freenom.Dns - Client implementation for freenom.com dns