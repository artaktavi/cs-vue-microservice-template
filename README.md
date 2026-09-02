# Full-stack service template

Чистый стартовый каркас для нового full-stack приложения без предметной логики.

## Состав

- `api-proxy` — YARP reverse proxy и хост для собранного Vue-приложения.
- `app-service` — минимальный ASP.NET Core сервис с PostgreSQL, миграциями, health checks и метриками.
- `shared` — общий слой наблюдаемости и запуска PostgreSQL-миграций.
- `web-frontend` — Vue 3, TypeScript, Vite, Naive UI, Router и i18n.
- `postgres-db` — инициализация БД и пример версионируемой миграции.
- `observability` — OpenTelemetry Collector, Prometheus, Grafana, Loki, Tempo, Promtail и node-exporter.
- `.github/workflows` — CI, публикация образов в GHCR и выборочный deploy по SSH.

## Локальный запуск

1. Скопируйте `.env.example` в `.env` и замените пароли.
2. Выполните `docker compose up --build`.
3. Откройте `https://localhost` или `http://localhost`.

Сервис доступен через proxy по `/api/app/status`. Технические endpoints: `/health`, `/healthmetrics`, `/metrics`.

## Проверки без Docker

```bash
dotnet restore template.slnx
dotnet build template.slnx -c Release
dotnet test app-service/Tests/AppService.IntegrationTests.csproj -c Release
cd web-frontend
npm ci
npm run lint:ci
npm run type-check
npm run build
```

Перед первым deployment настройте GitHub Environments `testing` и `production`, secrets из `DEPLOYMENT.md` и замените нейтральные имена образов/namespace при необходимости.
