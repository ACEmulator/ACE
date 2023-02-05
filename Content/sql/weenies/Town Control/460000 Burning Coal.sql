DELETE FROM `weenie` WHERE `class_Id` = 460000;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (460000, 'gemburningcoalTC', 38, '2005-02-09 10:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (460000,   1,         32) /* ItemType - Food */
     , (460000,   5,         75) /* EncumbranceVal */
     , (460000,   8,         75) /* Mass */
     , (460000,  11,         10) /* MaxStackSize */
     , (460000,  12,          1) /* StackSize */
     , (460000,  13,         75) /* StackUnitEncumbrance */
     , (460000,  14,         75) /* StackUnitMass */
     , (460000,  15,        100) /* StackUnitValue */
     , (460000,  16,          8) /* ItemUseable - Contained */
     , (460000,  18,          1) /* UiEffects - Magical */
     , (460000,  19,        100) /* Value */
	 , (460000, 114,          1) /* Attuned - Attuned */
	 , (460000,  33,          1) /* Bonded - Bonded */
     , (460000,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (460000,  94,         16) /* TargetType - Creature */
     , (460000, 106,        150) /* ItemSpellcraft */
     , (460000, 107,         50) /* ItemCurMana */
     , (460000, 108,         50) /* ItemMaxMana */
     , (460000, 109,        200) /* ItemDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (460000,  23, True ) /* DestroyOnSell */
     , (460000,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (460000,   1, 'Burning Coal') /* Name */
     , (460000,  14, 'Use this item to stoke the fire within.') /* Use */
     , (460000,  16, 'A smoldering coal. The center of this rock seems to glow with intense heat, yet the surface is cool to the touch.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (460000,   1, 0x02000FF5) /* Setup */
     , (460000,   3, 0x20000014) /* SoundTable */
     , (460000,   8, 0x06003328) /* Icon */
     , (460000,  22, 0x3400002B) /* PhysicsEffectTable */
     , (460000,  28,       3204) /* Spell - Blazing Heart */;
