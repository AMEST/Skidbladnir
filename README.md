# Skidbladnir libraries

[![Skidbladnur publish (build and publish master and tags)](https://github.com/AMEST/Skidbladnir/actions/workflows/main.yml/badge.svg)](https://github.com/AMEST/Skidbladnir/actions/workflows/main.yml)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/amest/Skidbladnir)
![GitHub](https://img.shields.io/github/license/amest/Skidbladnir)

[Search libraries on nuget](https://www.nuget.org/packages?q=Skidbladnir)

- [Skidbladnir libraries](#skidbladnir-libraries)
  - [Description](#description)
  - [Content of Skidbladnir](#content-of-skidbladnir)

## Description

Skidbladnir - This is the general name for a repository containing useful libraries for simplified creation of applications, implementation of clients for services, simple modular system, as well as just useful tools.

## Content of Skidbladnir

The repository contains the following directions and libraries:

- [Repository](src/Repository/README.md) - are an abstraction of repositories for accessing databases.
  - [Repository.MongoDB](src/Repository/Skidbladnir.Repository.MongoDB/README.md) - Repository implementation using MongoDB
  - [Repository.EntityFrameworkCore](src/Repository/Skidbladnir.Repository.EntityFrameworkCore/README.md) - Repository implementation using EntityFramework Core
- [Caching](src/Caching/README.md) - Temporary storage of computed data
  - [DistributedCache.MongoDB](src/Caching/Skidbladnir.Caching.Distributed.MongoDB/README.md) - Distributed cache implementation using MongoDB
- [DataProtection](src/DataProtection/README.md) - Abstraction of data protection asp net core subsystem for Skidbladnir libraries
  - [DataProtection.MongoDB](src/DataProtection/Skidbladnir.DataProtection.MongoDb/README.md) - Data protection implementation using Skidbladnir.Repository.MongoDB
- [Client](src/Client/README.md) - Clients implementations for various services
  - [Client.Freenom.Dns](src/Client/Skidbladnir.Client.Freenom.Dns/README.md) - Client for managing dns zones in freenom
- [Modules](src/Modules/Skidbladnir.Modules/README.md) - Simple modular system
- [Storage](src/Storage/README.md) - File subsystem abstraction
  - [Storage.GridFs](src/Storage/Skidbladnir.Storage.GridFS/README.md) - GridFs storage implementation
  - [Storage.LocalFs](src/Storage/Skidbladnir.Storage.LocalFileStorage/README.md) - Local file system storage implementation
  - [Storage.WebDav](src/Storage/Skidbladnir.Storage.WebDav/README.md) - Implementing a file storage abstraction based on WebDav protocol
  - [Storage.S3](src/Storage/Skidbladnir.Storage.S3/README.md) - Implementing a file storage abstraction based on S3 protocol
- [Messaging](src/Messaging/README.md) - Pub/Sub and Send/Receive abstraction
  - [Messaging.Redis](src/Messaging/Skidbladnir.Messaging.Redis/README.md) - Redis implementation Pub/Sub and Send/Receive message bus
- [Utilities](src/Utility/Skidbladnir.Utility.Common/README.md) - A set of utilities to simplify development
