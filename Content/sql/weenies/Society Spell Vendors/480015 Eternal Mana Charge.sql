DELETE FROM `weenie` WHERE `class_Id` = 480015;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480015, 'manastonerareeternalmajorpk', 37, '2021-11-01 00:00:00') /* ManaStone */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480015,   1,     524288) /* ItemType - ManaStone */
     , (480015,   5,         50) /* EncumbranceVal */
     , (480015,   8,         50) /* Mass */
     , (480015,  16,     655368) /* ItemUseable - SourceContainedTargetSelfOrContained */
     , (480015,  18,          1) /* UiEffects - Magical */
     , (480015,  19,          200) /* Value */
     , (480015,  33,         1) /* Bonded - Slippery */
	 , (480015, 114,          1) /* Attuned - Attuned */
     , (480015,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480015,  94,      35103) /* TargetType - Jewelry, Creature, Gem, RedirectableItemEnchantmentTarget */
     , (480015, 107,      10000) /* ItemCurMana */
     , (480015, 108,      10000) /* ItemMaxMana */
     , (480015, 150,        103) /* HookPlacement - Hook */
     , (480015, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480015,  22, True ) /* Inscribable */
     , (480015,  63, True ) /* UnlimitedUse */
     , (480015,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480015,  87,       1) /* ItemEfficiency */
     , (480015, 137,       0) /* ManaStoneDestroyChance */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480015,   1, 'Eternal Mana Charge') /* Name */
     , (480015,  16, 'This mana stone does not run out of charges. It will not be destroyed upon use.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480015,   1, 0x020004B9) /* Setup */
     , (480015,   8, 0x06005B72) /* Icon */
     , (480015,  52, 0x06005B0C) /* IconUnderlay */;
