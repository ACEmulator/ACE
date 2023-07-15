DELETE FROM `weenie` WHERE `class_Id` = 480613;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480613, 'burningcoalTC', 38, '2005-02-09 10:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480613,   1,         32) /* ItemType - Food */
     , (480613,   5,         75) /* EncumbranceVal */
     , (480613,   8,         75) /* Mass */
     , (480613,  11,         10) /* MaxStackSize */
     , (480613,  12,          1) /* StackSize */
     , (480613,  13,         75) /* StackUnitEncumbrance */
     , (480613,  14,         75) /* StackUnitMass */
     , (480613,  19,         5) /* Value */
     , (480613,  16,          8) /* ItemUseable - Contained */
     , (480613,  18,          1) /* UiEffects - Magical */
     , (480613,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480613,  94,         16) /* TargetType - Creature */
     , (480613, 106,        150) /* ItemSpellcraft */
     , (480613, 107,         50) /* ItemCurMana */
     , (480613, 108,         50) /* ItemMaxMana */
     , (480613, 109,        200) /* ItemDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480613,  23, True ) /* DestroyOnSell */
     , (480613,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480613,   1, 'Burning Coal') /* Name */
     , (480613,  14, 'Use this item to stoke the fire within.') /* Use */
     , (480613,  16, 'A smoldering coal. The center of this rock seems to glow with intense heat, yet the surface is cool to the touch.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480613,   1, 0x02000FF5) /* Setup */
     , (480613,   3, 0x20000014) /* SoundTable */
     , (480613,   8, 0x06003328) /* Icon */
     , (480613,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480613,  28,       3204) /* Spell - Blazing Heart */;
