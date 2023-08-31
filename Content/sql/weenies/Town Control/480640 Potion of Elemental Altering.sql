DELETE FROM `weenie` WHERE `class_Id` = 480640;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480640, 'ace480640-Slayerupgrademorphgem', 38, '2022-01-29 01:15:03') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480640,   1,       2048) /* ItemType - Gem */
     , (480640,   5,         10) /* EncumbranceVal */
     , (480640,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (480640,  18,          1) /* UiEffects - Magical */
     , (480640,  19,         100) /* Value */
     , (480640,  65,        101) /* Placement - Resting */
     , (480640,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480640,  94,          33025) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480640,   1, False) /* Stuck */
     , (480640,  11, True ) /* IgnoreCollisions */
     , (480640,  13, True ) /* Ethereal */
     , (480640,  14, True ) /* GravityStatus */
     , (480640,  19, True ) /* Attackable */
     , (480640,  22, True ) /* Inscribable */
     , (480640,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480640,   1, 'Potion of Elemental Altering') /* Name */
     , (480640,  14, 'Applying this potion to a loot generated weapon may alter the damage type to one of the elements. It will not change it to a physical damage type.  It will not alter any imbues already on the item.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480640,   1, 0x02000179) /* Setup */
     , (480640,   3, 0x20000014) /* SoundTable */
     , (480640,   6, 0x04000BEF) /* PaletteBase */
     , (480640,   7, 0x1000010B) /* ClothingBase */
     , (480640,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480640,  50, 0x06002001) /* IconOverlay */
     , (480640,  52, 0x060065FB) /* IconUnderlay */;

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
