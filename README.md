# Search Count

Aggregates search result counts from two providers and exposes them through a small React UI and ASP.NET Core API.

## Prerequisites

- [.NET SDK 10](https://dotnet.microsoft.com/download) (see `apps/api/api.csproj`)
- [Node.js](https://nodejs.org/) (LTS recommended) with npm

## API token (user secrets)

The API calls external search engines with an `x-api-token` header. Tokens are **not** checked into source control; configure them with [.NET User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets) in the API project.

From the repository root:

```bash
cd apps/api

dotnet user-secrets set "SearchProviders:EngineOne:Token" "<your-api-token>"
dotnet user-secrets set "SearchProviders:EngineTwo:Token" "<your-api-token>"
```

Use the API token supplied for the Voyado test task. Both engines currently share the same base URL, so you will usually set the **same** token for `EngineOne` and `EngineTwo`.

Verify secrets are stored (values are hidden in the list output):

```bash
dotnet user-secrets list
```

Base URLs for the engines live in `apps/api/appsettings.json` under `SearchProviders`; only the tokens belong in user secrets.

## Install dependencies

**Frontend** (from repo root):

```bash
cd apps/web
npm install
```

**API**:

```bash
cd apps/api
dotnet restore
```

## Run locally

Use two terminals.

**1. API** (listens on `http://localhost:5135`):

```bash
cd apps/api
dotnet run
```

**2. Web** (Vite dev server; proxies `/api` to the API):

```bash
# from repo root
npm run dev:web
```

Or from `apps/web`:

```bash
npm run dev
```

Open the URL Vite prints (typically `http://localhost:5173`). Search requests go to `/api/search` on the dev server and are proxied to the API.

## Build

**Web**:

```bash
cd apps/web
npm run build
```

**API**:

```bash
cd apps/api
dotnet build
```

## Useful scripts (repo root)

| Script           | Description                          |
| ---------------- | ------------------------------------ |
| `npm run dev:web` | Start Vite dev server               |
| `npm run build:web` | Production build of the web app   |
| `npm run typecheck` | TypeScript check (`apps/web`)     |
| `npm run check`   | Biome lint/format check (`apps/web`) |

## Health check

With the API running:

```bash
curl http://localhost:5135/health
```
