DELETE FROM `weenie` WHERE `class_Id` = 480639;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480639, 'ace480639-Slayerupgrademorphgem', 38, '2022-01-29 01:15:03') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480639,   1,       2048) /* ItemType - Gem */
     , (480639,   5,         10) /* EncumbranceVal */
     , (480639,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (480639,  18,          1) /* UiEffects - Magical */
     , (480639,  19,         100) /* Value */
     , (480639,  65,        101) /* Placement - Resting */
     , (480639,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480639,  94,          33025) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480639,   1, False) /* Stuck */
     , (480639,  11, True ) /* IgnoreCollisions */
     , (480639,  13, True ) /* Ethereal */
     , (480639,  14, True ) /* GravityStatus */
     , (480639,  19, True ) /* Attackable */
     , (480639,  22, True ) /* Inscribable */
     , (480639,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480639,   1, 'Slayer Upgrade Gem') /* Name */
     , (480639,  14, 'Applying this gem to a creature slaying loot generated weapon may significantly improve the effectiveness of creature slaying property currently on the weapon.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480639,   1, 0x02000179) /* Setup */
     , (480639,   3, 0x20000014) /* SoundTable */
     , (480639,   6, 0x04000BEF) /* PaletteBase */
     , (480639,   7, 0x1000010B) /* ClothingBase */
     , (480639,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480639,  50, 0x06001ECE) /* IconOverlay */
     , (480639,  52, 0x060065FB) /* IconUnderlay */;

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
