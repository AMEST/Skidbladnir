# [Skidbladnir Home](../../README.md)

## Caching

### Description

This direction of the set of Skidbladnir libraries will contain caching implementations (local or distributed) using different storage.

The cache is an intermediate buffer with quick access to it, containing information that can be requested with the highest probability.

In computing, a distributed cache is an extension of the traditional concept of cache used in a single locale. A distributed cache may span multiple servers so that it can grow in size and in transactional capacity. It is mainly used to store application data residing in database and web session data.

### Cache implementation

1. [MongoDB Distributed cache](Skidbladnir.Caching.Distributed.MongoDB/README.md) - Distributed Cache implementation using mongodb as storage
