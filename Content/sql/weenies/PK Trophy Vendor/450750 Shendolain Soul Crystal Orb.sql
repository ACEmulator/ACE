DELETE FROM `weenie` WHERE `class_Id` = 450750;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450750, 'orbsoulcrystalshentailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450750,   1,      32768) /* ItemType - Caster */
     , (450750,   3,         13) /* PaletteTemplate - Purple */
     , (450750,   5,         0) /* EncumbranceVal */
     , (450750,   8,         50) /* Mass */
     , (450750,   9,   16777216) /* ValidLocations - Held */
     , (450750,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (450750,  18,          1) /* UiEffects - Magical */
     , (450750,  19,       20) /* Value */
     , (450750,  33,          1) /* Bonded - Bonded */
     , (450750,  46,        512) /* DefaultCombatStyle - Magic */
     , (450750,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450750,  94,         16) /* TargetType - Creature */
     , (450750, 150,        103) /* HookPlacement - Hook */
     , (450750, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450750,  15, True ) /* LightsStatus */
     , (450750,  22, True ) /* Inscribable */
     , (450750,  23, True ) /* DestroyOnSell */
     , (450750,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450750,   5,   -0.05) /* ManaRate */
     , (450750,  12,     0.2) /* Shade */
     , (450750,  29,       1) /* WeaponDefense */
     , (450750,  76,     0.5) /* Translucency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450750,   1, 'Shendolain Soul Crystal Orb') /* Name */
     , (450750,  15, 'An orb imbued with the power of the Shendolain Soul Crystal.') /* ShortDesc */
     , (450750,  16, 'An orb imbued with the power of the Shendolain Soul Crystal.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450750,   1, 0x0200091F) /* Setup */
     , (450750,   3, 0x20000014) /* SoundTable */
     , (450750,   6, 0x04000BF8) /* PaletteBase */
     , (450750,   7, 0x10000249) /* ClothingBase */
     , (450750,   8, 0x06001E09) /* Icon */
     , (450750,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450750,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450750,  36, 0x0E000016) /* MutateFilter */;
