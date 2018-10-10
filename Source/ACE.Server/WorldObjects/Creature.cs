using System;
using System.Linq;
using System.Numerics;
using log4net;

using ACE.Database.Models.World;
using ACE.DatLoader.FileTypes;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Server.Entity.Actions;
using ACE.Database.Models.Shard;
using ACE.Server.Network.GameMessages.Messages;
using ACE.Server.Network.Motion;
using ACE.Server.Network.Sequence;
using ACE.Server.Entity;
using ACE.Server.WorldObjects.Entity;

namespace ACE.Server.WorldObjects
{
    public partial class Creature : Container
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public bool IsExhausted { get => Stamina.Current == 0; }

        /// <summary>
        /// A new biota be created taking all of its values from weenie.
        /// </summary>
        public Creature(Weenie weenie, ObjectGuid guid) : base(weenie, guid)
        {
            SetEphemeralValues();
        }

        /// <summary>
        /// Restore a WorldObject from the database.
        /// </summary>
        public Creature(Biota biota) : base(biota)
        {
            SetEphemeralValues();
        }

        private void SetEphemeralValues()
        {
            CombatMode = CombatMode.NonCombat;
            DamageHistory = new DamageHistory(this);

            if (CreatureType == ACE.Entity.Enum.CreatureType.Human && !(WeenieClassId == 1 || WeenieClassId == 4))
                GenerateNewFace();

            if (CreatureType == ACE.Entity.Enum.CreatureType.Shadow || CreatureType == ACE.Entity.Enum.CreatureType.Simulacrum)
                GenerateNewFace();

            // If any of the vitals don't exist for this biota, one will be created automatically in the CreatureVital ctor
            Vitals[PropertyAttribute2nd.MaxHealth] = new CreatureVital(this, PropertyAttribute2nd.MaxHealth);
            Vitals[PropertyAttribute2nd.MaxStamina] = new CreatureVital(this, PropertyAttribute2nd.MaxStamina);
            Vitals[PropertyAttribute2nd.MaxMana] = new CreatureVital(this, PropertyAttribute2nd.MaxMana);

            // If any of the attributes don't exist for this biota, one will be created automatically in the CreatureAttribute ctor
            Attributes[PropertyAttribute.Strength] = new CreatureAttribute(this, PropertyAttribute.Strength);
            Attributes[PropertyAttribute.Endurance] = new CreatureAttribute(this, PropertyAttribute.Endurance);
            Attributes[PropertyAttribute.Coordination] = new CreatureAttribute(this, PropertyAttribute.Coordination);
            Attributes[PropertyAttribute.Quickness] = new CreatureAttribute(this, PropertyAttribute.Quickness);
            Attributes[PropertyAttribute.Focus] = new CreatureAttribute(this, PropertyAttribute.Focus);
            Attributes[PropertyAttribute.Self] = new CreatureAttribute(this, PropertyAttribute.Self);

            foreach (var skillProperty in Biota.BiotaPropertiesSkill)
                Skills[(Skill)skillProperty.Type] = new CreatureSkill(this, skillProperty);

            if (Health.Current == 0)
                Health.Current = Health.MaxValue;
            if (Stamina.Current == 0)
                Stamina.Current = Stamina.MaxValue;
            if (Mana.Current == 0)
                Mana.Current = Mana.MaxValue;

            if (!(this is Player))
            {
                GenerateWieldList();
                GenerateWieldedTreasure();

                EquipInventoryItems();
            }

            Value = null; // Creatures don't have value. By setting this to null, it effectively disables the Value property. (Adding/Subtracting from null results in null)

            //CurrentMotionState = new UniversalMotion(MotionStance.NonCombat);     // breaks emotes?
            //CurrentMotionState.MovementData.ForwardCommand = (uint)MotionCommand.Ready;   // already the default?

            //CurrentMotionState = new UniversalMotion(MotionStance.NonCombat, new MotionItem(MotionCommand.Ready));
            CurrentMotionState = new UniversalMotion(MotionStance.Invalid, new MotionItem(MotionCommand.Invalid));
        }

        public void GenerateNewFace()
        {
            var cg = DatManager.PortalDat.CharGen;

            if (!Heritage.HasValue)
            {
                if (!String.IsNullOrEmpty(HeritageGroup))
                {
                    HeritageGroup parsed = (HeritageGroup)Enum.Parse(typeof(HeritageGroup), HeritageGroup.Replace("'", ""), true);
                    if (parsed != 0)
                        Heritage = (int)parsed;
                }
            }

            if (!Gender.HasValue)
            {
                if (!String.IsNullOrEmpty(Sex))
                {
                    Gender parsed = (Gender)Enum.Parse(typeof(Gender), Sex, true);
                    if (parsed != 0)
                        Gender = (int)parsed;
                }
            }

            if (!Heritage.HasValue || !Gender.HasValue)
                return;

            SexCG sex = cg.HeritageGroups[(uint)Heritage].Genders[(int)Gender];

            PaletteBaseId = sex.BasePalette;

            Appearance appearance = new Appearance();

            appearance.HairStyle = 1;
            appearance.HairColor = 1;
            appearance.HairHue = 1;

            appearance.EyeColor = 1;
            appearance.Eyes = 1;

            appearance.Mouth = 1;
            appearance.Nose = 1;

            appearance.SkinHue = 1;

            // Get the hair first, because we need to know if you're bald, and that's the name of that tune!
            int size = sex.HairStyleList.Count / 3; // Why divide by 3 you ask? Because AC runtime generated characters didn't have much range in hairstyles.
            Random rand = new Random();
            appearance.HairStyle = (uint)rand.Next(size);

            HairStyleCG hairstyle = sex.HairStyleList[Convert.ToInt32(appearance.HairStyle)];
            bool isBald = hairstyle.Bald;

            size = sex.HairColorList.Count;
            appearance.HairColor = (uint)rand.Next(size);
            appearance.HairHue = rand.NextDouble();

            size = sex.EyeColorList.Count;
            appearance.EyeColor = (uint)rand.Next(size);
            size = sex.EyeStripList.Count;
            appearance.Eyes = (uint)rand.Next(size);

            size = sex.MouthStripList.Count;
            appearance.Mouth = (uint)rand.Next(size);

            size = sex.NoseStripList.Count;
            appearance.Nose = (uint)rand.Next(size);

            appearance.SkinHue = rand.NextDouble();

            //// Certain races (Undead, Tumeroks, Others?) have multiple body styles available. This is controlled via the "hair style".
            ////if (hairstyle.AlternateSetup > 0)
            ////    character.SetupTableId = hairstyle.AlternateSetup;

            if (!EyesTextureDID.HasValue)
                EyesTextureDID = sex.GetEyeTexture(appearance.Eyes, isBald);
            if (!DefaultEyesTextureDID.HasValue)
                DefaultEyesTextureDID = sex.GetDefaultEyeTexture(appearance.Eyes, isBald);
            if (!NoseTextureDID.HasValue)
                NoseTextureDID = sex.GetNoseTexture(appearance.Nose);
            if (!DefaultNoseTextureDID.HasValue)
                DefaultNoseTextureDID = sex.GetDefaultNoseTexture(appearance.Nose);
            if (!MouthTextureDID.HasValue)
                MouthTextureDID = sex.GetMouthTexture(appearance.Mouth);
            if (!DefaultMouthTextureDID.HasValue)
                DefaultMouthTextureDID = sex.GetDefaultMouthTexture(appearance.Mouth);
            if (!HeadObjectDID.HasValue)
                HeadObjectDID = sex.GetHeadObject(appearance.HairStyle);

            // Skin is stored as PaletteSet (list of Palettes), so we need to read in the set to get the specific palette
            var skinPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(sex.SkinPalSet);
            if (!SkinPaletteDID.HasValue)
                SkinPaletteDID = skinPalSet.GetPaletteID(appearance.SkinHue);

            // Hair is stored as PaletteSet (list of Palettes), so we need to read in the set to get the specific palette
            var hairPalSet = DatManager.PortalDat.ReadFromDat<PaletteSet>(sex.HairColorList[Convert.ToInt32(appearance.HairColor)]);
            if (!HairPaletteDID.HasValue)
                HairPaletteDID = hairPalSet.GetPaletteID(appearance.HairHue);

            // Eye Color
            if (!EyesPaletteDID.HasValue)
                EyesPaletteDID = sex.EyeColorList[Convert.ToInt32(appearance.EyeColor)];
        }

        /// <summary>
        /// This will be false when creature is dead and waits for respawn
        /// </summary>
        public bool IsAlive { get => Health.Current > 0; }

        public void SetMotionState(WorldObject obj, UniversalMotion motionState)
        {
            CurrentMotionState = motionState;
            motionState.IsAutonomous = false;
            GameMessageUpdateMotion updateMotion = new GameMessageUpdateMotion(Guid, Sequences.GetCurrentSequence(SequenceType.ObjectInstance), obj.Sequences, motionState);
            EnqueueBroadcast(updateMotion);
        }

        /// <summary>
        /// This signature services MoveToObject and TurnToObject
        /// Update Position prior to start, start them moving or turning, set statemachine to moving.
        /// Moved from player - we need to be able to move creatures as well.   Og II
        /// </summary>
        /// <param name="worldObjectPosition">Position in the world</param>
        /// <param name="sequence">Sequence for the object getting the message.</param>
        /// <param name="movementType">What type of movement are we about to execute</param>
        /// <param name="targetGuid">Who are we moving or turning toward</param>
        public void OnAutonomousMove(ACE.Entity.Position worldObjectPosition, SequenceManager sequence, MovementTypes movementType, ObjectGuid targetGuid, float distanceFrom = 0.6f)
        {
            var target = CurrentLandblock.GetObject(targetGuid);
            if (target != null && target is Creature)
                distanceFrom = 0.6f;    // todo: use correct distancefrom for object

            // TODO: determine threshold for walking/running

            UniversalMotion newMotion = new UniversalMotion(CurrentMotionState.Stance, worldObjectPosition, targetGuid);
            newMotion.DistanceFrom = distanceFrom;
            newMotion.MovementTypes = movementType;

            // TODO: find the correct runrate here
            // the default runrate / charge seems much too fast...
            newMotion.RunRate = GetRunRate();
            if (CurrentMotionState.Stance == MotionStance.NonCombat)
                newMotion.RunRate /= 4.0f;

            // TODO: get correct flags from pcaps
            // 'CanCharge' is the only flag that seems to let the player run instead of walk
            // CanWalk and CanRun are already set by default

            // also, the default 'WalkRunThreshold' of 15 in UniversalMotion does not seem to be used automatically by client
            // maybe we have to check if above or below the WalkRunThreshold distance to target on server,
            // and send the CanCharge flag accordingly?
            var dist = Vector3.Distance(Location.ToGlobal(), worldObjectPosition.ToGlobal());
            if (dist >= 8.0f)   // arbitrary, the default seems too far, esp. with the weird in-combat walking motion?
                newMotion.Flag |= MovementParams.CanCharge;

            EnqueueBroadcast(new GameMessageUpdatePosition(this));
            EnqueueBroadcastMotion(newMotion);
        }

        /// <summary>
        /// This is raised by Player.HandleActionUseItem.<para />
        /// The item does not exist in the players possession.<para />
        /// If the item was outside of range, the player will have been commanded to move using DoMoveTo before ActOnUse is called.<para />
        /// When this is called, it should be assumed that the player is within range.
        /// 
        /// This is the OnUse method.   This is just an initial implemention.   I have put in the turn to action at this point.
        /// If we are out of use radius, move to the object.   Once in range, let's turn the creature toward us and get started.
        /// Note - we may need to make an NPC class vs monster as using a monster does not make them turn towrad you as I recall. Og II
        ///  Also, once we are reading in the emotes table by weenie - this will automatically customize the behavior for creatures.
        /// </summary>
        public override void ActOnUse(WorldObject worldObject)
        {
            if (worldObject is Player)
            {
                var player = worldObject as Player;

                var actionChain = new ActionChain();
                actionChain.AddDelaySeconds(player.Rotate(this));
                if (Biota.BiotaPropertiesEmote.Count > 0)
                {
                    var emoteSets = Biota.BiotaPropertiesEmote.Where(x => x.Category == (int)EmoteCategory.Use).ToList();

                    if (emoteSets.Count > 0)
                    {
                        var selectedEmoteSet = emoteSets.FirstOrDefault(x => x.Probability == 1);

                        if (selectedEmoteSet == null)
                        {
                            var rng = Physics.Common.Random.RollDice(0.0f, 1.0f);

                            selectedEmoteSet = emoteSets.FirstOrDefault(x => x.Probability >= rng);
                        }

                        if (selectedEmoteSet == null)
                        {
                            player.SendUseDoneEvent();
                            return;
                        }

                        foreach (var action in selectedEmoteSet.BiotaPropertiesEmoteAction)
                        {
                            EmoteManager.ExecuteEmote(selectedEmoteSet, action, actionChain, this, player);
                        }
                        actionChain.EnqueueChain();
                    }
                    
                    player.SendUseDoneEvent();
                }
                else
                {
                    player.SendUseDoneEvent();
                }
            }
        }
    }
}
