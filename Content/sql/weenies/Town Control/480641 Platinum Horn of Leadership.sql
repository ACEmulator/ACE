DELETE FROM `weenie` WHERE `class_Id` = 480641;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480641, 'ace480641-platinumhornofleadership', 38, '2021-11-01 00:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480641,   1,       2048) /* ItemType - Gem */
     , (480641,   5,        200) /* EncumbranceVal */
     , (480641,  16,          8) /* ItemUseable - Contained */
     , (480641,  18,          1) /* UiEffects - Magical */
     , (480641,  19,          750) /* Value */
     , (480641,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480641,  94,         16) /* TargetType - Creature */
     , (480641, 151,         11) /* HookType - Floor, Wall, Yard */
     , (480641, 280,          6) /* SharedCooldown */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480641,  22, True ) /* Inscribable */
     , (480641,  63, True ) /* UnlimitedUse */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480641, 167,      30) /* CooldownDuration */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480641,   1, 'Platinum Horn of Leadership') /* Name */
     , (480641,  16, 'This horn is eternal. Use this horn to increase the Health of your Fellowship by 10.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480641,   1, 0x02000179) /* Setup */
     , (480641,   3, 0x20000014) /* SoundTable */
     , (480641,   8, 0x06006A97) /* Icon */
     , (480641,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480641,  28,       5122) /* Spell - Call of Leadership V */;
