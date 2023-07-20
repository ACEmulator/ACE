DELETE FROM `weenie` WHERE `class_Id` = 480702;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480702, 'ace480702-arenararedamagereductionvpk', 38, '2021-11-01 00:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480702,   1,       2048) /* ItemType - Gem */
     , (480702,   5,          5) /* EncumbranceVal */
     , (480702,  16,          8) /* ItemUseable - Contained */
     , (480702,  18,          1) /* UiEffects - Magical */
     , (480702,  19,      	  1) /* Value */
     , (480702,  33,          1) /* Bonded - Normal */
     , (480702,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480702,  94,         16) /* TargetType - Creature */
     , (480702, 106,        325) /* ItemSpellcraft */
     , (480702, 107,      10000) /* ItemCurMana */
     , (480702, 108,      10000) /* ItemMaxMana */
     , (480702, 109,          0) /* ItemDifficulty */
     , (480702, 114,          1) /* Attuned - Attuned */
     , (480702, 151,         11) /* HookType - Floor, Wall, Yard */
	 , (480702, 280,          3) /* SharedCooldown */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480702,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480702,   1, 'Arena Rare Damage Reduction V') /* Name */
     , (480702,  16, 'Using this gem will cast Rare Damage Reduction V, which increases your Damage Resistance Rating by 5 for 3 hours.\n\nCan only be used by a player in an active arena match.') /* LongDesc */
     , (480702,  20, 'Arena Rare Damage Reduction Vs') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480702,   1, 0x02000179) /* Setup */
     , (480702,   3, 0x20000014) /* SoundTable */
     , (480702,   6, 0x04000BEF) /* PaletteBase */
     , (480702,   8, 0x06006A88) /* Icon */
     , (480702,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480702,  28,       5192) /* Spell - Rare Damage Reduction V */
     , (480702,  50, 0x06005B25) /* IconOverlay */
     , (480702,  52, 0x06006E89) /* IconUnderlay */;
