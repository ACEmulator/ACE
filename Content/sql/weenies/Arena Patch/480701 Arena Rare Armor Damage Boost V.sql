DELETE FROM `weenie` WHERE `class_Id` = 480701;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480701, 'ace480701-rarearmordamageboostvpk', 38, '2021-11-01 00:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480701,   1,       2048) /* ItemType - Gem */
     , (480701,   5,          5) /* EncumbranceVal */
     , (480701,  16,          8) /* ItemUseable - Contained */
     , (480701,  18,          1) /* UiEffects - Magical */
     , (480701,  19,      	  1) /* Value */
     , (480701,  33,          1) /* Bonded - Normal */
     , (480701,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480701,  94,         16) /* TargetType - Creature */
     , (480701, 106,        325) /* ItemSpellcraft */
     , (480701, 107,      10000) /* ItemCurMana */
     , (480701, 108,      10000) /* ItemMaxMana */
     , (480701, 109,          0) /* ItemDifficulty */
     , (480701, 114,          1) /* Attuned - Attuned */
     , (480701, 151,         11) /* HookType - Floor, Wall, Yard */
	 , (480701, 280,          3) /* SharedCooldown */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480701,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480701,   1, 'Arena Rare Armor Damage Boost V') /* Name */
     , (480701,  16, 'Using this gem will cast Rare Armor Damage Boost V, which increases your Damage Rating by 5 for 3 hours.\n\nCan only be used by a player in an active arena match.') /* LongDesc */
     , (480701,  20, 'Arena Rare Armor Damage Boost V') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480701,   1, 0x02000179) /* Setup */
     , (480701,   3, 0x20000014) /* SoundTable */
     , (480701,   6, 0x04000BEF) /* PaletteBase */
     , (480701,   8, 0x06006A88) /* Icon */
     , (480701,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480701,  28,       5978) /* Spell - Rare Armor Damage Boost V */
     , (480701,  50, 0x06005B2B) /* IconOverlay */
     , (480701,  52, 0x06006E89) /* IconUnderlay */;
