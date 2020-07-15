# Sybiz Vision Web API Sample Code. 

The Sybiz Web API is a REST based service providing access to the Sybiz Vision Platform.

The project and solution provided here give some basic examples of how to consume and use the Vision Web Service, you will need to change the username & password values passed into each call.

Formal documentation is available here: https://documenter.getpostman.com/view/8312536/SzmmWFax?version=latest

## Authentication

Authentication of the API is handled through the use of both bearer tokens and refresh tokens. Bearer tokens are required for every request to the API and are supplied through the authorization header using the bearer token type. Refresh tokens are generated alongside bearer tokens which allow the generation of a new bearer token after it has expired without having to send through sensitive credentials. Bearer tokens have an expiry period of 7 days and refresh tokens have an expiry period of 30 days. Consult the requests in ADM/Security for more detailed information on generating these tokens.

## Authorization

Authorization for the requests are handled through the roles in the platform in the same way that an installed version or third party application would. As a result, if a user does not have add, edit, delete or access permissions then the relevant requests will return the forbidden response of 403.

## Structure

If you&#x27;re familiar with the structure of the platform then you&#x27;ll be familiar with the way in which the API is structured. Much like the platform, the API is broken down into editable objects, info lists and lookup info lists. Generally, each type will have the same basic functionality and structure of others of the same type.

### Editable Objects

Editable objects are general representations of the edit forms that you would see on an installed version of Vision. Editable objects contain all the fields and sublists that you would see on these screens and support get, post and delete methods. Editable objects generally support field filtering and need the relevant add, edit, delete or access authorization depending on the method of the request.

    API/DR/Customer/12
    API/DR/Customer/12/Contacts
    API/DR/Customer/12/Contacts/36
    API/DR/SalesQuote/25
    API/DR/SalesQuote/25/Lines
    API/DR/SalesQuote/25/Lines/50
	
### Info Lists

Info lists are general representations of the lists that you would see on an installed version of Vision. Info lists contain the fields visible on the grid, the fields available on the column chooser and a list of extended properties. Info lists generally support field filtering, sorting and paging and need the relevant access authorization in order to make the request. Only the get method is supported for an info list.

    API/DR/CustomerInfoList
    API/CM/AnalysisCodeInfoList

### Lookup Info Lists

Lookup info lists are general representations of the drop-down lists that you would see on an installed version of Vision. Lookup info lists contain very limited fields often only comprising of id, code, description and active fields. Sometimes a few extra fields are available which can be used to filter the list if needed. Lookup info lists generally support field filtering, sorting and paging and don&#x27;t require any authorization in order to make the request. Only the get method is supported for a lookup info list.

    API/DR/CustomerLookupInfoList
    API/CM/AnalysisCodeLookupInfoList

## Response Codes

200 (Ok)

* Returned when a request completes successfully

204 (No Content)

* Returned when a request successfully deletes an object
* Returned when a request returns no results

400 (Bad Request)

* Returned when a request attempts to process an object that isn&#x27;t in a processable state
* Returned when a request attempts to save an object that isn&#x27;t in a saveable state
* Returned when a request contains JSON that couldn&#x27;t be converted to an object
* Returned when an invalid authorization token is provided with a request

401 (Unauthorized)

* Returned when no authorization is provided with a request

403 (Forbidden)

* Returned when a user attempts an operation for which they do not have authorization (add, edit, delete, access)
* Returned when a user attempts to modify an object that is currently locked by another user
* Returned when a user attempts to delete or deactivate an object that has references

500 (Internal Server Error)
* Returned when a general error has occurred from the API