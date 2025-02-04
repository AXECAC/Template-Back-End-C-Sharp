# Run docker + postgres

```bash
sudo docker-compose up --no-start

sudo docker start pgadmin4Template

sudo docker start postgresTemplate
```

# Stop docker + postgres

```bash
sudo docker stop pgadmin4Template

sudo docker stop postgresTemplate
```

# Backup db

```bash
sudo docker exec postgresTemplate pg_dump -U aragami templatedb > backup.sql
```

# Restore db

```bash
sudo docker exec -i postgresTemplate psql -U aragami -d templatedb < backup.sql
```
