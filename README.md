# .NET 5.0 with Aurelia SPA (TypeScript)

The project is written in .NET Core on backend and Aurelia with TypeScript on frontend. 	 
	 
### About Solution

SolutionName: AureliaWithDotNet.Application

Projects:
AureliaWithDotNet.Web - .Net 5.0 Kestrel Host, Hosts the static content as well as a WebAPI for the Frontend
AureliaWithDotNet.Data - .Net 5.0 Class Library handles every DataAccess also HTTPDataAccess for country validation, uses RepositoryPattern and UnitOfWork Pattern
AureliaWithDotNet.Domain – .Net 5.0 Class Library containing business logic, validators, interfaces and Models
The solution contains the needed docker files to run the application out of the box in docker environment.

Web solution have rest endpoints, as well have an user interface/form to apply data.
The Api have the following actions:
POST for Creating an Object – returning an 201 on successful creation of the object and the url where the object can be called
GET with id parameter – to ask for an object by id
PUT – to update the object with the given id
DELETE – to delete the object with the given id

The Object handled is the class named Asset with the following properties:
ID (int)
AssetName (string)
Department (enum) - the following values are (HQ, Store1, Store2, Store3, MaintenanceStation)
CountryOfDepartment (string)
EMailAdressOfDepartment (string)
PurchaseDate (Datetime) - in UTC.
broken (bool) – false if not provided.

The WebApi accepts and returns application/json data.

The object and the properties are validated by FluentValidation ( nuget ) with the following rules:
AssetName – at least 5 Characters
Department – must be a valid enumvalue
CountryOfDepartment – must be a valid Country – therefore asked with an httpclient here https://restcountries.eu/rest/v2/… – ApiDescription: https://restcountries.eu/#api-endpoints-full-name if the country is found, the country is valid.
PurchaseDate - must not be older then one year.
EMailAdressOfDepartment – must be an valid email (only check for valid syntax *@*.[valid topleveldomain])
broken – If provided should not be null
If the object is invalid ( on post and put ) – returns 400 and an information what property does not fullyfy the requirements and which requirement is not fullyfied.

### Swagger

A described API with swagger using Swashbuckle v5 host and the SwaggerUI under [localhost]/swagger.
Examples data was provided in the SwaggerUI, so when you click on try it out there is already useful valid data in the object that can be posted.

### Logging

Using .NetCore logging to log each interaction with the API wherever it’s meaningful to do so and also write the log to a 
serilog rolling file sink the name is setable in the applicationsettings.json file. 

### Frontend

The including Form is an Aurelia ( http://aurelia.io ) Application which uses the API to Post Data AND Validate all the inputs with the exact same parameters as the API does.
	uses Typescript
	uses Webpack
	form can only be send if the data is valid
	uses Boostrap for the UI
	uses aurelia-validation
	uses a Bootstrap FormRenderer
	invalid fields are marked with an red border and an explanation why the date is invalid
	the form has two buttons - send and reset.
	clicking the reset button an aurelia-dialog is shown - which ask if the user is really sure to reset all the data
	the reset button is only enabled if the user has typed in data -> if all fields are empty the reset button is not enabled.
	when the user has touched a field but afterwards deleted all entries, the reset button is also not enabled.
	the send button is only active if all required fields are filled out and are valid.
	after sending the data, the aurelia router redirects to a view which confirms the sending.
	if the sending was not successful an error message is shown in a aurelia-dialog. Describing what was wrong.

### Database

The project uses EntityFramework core 5.0 and EntityFramework in memory database to save the data.


### Running the project

Run these commands to install the dependencies and to run the project

	npm install
	
	dotnet restore
	
	npm run dev
	
	dotnet run
	
When running `npm run dev` each change updates the output dev resources, `wwwroot`, so even if you stop your Web App, it will remains in a working state to Aurelia.

### Running the project with Docker

	docker build -t hahn:latest .
	
	docker run -p 5000:5000 -t hahn .
	
You will be able to see the project running accessing `http://localhost:5000`


### Testing

Run the unit tests and integration tests with

     dotnet test