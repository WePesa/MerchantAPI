namespace Monitex

open HttpClient

open System
open System.Text

open Newtonsoft.Json

type CreatePaymentRequest(amount : decimal, currency : string) =

    let mutable amount : decimal = amount
    let mutable currency : string = currency
    let mutable data : string = null
    let mutable priority : int = 0
    let mutable ttl : int = 5 // minutes
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

type Payment() =
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

type PaymentStatus() =
    let mutable status : string = null

    [<JsonProperty("Status")>]
    member x.Status with get() = status and set(v) = status <- v

type Client(serverUrl : string, termailId : string, password : string) =

    let call endPoint data =
      let endPointUrl = serverUrl + endPoint
      let request = 
        createRequest Post endPointUrl
        |> HttpClient.withBasicAuthentication termailId password
        |> withBody data

      getResponseBody request

    let get endPoint =
      let endPointUrl = serverUrl + endPoint
      let request = 
        createRequest Get endPointUrl
        |> HttpClient.withBasicAuthentication termailId password

      getResponseBody request

    member this.CreatePaymentAsString(request : CreatePaymentRequest) : string =
      let serializedObject = JsonConvert.SerializeObject(request)
      let response = call "/v1/payments" serializedObject
      response

    member this.CreatePayment(request : CreatePaymentRequest) : Payment =
        let response = this.CreatePaymentAsString(request)
        JsonConvert.DeserializeObject<Payment>(response)

    member this.GetPayment(id : String) : Payment =
        let response = get ("/v1/payments/" + id)
        JsonConvert.DeserializeObject<Payment>(response)

    member this.GetPaymentStatus(id : String) : PaymentStatus =
        let response = get ("/v1/payments/" + id + "/status")
        JsonConvert.DeserializeObject<PaymentStatus>(response)
