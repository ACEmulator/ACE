DELETE FROM `weenie` WHERE `class_Id` = 480482;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480482, 'ace480482-luminouscrystalofraredamagereductionvpk', 38, '2021-11-01 00:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480482,   1,       2048) /* ItemType - Gem */
     , (480482,   5,          5) /* EncumbranceVal */
     , (480482,  16,          8) /* ItemUseable - Contained */
     , (480482,  18,          1) /* UiEffects - Magical */
     , (480482,  19,      100) /* Value */
     , (480482,  33,          1) /* Bonded - Normal */
     , (480482,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480482,  94,         16) /* TargetType - Creature */
     , (480482, 106,        325) /* ItemSpellcraft */
     , (480482, 107,      10000) /* ItemCurMana */
     , (480482, 108,      10000) /* ItemMaxMana */
     , (480482, 109,          0) /* ItemDifficulty */
     , (480482, 114,          1) /* Attuned - Attuned */
     , (480482, 151,         11) /* HookType - Floor, Wall, Yard */
	 , (480482, 267,     86400) /* Lifespan */
	 , (480482, 280,          3) /* SharedCooldown */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480482,  69, False) /* IsSellable */
     , (480482,  63, True ) /* UnlimitedUse */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480482,   1, 'Luminous Crystal of Rare Damage Reduction V') /* Name */
     , (480482,  16, 'Using this gem will cast Rare Damage Reduction V, which increases your Damage Resistance Rating by 5 for 3 hours.') /* LongDesc */
     , (480482,  20, 'Luminous Crystals of Rare Damage Reduction V') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480482,   1, 0x02000179) /* Setup */
     , (480482,   3, 0x20000014) /* SoundTable */
     , (480482,   6, 0x04000BEF) /* PaletteBase */
     , (480482,   8, 0x06006A88) /* Icon */
     , (480482,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480482,  28,       5192) /* Spell - Rare Damage Reduction V */
     , (480482,  50, 0x06005B25) /* IconOverlay */
     , (480482,  52, 0x06006E89) /* IconUnderlay */;
