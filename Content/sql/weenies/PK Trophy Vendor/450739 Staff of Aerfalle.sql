DELETE FROM `weenie` WHERE `class_Id` = 450739;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450739, 'staffaerfalletailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450739,   1,      32768) /* ItemType - Caster */
     , (450739,   3,         20) /* PaletteTemplate - Silver */
     , (450739,   5,        0) /* EncumbranceVal */
     , (450739,   8,         25) /* Mass */
     , (450739,   9,   16777216) /* ValidLocations - Held */
     , (450739,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (450739,  18,          1) /* UiEffects - Magical */
     , (450739,  19,      20) /* Value */
     , (450739,  46,        512) /* DefaultCombatStyle - Magic */
     , (450739,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450739,  94,         16) /* TargetType - Creature */
     , (450739, 150,        103) /* HookPlacement - Hook */
     , (450739, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450739,  15, True ) /* LightsStatus */
     , (450739,  22, True ) /* Inscribable */
     , (450739,  23, True ) /* DestroyOnSell */
     , (450739,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450739,   5,  -0.083) /* ManaRate */
     , (450739,  29,       1) /* WeaponDefense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450739,   1, 'Staff of Aerfalle') /* Name */
     , (450739,   7, 'Made with the help of Rytheran and, in beneficence, His Eternal Splendor.') /* Inscription */
     , (450739,   8, 'Lady Aerfalle') /* ScribeName */
     , (450739,  15, 'A staff of petrified wood.') /* ShortDesc */
     , (450739,  16, 'A staff made from the petrified wood of Aerlinthe, taken from the claws of the Dark Magus of that island. This artifact is several centuries old.') /* LongDesc */
     , (450739,  33, 'AerfalleStaffObtained') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450739,   1, 0x02000896) /* Setup */
     , (450739,   3, 0x20000014) /* SoundTable */
     , (450739,   6, 0x04000BEF) /* PaletteBase */
     , (450739,   7, 0x10000230) /* ClothingBase */
     , (450739,   8, 0x06001D20) /* Icon */
     , (450739,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450739,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450739,  36, 0x0E000016) /* MutateFilter */
     , (450739,  37,         34) /* ItemSkillLimit - WarMagic */;


