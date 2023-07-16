DELETE FROM `weenie` WHERE `class_Id` = 480485;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480485, 'ace480485-playerwieldmorphgem', 38, '2022-01-29 01:15:03') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480485,   1,       2048) /* ItemType - Gem */
     , (480485,   5,         10) /* EncumbranceVal */
     , (480485,  11,          1) /* MaxStackSize */
     , (480485,  12,          1) /* StackSize */
     , (480485,  13,         10) /* StackUnitEncumbrance */
     , (480485,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (480485,  18,          1) /* UiEffects - Magical */
     , (480485,  19,         10) /* Value */
     , (480485,  65,        101) /* Placement - Resting */
     , (480485,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480485,  94,          35215) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480485,   1, False) /* Stuck */
     , (480485,  11, True ) /* IgnoreCollisions */
     , (480485,  13, True ) /* Ethereal */
     , (480485,  14, True ) /* GravityStatus */
     , (480485,  19, True ) /* Attackable */
     , (480485,  22, True ) /* Inscribable */
     , (480485,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480485,   1, 'Player Wield Requirement Morph Gem') /* Name */
     , (480485,  14, 'Applying this gem to loot type items will remove the player wield activation requirement.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480485,   1, 0x02000179) /* Setup */
     , (480485,   3, 0x20000014) /* SoundTable */
     , (480485,   6, 0x04000BEF) /* PaletteBase */
     , (480485,   7, 0x1000010B) /* ClothingBase */
     , (480485,   8, 0x06002971) /* Icon */
     , (480485,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480485,  50,  100668355) /* IconOverlay */
     , (480485,  52, 0x060065FB) /* IconUnderlay */;

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
