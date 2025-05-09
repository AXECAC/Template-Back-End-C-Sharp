# Template-Back-End-C-Sharp

## Development

### Create .env file

- Create file `.env` in Controllers directory
- Add the following parameters to the file:
    - `POSTGRES_HOST` - address of Postgres database (set to `localhost` when deploying db on your device)
    - `POSTGRES_PORT` - port of Postgres database
    - `POSTGRES_DB` - name of Postgres database
    - `POSTGRES_USER` - user name for connecting to Postgres database
    - `POSTGRES_PASSWORD` - password for connection to Postgres database
    - `REDIS_HOST` - address of Redis database
    - `REDIS_PORT` - port of Redis database
    - `SECRET_KEY` - encryption key for passwords hashing
        - Use following command to generate key:
        ```sh
        openssl rand -hex 32
        ```
Example of .env file:
```
POSTGRES_HOST=localhost
POSTGRES_PORT=5431
POSTGRES_DB=templatedb
POSTGRES_USER=aragami
POSTGRES_PASSWORD=password
REDIS_HOST=redis
REDIS_PORT=6379
SECRET_KEY=166eaa88fe76de61d37d56b3032b671f0150630f40f7c86d04918968246c9e01
```

### Build and run API

```sh
dotnet build Controllers/Controllers.csproj
dotnet run --project Controllers/Controllers.csproj
```

### Deploy DBs

```sh
sudo docker-compose -f DataBase/Docker/docker-compose.yml --env-file Controllers/.env up --no-start
sudo docker start pgadmin4Template postgresTemplate docker-redis-1
```

### Stop DBs

```sh
sudo docker stop pgadmin4Template postgresTemplate docker-redis-1
```

---

## Production deploy

### Create .env file

- Create file `.env` in the root project's directory
- Add all the parameters mentioned above in the “Development” point to the file, but for some, set the following values:
    - `POSTGRES_HOST=postgresTemplate`
    - `POSTGRES_PORT=5432`
    - `REDIS_HOST=redis`
    - `REDIS_PORT=6379`

### Deploy all containers

```sh
sudo docker-compose up --build
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

