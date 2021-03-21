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

   ``` c#
    Retry.Do( () => DoSomething(), retryCount: 3, delay: TimeSpan.FromSeconds(1));
   ```
2. Pluarization - Attempts to pluralize the specified text according to the rules of the English language.  
   Sample:
   ```c#
    var text = "Index";
    Console.WriteLine(text.Plural()); //Writes: Indexes
   ```