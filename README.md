# DevUtilityHub

A full-stack developer utility web application.
All processing logic lives in an ASP.NET Core Web API; React is a thin presentation layer that sends input and displays output only.

<!--**Live:** [toolkit.nikkapaola.com](https://toolkit.nikkapaola.com) &nbsp;|&nbsp; **API:** [api.toolkit.nikkapaola.com](https://api.toolkit.nikkapaola.com)-->

---

## Tools

| Tool                            | Description                                                 |
| ------------------------------- | ----------------------------------------------------------- |
| **Universal Formatter**         | Prettify, minify, and validate JSON and XML                 |
| **Base64 / URL / HTML Encoder** | Encode and decode all three formats                         |
| **Text Tools**                  | Case conversion, trim, sort, deduplicate, reverse, and more |
| **Diff Checker**                | Side-by-side text diff with line-level change highlighting  |
| **Timestamp Converter**         | Unix ↔ human-readable, seconds and milliseconds             |

---

## Stack

### Frontend

- React 19 + TypeScript + Vite
- Tailwind CSS v4 with a terminal-inspired dark/light design system
- Axios for API communication
<!--- Deployed to **Vercel**-->

### Backend

- .NET 10 Web API (ASP.NET Core)
- Layered architecture: Controllers → Services → Core
- OpenAPI / Swagger documentation
<!--- Deployed to **Azure App Service**-->

---

## Architecture

```
Request → Controller → Service → Core → Response
```

- **Controllers** (`Controllers/`) — HTTP routing only, delegate to services
- **Services** (`Services/`) — orchestrate Core logic, registered via DI
- **Core** (`Core/`) — pure algorithm implementations with no dependencies:
  - `Core/Encoders/` — Base64, URL, HTML
  - `Core/Formatters/` — JSON, XML
  - `Core/Transformers/` — case, cleanup, line operations
  - `Core/Diff/` — text diff engine (powered by DiffPlex)
  - `Core/TimestampConverter.cs` — Unix ↔ datetime conversions
  - `Core/Detectors/` — auto-detect input format
- **Models** (`Models/`) — request and response DTOs

Error handling is centralized in `Middleware/ExceptionMiddleware.cs`.
Service registrations live in `Extensions/ServiceCollectionExtensions.cs`.

### Frontend structure

```
src/
├── pages/          # One page per tool
├── components/
│   ├── common/     # Button, TextArea, TerminalPane, Loader
│   ├── layout/     # Header, Sidebar, PageLayout
│   ├── diff/       # Diff-specific components
│   ├── formatter/  # Formatter-specific components
│   └── text/       # Text tool components
├── api/            # One module per backend controller
├── hooks/          # useApi — shared async state pattern
├── types/          # TypeScript interfaces mirroring backend models
└── context/        # ThemeContext (dark/light, persisted to localStorage)
```

---

## Getting Started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Node.js 20+](https://nodejs.org/)

### Backend

```bash
cd DevUtilityHub.Api
dotnet build
dotnet run
```

API is available at `https://localhost:7xxx` / `http://localhost:5000`.
Swagger UI is available at `/swagger` in Development.

### Frontend

```bash
cd dev-utility-hub-ui
npm install
npm run dev
```

App is available at `http://localhost:5173`.

The frontend reads `VITE_API_BASE_URL` from `.env`:

```env
VITE_API_BASE_URL=http://localhost:5000/api
```

---

## Commands

### Backend (`DevUtilityHub.Api/`)

```bash
dotnet build    # Build the API
dotnet run      # Run the API
dotnet test     # Run all tests
```

### Frontend (`dev-utility-hub-ui/`)

```bash
npm run dev     # Start Vite dev server with HMR
npm run build   # Type check + production build
npm run lint    # ESLint check
npm run preview # Preview production build locally
```

### Docker (local development only)

```bash
docker build -t devutilityhub ./DevUtilityHub.Api
```

> Azure App Service Free F1 does not support Docker containers. Docker is for local containerized development only. A B1 plan or higher is required for container deployment to Azure.

## <!--

## Deployment

| Target   | Platform                  | URL                          |
| -------- | ------------------------- | ---------------------------- |
| Frontend | Vercel                    | `toolkit.nikkapaola.com`     |
| Backend  | Azure App Service Free F1 | `api.toolkit.nikkapaola.com` |

**CORS** allows `https://toolkit.nikkapaola.com` and `http://localhost:5173`.

Set `VITE_API_BASE_URL=https://api.toolkit.nikkapaola.com/api` as a Vercel environment variable for production builds.-->

---

## Testing

Unit tests target the Core layer directly — pure C# with no HTTP or DI dependencies.

```bash
dotnet test
```

Test files mirror the Core module structure (Encoders, Formatters, Transformers, etc.).

---

## API Overview

All endpoints accept and return JSON. Error responses follow a consistent structure handled by `ExceptionMiddleware`.

| Method | Endpoint              | Description                                        |
| ------ | --------------------- | -------------------------------------------------- |
| POST   | `/api/format/process` | Format, minify, or validate JSON / XML             |
| POST   | `/api/encode/process` | Encode or decode Base64 / URL / HTML               |
| POST   | `/api/text/transform` | Apply text transformations pipeline                |
| POST   | `/api/diff`           | Compute line-level text diff                       |
| POST   | `/api/time/convert`   | Convert between Unix and human-readable timestamps |

Full schema is available via Swagger at `/swagger` when running locally.
