using System;
using System.IO;
using System.Text;

using log4net;

using ACE.Entity.Enum;
using ACE.Server.Command.Handlers;
using ACE.Server.Network.Structure;
using ACE.Server.WorldObjects;

namespace ACE.Server.Entity
{
    public enum RecordCastMode
    {
        Disabled,
        Enabled,
        LogError
    };

    public class RecordCast
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public Player Player;

        public static RecordCastMode DefaultMode = RecordCastMode.LogError;

        public RecordCastMode Mode = DefaultMode;

        public bool Enabled
        {
            get => Mode != RecordCastMode.Disabled;
            set
            {
                if (value)
                    Mode = RecordCastMode.Enabled;
                else
                    Mode = DefaultMode;
            }
        }

        public string Filename => $"{Player.Name}Cast.log";

        public string DebugFilename => $"{Player.Name}-FixCast.log";

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
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss,fff");

            var timestamp_line = $"[{timestamp}] {line}";

            Buffer.AppendLine(timestamp_line);
            //Console.WriteLine(timestamp_line);
        }

        public void Flush()
        {
            if (Mode == RecordCastMode.Enabled)
                File.AppendAllText(Filename, Buffer.ToString());

            Buffer.Clear();
        }

        public void ShowInfo(string debugCast)
        {
            var timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd hh:mm:ss,fff");

            var stanceLog = Player.StanceLog.ToString();

            var info = $"[{timestamp}] {Player.Name} used /fixcast after being frozen for 5+ seconds:\n{debugCast}\n{Buffer}\n{stanceLog}\n===================================================";

            File.AppendAllText(DebugFilename, info);
        }
    }
}
