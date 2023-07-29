# Flight Plan API

*[Pluralsight](https://app.pluralsight.com/library/courses/asp-dot-net-core-6-webapi-using-nosql-databases/table-of-contents)

* dotnet new webapi -n flightplan
* cd flightplan
* dotnet add package Microsoft.Azure.Cosmos
* dotnet add package Mongodb.Driver
* dotnet build
* dotnet run

* URI = Uniform Resource Identifier - Identity of resource - user name
* URL - Unitform Resource Locator - Location of resource - user email

## Endpoints

Let the HTTP Verbs do the job of describing the action, just describe the entity in the URI path.

* GET http://api.starwalks.com/dogs
* GET http://api.starwalks.com/dogs/clientid/
* GET http://api.starwalks.com/dogs/clientid/{dogname}
* POST http://api.starwalks.com/dogs/clientid/ -body
* PUT http://api.starwalks.com/dogs/clientid/{dogname} -body
* DELETE http://api.starwalks.com/dogs/clientid/{dogname}

## Status Codes

* 200 - ok
* 204 - no content
* 400 - bad request
* 401 - unauth
* 404 - not found
* 500 - internal server error

## Auth

* oauth
* openid
* basic auth
* active AD

## Versioning Examples

* GET http://api.starwalks.com/v1/dogs  
* GET http://api.starwalks.com/dogs/?version=1.1

## Test Data Gen

* https://www.mockaroo.com/
* https://www.mockaroo.com/schemas/428948

## Development

* See properties/launchsettings which is only for local. See urls and ssl ports.

## Model

* Json property name added to each property to make serialisation and deserialisation work

## Separating Data into Interface

* This means can swap data out. This is the IDatabaseAdapter.cs file.
* Then create database specific class which has to implement the methods in IDatabaseAdapter class.
* Created TransactionResult file to contain enum to handle status return codes consistently so descriptive in our interface and can be consistent across database providers. We return these in our POST and PUT.
* DELETE stays as a simple bool.

## Why are we using async calls?

* Because depends on an external resource - the database.