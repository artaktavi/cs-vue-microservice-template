#!/bin/bash
set -e

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" <<-EOSQL
    CREATE USER app_service WITH PASSWORD '$APP_DB_PASSWORD';
    GRANT CONNECT ON DATABASE app_db TO app_service;
EOSQL

psql -v ON_ERROR_STOP=1 --username "$POSTGRES_USER" --dbname "app_db" <<-EOSQL
    GRANT ALL ON SCHEMA public TO app_service;
    ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON TABLES TO app_service;
    ALTER DEFAULT PRIVILEGES IN SCHEMA public GRANT ALL ON SEQUENCES TO app_service;
EOSQL

