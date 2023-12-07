DELETE FROM `weenie` WHERE `class_Id` = 490070;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490070, 'ace490070-Water of Enlightenment', 38, '2022-07-22 16:06:17') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490070,   1,       2048) /* ItemType - Gem */
     , (490070,   5,          5) /* EncumbranceVal */
     , (490070,  16,          8) /* ItemUseable - Contained */
     , (490070,  18,          1) /* UiEffects - Magical */
	 , (490070,  11,        10) /* MaxStackSize */
     , (490070,  12,          1) /* StackSize */
     , (490070,  13,         10) /* StackUnitEncumbrance */
     , (490070,  15,      10000) /* StackUnitValue */
     , (490070,  19,          0) /* Value */
     , (490070,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490070,  94,         16) /* TargetType - Creature */
     , (490070, 106,        325) /* ItemSpellcraft */
     , (490070, 107,      10000) /* ItemCurMana */
     , (490070, 108,      10000) /* ItemMaxMana */
     , (490070, 109,          0) /* ItemDifficulty */
     , (490070, 151,         11) /* HookType - Floor, Wall, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490070,   1, False) /* Stuck */
     , (490070,  11, True ) /* IgnoreCollisions */
     , (490070,  13, True ) /* Ethereal */
     , (490070,  14, True ) /* GravityStatus */
     , (490070,  19, True ) /* Attackable */
     , (490070,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490070,   1, 'Water of Enlightenment') /* Name */
     , (490070,  16, 'This water appears to have a unique glow and was drawn from the Font of Enlightenment. It appears to have been instilled with experience. Drinking this water will grant the Player 1 Billion experience. This potion has been enhanced and appears to work for Enlightened characters. The experience gained will be decreased based on the enlightenment level of the Player.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490070,   1, 0x02000179) /* Setup */
     , (490070,   3, 0x20000014) /* SoundTable */
     , (490070,   6, 0x04000BEF) /* PaletteBase */
     , (490070,   8, 0x0600332B) /* Icon */
     , (490070,  22, 0x3400002B) /* PhysicsEffectTable */
	 , (490070,  23,         64) /* UseSound - Eat1 */
     , (490070,  27, 0x13000081) /* UseUserAnimation - MimeEat */
     , (490070,  52, 0x06005B0C) /* IconUnderlay */;
