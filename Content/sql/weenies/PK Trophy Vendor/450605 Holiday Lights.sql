DELETE FROM `weenie` WHERE `class_Id` = 450605;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450605, 'holiday2002decorationtailor', 35, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450605,   1,      32768) /* ItemType - Caster */
     , (450605,   5,        0) /* EncumbranceVal */
     , (450605,   9,   16777216) /* ValidLocations - Held */
     , (450605,  16,     655364) /* ItemUseable - 655364 */
     , (450605,  19,         20) /* Value */
     , (450605,  46,        512) /* DefaultCombatStyle - Magic */
     , (450605,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450605,  94,         16) /* TargetType - Creature */
     , (450605, 150,        103) /* HookPlacement - Hook */
     , (450605, 151,          2) /* HookType - Wall */
     , (450605, 353,          0) /* WeaponType - Undef */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450605,  22, True ) /* Inscribable */
     , (450605,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450605,  12,     0.5) /* Shade */
     , (450605,  39,     0.3) /* DefaultScale */
     , (450605,  44,      30) /* TimeToRot */
     , (450605,  76,     0.4) /* Translucency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450605,   1, 'Holiday Lights') /* Name */
     , (450605,  14, 'This item can be used on ceiling and wall hooks.') /* Use */
     , (450605,  15, 'A small reflective bauble with dancing colored lights around it. Don''t drop it unless you want to lose it. This item will quickly disappear if dropped on the ground -- it will even disappear from inside a pack, if that pack is dropped on the ground.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450605,   1, 0x02000E8D) /* Setup */
     , (450605,   3, 0x20000014) /* SoundTable */
     , (450605,   8, 0x06002974) /* Icon */
     , (450605,  22, 0x3400002B) /* PhysicsEffectTable */;
