DELETE FROM `weenie` WHERE `class_Id` = 490071;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490071, 'ace490071-Ancient Bottle', 38, '2022-07-22 16:06:17') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490071,   1,       2048) /* ItemType - Gem */
     , (490071,   5,          5) /* EncumbranceVal */
     , (490071,  16,          8) /* ItemUseable - Contained */
     , (490071,  18,          1) /* UiEffects - Magical */
     , (490071,  19,          0) /* Value */
     , (490071,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490071,  94,         16) /* TargetType - Creature */
     , (490071, 106,        325) /* ItemSpellcraft */
     , (490071, 107,      10000) /* ItemCurMana */
     , (490071, 108,      10000) /* ItemMaxMana */
     , (490071, 109,          0) /* ItemDifficulty */
     , (490071, 319,         1) /* HookType - Floor, Wall, Yard */
	 , (490071, 320,         1) /* HookType - Floor, Wall, Yard */
	, (490071, 151,         11) /* HookType - Floor, Wall, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490071,   1, False) /* Stuck */
     , (490071,  11, True ) /* IgnoreCollisions */
     , (490071,  13, True ) /* Ethereal */
     , (490071,  14, True ) /* GravityStatus */
     , (490071,  19, True ) /* Attackable */
     , (490071,  69, False) /* IsSellable */;
	 
INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (490071,   4,          0) /* ItemTotalXp */
     , (490071,   5, 10000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490071,   1, 'Ancient Bottle') /* Name */
     , (490071,  16, 'This bottle appears to have been crafted by the Empyrean. It looks to have the unique ability to store experience in it to be used at a later date. A small portion of your earned experience will be stored in this bottle until it is full.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490071,   1, 0x02000179) /* Setup */
     , (490071,   3, 0x20000014) /* SoundTable */
     , (490071,   6, 0x04000BEF) /* PaletteBase */
     , (490071,   8, 0x0600371B) /* Icon */
     , (490071,  22, 0x3400002B) /* PhysicsEffectTable */
	 , (490071,  23,         64) /* UseSound - Eat1 */
     , (490071,  27, 0x13000081) /* UseUserAnimation - MimeEat */
     , (490071,  52, 0x06005B0C) /* IconUnderlay */;
