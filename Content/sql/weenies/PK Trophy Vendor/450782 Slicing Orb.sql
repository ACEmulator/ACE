DELETE FROM `weenie` WHERE `class_Id` = 450782;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450782, 'casterslashingpk', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450782,   1,      32768) /* ItemType - Caster */
     , (450782,   3,         21) /* PaletteTemplate - Gold */
     , (450782,   5,         0) /* EncumbranceVal */
     , (450782,   8,         50) /* Mass */
     , (450782,   9,   16777216) /* ValidLocations - Held */
     , (450782,  16,          1) /* ItemUseable - No */
     , (450782,  18,       1024) /* UiEffects - Slashing */
     , (450782,  19,        20) /* Value */
     , (450782,  46,        512) /* DefaultCombatStyle - Magic */
     , (450782,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450782,  94,         16) /* TargetType - Creature */
     , (450782, 150,        103) /* HookPlacement - Hook */
     , (450782, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450782,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450782,  29,       1) /* WeaponDefense */
     , (450782,  39,     1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450782,   1, 'Slicing Orb') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450782,   1, 0x020011EA) /* Setup */
     , (450782,   3, 0x20000014) /* SoundTable */
     , (450782,   6, 0x0400195D) /* PaletteBase */
     , (450782,   7, 0x10000588) /* ClothingBase */
     , (450782,   8, 0x06001532) /* Icon */
     , (450782,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450782,  27, 0x40000031) /* UseUserAnimation - MagicHeal */
     , (450782,  36, 0x0E000016) /* MutateFilter */;
