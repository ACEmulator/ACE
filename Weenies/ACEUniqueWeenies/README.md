# ACE Unique Weenies for OpenDereth

This folder contains the 76 SQL weenies from
[titaniumweiner/ACEUniqueWeenies](https://github.com/titaniumweiner/ACEUniqueWeenies),
copied from source commit `75d7036c5b281e70b23aba60732aca681ee443d6`.

They are bundled as optional, user-selected content. OpenDereth does not import
them automatically.

## Import

1. Stop the game and server.
2. Open **Custom Weenies** in OpenDereth.
3. Click **Choose Weenie Folder...** and select this folder.
4. Review the validation results and click **IMPORT VALID WEENIES**.

The launcher backs up `ace_world` before importing. The content is experimental
and has not been thoroughly play-tested as a complete pack; use a disposable
world first.

The pack depends on OpenDereth's selectable **Cast on Strike Expanded** server
mod for its intended cast-on-strike behavior. Enable that mod separately in the
Mods screen before playing.

## Collision-safe ID changes

The upstream files intentionally reused several original-game WCIDs. OpenDereth
uses new WCIDs for those records so importing this folder does not replace the
retail shopkeeper or Lugians:

| Content | Upstream WCID | OpenDereth WCID |
| --- | ---: | ---: |
| Suntar al-Tashqat the Shopkeep | 1057 | 910000001 |
| Extas Raider | 8138 | 910000002 |
| Gotrok Extas | 24494 | 910000003 |
| Gotrok Tiatus | 24497 | 910000004 |
| Gotrok Montok | 24955 | 910000005 |

The bundled Lugian Generator (`8800387`) points to the four collision-safe,
high-ID Lugian copies. Because the retail IDs are no longer replaced, existing landscape
spawns remain unchanged. The custom creatures and shopkeeper must be placed or
spawned through separate world-content work; importing these definitions alone
does not add new landscape positions.

`UPSTREAM_README.md` is the original project description. Its instructions
about replacing landscape Lugians describe the unmodified upstream IDs and do
not apply to this collision-safe OpenDereth copy.
