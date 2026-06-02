# Search Count

Aggregates search hit counts from two external search providers. A React UI submits queries to an ASP.NET Core API, which queries both engines and returns combined totals.

## Prerequisites

- [.NET SDK 10](https://dotnet.microsoft.com/download) (`net10.0` in [`apps/api/api.csproj`](apps/api/api.csproj))
- [Node.js](https://nodejs.org/) (LTS recommended) with npm

## Installation

From the repository root:

```bash
npm install
dotnet restore
```

- **Root** — `concurrently` (for `npm run dev`)
- **Web** — `apps/web` dependencies
- **API** — NuGet packages for `apps/api`

## How to run locally

**Primary command** — from the repository root:

```bash
npm run dev
```

Starts the API (`dotnet watch`, `http://localhost:5135`) and Web (Vite, typically `http://localhost:5173`) in one terminal with labeled output (`API`, `WEB`). Open the URL Vite prints.

Configure API tokens before searching — see [Environment / secrets setup](#environment--secrets-setup). Without them, engine calls fail.

During development, the Vite dev server proxies `/api/*` to the API ([`apps/web/vite.config.ts`](apps/web/vite.config.ts)), so the browser calls same-origin `/api/search`.

### Manual fallback (two terminals)

**Terminal 1 — API:**

```bash
cd apps/api
dotnet run
```

**Terminal 2 — Web** (from repo root):

```bash
npm run dev:web
```

Or from `apps/web`: `npm run dev`

## Environment / secrets setup

The API authenticates to external search engines with an `x-api-token` header. Tokens are not in source control — set them with [.NET User Secrets](https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets):

```bash
cd apps/api

dotnet user-secrets set "SearchProviders:EngineOne:Token" "<engine-one-token>"
dotnet user-secrets set "SearchProviders:EngineTwo:Token" "<engine-two-token>"
```

Use the API tokens supplied for the Voyado test task. Both engines share the same base URL in [`appsettings.json`](apps/api/appsettings.json); set `EngineOne` and `EngineTwo` tokens independently.

Verify (values hidden in output):

```bash
dotnet user-secrets list
```

Engine base URLs live in [`apps/api/appsettings.json`](apps/api/appsettings.json) under `SearchProviders`; only tokens belong in user secrets.

## Tech stack

**API** — ASP.NET Core 10, Serilog (console), `IMemoryCache`, `Microsoft.Extensions.Http.Resilience`

**Web** — React 19, Vite 8, TypeScript, Tailwind CSS 4, Biome

**Tooling** — `concurrently` at repo root

## Architecture overview

A React + Vite frontend talks to an ASP.NET Core API, which calls two external search engines. The user enters a query in the UI; the API tokenizes it, fetches hit counts from both engines per token, caches results, and returns aggregated totals to the UI.

## Key features

- **Tokenization** — query split on whitespace; each term queried against both engines in parallel
- **Aggregation** — per-provider counts (`engineOne`, `engineTwo`); `totalHits` is the sum
- **Caching** — `IMemoryCache`, 5-minute TTL; key `search:{terms sorted and joined by |}`; hit/miss logged
- **Logging** — Serilog to console ([`Program.cs`](apps/api/Program.cs))
- **Resilience** — HTTP clients retry 3 times (2s delay); per engine/term failures logged as `0`; HTTP 200 with partial results
- **UI** — search input, loading skeleton, total hits or error message

## API documentation

### Health

```bash
curl http://localhost:5135/health
```

Response: `{ "status": "ok" }`

### Search

```bash
curl "http://localhost:5135/api/search?q=hello%20world"
```

Via Vite proxy (dev servers running):

```bash
curl "http://localhost:5173/api/search?q=hello%20world"
```

| Item | Detail |
| ---- | ------ |
| Method | `GET` |
| Path | `/api/search` |
| Query | `q` (required; whitespace-only → `400`) |

**Example response:**

```json
{
  "query": "hello world",
  "results": [
    { "provider": "engineOne", "count": 42 },
    { "provider": "engineTwo", "count": 17 }
  ],
  "totalHits": 59
}
```

REST examples for IDE clients: [`apps/api/api.http`](apps/api/api.http).

**400** when `q` is missing or whitespace:

```json
{ "error": "Query parameter 'q' is required." }
```

## Testing

From the repository root:

```bash
dotnet test
```

Unit tests cover `QueryTokenizer`, `SearchResultAggregator`, and `SearchService` using mocked search engines. Built with xUnit, Moq, and FluentAssertions.