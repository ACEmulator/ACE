DELETE FROM `weenie` WHERE `class_Id` = 450779;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450779, 'casterfrostpk', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450779,   1,      32768) /* ItemType - Caster */
     , (450779,   3,         77) /* PaletteTemplate - BlueGreen */
     , (450779,   5,         0) /* EncumbranceVal */
     , (450779,   8,         50) /* Mass */
     , (450779,   9,   16777216) /* ValidLocations - Held */
     , (450779,  16,          1) /* ItemUseable - No */
     , (450779,  18,        128) /* UiEffects - Frost */
     , (450779,  19,        20) /* Value */
     , (450779,  46,        512) /* DefaultCombatStyle - Magic */
     , (450779,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450779,  94,         16) /* TargetType - Creature */
     , (450779, 150,        103) /* HookPlacement - Hook */
     , (450779, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450779,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450779,  29,       1) /* WeaponDefense */
     , (450779,  39,     1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450779,   1, 'Freezing Orb') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450779,   1, 0x020011EC) /* Setup */
     , (450779,   3, 0x20000014) /* SoundTable */
     , (450779,   6, 0x0400195D) /* PaletteBase */
     , (450779,   7, 0x10000588) /* ClothingBase */
     , (450779,   8, 0x06001532) /* Icon */
     , (450779,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450779,  27, 0x40000031) /* UseUserAnimation - MagicHeal */
     , (450779,  36, 0x0E000016) /* MutateFilter */
     , (450779,  46, 0x38000030) /* TsysMutationFilter */;
