# Skidbladnir libraries
[![Skidbladnur publish (build and publish master and tags)](https://github.com/AMEST/Skidbladnir/actions/workflows/main.yml/badge.svg)](https://github.com/AMEST/Skidbladnir/actions/workflows/main.yml)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/amest/Skidbladnir)
![GitHub](https://img.shields.io/github/license/amest/Skidbladnir)


- [Skidbladnir libraries](#skidbladnir-libraries)
  - [Description](#description)
  - [Content of Skidbladnir](#content-of-skidbladnir)

## Description

Skidbladnir - This is the general name for a repository containing useful libraries for simplified creation of applications, implementation of clients for services, simple modular system, as well as just useful tools.

## Content of Skidbladnir

The repository contains the following directions and libraries:
* [Repository](src/Repository/README.md) - are an abstraction of repositories for accessing databases.
  * [Repository.MongoDB](src/Repository/Skidbladnir.Repository.MongoDB/README.md) - Repository implementation using MongoDB
* [Caching](src/Caching/README.md) - Temporary storage of computed data
  * [DistributedCache.MongoDB](src/Caching/Skidbladnir.Caching.Distributed.MongoDB/README.md) - Distributed cache implementation using MongoDB
* [DataProtection](src/DataProtection/README.md) - Abstraction of data protection asp net core subsystem for Skidbladnir libraries
  * [DataProtection.MongoDB](src/DataProtection/Skidbladnir.DataProtection.MongoDb/README.md) - Data protection implementation using Skidbladnir.Repository.MongoDB
* [Client](src/Client/README.md) - Clients implementations for various services
  * [Client.Freenom.Dns](src/Client/Skidbladnir.Client.Freenom.Dns/README.md) - Client for managing dns zones in freenom
* [Modules](src/Modules/Skidbladnir.Modules/README.md) - Simple modular system
* Storage - File subsystem abstraction
  * Storage.GridFs - GridFs storage implementation
  * Storage.LocalFs - Local file system storage implementation
* [Utilities](src/Utility/Skidbladnir.Utility.Common/README.md) - A set of utilities to simplify development