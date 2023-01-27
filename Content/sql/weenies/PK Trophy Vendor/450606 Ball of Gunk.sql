DELETE FROM `weenie` WHERE `class_Id` = 450606;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450606, 'ballofgunktailor', 35, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450606,   1,      32768) /* ItemType - Caster */
     , (450606,   5,        0) /* EncumbranceVal */
     , (450606,   9,   16777216) /* ValidLocations - Held */
     , (450606,  16,     655364) /* ItemUseable - 655364 */
     , (450606,  19,         20) /* Value */
     , (450606,  46,        512) /* DefaultCombatStyle - Magic */
     , (450606,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450606,  94,         16) /* TargetType - Creature */
     , (450606, 150,        103) /* HookPlacement - Hook */
     , (450606, 151,          2) /* HookType - Wall */
     , (450606, 353,          0) /* WeaponType - Undef */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450606,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450606,   1, 'Ball of Gunk') /* Name */
     , (450606,  16, 'A squishy ball of green gunk.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450606,   1, 0x02000F05) /* Setup */
     , (450606,   3, 0x20000014) /* SoundTable */
     , (450606,   8, 0x06002AB7) /* Icon */
     , (450606,  22, 0x3400002B) /* PhysicsEffectTable */;
