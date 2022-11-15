DELETE FROM `weenie` WHERE `class_Id` = 450599;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450599, 'ace450599-tomeofflametailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450599,   1,      32768) /* ItemType - Caster */
     , (450599,   5,        0) /* EncumbranceVal */
     , (450599,   9,   16777216) /* ValidLocations - Held */
     , (450599,  16,     655364) /* ItemUseable - 655364 */
     , (450599,  19,         20) /* Value */
     , (450599,  45,         0) /* DamageType - Fire */
     , (450599,  46,        512) /* DefaultCombatStyle - Magic */
     , (450599,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450599,  94,         16) /* TargetType - Creature */
     , (450599, 150,        103) /* HookPlacement - Hook */
     , (450599, 151,          2) /* HookType - Wall */
     , (450599, 263,         16) /* ResistanceModifierType - Fire */
     , (450599, 353,          0) /* WeaponType - Undef */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450599,  11, True ) /* IgnoreCollisions */
     , (450599,  13, True ) /* Ethereal */
     , (450599,  14, True ) /* GravityStatus */
     , (450599,  19, True ) /* Attackable */
     , (450599,  22, True ) /* Inscribable */
     , (450599,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450599,   5,   -0.05) /* ManaRate */
     , (450599,  29,       1) /* WeaponDefense */
     , (450599, 144,    0.15) /* ManaConversionMod */
     , (450599, 150,   1.025) /* WeaponMagicDefense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450599,   1, 'Tome of Flame') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450599,   1, 0x02001882) /* Setup */
     , (450599,   3, 0x20000014) /* SoundTable */
     , (450599,   6, 0x0400195D) /* PaletteBase */
     , (450599,   8, 0x060069C0) /* Icon */
     , (450599,  22, 0x3400002B) /* PhysicsEffectTable */;

