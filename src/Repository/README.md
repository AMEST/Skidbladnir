# [Skidbladnir Home](../../README.md)

## Repository

### Description

This direction of the Skidbladnir library set will contain implementations of the Repository Subsystem (`IRepository`) for data access.

The repository subsystem is needed to abstract from the implementation of data access, which allows you to change the data store (migrate from one database to another, for example, from mssql to mongodb) with minimal changes to the source code. access to data in the application is implemented through abstraction. It also allows you to work without unnecessary problems with various entities stored in different databases (and even not only in different databases on the same server, but even on different servers (for example, with data in mongodb and data in mysql)) without thinking where accurately the data was received.

### Repository implementation

1. [MongoDB](Skidbladnir.Repository.MongoDB/README.md) - Repository implementation using MongoDB
2. [EntityFramework Core](Skidbladnir.Repository.EntityFrameworkCore/README.md) - Repository implementation using EntityFramework Core for access to data store
