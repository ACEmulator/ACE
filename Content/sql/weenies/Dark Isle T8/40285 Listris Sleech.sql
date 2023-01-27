DELETE FROM `weenie` WHERE `class_Id` = 40285;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (40285, 'ace40285-listrissleech', 10, '2022-12-04 19:04:52') /* Creature */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (40285,   1,         16) /* ItemType - Creature */
     , (40285,   2,         45) /* CreatureType - Niffis */
     , (40285,   3,         10) /* PaletteTemplate - LightBlue */
     , (40285,   6,         -1) /* ItemsCapacity */
     , (40285,   7,         -1) /* ContainersCapacity */
     , (40285,  16,          1) /* ItemUseable - No */
     , (40285,  25,        200) /* Level */
     , (40285,  27,          0) /* ArmorType - None */
     , (40285,  40,          2) /* CombatMode - Melee */
     , (40285,  68,          9) /* TargetingTactic - Random, TopDamager */
     , (40285,  93,       1032) /* PhysicsState - ReportCollisions, Gravity */
     , (40285, 101,        131) /* AiAllowedCombatStyle - Unarmed, OneHanded, ThrownWeapon */
     , (40285, 133,          2) /* ShowableOnRadar - ShowMovement */
     , (40285, 140,          1) /* AiOptions - CanOpenDoors */
     , (40285, 146,     315000) /* XpOverride */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (40285,   1, True ) /* Stuck */
     , (40285,   6, True ) /* AiUsesMana */
     , (40285,  11, False) /* IgnoreCollisions */
     , (40285,  12, True ) /* ReportCollisions */
     , (40285,  13, False) /* Ethereal */
     , (40285,  50, True ) /* NeverFailCasting */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (40285,   1,       5) /* HeartbeatInterval */
     , (40285,   2,       0) /* HeartbeatTimestamp */
     , (40285,   3,     0.6) /* HealthRate */
     , (40285,   4,       3) /* StaminaRate */
     , (40285,   5,       1) /* ManaRate */
     , (40285,  12,     0.5) /* Shade */
     , (40285,  13,       1) /* ArmorModVsSlash */
     , (40285,  14,    0.95) /* ArmorModVsPierce */
     , (40285,  15,    0.95) /* ArmorModVsBludgeon */
     , (40285,  16,       1) /* ArmorModVsCold */
     , (40285,  17,       1) /* ArmorModVsFire */
     , (40285,  18,       1) /* ArmorModVsAcid */
     , (40285,  19,       1) /* ArmorModVsElectric */
     , (40285,  31,      18) /* VisualAwarenessRange */
     , (40285,  34,       1) /* PowerupTime */
     , (40285,  36,       1) /* ChargeSpeed */
     , (40285,  39,     0.9) /* DefaultScale */
     , (40285,  64,     0.6) /* ResistSlash */
     , (40285,  65,     0.6) /* ResistPierce */
     , (40285,  66,     0.7) /* ResistBludgeon */
     , (40285,  67,     0.5) /* ResistFire */
     , (40285,  68,     0.5) /* ResistCold */
     , (40285,  69,     0.5) /* ResistAcid */
     , (40285,  70,     0.5) /* ResistElectric */
     , (40285,  71,       1) /* ResistHealthBoost */
     , (40285,  72,       1) /* ResistStaminaDrain */
     , (40285,  73,       1) /* ResistStaminaBoost */
     , (40285,  74,       1) /* ResistManaDrain */
     , (40285,  75,       1) /* ResistManaBoost */
     , (40285,  80,       2) /* AiUseMagicDelay */
     , (40285, 104,      10) /* ObviousRadarRange */
     , (40285, 125,       1) /* ResistHealthDrain */
     , (40285, 166,     0.6) /* ResistNether */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (40285,   1, 'Listris Sleech') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (40285,   1, 0x020014A0) /* Setup */
     , (40285,   2, 0x09000193) /* MotionTable */
     , (40285,   3, 0x20000062) /* SoundTable */
     , (40285,   4, 0x3000002A) /* CombatTable */
     , (40285,   6, 0x04001EDC) /* PaletteBase */
     , (40285,   7, 0x10000639) /* ClothingBase */
     , (40285,   8, 0x06001DF1) /* Icon */
     , (40285,  22, 0x340000B8) /* PhysicsEffectTable */
     , (40285,  35,       2111) /* DeathTreasureType - Loot Tier: 7 */;

INSERT INTO `weenie_properties_attribute` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`)
VALUES (40285,   1, 370, 0, 0) /* Strength */
     , (40285,   2, 370, 0, 0) /* Endurance */
     , (40285,   3, 330, 0, 0) /* Quickness */
     , (40285,   4, 350, 0, 0) /* Coordination */
     , (40285,   5, 440, 0, 0) /* Focus */
     , (40285,   6, 490, 0, 0) /* Self */;

INSERT INTO `weenie_properties_attribute_2nd` (`object_Id`, `type`, `init_Level`, `level_From_C_P`, `c_P_Spent`, `current_Level`)
VALUES (40285,   1,   435, 0, 0, 620) /* MaxHealth */
     , (40285,   3,   500, 0, 0, 870) /* MaxStamina */
     , (40285,   5,  1000, 0, 0, 1490) /* MaxMana */;

INSERT INTO `weenie_properties_skill` (`object_Id`, `type`, `level_From_P_P`, `s_a_c`, `p_p`, `init_Level`, `resistance_At_Last_Check`, `last_Used_Time`)
VALUES (40285,  6, 0, 3, 0, 380, 0, 0) /* MeleeDefense        Specialized */
     , (40285,  7, 0, 3, 0, 290, 0, 0) /* MissileDefense      Specialized */
     , (40285, 14, 0, 3, 0,  70, 0, 0) /* ArcaneLore          Specialized */
     , (40285, 15, 0, 3, 0, 275, 0, 0) /* MagicDefense        Specialized */
     , (40285, 20, 0, 3, 0,  50, 0, 0) /* Deception           Specialized */
     , (40285, 31, 0, 3, 0, 175, 0, 0) /* CreatureEnchantment Specialized */
     , (40285, 32, 0, 3, 0, 175, 0, 0) /* ItemEnchantment     Specialized */
     , (40285, 33, 0, 3, 0, 300, 0, 0) /* LifeMagic           Specialized */
     , (40285, 34, 0, 3, 0, 300, 0, 0) /* WarMagic            Specialized */
     , (40285, 45, 0, 3, 0, 210, 0, 0) /* LightWeapons        Specialized */;

INSERT INTO `weenie_properties_body_part` (`object_Id`, `key`, `d_Type`, `d_Val`, `d_Var`, `base_Armor`, `armor_Vs_Slash`, `armor_Vs_Pierce`, `armor_Vs_Bludgeon`, `armor_Vs_Cold`, `armor_Vs_Fire`, `armor_Vs_Acid`, `armor_Vs_Electric`, `armor_Vs_Nether`, `b_h`, `h_l_f`, `m_l_f`, `l_l_f`, `h_r_f`, `m_r_f`, `l_r_f`, `h_l_b`, `m_l_b`, `l_l_b`, `h_r_b`, `m_r_b`, `l_r_b`)
VALUES (40285,  0,  4, 200, 0.75,  650,  650,  618,  618,  650,  650,  650,  650,    0, 1, 0.44,  0.3,    0,  0.4,  0.1,    0, 0.44,  0.3,    0,  0.4,  0.1,    0) /* Head */
     , (40285, 16,  4,  0,    0,  650,  650,  618,  618,  650,  650,  650,  650,    0, 2,  0.5, 0.48,  0.1,  0.5,  0.6,  0.1,  0.5, 0.48,  0.1,  0.5,  0.6, 0.22) /* Torso */
     , (40285, 21,  4,  0,    0,  650,  650,  618,  618,  650,  650,  650,  650,    0, 2,    0,    0,  0.3,    0,    0,  0.3,    0,    0,  0.3,    0,    0, 0.28) /* Wings */
     , (40285, 24,  4,  0,    0,  650,  650,  618,  618,  650,  650,  650,  650,    0, 2, 0.06, 0.22,  0.3,  0.1,  0.2,  0.3, 0.06, 0.22,  0.3,  0.1,  0.2, 0.22) /* UpperTentacle */
     , (40285, 25,  4, 200,  0.5,  650,  650,  618,  618,  650,  650,  650,  650,    0, 3,    0,    0,  0.3,    0,  0.1,  0.3,    0,    0,  0.3,    0,  0.1, 0.28) /* LowerTentacle */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (40285,  2074,   2.15)  /* Gossamer Flesh */
     , (40285,  2122,   2.15)  /* Disintegration */
     , (40285,  2162,   2.02)  /* Olthoi's Gift */;

INSERT INTO `weenie_properties_create_list` (`object_Id`, `destination_Type`, `weenie_Class_Id`, `stack_Size`, `palette`, `shade`, `try_To_Bond`)
VALUES (40285, 9, 41979,  1, 0, 0.02, False) /* Create Shattered Mana Forge Key (41979) for ContainTreasure */
     , (40285, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */
     , (40285, 9, 34277,  1, 0, 0.02, False) /* Create Ancient Falatacot Trinket (34277) for ContainTreasure */
     , (40285, 9,     0,  0, 0, 0.98, False) /* Create nothing for ContainTreasure */;
