# OpenDereth roadmap

Phase 1 deliberately keeps a narrow, dependable loopback server and direct-client path. Future work should be delivered without weakening Vanilla launch:

- Optional checksum-verified acquisition of a supported MariaDB distribution so private mode does not require a prior MariaDB installation.
- Signed or checksum-verified mod package installation with provenance and rollback.
- Port selected aquafir ACE server mods to current ACE/.NET after source review and automated compatibility tests.
- Back up and restore authentication, character/shard, world, settings, and managed database data.
- Import an existing ACE account without changing its character ownership.
- Optional last-character selection only if a safe, maintainable approach is found that does not depend on Thwarg, fragile memory offsets, or general UI automation.
- A signed installer that preserves Runtime and database data across upgrades.
- Automatic launcher/server updates with checksum validation, backups, rollback, and no silent client/DAT replacement.
- Implement and test the reserved Chorizite client-launch provider separately from Vanilla and Decal.
