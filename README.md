# EventReservation API

A REST API for managing events and reservations, built with ASP.NET Core. Organizers create events with limited capacity, participants reserve a spot, and the system enforces rules like capacity limits and duplicate-reservation prevention.

## About the project

Built as a portfolio project to focus on two areas I hadn't explored yet: JWT authentication/authorization and automated testing. The API follows a layered structure (Domain, Infrastructure, Api, Tests), with business rules isolated in the domain layer to keep them easy to test.

## Tech stack

ASP.NET Core 9 · Entity Framework Core · SQL Server (Azure SQL) · JWT · ASP.NET Core Identity (password hashing) · xUnit · SQLite (in-memory, for tests) · Azure App Service · GitHub Actions

## Features

- User registration and login, with hashed passwords
- JWT authentication with role claims
- Role-based authorization: `Admin`, `Organizer`, `Participant`
- Event CRUD with ownership control (only the organizer or an admin can edit/delete)
- Reservation system with business rules: capacity limit, no duplicate reservations, no reservations for past events, cancellation (soft delete)
- Automatic admin user seeding on first run
- Unit and integration tests covering business rules, auth, and API flows

## Architecture

Four projects in one solution, following a layered structure inspired by Clean Architecture:

- `EventReservation.Domain` - entities and pure business rules, no external dependencies
- `EventReservation.Infrastructure` - EF Core, DbContext, migrations
- `EventReservation.Api` - controllers, authentication, configuration
- `EventReservation.Tests` - unit and integration tests

`Domain` doesn't depend on any other layer, so business rules (like checking an event's capacity) can be tested without a database or HTTP server.

## Running locally

**Prerequisites:** .NET 9 SDK, SQL Server, `dotnet tool install --global dotnet-ef`

```bash
git clone https://github.com/Vitormjere/EventReservation.git
cd EventReservation

dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YOUR_CONNECTION_STRING" --project EventReservation.Api
dotnet user-secrets set "Jwt:SecretKey" "YOUR_SECRET_KEY" --project EventReservation.Api
dotnet user-secrets set "Jwt:Issuer" "EventReservationApi" --project EventReservation.Api
dotnet user-secrets set "Jwt:Audience" "EventReservationApiUsers" --project EventReservation.Api

dotnet ef database update --project EventReservation.Infrastructure --startup-project EventReservation.Api
dotnet run --project EventReservation.Api
```

On first run, an admin user is created automatically (email: `admin@eventreservation.com`, password: `Admin@123` — recommended to change it before any real use).

## Tests

```bash
dotnet test
```

Includes unit tests (isolated business rules and services) and integration tests (full HTTP flow, using an in-memory SQLite database).

## Live API

`https://eventreservation-api-vitor-dugnc2fra6d3hne7.brazilsouth-01.azurewebsites.net`

## Author

Vitor Miranda Jeremias — [GitHub](https://github.com/Vitormjere)
