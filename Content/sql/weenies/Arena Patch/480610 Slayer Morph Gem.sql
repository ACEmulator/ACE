DELETE FROM `weenie` WHERE `class_Id` = 480610;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480610, 'ace480610-Slayerrandomizermorphgem', 38, '2022-01-29 01:15:03') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480610,   1,       2048) /* ItemType - Gem */
     , (480610,   5,         10) /* EncumbranceVal */
     , (480610,  11,          1) /* MaxStackSize */
     , (480610,  12,          1) /* StackSize */
     , (480610,  13,         10) /* StackUnitEncumbrance */
     , (480610,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (480610,  18,          1) /* UiEffects - Magical */
     , (480610,  19,         15) /* Value */
     , (480610,  65,        101) /* Placement - Resting */
     , (480610,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480610,  94,          33025) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480610,   1, False) /* Stuck */
     , (480610,  11, True ) /* IgnoreCollisions */
     , (480610,  13, True ) /* Ethereal */
     , (480610,  14, True ) /* GravityStatus */
     , (480610,  19, True ) /* Attackable */
     , (480610,  22, True ) /* Inscribable */
     , (480610,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480610,   1, 'Slayer Morph Gem') /* Name */
     , (480610,  14, 'Applying this gem to creature slaying loot generated weapons may alter the current creature slaying property of the weapon.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480610,   1, 0x02000179) /* Setup */
     , (480610,   3, 0x20000014) /* SoundTable */
     , (480610,   6, 0x04000BEF) /* PaletteBase */
     , (480610,   7, 0x1000010B) /* ClothingBase */
     , (480610,   8, 0x06002971) /* Icon */
     , (480610,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480610,  50, 0x06007551) /* IconOverlay */
     , (480610,  52, 0x060065FB) /* IconUnderlay */;

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
