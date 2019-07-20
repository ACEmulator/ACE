using System;
using log4net;

using ACE.Database.Models.World;
using ACE.DatLoader.FileTypes;
using ACE.DatLoader;
using ACE.DatLoader.Entity;
using ACE.Entity;
using ACE.Entity.Enum;
using ACE.Entity.Enum.Properties;
using ACE.Database.Models.Shard;
using ACE.Server.Entity;
using ACE.Server.WorldObjects.Entity;

using Position = ACE.Entity.Position;

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

            if (Health.Current <= 0)
                Health.Current = Health.MaxValue;
            if (Stamina.Current <= 0)
                Stamina.Current = Stamina.MaxValue;
            if (Mana.Current <= 0)
                Mana.Current = Mana.MaxValue;

            if (!(this is Player))
            {
                if (!(this is CombatPet)) //combat pets normally wouldn't have these items, but due to subbing in code currently, sometimes they do. this skips them for now.
                {
                    GenerateWieldList();
                    GenerateWieldedTreasure();


                    EquipInventoryItems();
                }

                // TODO: fix tod data
                Health.Current = Health.MaxValue;
                Stamina.Current = Stamina.MaxValue;
                Mana.Current = Mana.MaxValue;
            }

            SetMonsterState();

            CurrentMotionState = new Motion(MotionStance.NonCombat, MotionCommand.Ready);
        }

        public void GenerateNewFace()
        {
            var cg = DatManager.PortalDat.CharGen;

            if (!Heritage.HasValue)
            {
                if (!String.IsNullOrEmpty(HeritageGroupName))
                {
                    HeritageGroup parsed = (HeritageGroup)Enum.Parse(typeof(HeritageGroup), HeritageGroupName.Replace("'", ""), true);
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

        public virtual float GetBurdenMod()
        {
            return 1.0f;    // override for players
        }

        /// <summary>
        /// This will be false when creature is dead and waits for respawn
        /// </summary>
        public bool IsAlive { get => Health.Current > 0; }

        /// <summary>
        /// Sends the network commands to move a player towards an object
        /// </summary>
        public void MoveToObject(WorldObject target, float? useRadius = null)
        {
            var distanceToObject = useRadius ?? target.UseRadius ?? 0.6f;

            var moveToObject = new Motion(this, target, MovementType.MoveToObject);
            moveToObject.MoveToParameters.DistanceToObject = distanceToObject;

            // move directly to portal origin
            //if (target is Portal)
                //moveToObject.MoveToParameters.MovementParameters &= ~MovementParams.UseSpheres;

            SetWalkRunThreshold(moveToObject, target.Location);

            EnqueueBroadcastMotion(moveToObject);
        }

        /// <summary>
        /// Sends the network commands to move a player towards a position
        /// </summary>
        public void MoveToPosition(Position position)
        {
            var moveToPosition = new Motion(this, position);
            moveToPosition.MoveToParameters.DistanceToObject = 0.0f;

            SetWalkRunThreshold(moveToPosition, position);

            EnqueueBroadcastMotion(moveToPosition);
        }

        public void SetWalkRunThreshold(Motion motion, Position targetLocation)
        {
            // FIXME: WalkRunThreshold (default 15 distance) seems to not be used automatically by client
            // player will always walk instead of run, and if MovementParams.CanCharge is sent, they will always charge
            // to remedy this, we manually calculate a threshold based on WalkRunThreshold

            var dist = Location.DistanceTo(targetLocation);
            if (dist >= motion.MoveToParameters.WalkRunThreshold / 2.0f)     // default 15 distance seems too far, especially with weird in-combat walking animation?
            {
                motion.MoveToParameters.MovementParameters |= MovementParams.CanCharge;

                // TODO: find the correct runrate here
                // the default runrate / charge seems much too fast...
                //motion.RunRate = GetRunRate() / 4.0f;
                motion.RunRate = GetRunRate();
            }
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
            // handled in base.OnActivate -> EmoteManager.OnUse()
        }

        public override void OnCollideObject(WorldObject target)
        {
            if (target.ReportCollisions == false)
                return;

            if (target is Door door)
                door.OnCollideObject(this);
        }
    }
}
