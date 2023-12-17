DELETE FROM `weenie` WHERE `class_Id` = 490040;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490040, 'ace490040-Rareupgrademorphgem', 38, '2022-01-29 01:15:03') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490040,   1,       2048) /* ItemType - Gem */
     , (490040,   5,         10) /* EncumbranceVal */
     , (490040,  11,          1) /* MaxStackSize */
     , (490040,  12,          1) /* StackSize */
     , (490040,  13,         10) /* StackUnitEncumbrance */
     , (490040,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (490040,  18,          1) /* UiEffects - Magical */
     , (490040,  19,         25) /* Value */
     , (490040,  65,        101) /* Placement - Resting */
     , (490040,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490040,  94,          35215) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490040,   1, False) /* Stuck */
     , (490040,  11, True ) /* IgnoreCollisions */
     , (490040,  13, True ) /* Ethereal */
     , (490040,  14, True ) /* GravityStatus */
     , (490040,  19, True ) /* Attackable */
     , (490040,  22, True ) /* Inscribable */
     , (490040,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490040,   1, 'Potion of Polishing') /* Name */
     , (490040,  14, 'Applying this potion to Rare Armor or Jewelry will upgrade the current Epic spells to Legendary spells.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490040,   1, 0x020000AB) /* Setup */
     , (490040,   3, 0x20000014) /* SoundTable */
     , (490040,   6, 0x04000BEF) /* PaletteBase */
     , (490040,   7, 0x10000168) /* ClothingBase */
     , (490040,   8, 0x06005A38) /* Icon */
     , (490040,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490040,  52, 0x06005B0C) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "Changelog": [
    {
      "created": "2022-01-17T02:18:55.5489445Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "IsDone": false
}
*/
