DELETE FROM `weenie` WHERE `class_Id` = 480481;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480481, 'ace480481-luminouscrystalofrarearmordamageboostvpk', 38, '2021-11-01 00:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480481,   1,       2048) /* ItemType - Gem */
     , (480481,   5,          5) /* EncumbranceVal */
     , (480481,  16,          8) /* ItemUseable - Contained */
     , (480481,  18,          1) /* UiEffects - Magical */
     , (480481,  19,      100) /* Value */
     , (480481,  33,          1) /* Bonded - Normal */
     , (480481,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480481,  94,         16) /* TargetType - Creature */
     , (480481, 106,        325) /* ItemSpellcraft */
     , (480481, 107,      10000) /* ItemCurMana */
     , (480481, 108,      10000) /* ItemMaxMana */
     , (480481, 109,          0) /* ItemDifficulty */
     , (480481, 114,          1) /* Attuned - Attuned */
     , (480481, 151,         11) /* HookType - Floor, Wall, Yard */
	 , (480481, 267,     86400) /* Lifespan */
	 , (480481, 280,          3) /* SharedCooldown */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480481,  69, False) /* IsSellable */
     , (480481,  63, True ) /* UnlimitedUse */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480481,   1, 'Luminous Crystal of Rare Armor Damage Boost V') /* Name */
     , (480481,  16, 'Using this gem will cast Rare Armor Damage Boost V, which increases your Damage Rating by 5 for 3 hours.') /* LongDesc */
     , (480481,  20, 'Luminous Crystals of Rare Armor Damage Boost V') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480481,   1, 0x02000179) /* Setup */
     , (480481,   3, 0x20000014) /* SoundTable */
     , (480481,   6, 0x04000BEF) /* PaletteBase */
     , (480481,   8, 0x06006A88) /* Icon */
     , (480481,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480481,  28,       5978) /* Spell - Rare Armor Damage Boost V */
     , (480481,  50, 0x06005B2B) /* IconOverlay */
     , (480481,  52, 0x06006E89) /* IconUnderlay */;
