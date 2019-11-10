using System;
using System.IO;
using System.Text;

using ACE.Entity.Enum;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public class RecordCast
    {
        public Player Player;

        public bool Enabled;

        public string Filename => $"{Player.Name}Cast.log";

        public StringBuilder Buffer = new StringBuilder();

        public RecordCast(Player player)
        {
            Player = player;
        }

        public void OnMoveToState(MoveToState moveToState)
        {
            var rawState = moveToState.RawMotionState;

            var line = rawState.ToString(false).Replace(Environment.NewLine, " | ");
            line = line.Length >= 8 ? line.Substring(0, line.Length - 8) : "";

            Output(line);
        }

        public void OnCastTargetedSpell(Spell spell, WorldObject target)
        {
            var line = $"HandleActionCastTargetedSpell({spell.Id} - {spell.Name}, {target.Name} ({target.Guid}))";

            Output(line);
        }

        public void OnCastUntargetedSpell(Spell spell)
        {
            var line = $"HandleActionCastUntargetedSpell({spell.Id} - {spell.Name})";

            Output(line);
        }

        public void OnJump(JumpPack jump)
        {
            var line = $"HandleActionJump: Velocity={jump.Velocity}, Extent={jump.Extent}";

            Output(line);
        }

        public void OnSetCombatMode(CombatMode combatMode)
        {
            var line = $"HandleActionChangeCombatMode({combatMode})";

            Output(line);
        }

        public void Log(string line)
        {
            Output(line);
        }

        public void Output(string line)
        {
            var timestamp = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss,fff");

            var timestamp_line = $"[{timestamp}] {line}";

            Buffer.AppendLine(timestamp_line);
            Console.WriteLine(timestamp_line);
        }

        public void Flush()
        {
            File.AppendAllText(Filename, Buffer.ToString());
            Buffer.Clear();
        }
    }
}
