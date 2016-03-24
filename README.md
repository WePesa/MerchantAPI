# MerchantAPI

This is a .NET client for Monitex's Bitcoin Payments API.

This is the reference implementation.

[![Build status](https://ci.appveyor.com/api/projects/status/95vn1krk9hajaax8/branch/master?svg=true)](https://ci.appveyor.com/project/AdemarGonzalez/merchantapi/branch/master)



NuGet
=====

To install the library in your .NET project, run the following command in the Package Manager Console

```bash
PM> Install-Package Monitex.Client
```

Sample client code in C#
========================

```csharp
var client = new Monitex.Client("https://api.bitbuy.ca"
  // Do not use these keys, create a new pair of API keys in the control panel
  , "HAWK_ID"
  , "HAWK_KEY" );
  
var request = new CreateOrderRequest(invoice.Amount,"cad");

request.Data = invoice.Id;
request.Ttl  = 30; // minutes

var response = client.CreateOrder(request);

RedirectToUrl(response.url);
```
