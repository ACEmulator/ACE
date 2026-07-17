# Importing AceForge custom weenies

OpenDereth can import custom ACE World objects saved as per-weenie SQL files by [AceForge](https://github.com/shemtar-90/AceForge/releases/tag/v0.3.36). This is intentionally separate from server DLL mods: a server mod changes code while a custom weenie becomes persistent data in `ace_world`.

## Import workflow

1. Create or edit the object in AceForge and save its SQL. AceForge normally writes files named like `850001 Example Item.sql` in its configured output folder.
2. Exit the game and stop the local server.
3. Open **Custom Weenies** in OpenDereth.
4. Click **Choose AceForge Folder...** to scan a complete output folder recursively, or **Choose SQL Files...** for individual files.
5. Review the WCID, name, type, and validation notes. Invalid and unsupported files are skipped rather than executed.
6. Click **IMPORT VALID WEENIES**. If a WCID already exists, the launcher shows the existing records and requires a second confirmation before replacing them.
7. The launcher starts the private database, creates a complete `ace_world` SQL backup, and applies all accepted files together in a transaction. Start the game after the success message.

Backups and import-history manifests are stored under:

```text
%LOCALAPPDATA%\OpenDereth\Backups\CustomWeenies
```

The **Open Backups** button opens the actual configured location.

## Accepted SQL

The importer accepts one weenie per file in ACE's normal idempotent form:

```sql
DELETE FROM `weenie` WHERE `class_Id` = 850001;
INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (850001, 'Example Item', 6, '2026-07-16 00:00:00');
```

It also accepts inserts into the current ACE `weenie_properties_*` tables when every row belongs to the same WCID. AceForge's `SET @parent_id = LAST_INSERT_ID()` emote pattern is supported. Other statements, other tables, executable comments, property rows targeting a different WCID, multi-weenie files, duplicate selected WCIDs, oversized files, and malformed SQL are rejected.

This strict format is a safety boundary, not a promise that the content behaves correctly in game. The launcher cannot prove that a DID, spell, generator target, emote, or client-facing asset is valid.

## Current limits

- Automatic import requires **Private Database** mode. The launcher does not modify an external MariaDB installation automatically.
- This screen imports weenies only. AceForge quest, recipe, event, treasure, landscape, and client-DAT output is reported as unsupported.
- Imported data has no one-click uninstaller. Re-importing the same WCID replaces it; otherwise, removal or rollback requires a deliberate database migration or restoration from the pre-import backup.
- WCIDs must be chosen for this exact world. Do not overwrite an existing WCID unless replacing that object is intentional.
- Test new content on a disposable save before relying on it in a long-lived world.

AceForge v0.3.36 is an independent MIT-licensed project. It is linked for convenience and is not bundled with OpenDereth.
