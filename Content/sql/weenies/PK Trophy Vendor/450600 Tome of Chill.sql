DELETE FROM `weenie` WHERE `class_Id` = 450600;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450600, 'ace450600-tomeofchilltailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450600,   1,      32768) /* ItemType - Caster */
     , (450600,   5,        0) /* EncumbranceVal */
     , (450600,   9,   16777216) /* ValidLocations - Held */
     , (450600,  16,     655364) /* ItemUseable - 655364 */
     , (450600,  19,         20) /* Value */
     , (450600,  45,          8) /* DamageType - Cold */
     , (450600,  46,        512) /* DefaultCombatStyle - Magic */
     , (450600,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450600,  94,         16) /* TargetType - Creature */
     , (450600, 150,        103) /* HookPlacement - Hook */
     , (450600, 151,          2) /* HookType - Wall */
     , (450600, 353,          0) /* WeaponType - Undef */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450600,  11, True ) /* IgnoreCollisions */
     , (450600,  13, True ) /* Ethereal */
     , (450600,  14, True ) /* GravityStatus */
     , (450600,  19, True ) /* Attackable */
     , (450600,  22, True ) /* Inscribable */
     , (450600,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450600,   5,   -0.05) /* ManaRate */
     , (450600,  29,       1) /* WeaponDefense */
     , (450600, 144,    0.15) /* ManaConversionMod */
     , (450600, 150,   1.025) /* WeaponMagicDefense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450600,   1, 'Tome of Chill') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450600,   1, 0x02001883) /* Setup */
     , (450600,   3, 0x20000014) /* SoundTable */
     , (450600,   6, 0x0400195D) /* PaletteBase */
     , (450600,   8, 0x060069C2) /* Icon */
     , (450600,  22, 0x3400002B) /* PhysicsEffectTable */;


