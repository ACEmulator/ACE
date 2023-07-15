DELETE FROM `weenie` WHERE `class_Id` = 480483;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480483, 'ace480483-meleedmorphgem', 38, '2022-01-29 01:15:03') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480483,   1,       2048) /* ItemType - Gem */
     , (480483,   5,         10) /* EncumbranceVal */
     , (480483,  11,          1) /* MaxStackSize */
     , (480483,  12,          1) /* StackSize */
     , (480483,  13,         10) /* StackUnitEncumbrance */
     , (480483,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (480483,  18,          1) /* UiEffects - Magical */
     , (480483,  19,         100) /* Value */
     , (480483,  65,        101) /* Placement - Resting */
     , (480483,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480483,  94,          6) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480483,   1, False) /* Stuck */
     , (480483,  11, True ) /* IgnoreCollisions */
     , (480483,  13, True ) /* Ethereal */
     , (480483,  14, True ) /* GravityStatus */
     , (480483,  19, True ) /* Attackable */
     , (480483,  22, True ) /* Inscribable */
     , (480483,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480483,   1, 'Melee Defense Requirement Morph Gem') /* Name */
     , (480483,  14, 'Applying this gem to loot type items will remove the melee defense activation requirement.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480483,   1, 0x02000179) /* Setup */
     , (480483,   3, 0x20000014) /* SoundTable */
     , (480483,   6, 0x04000BEF) /* PaletteBase */
     , (480483,   7, 0x1000010B) /* ClothingBase */
     , (480483,   8, 0x06002971) /* Icon */
     , (480483,  22, 0x3400002B) /* PhysicsEffectTable */
     , (480483,  50,  100668331) /* IconOverlay */
     , (480483,  52, 0x060065FB) /* IconUnderlay */;

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
