# Template-Back-End-C-Sharp

## Development

### Build and run API

```sh
dotnet build Controllers/Controllers.csproj
dotnet run --project Controllers/Controllers.csproj
```

### Deploy DBs

```sh
docker-compose -f DataBase/Docker/docker-compose.yml --env-file Controllers/.env up --no-start
docker start pgadmin4Template postgresTemplate docker-redis-1
```

### Stop DBs

```sh
docker stop pgadmin4Template postgresTemplate docker-redis-1
```

---

## Production deploy

```sh
docker-compose up --build
```

---


## Backup db

<!-- TODO: Проверить, правильно ли подтягиваются переменные окружения -->

```sh
sudo docker exec postgresTemplate pg_dump -U $POSTGRES_USER -d $POSTGRES_DB > backup.sql
```

## Restore db

```sh
sudo docker exec -i postgresTemplate psql -U $POSTGRES_USER -d $POSTGRES_DB < backup.sql
```

