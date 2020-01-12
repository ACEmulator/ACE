using System;
using System.Collections.Generic;
using System.Text;
using ACE.Entity.Enum;

namespace ACE.Server.Physics.Command
{
    public class ACCmdInterp : CommandInterpreter
    {
        public void CommenceJump()
        {
            /*var combatSystem = ClientCombatSystem.GetCombatSystem();
            if (combatSystem != null)
                combatSystem.CommenceJump();*/
        }

        public void DoJump()
        {
            /*var combatSystem = ClientCombatSystem.DoJump();
            if (combatSystem != null)
                combatSystem.DoJump();*/
        }

        public override void HandleNewForwardMovement()
        {
            /*var combatSystem = ClientCombatSystem.GetCombatSystem();
            if (combatSystem != null)
                combatSystem.AbortAutomaticAttack();*/

            base.HandleNewForwardMovement();
        }

        public override void TakeControlFromServer()
        {
            /*var combatSystem = ClientCombatSystem.GetCombatSystem();
            if (combatSystem != null)
                combatSystem.AbortAutomaticAttack();*/

            base.TakeControlFromServer();
        }

        public bool OnAction(InputEvent evt)
        {
            // vfptr[12] - IsActive
            if (!IsActive())
                return true;

            switch (evt.InputAction)
            {
                case 0x32:
                    // vfptr[2].OnLoseFocus
                    return true;

                case 0x30:
                    SetMotion(MotionCommand.AutoRun, true);
                    return true;

                case 0x29:
                    SetMotion(MotionCommand.WalkForward, evt.Start);
                    return true;

                case 0x2A:
                    SetMotion(MotionCommand.WalkBackwards, evt.Start);
                    return true;

                case 0x2B:
                    SetMotion(MotionCommand.Ready, true);
                    return true;

                case 0x2E:
                    SetMotion(MotionCommand.TurnRight, evt.Start);
                    return true;

                case 0x2F:
                    SetMotion(MotionCommand.TurnLeft, evt.Start);
                    return true;

                case 0x2C:
                    SetMotion(MotionCommand.SideStepRight, evt.Start);
                    return true;

                case 0x2D:
                    SetMotion(MotionCommand.SideStepLeft, evt.Start);
                    return true;

                case 0x31:
                    if (evt.Start)
                        CommenceJump();     // vfptr[5].OnAction - CommenceJump
                    else
                        DoJump();           // vfptr[5].OnLoseFocus - DoJump

                    return true;

                default:
                    //var result = HashEmoteInputActionsToCommands.TryGetValue(evt.InputAction, out var emoteCommand);
                    var result = false;
                    //if (result)
                        //SetMotion(emoteCommand, true);
                    return result;
            }
        }

        public void SetMotion(MotionCommand motion, bool start)
        {
            if (Player == null)
                return;

            var cmdStruct = new CmdStruct();
            cmdStruct.Args[0] = 0;
            cmdStruct.Args[1] = (uint)motion;
            cmdStruct.Args[2] = Convert.ToUInt32(start);
            cmdStruct.Args[3] = 4;

            // vfptr[12].OnLoseFocus - HandleKeyboardCommand
            HandleKeyboardCommand(cmdStruct, motion);
        }
    }
}
