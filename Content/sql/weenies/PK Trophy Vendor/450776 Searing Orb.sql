DELETE FROM `weenie` WHERE `class_Id` = 450776;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450776, 'casteracidpk', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450776,   1,      32768) /* ItemType - Caster */
     , (450776,   3,          8) /* PaletteTemplate - Green */
     , (450776,   5,         0) /* EncumbranceVal */
     , (450776,   8,         50) /* Mass */
     , (450776,   9,   16777216) /* ValidLocations - Held */
     , (450776,  16,          1) /* ItemUseable - No */
     , (450776,  18,        256) /* UiEffects - Acid */
     , (450776,  19,        20) /* Value */
     , (450776,  46,        512) /* DefaultCombatStyle - Magic */
     , (450776,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450776,  94,         16) /* TargetType - Creature */
     , (450776, 150,        103) /* HookPlacement - Hook */
     , (450776, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450776,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450776,  29,       1) /* WeaponDefense */
     , (450776,  39,     1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450776,   1, 'Searing Orb') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450776,   1, 0x020011F0) /* Setup */
     , (450776,   3, 0x20000014) /* SoundTable */
     , (450776,   6, 0x0400195D) /* PaletteBase */
     , (450776,   7, 0x10000588) /* ClothingBase */
     , (450776,   8, 0x06001532) /* Icon */
     , (450776,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450776,  27, 0x40000031) /* UseUserAnimation - MagicHeal */
     , (450776,  36, 0x0E000016) /* MutateFilter */
     , (450776,  46, 0x38000030) /* TsysMutationFilter */;
