DELETE FROM `weenie` WHERE `class_Id` = 450747;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450747, 'wandskulltailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450747,   1,      32768) /* ItemType - Caster */
     , (450747,   5,        150) /* EncumbranceVal */
     , (450747,   8,         10) /* Mass */
     , (450747,   9,   16777216) /* ValidLocations - Held */
     , (450747,  16,          1) /* ItemUseable - No */
     , (450747,  18,          1) /* UiEffects - Magical */
     , (450747,  19,         20) /* Value */
     , (450747,  46,        512) /* DefaultCombatStyle - Magic */
     , (450747,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450747,  94,         16) /* TargetType - Creature */
     , (450747, 150,        103) /* HookPlacement - Hook */
     , (450747, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450747,  22, True ) /* Inscribable */
     , (450747,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450747,  29,       1) /* WeaponDefense */
     , (450747,  39,     1.2) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450747,   1, 'Skull Wand') /* Name */
     , (450747,  15, 'A wand with a shrunken skull on it.') /* ShortDesc */
     , (450747,  16, 'A wand with a shrunken skull on it.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450747,   1, 0x02000B7B) /* Setup */
     , (450747,   3, 0x20000014) /* SoundTable */
     , (450747,   8, 0x060022B5) /* Icon */
     , (450747,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450747,  36, 0x0E000016) /* MutateFilter */;
