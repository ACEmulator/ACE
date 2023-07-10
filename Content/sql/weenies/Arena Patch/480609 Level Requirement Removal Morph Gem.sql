DELETE FROM `weenie` WHERE `class_Id` = 480609;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480609, 'ace480609-Removelevelmorphgem', 38, '2022-01-29 01:15:03') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480609,   1,       2048) /* ItemType - Gem */
     , (480609,   5,         10) /* EncumbranceVal */
     , (480609,  11,          1) /* MaxStackSize */
     , (480609,  12,          1) /* StackSize */
     , (480609,  13,         10) /* StackUnitEncumbrance */
     , (480609,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (480609,  18,          1) /* UiEffects - Magical */
     , (480609,  19,         50) /* Value */
     , (480609,  65,        101) /* Placement - Resting */
     , (480609,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480609,  94,          35215) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480609,   1, False) /* Stuck */
     , (480609,  11, True ) /* IgnoreCollisions */
     , (480609,  13, True ) /* Ethereal */
     , (480609,  14, True ) /* GravityStatus */
     , (480609,  19, True ) /* Attackable */
     , (480609,  22, True ) /* Inscribable */
     , (480609,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480609,   1, 'Level Requirement Removal Morph Gem') /* Name */
     , (480609,  14, 'Applying this gem to items will remove the level requirement to wield it.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480609,   1, 0x02000179) /* Setup */
     , (480609,   3, 0x20000014) /* SoundTable */
     , (480609,   6, 0x04000BEF) /* PaletteBase */
     , (480609,   7, 0x1000010B) /* ClothingBase */
     , (480609,   8, 0x06002971) /* Icon */
     , (480609,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480609,  50, 0x0600665F) /* IconOverlay */
     , (480609,  52, 0x060065FB) /* IconUnderlay */;

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
