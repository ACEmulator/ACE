DELETE FROM `weenie` WHERE `class_Id` = 450603;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450603, 'ace450603-festivallights', 35, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450603,   1,      32768) /* ItemType - Caster */
     , (450603,   5,        0) /* EncumbranceVal */
     , (450603,   9,   16777216) /* ValidLocations - Held */
     , (450603,  16,     655364) /* ItemUseable - 655364 */
     , (450603,  19,         20) /* Value */
     , (450603,  46,        512) /* DefaultCombatStyle - Magic */
     , (450603,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450603,  94,         16) /* TargetType - Creature */
     , (450603, 150,        103) /* HookPlacement - Hook */
     , (450603, 151,          2) /* HookType - Wall */
     , (450603, 353,          0) /* WeaponType - Undef */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450603,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450603,  39,     0.3) /* DefaultScale */
     , (450603,  44,      30) /* TimeToRot */
     , (450603,  76,     0.3) /* Translucency */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450603,   1, 'Festival Lights') /* Name */
     , (450603,  14, 'This item can be used on ceiling and wall hooks.') /* Use */
     , (450603,  15, 'A small reflective pumpkin bauble with dancing colored lights around it. Don''t drop it unless you want to lose it. This item will quickly disappear if dropped on the ground -- it will even disappear from inside a pack, if that pack is dropped on the ground.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450603,   1, 0x020014C7) /* Setup */
     , (450603,   3, 0x20000014) /* SoundTable */
     , (450603,   8, 0x0600624F) /* Icon */
     , (450603,  22, 0x3400002B) /* PhysicsEffectTable */;
