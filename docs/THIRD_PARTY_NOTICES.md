# Third-party components in the portable release

The ready-to-play OpenDereth release bundles these unmodified upstream components so players do not need to install a database server or import the world manually.

## ACEmulator ACE

- Upstream: <https://github.com/ACEmulator/ACE>
- Pinned upstream commit: `650c5b75ae909957feaf58db320e46be16502653`
- Server build: `1.77.4782`
- License: GNU Affero General Public License version 3
- Corresponding source, including the single-player modifications: <https://github.com/titaniumweiner/OpenDereth>

## ACE World Database

- Upstream: <https://github.com/ACEmulator/ACE-World-16PY-Patches>
- Release: `v0.9.294`
- Asset: `ACE-World-Database-v0.9.294.sql.zip`
- SHA-256: `aa8275a2fd8edd8c2b95092d2407ece4616ba7b8d7eab1405719bbbfa80c8f89`
- License: GNU Affero General Public License version 3
- Source: <https://github.com/ACEmulator/ACE-World-16PY-Patches/tree/v0.9.294>

## MariaDB Community Server

- Upstream: <https://mariadb.org/>
- Release: `12.3.2 LTS`, official Windows x64 ZIP
- SHA-256: `67347c129eb9c5923d002ea34fbfa27c60eb95d36dd73b85af2651cdeceecac5`
- License: GNU General Public License version 2
- Source: <https://archive.mariadb.org/mariadb-12.3.2/source/>

## CustomClothingBase

- Author: OptimShi
- Upstream and source: <https://github.com/OptimShi/CustomClothingBase>
- Official release: `v1.11`
- Official asset SHA-256: `505dcb951bdba9ec7788b2f947f3b8d6a7638e06c43000bd38beb129689873a6`
- Packaging: upstream DLLs are unmodified; OpenDereth adds only its install manifest, metadata, and instructions
- License notice: the upstream repository currently contains no `LICENSE` file. Redistribution is included based on OptimShi's permission reported by the OpenDereth project maintainer. OptimShi retains ownership of the upstream mod.

The applicable published license texts are included in the release's `Licenses` directory. Asheron's Call itself, `acclient.exe`, client DAT files, Decal, and ThwargLauncher are not included.
