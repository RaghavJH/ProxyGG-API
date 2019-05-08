# ProxyGG-CSharp-API
A C# wrapper for the ProxyGG API (https://proxy.gg)
### Download
NuGet: https://www.nuget.org/packages/ProxyGGAPI/<br>
or<br>
Download DLL [here](https://github.com/RaghavJH/ProxyGG-CSharp-API/blob/master/ProxyGGAPI/bin/Debug/ProxyGGAPI.dll)
### How To Implement
The API supports asynchronous calls, but for the examples, all the code presented will be synchronous.<br>
You are expected to handle ALL exceptions (custom exceptions within the API and network-related exceptions)
### Example
An example of getting the proxies as a list of strings (LinkedList)
```csharp
//Create the object
ProxyGGAPI proxyggApi = new ProxyGGAPI("your_api_key_here");
/* Received parsed proxies (parsed into a LinkedList) */
//Get a list of proxies
LinkedList<string> proxies = proxyggApi.GetProxiesAsync(ProxyType.HTTPS, 100).Result;
//You can also supply a country code
//LinkedList<string> proxies = proxyggApi.GetProxiesAsync(ProxyType.HTTPS, 100, "US").Result;
//Print every proxy
foreach(string proxy in proxies)
{
    Console.WriteLine(proxy);
}
//Print count
Console.WriteLine($"Received {proxies.Count} proxies.");
```
An example of getting the raw proxies (i.e. straight from the API, without any alteration) and printing
```csharp
/* Receive raw (straight from the API) proxies */
//Create the object
ProxyGGAPI proxyggApi = new ProxyGGAPI("your_api_key_here");
//Get raw proxies and print
//Can be Format.DOM or Format.TABLE too.
string proxiesInJson = proxyggApi.GetRawProxiesAsync(ProxyType.HTTPS, 100, Format.JSON).Result;
//You can also supply a country code
//string proxiesInJson = proxyggApi.GetRawProxiesAsync(ProxyType.HTTPS, 100, Format.JSON, "US").Result;
//Print the proxies
Console.WriteLine(proxiesInJson);
```
## Known Issues
There are no known issues as yet.
