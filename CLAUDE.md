# Umbraco.UIBuilder.Demo

Blog demo site for Umbraco UI Builder. Not a shipped package.

## Branches
- `main` = latest stable major. `support/{major}.x` = older majors still in support. `release/{version}` = the next major, on prerelease packages, until it ships.
- Keep a branch for every UI Builder major whose Umbraco CMS security phase hasn't ended (see https://umbraco.com/products/knowledge-center/long-term-support-and-end-of-life/).
- An update for an older major goes to its `support/{major}.x` branch, never `main`.
- Work on `topic/{major}.x/{name}`, PR into the target branch.

## Updating the demo to newer packages
1. Bump every `Umbraco.Cms*` and `Umbraco.UIBuilder*` `PackageReference` in both `src/*/*.csproj` to the latest stable version **within the branch's major**. Check `https://api.nuget.org/v3-flatcontainer/{id}/index.json`. Don't add new package references.
2. `dotnet build`. Commit the regenerated `appsettings-schema*.json` and `umbraco-package-schema.json` files.
3. **SQLite** (`src/Umbraco.UIBuilder.Demo.Web/umbraco/Data/Umbraco.sqlite.db`):
   - From `src/Umbraco.UIBuilder.Demo.Web`: `Umbraco__CMS__Unattended__UpgradeUnattended=true ASPNETCORE_ENVIRONMENT=Development ASPNETCORE_URLS=https://localhost:<port> dotnet run --no-build --no-launch-profile`.
   - Confirm in `umbraco/Logs/*.json`: `Unattended upgrade completed successfully`, no `Error`/`Fatal`.
   - Check `/`, a blog post page, and in the backoffice: Repositories > Comments, and Content > Pending Comments dashboard.
   - Stop the site before committing (flushes the `-wal` file into the db). Commit `Umbraco.sqlite.db`.
4. On a major change, set `sbom_project_version` in `azure-pipelines.yml` to the branch's major.
5. PR description: list version changes and anything else you had to fix.

## SBOM
`azure-pipelines.yml` runs the shared `Umbraco.Build` SBOM template on `main` and `support/*`, uploading to Dependency-Track project `Umbraco.UIBuilder.Demo` with the major as the version. Its Azure DevOps pipeline lives in the `Umbraco UI Builder` project and needs the `dependency-track` variable group.

## Gotchas
- Backoffice login needs HTTPS. Login: `admin@example.com` / `password1234`.
- Log warning `Could not find an editor with alias Umbraco.MediaPicker` comes from the demo's old content. Ignore it.
- App-local ICU is skipped on macOS (no macOS build of the ICU package), so the site runs there.
