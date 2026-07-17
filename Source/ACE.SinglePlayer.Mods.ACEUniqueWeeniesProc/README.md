# Expanded Cast on Strike for ACE Single Player

This .NET 10 Harmony mod implements the server-code change documented by [titaniumweiner/ACEUniqueWeenies](https://github.com/titaniumweiner/ACEUniqueWeenies). It lets equipped jewelry, armor, and other items use cast-on-strike proc properties instead of limiting the equipped-item pass to Aetheria. Items with `CloakWeaveProc` type `1` remain excluded.

The patch preserves the rest of the pinned ACE `TryProcEquippedItems` method, including direct-object, active-weapon, and monster-projectile proc handling. Because the requested filter also sees an active weapon in the equipped-object collection, it intentionally matches the repository's exact behavior rather than silently adding a weapon exclusion.

The mod changes combat behavior only. It does not include or import the repository's custom weenie SQL. Use the launcher's **Custom Weenies** section for compatible per-weenie SQL files.

Reviewed content commit: <https://github.com/titaniumweiner/ACEUniqueWeenies/commit/75d7036c5b281e70b23aba60732aca681ee443d6>

Ported source: <https://github.com/titaniumweiner/ACE-SinglePlayer/tree/main/Source/ACE.SinglePlayer.Mods.ACEUniqueWeeniesProc>

This mod is derived from ACE server behavior and is distributed under the GNU Affero General Public License v3.0 included with ACE Single Player. The linked ACEUniqueWeenies repository does not currently declare a license; its SQL content is not redistributed by this mod package.
