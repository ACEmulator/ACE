DELETE FROM `weenie` WHERE `class_Id` = 490034;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490034, 'ace490034-Disciple of Bael''Zharon', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490034,   1,         16) /* ItemType - Creature */
     , (490034,   2,         31) /* CreatureType - Human */
     , (490034,   3,         39) /* PaletteTemplate - Black */
     , (490034,   6,         -1) /* ItemsCapacity */
     , (490034,   7,         -1) /* ContainersCapacity */
     , (490034,  16,          1) /* ItemUseable - No */
     , (490034,  25,        300) /* Level */
     , (490034,  68,          5) /* TargetingTactic - Random, LastDamager */
     , (490034,  81,          5) /* MaxGeneratedObjects */
	 , (490034,  72,         52) /* FriendType - Virindi */
     , (490034,  82,          0) /* InitGeneratedObjects */
     , (490034,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (490034, 103,          2) /* GeneratorDestructionType - Destroy */
     , (490034, 113,          1) /* Gender - Male */
     , (490034, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (490034, 145,          2) /* GeneratorEndDestructionType - Destroy */
     , (490034, 188,         11) /* HeritageGroup - Undead */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490034,   1, True ) /* Stuck */
     , (490034,   6, False) /* AiUsesMana */
     , (490034,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490034,   1,       5) /* HeartbeatInterval */
     , (490034,   2,       0) /* HeartbeatTimestamp */
     , (490034,   3,     0.2) /* HealthRate */
     , (490034,   4,     0.5) /* StaminaRate */
     , (490034,   5,       2) /* ManaRate */
     , (490034,  12,       0) /* Shade */
     , (490034,  13,     0.6) /* ArmorModVsSlash */
     , (490034,  14,    0.95) /* ArmorModVsPierce */
     , (490034,  15,     0.6) /* ArmorModVsBludgeon */
     , (490034,  16,    0.95) /* ArmorModVsCold */
     , (490034,  17,       1) /* ArmorModVsFire */
     , (490034,  18,     0.9) /* ArmorModVsAcid */
     , (490034,  19,    0.95) /* ArmorModVsElectric */
     , (490034,  31,      50) /* VisualAwarenessRange */
     , (490034,  34,       2) /* PowerupTime */
     , (490034,  36,       1) /* ChargeSpeed */
     , (490034,  39,     1.2) /* DefaultScale */
	 , (490034,  55,      110) /* HomeRadius */
     , (490034,  41,     180) /* RegenerationInterval */
     , (490034,  64,    0.75) /* ResistSlash */
     , (490034,  65,    0.75) /* ResistPierce */
     , (490034,  66,     0.5) /* ResistBludgeon */
     , (490034,  67,       1) /* ResistFire */
     , (490034,  68,     0.5) /* ResistCold */
     , (490034,  69,     0.5) /* ResistAcid */
     , (490034,  70,     0.5) /* ResistElectric */
     , (490034,  71,       1) /* ResistHealthBoost */
     , (490034,  72,       1) /* ResistStaminaDrain */
     , (490034,  73,       1) /* ResistStaminaBoost */
     , (490034,  74,       1) /* ResistManaDrain */
     , (490034,  75,       1) /* ResistManaBoost */
     , (490034,  80,       3) /* AiUseMagicDelay */
     , (490034, 104,      40) /* ObviousRadarRange */
     , (490034, 121,       1) /* GeneratorInitialDelay */
     , (490034, 122,       2) /* AiAcquireHealth */
     , (490034, 125,       1) /* ResistHealthDrain */
     , (490034, 166,       1) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490034,   1, 'Disciple of Bael''Zharon') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490034,   1, 0x02000001) /* Setup */
     , (490034,   2, 0x0900020E) /* MotionTable */
     , (490034,   3, 0x20000016) /* SoundTable */
     , (490034,   4, 0x30000000) /* CombatTable */
     , (490034,   6, 0x0400007E) /* PaletteBase */
     , (490034,   7, 0x100007AE) /* ClothingBase */
     , (490034,   8, 0x06001036) /* Icon */
     , (490034,  22, 0x34000004) /* PhysicsEffectTable */
     , (490034,  35,       2000) /* DeathTreasureType */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (490034,   1, 600, 0, 0) /* Strength */
     , (490034,   2, 400, 0, 0) /* Endurance */
     , (490034,   3, 400, 0, 0) /* Quickness */
     , (490034,   4, 400, 0, 0) /* Coordination */
     , (490034,   5, 350, 0, 0) /* Focus */
     , (490034,   6, 500, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (490034,   1, 49800, 0, 0, 50000) /* MaxHealth */
     , (490034,   3,  4600, 0, 0, 5000) /* MaxStamina */
     , (490034,   5, 500000, 0, 0, 500500) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (490034,  6, 0, 2, 0, 500, 0, 0) /* MeleeDefense        Trained */
     , (490034,  7, 0, 2, 0, 550, 0, 0) /* MissileDefense      Trained */
     , (490034, 15, 0, 2, 0, 350, 0, 0) /* MagicDefense        Trained */
     , (490034, 16, 0, 2, 0, 450, 0, 0) /* ManaConversion      Trained */
     , (490034, 31, 0, 2, 0, 450, 0, 0) /* CreatureEnchantment Trained */
     , (490034, 33, 0, 2, 0, 450, 0, 0) /* LifeMagic           Trained */
     , (490034, 34, 0, 2, 0, 450, 0, 0) /* WarMagic            Trained */
     , (490034, 43, 0, 2, 0, 450, 0, 0) /* VoidMagic           Trained */
     , (490034, 45, 0, 2, 0, 600, 0, 0) /* LightWeapons        Trained */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (490034,  0,  4,  0,    0,  980,  588,  931,  588,  931,  980,  882,  931,    0, 1, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0, 0.33,    0,    0) /* Head */
     , (490034,  1,  4,  0,    0,  980,  588,  931,  588,  931,  980,  882,  931,    0, 2, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0, 0.44, 0.17,    0) /* Chest */
     , (490034,  2,  4,  0,    0,  980,  588,  931,  588,  931,  980,  882,  931,    0, 3,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0,    0, 0.17,    0) /* Abdomen */
     , (490034,  3,  4,  0,    0,  980,  588,  931,  588,  931,  980,  882,  931,    0, 1, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0, 0.23, 0.03,    0) /* UpperArm */
     , (490034,  4,  4,  0,    0,  980,  588,  931,  588,  931,  980,  882,  931,    0, 2,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0) /* LowerArm */
     , (490034,  5,  4, 800, 0.75,  980,  588,  931,  588,  931,  980,  882,  931,    0, 2,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0,    0,  0.2,    0) /* Hand */
     , (490034,  6,  4,  0,    0,  980,  588,  931,  588,  931,  980,  882,  931,    0, 3,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18,    0, 0.13, 0.18) /* UpperLeg */
     , (490034,  7,  4,  0,    0,  980,  588,  931,  588,  931,  980,  882,  931,    0, 3,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6,    0,    0,  0.6) /* LowerLeg */
     , (490034,  8,  4, 800, 0.75,  980,  588,  931,  588,  931,  980,  882,  931,    0, 3,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22,    0,    0, 0.22) /* Foot */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (490034,  2042,  2.062)  /* Demon's Tongues */
     , (490034,  2710,  2.066)  /* Volcanic Blast */
     , (490034,  3110,   2.07)  /* Sear Flesh */
     , (490034,  3878,  2.075)  /* Incendiary Strike */
     , (490034,  3882,  2.082)  /* Incendiary Ring */
     , (490034,  3883,  2.089)  /* Pyroclastic Explosion */
     , (490034,  3908,  2.098)  /* Mana Blast */
     , (490034,  4438,  2.108)  /* Incantation of Flame Blast */
     , (490034,  4441,  2.121)  /* Incantation of Flame Volley */
     , (490034,  4477,  2.138)  /* Incantation of Bludgeoning Vulnerability Other */
     , (490034,  4481,   2.16)  /* Incantation of Fire Vulnerability Other */
     , (490034,  5532,   2.19)  /* Incantation of Bloodstone Bolt */
     , (490034,  3905,  2.235)  /* Essence's Fury */;

INSERT INTO `weenie_properties_generator` (`object_Id`, `probability`, `weenie_Class_Id`, `delay`, `init_Create`, `max_Create`, `when_Create`, `where_Create`, `stack_Size`, `palette_Id`, `shade`, `obj_Cell_Id`, `origin_X`, `origin_Y`, `origin_Z`, `angles_W`, `angles_X`, `angles_Y`, `angles_Z`)
VALUES (19545, -1, 490035, 10, 10, 10, 1, 2, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Flamma (8405) (x6 up to max of 6) - Regenerate upon Destruction - Location to (re)Generate: Scatter */
, (19545, -1, 490036, 10, 4, 4, 1, 2, -1, 0, 0, 0, 0, 0, 0, 1, 0, 0, 0) /* Generate Flamma (8405) (x6 up to max of 6) - Regenerate upon Destruction - Location to (re)Generate: Scatter */;
