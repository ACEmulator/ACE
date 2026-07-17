# CriticalOverride for OpenDereth

This is a .NET 10 port of aquafir's `ACE.BaseMod` CriticalOverride sample. It overrides physical and magic critical-hit chances against non-player creatures while leaving player-versus-player calculations unchanged.

Edit `Settings.json` while the local server is stopped. Values are probabilities from `0` to `1`; for example, `0.5` is 50 percent. Restart the server after changes.

Original source: <https://github.com/aquafir/ACE.BaseMod/tree/master/Samples/CriticalOverride>

The original and modified source are licensed under the GNU Affero General Public License v3.0, the same license included with OpenDereth.
