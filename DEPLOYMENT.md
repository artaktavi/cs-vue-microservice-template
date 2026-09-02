# Deployment

Pipeline сохраняет трехступенчатую схему: CI проверяет код и Dockerfile, `Publish Images` публикует выбранные образы в GHCR, `Deploy` устанавливает выбранные сервисы на сервер по SSH.

## GitHub secrets для environment

- `DEPLOY_HOST`
- `DEPLOY_PORT`
- `DEPLOY_USER`
- `DEPLOY_PATH`
- `DEPLOY_SSH_PRIVATE_KEY`
- `DEPLOY_HEALTHCHECK_URL` — необязательный внешний URL проверки.

## Подготовка сервера

В `DEPLOY_PATH` должен находиться приватный `.env`, созданный по `.env.example`. TLS-сертификат для proxy можно смонтировать отдельно и задать через переменные Kestrel. Workflow не загружает секреты и не перезаписывает серверный `.env`.

Рекомендуемый порядок: успешный `CI` для commit → `Publish Images` с immutable тегом `sha-<commit>` → `Deploy` с тем же тегом.

