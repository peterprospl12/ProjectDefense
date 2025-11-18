# ProjectDefense

ProjectDefense is a booking system for project defense time slots in university labs — a web application (Razor Pages + Minimal API), a domain layer, an application layer, and a simple console client. The project includes authentication/authorization (Identity), automatic slot generation based on lecturer availability, student self‑booking, and exporting reservation lists.

## Table of contents

- [Requirements](#requirements)
- [Quick start (local)](#quick-start-local)
- [Configuration](#configuration)
- [Build and run](#build-and-run)
- [Project architecture](#project-architecture)
- [API (Minimal Endpoints)](#api-minimal-endpoints)
- [Models and DTOs](#models-and-dtos)
- [Database and migrations](#database-and-migrations)
- [UI and Pages (Razor)](#ui-and-pages-razor)
- [Console client](#console-client)
- [Data export](#data-export)

## Requirements

- .NET 9 SDK (the project targets `net9.0`)
- (Optional) Visual Studio 2022/2023, Rider, or another .NET‑capable IDE
- SQLite (the repository includes a default `ProjectDefense.db` file)

## Quick start (local)

1. Clone the repository:

```powershell
git clone <repo-url>
cd ProjectDefense
```

2. Restore dependencies and build the solution:

```powershell
dotnet restore
dotnet build
```

3. Run the web application (the `ProjectDefense` project under the `ProjectDefense` folder):

```powershell
dotnet run --project ProjectDefense\ProjectDefense.Web.csproj
```

Swagger (Development only) is enabled by default when the app runs in Development.

## Configuration

Main settings live in `ProjectDefense/appsettings.json` and `appsettings.Development.json`.

- ConnectionStrings: `DefaultConnection` is set to `Data Source=ProjectDefense.db` (SQLite) by default.
- MailGun: email delivery configuration (used by `IEmailService`) — consider storing production secrets in user‑secrets or environment variables.
- ASPNETCORE_ENVIRONMENT: `Development` / `Production` — affects Swagger, error pages, and data seeding.

## Build and run

- Build (Release):

```powershell
dotnet publish ProjectDefense\ProjectDefense.Web.csproj -c Release -o ./publish
```

- Run in Development (see Quick start above).

## Project architecture

The solution is split into layers:

- `ProjectDefense.Web` – web app (Razor Pages), middleware, DI configuration, minimal API endpoints.
- `ProjectDefense.Application` – application logic: DTOs, repository interfaces, use cases (commands & queries), validators.
- `ProjectDefense.Domain` – domain entities (`User`, `Room`, `LecturerAvailability`, `Reservation`, `StudentBlock`).
- `ProjectDefense.Infrastructure` – repository implementations, `ApplicationDbContext` (Identity + EF Core), migrations, and services (Email, Export).
- `ProjectDefense.ConsoleApp` – a simple console client that calls the API.

A broader requirements overview is available in `ProjectDefense/architecture.md`.

## API (Minimal Endpoints)

Minimal endpoints are defined in `ProjectDefense/Api/SlotsController.cs`:

- GET /api/slots/available
  - Returns a list of available slots: `AvailableReservationDto` (Id, StartTime, EndTime, RoomName, RoomNumber)

- GET /api/rooms
  - Returns a list of rooms: `RoomDto` (Id, Name, Number)

- POST /api/slots/{id}/book
  - Books a slot with the specified `id`. Request body: `BookReservationDto` { StudentIndex }
  - May return 200 OK or 404 NotFound

Example calls (curl):

```bash
curl http://localhost:5172/api/slots/available

curl -X POST http://localhost:5172/api/slots/123/book \
  -H "Content-Type: application/json" \
  -d '{"studentIndex":"s12345"}'
```

> Note: Authentication and roles are configured through Identity; not all endpoints must be exposed in the UI.

## Models and DTOs

Key DTOs live in `ProjectDefense.Application/DTOs`:

- `AvailableReservationDto` – simple DTO for available slots.
- `BookReservationDto` – request model for booking (`StudentIndex`).
- `ReservationDto` – a richer reservation DTO with student and timing info.
- `RoomDto` – room DTO.

Domain entities (short):

- `User` (inherits `IdentityUser`) – extra `Role` field.
- `Room` – Id, Name, Number.
- `LecturerAvailability` – lecturer availability definition (StartDate, EndDate, RoomId, SlotDurationInMinutes) and generated `Reservation` list.
- `Reservation` – StartTime, EndTime, AvailabilityId, StudentId, IsBlocked.
- `StudentBlock` – temporarily or permanently blocks students.

## Database and migrations

The project uses EF Core with SQLite via the default connection string `Data Source=ProjectDefense.db`.

Migrations are in `ProjectDefense.Infrastructure/Migrations` (e.g., `20251115141711_InitialCreate`).

On startup, the app runs migrations automatically (see `Program.cs` and `dbContext.Database.MigrateAsync()`).

To run migrations manually:

```powershell
# from the solution root
dotnet ef database update \
  --project ProjectDefense\ProjectDefense.Infrastructure\ProjectDefense.Infrastructure.csproj \
  --startup-project ProjectDefense\ProjectDefense.Web.csproj
```

## UI and Pages (Razor)

Razor Pages reside under `ProjectDefense/Pages`. There are dedicated areas/views for Student and Lecturer roles, and Identity pages live under `Areas/Identity`.

Main local routes:

- Home: `/` (Index)
- Student panel: `/Student` (list of available slots)
- Lecturer panel: `/Lecturer` (manage rooms, availability, export)

## Console client

`ProjectDefense.ConsoleApp` is a simple client that issues HTTP calls to the minimal API (e.g., fetch available slots and book).

Run:

```powershell
dotnet run --project ProjectDefense\ProjectDefense.ConsoleApp\ProjectDefense.ConsoleApp.csproj
```

## Data export

The infrastructure layer includes an `ExportService` that can export reservations to `.txt`, `.xlsx` (ClosedXML), and `.pdf` (QuestPDF). Usage is exposed in the Lecturer UI (Export page).
