DELETE FROM `weenie` WHERE `class_Id` = 490027;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490027, 'ace490027-workmanshipmorphgem', 38, '2022-01-29 01:15:03') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490027,   1,       2048) /* ItemType - Gem */
     , (490027,   5,         10) /* EncumbranceVal */
     , (490027,  11,          1) /* MaxStackSize */
     , (490027,  12,          1) /* StackSize */
     , (490027,  13,         10) /* StackUnitEncumbrance */
     , (490027,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (490027,  18,          1) /* UiEffects - Magical */
     , (490027,  19,         4) /* Value */
     , (490027,  65,        101) /* Placement - Resting */
     , (490027,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490027,  94,          35215) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490027,   1, False) /* Stuck */
     , (490027,  11, True ) /* IgnoreCollisions */
     , (490027,  13, True ) /* Ethereal */
     , (490027,  14, True ) /* GravityStatus */
     , (490027,  19, True ) /* Attackable */
     , (490027,  22, True ) /* Inscribable */
     , (490027,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490027,   1, 'Workmanship Morph Gem') /* Name */
     , (490027,  14, 'Applying this gem to loot type items has a chance to alter its Workmanship.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES  (490027,   1, 0x02000179) /* Setup */
     , (490027,   3, 0x20000014) /* SoundTable */
     , (490027,   6, 0x04000BEF) /* PaletteBase */
     , (490027,   7, 0x1000010B) /* ClothingBase */
     , (490027,   8, 0x06002971) /* Icon */
     , (490027,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490027,  50, 0x060026BA) /* IconOverlay */
     , (490027,  52, 0x060065FB) /* IconUnderlay */;

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
