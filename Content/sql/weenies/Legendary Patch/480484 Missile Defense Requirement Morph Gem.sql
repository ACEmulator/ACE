DELETE FROM `weenie` WHERE `class_Id` = 480484;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480484, 'ace480484-missiledmorphgem', 38, '2022-01-29 01:15:03') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480484,   1,       2048) /* ItemType - Gem */
     , (480484,   5,         10) /* EncumbranceVal */
     , (480484,  11,          1) /* MaxStackSize */
     , (480484,  12,          1) /* StackSize */
     , (480484,  13,         10) /* StackUnitEncumbrance */
     , (480484,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (480484,  18,          1) /* UiEffects - Magical */
     , (480484,  19,         100) /* Value */
     , (480484,  65,        101) /* Placement - Resting */
     , (480484,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480484,  94,          6) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480484,   1, False) /* Stuck */
     , (480484,  11, True ) /* IgnoreCollisions */
     , (480484,  13, True ) /* Ethereal */
     , (480484,  14, True ) /* GravityStatus */
     , (480484,  19, True ) /* Attackable */
     , (480484,  22, True ) /* Inscribable */
     , (480484,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480484,   1, 'Missile Defense Requirement Morph Gem') /* Name */
     , (480484,  14, 'Applying this gem to loot type items will remove the missile defense activation requirement.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480484,   1, 0x02000179) /* Setup */
     , (480484,   3, 0x20000014) /* SoundTable */
     , (480484,   6, 0x04000BEF) /* PaletteBase */
     , (480484,   7, 0x1000010B) /* ClothingBase */
     , (480484,   8, 0x06002971) /* Icon */
     , (480484,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480484,  50,  100669126) /* IconOverlay */
     , (480484,  52, 0x060065FB) /* IconUnderlay */;

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
