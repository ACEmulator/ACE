# SocietyTailoring for OpenDereth

This is a .NET 10 port of aquafir's `ACE.BaseMod` SocietyTailoring sample. It allows Society armor to pass ACE's tailoring requirement check while preserving the same-object, player-inventory, and retained-item protections in the pinned ACE server build.

Completed tailoring changes are permanent. Turning the mod off prevents future Society armor tailoring but does not undo items already tailored.

**Preview warning:** the port has passed build, Harmony target/signature, package, and launcher tests. It has not received thorough in-game testing across every Society armor and tailoring combination. Back up `%LOCALAPPDATA%\OpenDereth` before using it on a valued world.

Original source: <https://github.com/aquafir/ACE.BaseMod/tree/master/Samples/SocietyTailoring>

Modified source: <https://github.com/titaniumweiner/OpenDereth/tree/main/Source/ACE.SinglePlayer.Mods.SocietyTailoring>

The original and modified source are licensed under the GNU Affero General Public License v3.0, the same license included with OpenDereth.
