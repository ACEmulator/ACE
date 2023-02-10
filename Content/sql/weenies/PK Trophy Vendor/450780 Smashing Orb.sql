DELETE FROM `weenie` WHERE `class_Id` = 450780;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450780, 'casterbludgeoningpk', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450780,   1,      32768) /* ItemType - Caster */
     , (450780,   3,         61) /* PaletteTemplate - White */
     , (450780,   5,         0) /* EncumbranceVal */
     , (450780,   8,         50) /* Mass */
     , (450780,   9,   16777216) /* ValidLocations - Held */
     , (450780,  16,          1) /* ItemUseable - No */
     , (450780,  18,        512) /* UiEffects - Bludgeoning */
     , (450780,  19,        20) /* Value */
     , (450780,  46,        512) /* DefaultCombatStyle - Magic */
     , (450780,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450780,  94,         16) /* TargetType - Creature */
     , (450780, 150,        103) /* HookPlacement - Hook */
     , (450780, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450780,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450780,  29,       1) /* WeaponDefense */
     , (450780,  39,     1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450780,   1, 'Smashing Orb') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450780,   1, 0x020011EF) /* Setup */
     , (450780,   3, 0x20000014) /* SoundTable */
     , (450780,   6, 0x0400195D) /* PaletteBase */
     , (450780,   7, 0x10000588) /* ClothingBase */
     , (450780,   8, 0x06001532) /* Icon */
     , (450780,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450780,  27, 0x40000031) /* UseUserAnimation - MagicHeal */
     , (450780,  36, 0x0E000016) /* MutateFilter */
     , (450780,  46, 0x38000030) /* TsysMutationFilter */;
