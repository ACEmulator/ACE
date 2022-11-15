DELETE FROM `weenie` WHERE `class_Id` = 450598;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450598, 'ace450598-tomeofcausticstailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450598,   1,      32768) /* ItemType - Caster */
     , (450598,   5,        0) /* EncumbranceVal */
     , (450598,   9,   16777216) /* ValidLocations - Held */
     , (450598,  16,     655364) /* ItemUseable - 655364 */
     , (450598,  19,         20) /* Value */
     , (450598,  45,         32) /* DamageType - Acid */
     , (450598,  46,        512) /* DefaultCombatStyle - Magic */
     , (450598,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450598,  94,         16) /* TargetType - Creature */
     , (450598, 150,        103) /* HookPlacement - Hook */
     , (450598, 151,          2) /* HookType - Wall */
     , (450598, 353,          0) /* WeaponType - Undef */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450598,  11, True ) /* IgnoreCollisions */
     , (450598,  13, True ) /* Ethereal */
     , (450598,  14, True ) /* GravityStatus */
     , (450598,  19, True ) /* Attackable */
     , (450598,  22, True ) /* Inscribable */
     , (450598,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450598,   5,   -0.05) /* ManaRate */
     , (450598,  29,       1) /* WeaponDefense */
     , (450598, 144,    0.15) /* ManaConversionMod */
     , (450598, 150,   1.025) /* WeaponMagicDefense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450598,   1, 'Tome of Caustics') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450598,   1, 0x02001881) /* Setup */
     , (450598,   3, 0x20000014) /* SoundTable */
     , (450598,   6, 0x0400195D) /* PaletteBase */
     , (450598,   8, 0x060069BB) /* Icon */
     , (450598,  22, 0x3400002B) /* PhysicsEffectTable */;

