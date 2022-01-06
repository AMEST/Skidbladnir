# [Skidbladnir Home](../../../README.md)

## Utility

[![NuGet](https://img.shields.io/nuget/vpre/Skidbladnir.Utility.Common.svg?label=Skidbladnir.Utility.Common)](https://www.nuget.org/packages/Skidbladnir.Utility.Common/absoluteLatest/)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/amest/Skidbladnir)
![GitHub](https://img.shields.io/github/license/amest/Skidbladnir)

### Table of content

### Description

A set of utilities to simplify development that do not fit into a separate assembly and have the ability to general use.

### Install

For use you needed install packages:

```
Install-Package Skidbladnir.Utility.Common
```

### Implementation

1. Retry - retry policy implementation  
   Sample:
   ```c#
   var result = Retry.Do( () => DoSomething(), retryCount: 3, delay: TimeSpan.FromSeconds(1));
   ```
2. `Try` - The class contains methods that allow you to perform operations with the ability to handle errors   
   Sample:  
   ```c#
   var result = await Try.DoAsync(() => DoSomething());
   ```
3. Pluarization - Attempts to pluralize the specified text according to the rules of the English language.  
   Sample:  
   ```c#
    var text = "Index";
    Console.WriteLine(text.Plural()); //Writes: Indexes
   ```
4. `StreamExtentions` - The class contains methods for reading streams to memory   
   Sample:  
   ```c#
   var streamBytes = fileStream.ReadAllBytes();
   ```