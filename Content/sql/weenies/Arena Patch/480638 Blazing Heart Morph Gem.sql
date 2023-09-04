DELETE FROM `weenie` WHERE `class_Id` = 480638;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480638, 'ace480638-blazingheartmorphgem', 38, '2022-01-29 01:15:03') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480638,   1,       2048) /* ItemType - Gem */
     , (480638,   5,         10) /* EncumbranceVal */
     , (480638,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (480638,  18,          1) /* UiEffects - Magical */
     , (480638,  19,         10) /* Value */
     , (480638,  65,        101) /* Placement - Resting */
     , (480638,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480638,  94,          35215) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480638,   1, False) /* Stuck */
     , (480638,  11, True ) /* IgnoreCollisions */
     , (480638,  13, True ) /* Ethereal */
     , (480638,  14, True ) /* GravityStatus */
     , (480638,  19, True ) /* Attackable */
     , (480638,  22, True ) /* Inscribable */
     , (480638,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480638,   1, 'Blazing Heart Morph Gem') /* Name */
     , (480638,  14, 'Applying this gem to loot type items has a 38% chance to apply the spell Blazing Heart.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480638,   1, 0x02000179) /* Setup */
     , (480638,   3, 0x20000014) /* SoundTable */
     , (480638,   6, 0x04000BEF) /* PaletteBase */
     , (480638,   7, 0x1000010B) /* ClothingBase */
     , (480638,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480638,  50, 0x06003328) /* IconOverlay */
     , (480638,  52, 100689403) /* IconUnderlay */;

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
