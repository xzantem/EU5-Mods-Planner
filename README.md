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
- `systemd` service template: [deploy/eu5modplanner.service](C:/Users/Patryczku/Documents/New%20project/Eu5ModPlanner/EU5%20Mod%20Planner/deploy/eu5modplanner.service)
- VPS Postgres env template: [deploy/.env.vps.example](C:/Users/Patryczku/Documents/New%20project/Eu5ModPlanner/EU5%20Mod%20Planner/deploy/.env.vps.example)
- App container build file: [Dockerfile](C:/Users/Patryczku/Documents/New%20project/Eu5ModPlanner/EU5%20Mod%20Planner/Dockerfile)

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
- .NET 9 runtime

Then:

1. Create the deploy directory.
2. Put the correct PostgreSQL password in `.env`.
3. Adjust the connection string inside `deploy/eu5modplanner.service`.
4. Make sure the app can be reached through your reverse proxy or open port `5068`.
