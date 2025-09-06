# EnglishUp Backend — Deployment Notes

## Image
- GitLab Registry: `$CI_REGISTRY_IMAGE/englishup-api:latest` (on main)
- Also tagged by commit SHA.

## Run (example docker run)
docker run -d --name englishup-api -p 5000:5000 \
  -e ASPNETCORE_ENVIRONMENT=Production \
  -e ASPNETCORE_URLS=http://0.0.0.0:5000 \
  -e ConnectionStrings__DefaultConnection="Host=<PROD_DB_HOST>;Port=5432;Database=englishup;Username=<USER>;Password=<PASS>" \
  -e Jwt__Issuer=EnglishUp \
  -e Jwt__Audience=EnglishUp \
  -e Jwt__Key="<STRONG_SECRET>" \
  registry.gitlab.com/<group>/<project>/englishup-api:latest

## Health
- Liveness/readiness: `GET /health` → 200 OK

## Ports
- API listens on 5000 (HTTP). Put Nginx/Ingress/ALB in front for HTTPS.

## Notes
- EF Core migrations auto-run at startup.
- Logs go to stdout (Docker logs).
