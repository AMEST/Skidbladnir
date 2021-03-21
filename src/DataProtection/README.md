# [Skidbladnir Home](../../README.md)
## Data Protection

### Description
This direction of the Skidbladnir library set will contain implementations of the `DataProtection` subsystem.

The DataProtection subsystem is mainly needed to create and store a distributed key to encrypt and decrypt data between multiple instances of an application. For example, to encrypt an autoisation cookie with a key stored in the data protection subsystem. This allows you to create and encrypt a cookie on one node and decrypt it on another node without any problems.

### Data protection implementation

1. [MongoDB DataProtection](Skidbladnir.DataProtection.MongoDB/README.md) - Distributed Cache implementation using mongodb as storage