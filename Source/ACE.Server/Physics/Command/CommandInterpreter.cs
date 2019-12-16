using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using ACE.Entity.Enum;
using ACE.Server.Physics.Animation;
using ACE.Server.Physics.Common;

namespace ACE.Server.Physics.Command
{
    public class CommandInterpreter
    {
        public PhysicsObj Player;
        public SmartBox SmartBox;
        public List<CommandListElement> SubstateList;
        public List<CommandListElement> TurnList;
        public List<CommandListElement> SidestepList;
        public uint AutonomyLevel;
        public bool ControlledByServer;
        public bool HoldRun;
        public bool HoldSidestep;
        public bool TransientState { get; set; }
        public bool Enabled;
        public bool AutoRun;
        public bool MouseLookActive;
        public bool MouseLeftDown;
        public float AutoRunSpeed;
        public uint ActionStamp;
        public DateTime LastSentPositionTime;
        public Position LastSentPosition;
        public Plane LastSentContactPlane;
        public const double TimeBetweenPositionEvents = 1.875f;

        public CommandInterpreter()
        {
            SubstateList = new List<CommandListElement>();
            TurnList = new List<CommandListElement>();
            SidestepList = new List<CommandListElement>();

            AutonomyLevel = 2;
            AutoRunSpeed = 1.0f;
            ControlledByServer = true;
            Enabled = true;
            ActionStamp = 1;

            LastSentPositionTime = DateTime.UtcNow;
            LastSentPosition = new Position();
            LastSentContactPlane = new Plane();
        }

        public void AddCommand(MotionCommand command, float speed, bool mouse, bool newHoldRun)
        {
            // vfptr[1](command) - whichlist
            var list = WhichList(command);
            if (list != null)
            {
                // CommandList.AddCommand
                list.Add(new CommandListElement(command, speed, newHoldRun));
                if (((uint)command & (uint)CommandMask.SubState) != 0)
                {
                    if (list == SubstateList)
                    {
                        // vfptr[6] - ACCmdInterp::HandleNewForwardMovement
                        HandleNewForwardMovement();
                    }

                    TransientState = false;
                }
            }
            else if (((uint)command & (uint)CommandMask.SubState) != 0)
            {
                if (((uint)command & (uint)CommandMask.Toggle) == 0)
                {
                    // vfptr[6] - ACCmdInterp::HandleNewForwardMovement
                    HandleNewForwardMovement();
                    if (command != MotionCommand.Ready)
                        TransientState = true;
                }
            }
        }

        public void ApplyCurrentMovement(int a2)
        {
            if (Player == null)
                return;

            if (AutoRun)
            {
                // vfptr[13].OnAction - MovePlayer
                MovePlayer(MotionCommand.WalkForward, true, AutoRunSpeed, true, true);
                // goto LABEL_9
            }
            else
            {
                if (SubstateList.Count != 0)
                {
                    // vfptr[3].OnLoseFocus - ApplyListHeadMovement
                    ApplyListHeadMovement(SubstateList);
                }
                else if (!TransientState)
                {
                    MovePlayer(MotionCommand.Ready, true, AutoRunSpeed, false, true);
                }
            }

            // LABEL_9
            if (TurnList.Count != 0)
            {
                // vfptr[3].OnLoseFocus - ApplyListHeadMovement w/ extra param?
                //ApplyListHeadMovement(TurnList, a2);
                ApplyListHeadMovement(TurnList);
            }
            else
            {
                //MovePlayer(MotionCommand.SideStepRight, false, 1.0f /* ? */, false, 0, a2);
                MovePlayer(MotionCommand.SideStepRight, false, 1.0f /* ? */, false, false);
                MovePlayer(MotionCommand.TurnRight, false, 1.0f /* /? */, false, false);
            }

            if (SidestepList.Count != 0)
            {
                ApplyListHeadMovement(SidestepList);
            }
            else
            {
                MovePlayer(MotionCommand.SideStepRight, false, 1.0f, false, false);
            }
        }

        public void ApplyHoldKeysToCommand(ref MotionCommand command, float speed)
        {
            if (!HoldSidestep)
                return;

            if (command == MotionCommand.TurnRight)
            {
                command = MotionCommand.SideStepRight;
            }
            else if (command == MotionCommand.TurnLeft)
            {
                command = MotionCommand.SideStepLeft;
            }
        }

        public void ApplyListHeadMovement(List<CommandListElement> list)
        {
            var head = list.LastOrDefault();
            if (head == null)
                return;

            // if head is mouse
            //MovePlayer(head.Command, true, head.Speed, true, head.HoldRun);

            // else
            MovePlayer(head.Command, true, head.Speed, false, false);   // always false?
        }

        public bool BookkeepCommandAndModifyIfNecessary(MotionCommand command, bool start, float speed, bool mouse, bool newHoldRun)
        {
            if (command == MotionCommand.Jump)
                return true;

            if (start)
            {
                // vfptr[1].OnAction - AddCommand
                AddCommand(command, speed, mouse, newHoldRun);
                return true;
            }
            else
            {
                // vfptr[1].OnLoseFocus - MovePlayer?
                MovePlayer(command, start, speed, mouse, newHoldRun);
            }
            return false;
        }

        public void ClearAllCommands()
        {
            SubstateList.Clear();
            TurnList.Clear();
            SidestepList.Clear();
        }

        public void Disable()
        {
            // vfptr[3].OnAction()
            // vfptr[2].OnLoseFocus(0);
            var autonomy = AutonomyLevel != 0;
            HoldSidestep = false;
            if (AutonomyLevel != 0 && ControlledByServer)
            {
                // vfptr[2].OnAction(this)
                // vfptr[6].OnAction(this)
            }
        }

        public void Enable()
        {
            Enabled = true;
            // vfptr[2].OnLoseFocus(HoldRun);
        }

        public bool GetMouseLeftDown()
        {
            return MouseLeftDown;
        }

        public bool GetMouseLookActive()
        {
            return MouseLookActive;
        }

        public void HandleExhaustion()
        {
            if (Player != null)
                Player.report_exhaustion();
        }

        public void HandleKeyboardCommand(CmdStruct cmdStruct, MotionCommand command)
        {
            // vfptr[12] - IsActive
            if (!IsActive()) return;

            bool start;

            if (cmdStruct.Command == MotionCommand.AutoRun)
            {
                start = Convert.ToBoolean(cmdStruct.Args[cmdStruct.Curr]);
                cmdStruct.Curr++;
                if (cmdStruct.Curr >= cmdStruct.Size)
                {
                    AutoRunSpeed = 1.0f;
                }
                else
                {
                    AutoRunSpeed = BitConverter.ToSingle(BitConverter.GetBytes(cmdStruct.Args[cmdStruct.Curr]));
                    cmdStruct.Curr++;
                }
                // vfptr[16].OnLoseFocus - ToggleAutoRun
                ToggleAutoRun();
                // vfptr[6].OnAction - SendMovementEvent
                SendMovementEvent();
                return;
            }

            if (((uint)cmdStruct.Command & (uint)CommandMask.UI) != 0)
                return;

            start = Convert.ToBoolean(cmdStruct.Args[cmdStruct.Curr]);
            cmdStruct.Curr++;

            var speed = 1.0f;
            if (cmdStruct.Curr < cmdStruct.Size)
            {
                speed = BitConverter.ToSingle(BitConverter.GetBytes(cmdStruct.Args[cmdStruct.Curr]));
                cmdStruct.Curr++;
            }

            if (ControlledByServer && !start)
            {
                // vfptr[1].OnLoseFocus - MovePlayer?
                MovePlayer((MotionCommand)cmdStruct.Command, start, speed, false, false);
                return;
            }

            // vfptr[8].OnLoseFocus(a2) - ACCmdInterp::TakeControlFromServer?
            TakeControlFromServer();

            if (cmdStruct.Command == MotionCommand.HoldRun‬)
            {
                // vfptr[2].OnLoseFocus

                if (!IsStandingStill())
                    SendMovementEvent();

                return;
            }

            if (cmdStruct.Command == MotionCommand.HoldSidestep)
            {
                // vfptr[3]

                if (!IsStandingStill())
                    SendMovementEvent();

                return;
            }

            // vfptr[2] - Bookkeep
            if (!BookkeepCommandAndModifyIfNecessary(cmdStruct.Command, start, speed, false, false))
            {
                SendMovementEvent();
                return;
            }

            // vfptr[4].OnAction - ApplyHoldKeysToCommand
            ApplyHoldKeysToCommand(ref cmdStruct.Command, speed);

            // vfptr[13].OnAction - MovePlayer
            MovePlayer(cmdStruct.Command, start, speed, false, false);

            // vfptr[6].OnAction - SendMovementEvent
            if (cmdStruct.Command != MotionCommand.Jump)
                SendMovementEvent();
        }

        public void HandleLogOff()
        {
            // vfptr[11]
            Disable();
        }

        public void HandleMouseMovementCommand(CmdStruct cmdStruct)
        {

        }

        public virtual void HandleNewForwardMovement()
        {
            // vfptr[17](0, 1) - SetAutoRun
            SetAutoRun(false, true);
        }

        public bool HandleSelectLeft(bool start)
        {
            MouseLeftDown = true;

            // CInputManager : ICIDM
            return false;
        }

        public bool IsActive()
        {
            return Enabled && Player != null;
        }

        public bool IsEnabled()
        {
            return Enabled;
        }

        public bool IsStandingStill()
        {
            if (Player == null)
                return true;

            var minterp = Player.get_minterp();

            return minterp.is_standing_still();
        }

        public void LoseControlToServer()
        {
            if (AutonomyLevel == 0)
                return;

            ControlledByServer = true;
            // vfptr[17](0, 0)
            SetAutoRun(false, false);
            // vfptr[6].OnLoseFocus(this)
        }

        public void LoseKeyboardFocus()
        {
            ClearAllCommands();
            // vfptr[2].OnLoseFocus(this, 0) - SetHoldRun
            SetHoldRun(false);

            HoldSidestep = false;
            // vfptr[6].OnLoseFocus(this) - ACCmdInterp::FinishJump

            if (AutonomyLevel != 0)
            {
                if (!ControlledByServer)
                {
                    // vfptr[2].OnAction
                    // vfptr[6].OnAction
                }
            }
        }

        public bool MaybeStopCompletely()
        {
            if (ControlledByServer)
                return true;

            // vfptr[15].OnLoseFocus
            return false;
        }

        public void MovePlayer(MotionCommand command, bool start, float speed, bool mouse, bool newHoldRun)
        {
            if (Player == null || Player.InqInterpretedMotionState() == null)
                return;

            // if vfptr[10] - PlayerIsDead
            if (PlayerIsDead())
            {
                // vfptr[9].OnAction - LoseKeyboardFocus
                LoseKeyboardFocus();
                // vfptr[17](0, 0) - SetAutoRun
                SetAutoRun(false, false);
                return;
            }

            // if !ICIDM::s_cidm->m_UseMouseTurning
            // - goto LABEL_55

            var mvp = new MovementParameters();

            if (mouse)
            {
                // someFlags &= 0xFFFFF7FF;
                // unset bit 11
                mvp.SetHoldKey = false;
                var holdRun = Convert.ToInt32(newHoldRun) + 1;
            }

            var turn = (MotionCommand)MotionStance.Invalid;
            var sidestep = (MotionCommand)MotionStance.Invalid;

            if (TurnList.Count != 0)
                turn = TurnList.FirstOrDefault().Command;

            if (SidestepList.Count != 0)
                sidestep = SidestepList.FirstOrDefault().Command;

            // vfptr[17].OnLoseFocus - GetMouseLookActive
            var mouselook = GetMouseLookActive();

            bool start_turn_left = false;
            bool start_turn_right = false;
            bool start_sidestep_left = false;
            bool start_sidestep_right = false;

            bool cancel_turn_left = false;
            bool cancel_turn_right = false;
            bool cancel_sidestep_left = false;
            bool cancel_sidestep_right = false;

            MotionCommand cmd1;

            if (!mouse)
            {
                if (!mouselook)
                {
                    cmd1 = command;
                    // goto LABEL_59
                }
                if (command != MotionCommand.TurnRight)
                {
                    if (command != MotionCommand.TurnLeft)
                    {
                        if (start)
                        {
                            cancel_turn_left = true;
                            start_sidestep_left = true;
                        }
                        else
                        {
                            cancel_sidestep_left = true;
                        }
                    }
                    else
                    {
                        cancel_turn_right = true;
                        cancel_turn_left = true;
                    }
                    // goto LABEL_38
                }
                if (!start)
                {
                    cancel_sidestep_right = true;
                    // goto LABEL_38
                }
                // LABEL_31:
                cancel_turn_right = true;
                start_sidestep_right = true;
                // goto LABEL_38
            }

            if (!mouselook)
            {
                if (turn == MotionCommand.TurnRight)
                {
                    cancel_sidestep_right = true;
                    start_turn_right = true;
                }
                else if (turn == MotionCommand.TurnLeft)
                {
                    cancel_sidestep_left = true;
                    start_turn_left = true;
                }
                // goto LABEL_38
            }

            if (command != MotionCommand.MouseLook)
            {
                // goto LABEL_38
            }

            if (turn == MotionCommand.TurnRight)
            {
                cancel_turn_right = true;

                if (sidestep == MotionCommand.SideStepLeft)
                    start_sidestep_left = true;
                else
                    start_sidestep_right = true;

                // goto LABEL_38
            }

            if (turn == MotionCommand.TurnLeft)
            {
                if (sidestep != MotionCommand.SideStepRight)
                {
                    cancel_turn_left = true;
                    start_sidestep_left = true;

                    // goto LABEL_38
                }
                // goto LABEL_31
            }

            if (MouseLeftDown)
            {
                start = true;
                command = MotionCommand.WalkForward;
            }

            // ============
            // LABEL 38:

            // vfptr[8].OnLoseFocus - TakeControlFromServer
            TakeControlFromServer();

            if (cancel_sidestep_right)
                Player.StopMotion((uint)MotionCommand.SideStepRight, mvp, true);

            if (cancel_sidestep_left)
                Player.StopMotion((uint)MotionCommand.SideStepLeft, mvp, true);

            if (cancel_turn_right)
                Player.StopMotion((uint)MotionCommand.TurnRight, mvp, true);

            if (cancel_turn_left)
                Player.StopMotion((uint)MotionCommand.TurnLeft, mvp, true);

            if (start_turn_right)
            {
                start = true;
                cmd1 = MotionCommand.TurnRight;
            }
            else
                cmd1 = command;

            if (start_turn_left)
            {
                start = true;
                cmd1 = MotionCommand.TurnLeft;
            }

            if (start_sidestep_right)
            {
                start = true;
                cmd1 = MotionCommand.SideStepRight;
                speed = 1.0f;
            }

            if (start_sidestep_left)
            {
                start = true;
                command = MotionCommand.SideStepLeft;
                speed = 1.0f;

                // LABEL_55:
                cmd1 = command;
            }

            var holdRunRel1 = 0;
            if (mouse)
            {
                holdRunRel1 = Convert.ToInt32(newHoldRun) + 1;
                // goto LABEL_60
            }

            // LABEL_59:
            holdRunRel1 = 0;

            // LABEL_60:
            if (AutonomyLevel != 0)
            {
                if (start)
                {
                    if (cmd1 != MotionCommand.Jump)
                    {
                        mvp = new MovementParameters();
                        // set 12th flag
                        mvp.Autonomous = true;
                        if (mouse)
                            mvp.SetHoldKey = false;     // unset 11th flag
                        if (((uint)cmd1 & (uint)CommandMask.Action) != 0)
                        {
                            // vfptr[15].OnLoseFocus(this)
                        }
                        var werror = Player.DoMotion((uint)cmd1, mvp);
                        switch (werror)
                        {
                            case WeenieError.None:
                                if (((uint)cmd1 & (uint)CommandMask.Action) != 0)
                                    ActionStamp++;
                                return;

                            case WeenieError.CantCrouchInCombat:
                                break;  // 72

                            case WeenieError.CantSitInCombat:
                                break;  // 73

                            case WeenieError.CantLieDownInCombat:
                                break;  // 73

                            case WeenieError.YouAreTooTiredToDoThat:
                                break;  // 73

                            case WeenieError.CantChatEmoteInCombat:
                                break;  // 73

                            case WeenieError.CantChatEmoteNotStanding:
                                break;

                            default:
                                return;
                        }
                    }
                }
                else if (cmd1 != MotionCommand.Jump)
                {
                    mvp = new MovementParameters();
                    var holdRunRel = 0;
                    if (mouse)
                    {
                        mvp.SetHoldKey = false;
                        holdRunRel = Convert.ToInt32(newHoldRun) + 1;
                    }
                    Player.StopMotion((uint)cmd1, mvp, true);
                }
            }
            else
            {
                // vfptr[4].OnLoseFocus - NonAutonomous?
                MovePlayer_NonAutonomous(cmd1, start, speed, (HoldKey)holdRunRel1);
            }
        }

        public void MovePlayer_NonAutonomous(MotionCommand command, bool start, float speed, HoldKey holdKey)
        {
            if (start)
            {
                if (command == MotionCommand.Jump)
                {
                    // vfptr[5].OnAction
                }
                else
                {
                    // vfptr[19].OnAction
                }
            }
            else if (command == MotionCommand.Jump)
            {
                // vfptr[5].OnLoseFocus
            }
            else
            {
                // vfptr[19].OnLoseFocus
            }
        }

        public void NewPlayer(PhysicsObj player, bool autonomous_movement)
        {
            Player = player;
            if (autonomous_movement)
            {
                // vfptr[2].OnAction
            }
            else
            {
                // vfptr[8].OnAction
            }
        }

        public bool NukeCommand(MotionCommand command, bool start, float speed, bool mouse, bool newHoldRun)
        {
            return false;
        }

        public bool PlayerIsDead()
        {
            if (Player != null)
            {
                var motionState = Player.InqInterpretedMotionState();
                if (motionState != null)
                    return motionState.ForwardCommand == (uint)MotionCommand.Dead;
            }
            return false;
        }

        public void PlayerTeleported()
        {
            // vfptr[17](0, 1) - SetAutoRun?
            SetAutoRun(false, true);

            // vfptr[6].OnAction - SendMovementEvent?
            SendMovementEvent();
        }

        public void SendMovementEvent()
        {

        }

        public void SendPositionEvent()
        {

        }

        public void SetAutoRun(bool val, bool apply_movement)
        {
            if (AutoRun != val)
            {
                AutoRun = val;
                TransientState = false;
                if (val)
                {
                    //vfptr[8].OnLoseFocus(this)
                    // display string: AutoRun ON
                }
                else
                {
                    // display string: AutoRun OFF
                }
            }
            if (apply_movement)
            {
                // vfptr[2].OnAction(this) - ApplyCurrentMovement
                //ApplyCurrentMovement();
            }
        }

        public bool SetAutonomyLevel(uint newLevel)
        {
            if (newLevel <= 2)
            {
                AutonomyLevel = newLevel;
                // vfptr[19](newLevel)
                return true;
            }
            return false;
        }

        public void SetHoldRun(bool newVal)
        {
            // vfptr[5] - ACCmdInterp::UITogglesRun()
        }

        public void SetHoldSidestep(bool newVal)
        {
            // vfptr[4](TurnList)
            HoldSidestep = newVal;
            // vfptr[2].OnAction
        }

        public void SetMouseLeftDown(bool active)
        {
            MouseLeftDown = active;
        }

        public void SetMouseLookActive(bool active)
        {
            MouseLookActive = active;
        }

        public void SetSmartBox(SmartBox smartBox)
        {
            SmartBox = smartBox;
            if (smartBox != null)
                Player = smartBox.Player;
            else
                Player = null;
        }

        public bool ShouldSendPositionEvent()
        {
            return true;
        }

        public bool StopCompletely()
        {
            return false;
        }

        public void StopDrift()
        {

        }

        public void StopListHeadMovement(List<CommandListElement> list)
        {

        }

        public virtual void TakeControlFromServer()
        {
            // vrptr[10]
            if (ControlledByServer && AutonomyLevel != 0 && true)
            {
                ControlledByServer = false;

                if (Player != null)
                {
                    Player.LastMoveWasAutonomous = true;
                    Player.StopCompletely(true);
                    Player.StopInterpolating();
                }

                // vfptr[2].OnLoseFocus(HoldRun)
                // vfptr[2].OnAction
            }
        }

        public void ToggleAutoRun()
        {
            var toggle = !AutoRun;
            // vfptr[17] - SetAutoRun
            SetAutoRun(toggle, true);
        }

        public bool TurnToHeading(float newHeading, bool run)
        {
            return false;
        }

        public void UpdateToggleRun()
        {
            // vfptr[2].OnLoseFocus(HoldRun)
            // vfptr[6].OnAction
        }

        public bool UsePositionFromServer()
        {
            return AutonomyLevel != 2;
        }

        public void UseTime()
        {

        }

        public List<CommandListElement> WhichList(MotionCommand command)
        {
            switch (command)
            {
                case MotionCommand.TurnLeft:
                case MotionCommand.TurnRight:
                    return TurnList;

                case MotionCommand.SideStepLeft:
                case MotionCommand.SideStepRight:
                    return SidestepList;

                default:
                    if (((int)command & 0x40000000) != 0)
                    {
                        if (((int)command & 0x4000000) != 0)
                        {
                            return SubstateList;
                        }
                    }
                    break;
            }
            return null;
        }
    }
}
