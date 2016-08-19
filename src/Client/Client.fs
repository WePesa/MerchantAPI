﻿namespace Monitex

open HttpClient

open System
open System.Text

open Newtonsoft.Json

type CreateOrderRequest(amount : decimal, currency : string) =

    let mutable amount : decimal = amount
    let mutable currency : string = currency
    let mutable data : string = null
    let mutable priority : int = 0
    let mutable ttl : int = 15 // minutes
    let mutable urlCallBack : string = null
    let mutable urlSuccess : string = null
    let mutable urlFailure : string = null

    
    [<JsonProperty("Amount")>]
    member x.Amount with get() = amount and set(v) = amount <- v
    [<JsonProperty("Currency")>]
    member x.Currency with get() = currency and set(v) = currency <- v
    [<JsonProperty("Data")>]
    member x.Data with get() = data and set(v) = data <- v
    [<JsonProperty("Priority")>]
    member x.Priority with get() = priority and set(v) = priority <- v
    [<JsonProperty("Ttl")>]
    member x.Ttl with get() = ttl and set(v) = ttl <- v
    [<JsonProperty("UrlCallback")>]
    member x.UrlCallback with get() = urlCallBack and set(v) = urlCallBack <- v
    [<JsonProperty("UrlSuccess")>]
    member x.UrlSuccess with get() = urlSuccess and set(v) = urlSuccess <- v
    [<JsonProperty("UrlFailure")>]
    member x.UrlFailure with get() = urlFailure and set(v) = urlFailure <- v

type CreateOrderResponse() =
    let mutable id : string = null
    let mutable total    : decimal = 0.00M
    let mutable currency       : string = "btc"
    let mutable btcRequired    : decimal = 0.00M
    let mutable data  : string  = null
    let mutable createdOn    : DateTime = DateTime.UtcNow
    let mutable expiresOn    : DateTime = DateTime.UtcNow
    let mutable status    : string = "active"
    let mutable last_update    : DateTime = DateTime.UtcNow
    let mutable url    : string = null

    [<JsonProperty("Id")>]
    member x.Id with get() = id and set(v) = id <- v

    [<JsonProperty("Total")>]
    member x.Total with get() = total and set(v) = total <- v
  
    [<JsonProperty("Currency")>]
    member x.Currency with get() = currency and set(v) = currency <- v
  
    [<JsonProperty("BtcRequired")>]
    member x.BtcRequired with get() = btcRequired and set(v) = btcRequired <- v

    [<JsonProperty("Data")>]
    member x.Data with get() = data and set(v) = data <- v

    [<JsonProperty("CreatedOn")>]
    member x.CreatedOn with get() = createdOn and set(v) = createdOn <- v

    [<JsonProperty("ExpiresOn")>]
    member x.ExpiresOn with get() = expiresOn and set(v) = expiresOn <- v

    [<JsonProperty("Status")>]
    member x.Status with get() = status and set(v) = status <- v

    [<JsonProperty("LastUpdate")>]
    member x.LastUpdate with get() = last_update and set(v) = last_update <- v

    [<JsonProperty("Url")>]
    member x.Url with get() = url and set(v) = url <- v

type Client(serverUrl : string, termailId : string, password : string) =
    
    let endPointUrl = serverUrl + "/createPayment"

    let call data =
      let request = 
        createRequest Post endPointUrl
        |> HttpClient.withBasicAuthentication termailId password
        |> withBody data

      getResponseBody request

    member this.CreatePaymentAsString(request : CreateOrderRequest) : string =
      let serializedObject = JsonConvert.SerializeObject(request)
      let response = call serializedObject
      response

    member this.CreatePayment(request : CreateOrderRequest) : CreateOrderResponse =
        let response = this.CreatePaymentAsString(request)
        JsonConvert.DeserializeObject<CreateOrderResponse>(response)
