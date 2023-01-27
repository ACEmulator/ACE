DELETE FROM `weenie` WHERE `class_Id` = 450222;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450222, 'ace450222-heartofdarkestflametailor', 35, '2021-12-14 05:15:31') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450222,   1,      32768) /* ItemType - Caster */
     , (450222,   3,          4) /* PaletteTemplate - Brown */
     , (450222,   5,        0) /* EncumbranceVal */
     , (450222,   8,         90) /* Mass */
     , (450222,   9,   16777216) /* ValidLocations - Held */
     , (450222,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450222,  19,      20) /* Value */
     , (450222,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450222,  46,        512) /* DefaultCombatStyle - Magic */
     , (450222,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450222,  94,         16) /* TargetType - Creature */
     , (450222, 151,          2) /* HookType - Wall */
     , (450222, 353,          0) /* WeaponType - Undef */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450222,  11, True ) /* IgnoreCollisions */
     , (450222,  13, True ) /* Ethereal */
     , (450222,  14, True ) /* GravityStatus */
     , (450222,  19, True ) /* Attackable */
     , (450222,  22, True ) /* Inscribable */
     , (450222, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450222,   5,   -0.05) /* ManaRate */
     , (450222,  12,    0.66) /* Shade */
     , (450222,  29,    1.0) /* WeaponDefense */
     , (450222,  39,       1) /* DefaultScale */
     , (450222, 138,    1.0) /* SlayerDamageBonus */
     , (450222, 144,    0.0) /* ManaConversionMod */
     , (450222, 147,     0.0) /* CriticalFrequency */
     , (450222, 152,    1.0) /* ElementalDamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450222,   1, 'Heart of Darkest Flame') /* Name */
     , (450222,  16, 'Due to the dark whispers that can be sometimes heard when the orb is wielded, it is often believed to be the heart of a slain Kemeroi. Whether or not this is belief is a true one, the Heart of Darkest Flame is a potent tool for those who wield the powers of the Void.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450222,   1, 0x02001A53) /* Setup */
     , (450222,   3, 0x20000014) /* SoundTable */
     , (450222,   6, 0x04000BEF) /* PaletteBase */
     , (450222,   8, 0x06006F47) /* Icon */
     , (450222,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450222,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450222,  28,       5355) /* Spell - Nether Bolt VII */
     , (450222,  36, 0x0E000012) /* MutateFilter */
     , (450222,  46, 0x38000032) /* TsysMutationFilter */
     , (450222,  52, 0x06005B0C) /* IconUnderlay */;
