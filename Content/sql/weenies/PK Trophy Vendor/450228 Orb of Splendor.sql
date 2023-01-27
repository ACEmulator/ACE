DELETE FROM `weenie` WHERE `class_Id` = 450228;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450228, 'orbsplendortailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450228,   1,      32768) /* ItemType - Caster */
     , (450228,   5,        0) /* EncumbranceVal */
     , (450228,   8,         50) /* Mass */
     , (450228,   9,   16777216) /* ValidLocations - Held */
     , (450228,  16,          1) /* ItemUseable - No */
     , (450228,  19,       20) /* Value */
     , (450228,  46,        512) /* DefaultCombatStyle - Magic */
     , (450228,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450228,  94,         16) /* TargetType - Creature */
     , (450228, 150,        103) /* HookPlacement - Hook */
     , (450228, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450228,  22, True ) /* Inscribable */
     , (450228,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450228,   5,  -0.033) /* ManaRate */
     , (450228,  29,       1) /* WeaponDefense */
     , (450228,  39,       1) /* DefaultScale */
     , (450228, 144,    0.12) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450228,   1, 'Orb of Splendor') /* Name */
     , (450228,  15, 'This orb is a representation of the splendor of the Firebird as realized by the adherents who follow the path of the Firebird.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450228,   1, 0x02001026) /* Setup */
     , (450228,   3, 0x20000014) /* SoundTable */
     , (450228,   8, 0x06003042) /* Icon */
     , (450228,  22, 0x3400002B) /* PhysicsEffectTable */;

