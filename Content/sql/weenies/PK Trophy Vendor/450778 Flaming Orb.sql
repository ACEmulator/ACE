DELETE FROM `weenie` WHERE `class_Id` = 450778;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450778, 'casterfirepk', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450778,   1,      32768) /* ItemType - Caster */
     , (450778,   3,         14) /* PaletteTemplate - Red */
     , (450778,   5,         0) /* EncumbranceVal */
     , (450778,   8,         50) /* Mass */
     , (450778,   9,   16777216) /* ValidLocations - Held */
     , (450778,  16,          1) /* ItemUseable - No */
     , (450778,  18,         32) /* UiEffects - Fire */
     , (450778,  19,        20) /* Value */
     , (450778,  46,        512) /* DefaultCombatStyle - Magic */
     , (450778,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450778,  94,         16) /* TargetType - Creature */
     , (450778, 150,        103) /* HookPlacement - Hook */
     , (450778, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450778,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450778,  29,       1) /* WeaponDefense */
     , (450778,  39,     1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450778,   1, 'Flaming Orb') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450778,   1, 0x020011ED) /* Setup */
     , (450778,   3, 0x20000014) /* SoundTable */
     , (450778,   6, 0x0400195D) /* PaletteBase */
     , (450778,   7, 0x10000588) /* ClothingBase */
     , (450778,   8, 0x06001532) /* Icon */
     , (450778,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450778,  27, 0x40000031) /* UseUserAnimation - MagicHeal */
     , (450778,  36, 0x0E000016) /* MutateFilter */
     , (450778,  46, 0x38000030) /* TsysMutationFilter */;
