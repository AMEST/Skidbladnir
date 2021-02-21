# [Skidbladnir Home](../../../README.md)
## [Client](../README.md)
## Freenom Dns
[![NuGet](https://img.shields.io/nuget/vpre/Skidbladnir.Client.Freenom.Dns.svg?label=Skidbladnir.Client.Freenom.Dns)](https://www.nuget.org/packages/Skidbladnir.Client.Freenom.Dns/absoluteLatest/)
![GitHub release (latest by date)](https://img.shields.io/github/v/release/amest/Skidbladnir)
![GitHub](https://img.shields.io/github/license/amest/Skidbladnir)

[_**Sample using library**_](../../../samples/Client/Skidbladnir.Client.Freenom.Dns.Sample)
### Description
Implementation of a client to freenom.com for obtaining information on registered domains, as well as managing DNS records of these domains.

### Implemented

* Get list zones (domains) with information registered in account
* Get dns records from zone
* Remove dns record from zone
* Add dns record to zone

### Install 
For use client you only needed install package:
```
Install-Package Skidbladnir.Client.Freenom.Dns
```

### Using

Creating client, authenticate and get list zones:

```c#
public async Task Main(){
    var dnsClient = FreenomClientFactory.Create();
    if(!(await dnsClient.IsAuthenticated()))
        await dnsClient.SignIn(Email, Password);
    
    var zones = await dnsClient.GetZones();
    Console.WriteLine("===========Zones============");
    foreach (var zone in zones)
    {
        Console.WriteLine(
            $"Zone: {zone.ZoneId} \t {zone.Name} \t {zone.ExpireDate} \t {zone.RegistrationDate} \t {zone.Status} \t {zone.Type}\n");
    }
}

```