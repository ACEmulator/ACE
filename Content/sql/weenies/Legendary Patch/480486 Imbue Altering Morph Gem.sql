DELETE FROM `weenie` WHERE `class_Id` = 480486;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480486, 'ace480486-ImbueSwapmorphgem', 38, '2022-01-29 01:15:03') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480486,   1,       2048) /* ItemType - Gem */
     , (480486,   5,         10) /* EncumbranceVal */
     , (480486,  11,          1) /* MaxStackSize */
     , (480486,  12,          1) /* StackSize */
     , (480486,  13,         10) /* StackUnitEncumbrance */
     , (480486,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (480486,  18,          1) /* UiEffects - Magical */
     , (480486,  19,         5) /* Value */
     , (480486,  65,        101) /* Placement - Resting */
     , (480486,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480486,  94,          33025) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480486,   1, False) /* Stuck */
     , (480486,  11, True ) /* IgnoreCollisions */
     , (480486,  13, True ) /* Ethereal */
     , (480486,  14, True ) /* GravityStatus */
     , (480486,  19, True ) /* Attackable */
     , (480486,  22, True ) /* Inscribable */
     , (480486,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480486,   1, 'Imbue Altering Morph Gem') /* Name */
     , (480486,  14, 'Applying this gem to a loot type weapon currently imbued with Critical Strike, Crushing Blow or Armor Rending has a chance to randomly alter the current imbue.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480486,   1, 0x02000179) /* Setup */
     , (480486,   3, 0x20000014) /* SoundTable */
     , (480486,   6, 0x04000BEF) /* PaletteBase */
     , (480486,   7, 0x1000010B) /* ClothingBase */
     , (480486,   8, 0x06002971) /* Icon */
     , (480486,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480486,  50, 0x060067EA) /* IconOverlay */
     , (480486,  52, 0x060065FB) /* IconUnderlay */;

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
