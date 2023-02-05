DELETE FROM `weenie` WHERE `class_Id` = 4200026;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200026, 'ace4200026-arcaneloremorphgem', 38, '2022-01-29 01:15:03') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200026,   1,       2048) /* ItemType - Gem */
     , (4200026,   5,         10) /* EncumbranceVal */
     , (4200026,  11,          1) /* MaxStackSize */
     , (4200026,  12,          1) /* StackSize */
     , (4200026,  13,         10) /* StackUnitEncumbrance */
     , (4200026,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (4200026,  18,          1) /* UiEffects - Magical */
     , (4200026,  19,         20) /* Value */
     , (4200026,  65,        101) /* Placement - Resting */
     , (4200026,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200026,  94,          6) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200026,   1, False) /* Stuck */
     , (4200026,  11, True ) /* IgnoreCollisions */
     , (4200026,  13, True ) /* Ethereal */
     , (4200026,  14, True ) /* GravityStatus */
     , (4200026,  19, True ) /* Attackable */
     , (4200026,  22, True ) /* Inscribable */
     , (4200026,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200026,   1, 'Arcane Lore Morph Gem') /* Name */
     , (4200026,  14, 'Applying this gem to loot type items has a chance to decrease its Arcane Lore...or increase it.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200026,   1, 0x02000179) /* Setup */
     , (4200026,   3, 0x20000014) /* SoundTable */
     , (4200026,   6, 0x04000BEF) /* PaletteBase */
     , (4200026,   7, 0x1000010B) /* ClothingBase */
     , (4200026,   8, 0x06002971) /* Icon */
     , (4200026,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200026,  50,  100668285) /* IconOverlay */
     , (4200026,  52, 0x060065FB) /* IconUnderlay */;

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
