# EU5 Mod Planner

Content database creator for planning Europa Universalis V mods.

## Local development

Start PostgreSQL with Docker:

```powershell
docker compose up -d
```

Run the app:

```powershell
dotnet run
```

Open:

```text
http://localhost:5068
```

## VPS deployment

This repo now includes:

- GitHub Actions workflow: [.github/workflows/deploy.yml](C:/Users/Patryczku/Documents/New%20project/Eu5ModPlanner/EU5%20Mod%20Planner/.github/workflows/deploy.yml)
- VPS Postgres env template: [deploy/.env.vps.example](C:/Users/Patryczku/Documents/New%20project/Eu5ModPlanner/EU5%20Mod%20Planner/deploy/.env.vps.example)
- App container build file: [Dockerfile](C:/Users/Patryczku/Documents/New%20project/Eu5ModPlanner/EU5%20Mod%20Planner/Dockerfile)
- nginx reverse proxy example: [deploy/eu5modplanner.nginx.conf](C:/Users/Patryczku/Documents/New%20project/Eu5ModPlanner/EU5%20Mod%20Planner/deploy/eu5modplanner.nginx.conf)

### GitHub secrets

- `HOST`
- `HOST_USER`
- `HOST_SSH_KEY`
- `HOST_PORT`

### GitHub variables

- `DEPLOY_PATH`

Example:

```text
/var/www/eu5modplanner
```

### First-time VPS setup

Install on the server:

- Docker
- Docker Compose plugin
- nginx

Then:

1. Create the deploy directory.
2. Put the correct PostgreSQL password in `.env`.
3. Start the containers with `docker compose up -d --build`.
4. Copy the nginx config from `deploy/eu5modplanner.nginx.conf`.
5. Make sure nginx points to the same host port as `APP_PORT` in `.env`.
