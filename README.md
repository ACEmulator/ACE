# ACEmulator Core Server

[![Discord](https://img.shields.io/discord/261242462972936192.svg?label=play+now!&style=for-the-badge&logo=discord)](https://discord.gg/C2WzhP9)

Build status: [![GitHub last commit (master)](https://img.shields.io/github/last-commit/acemulator/ace/master)](https://github.com/ACEmulator/ACE/commits/master) [![Windows CI](https://ci.appveyor.com/api/projects/status/rqebda31cgu8u59w/branch/master?svg=true)](https://ci.appveyor.com/project/LtRipley36706/ace/branch/master) [![docker build](https://github.com/ACEmulator/ACE/actions/workflows/docker-image.yml/badge.svg)](https://hub.docker.com/r/acemulator/ace)

[![Download Latest Server Release](https://img.shields.io/github/v/release/ACEmulator/ACE?label=latest%20server%20release) ![GitHub Release Date](https://img.shields.io/github/release-date/acemulator/ace)](https://github.com/ACEmulator/ACE/releases/latest)
[![Download Latest World Database Release](https://img.shields.io/github/v/release/ACEmulator/ACE-World-16PY-Patches?label=latest%20world%20database%20release) ![GitHub Release Date](https://img.shields.io/github/release-date/acemulator/ACE-World-16PY-Patches)](https://github.com/ACEmulator/ACE-World-16PY-Patches/releases/latest)

[![GitHub All Releases](https://img.shields.io/github/downloads/acemulator/ace/total?label=server%20downloads)](https://github.com/ACEmulator/ACE/releases) [![GitHub All Releases](https://img.shields.io/github/downloads/acemulator/ACE-World-16PY-Patches/total?label=database%20downloads)](https://github.com/ACEmulator/ACE-World-16PY-Patches/releases) [![Docker Pulls](https://img.shields.io/docker/pulls/acemulator/ace)](https://hub.docker.com/r/acemulator/ace)

**ACEmulator is a custom, completely from-scratch open source server implementation for Asheron's Call built on C#**
 * MySQL and MariaDB are used as the database engine.
 * Latest client supported.
 * [![License](https://img.shields.io/github/license/acemulator/ace)](https://github.com/ACEmulator/ACE/blob/master/LICENSE)

***
## Disclaimer
**This project is for educational and non-commercial purposes only, use of the game client is for interoperability with the emulated server.**
- Asheron's Call was a registered trademark of Turbine, Inc. and WB Games Inc which has since expired.
- ACEmulator is not associated or affiliated in any way with Turbine, Inc. or WB Games Inc.
***
## Getting Started
Extended documentation can be found on the project [Wiki](https://github.com/ACEmulator/ACE/wiki).
* [Developing ACE](https://github.com/ACEmulator/ACE/wiki/ACE-Development)
* [Hosting ACE](https://github.com/ACEmulator/ACE/wiki/ACE-Hosting)
* [Content Creation](https://github.com/ACEmulator/ACE/wiki/Content-Creation)

## Contributions
* Contributions in the form of issues and pull requests are welcomed and encouraged.
* The preferred way to contribute is to fork the repo and submit a pull request on GitHub.
* Code style information can be found on the [Wiki](https://github.com/ACEmulator/ACE/wiki/Code-Style).

Please note that this project is released with a [Contributor Code of Conduct](https://github.com/ACEmulator/ACE/blob/master/CODE_OF_CONDUCT.md). By participating in this project you agree to abide by its terms.

## Bug Reports
* Please use the [issue tracker](https://github.com/ACEmulator/ACE/issues) provided by GitHub to send us bug reports.
* You may also discuss issues and bug reports on our discord listed below.

## Contact
* [Discord Channel](https://discord.gg/C2WzhP9)

## Status API
When enabled in `Config.js`, the server exposes a lightweight HTTP API.

Enable the API with:

```json
"Api": {
    "Enabled": true,
    "Host": "127.0.0.1",
    "Port": 5000,
    "UseHttps": false,
    "CertificatePath": "",
    "CertificatePassword": "",
    "RequestsPerMinute": 60,
    "RequireApiKey": false,
    "ApiKeys": []
}
```

Set `UseHttps` to `true` and provide a valid `.pfx` certificate to serve the API
over HTTPS directly. When `UseHttps` is `false`, traffic is unencrypted HTTP and
should be protected behind a reverse proxy or firewall if exposed to the
internet. `RequestsPerMinute` limits how many API calls a single IP address may
make per minute; set to `0` to disable rate limiting. Rate limiter entries are
automatically removed after ten minutes of inactivity to free memory. When
`RequireApiKey` is `true`, each request must include one of the configured
keys in the `X-API-Key` header or `apikey` query string. Updates to `ApiKeys`
in `Config.js` are detected automatically and loaded without restarting the
server.

### Example
`GET /api/stats/players` returns a JSON list of online players:

```bash
curl http://localhost:5000/api/stats/players
```

Add `-H "X-API-Key: <key>"` if `RequireApiKey` is enabled.
You can also supply the key in the query string.

```bash
# header example
curl -H "X-API-Key: <key>" http://localhost:5000/api/stats/players
# query string example
curl http://localhost:5000/api/stats/players?apikey=<key>
```

```json
{ "onlineCount": 1, "players": ["Admin"] }
```

`GET /api/stats/character/{name}` returns details about a specific character:

```bash
curl http://localhost:5000/api/stats/character/Admin
```

```json
{ "allegiance_name": "Example", "level": 200 }
```

`GET /api/stats/performance` returns server metrics and performance data when the monitor is running:

```bash
curl http://localhost:5000/api/stats/performance
```

Include the `X-API-Key` header or `apikey` query string if required.

```json
{
  "uptimeSeconds": 123.4,
  "cpuUsagePercent": 5.2,
  "privateMemoryMB": 200.0,
  "gcMemoryMB": 150.0,
  "monitor": "Monitoring Durations: ..."
}
```

`GET /api/status` returns server uptime and build information:

```bash
curl http://localhost:5000/api/status
```

Include the `X-API-Key` header or `apikey` query string if required.

```json
{ "uptimeSeconds": 123.4, "version": "1.0.0", "startTime": "2025-05-11T02:21:11Z" }
```

If `UseHttps` is enabled, replace `http` with `https` in the above commands.
