DELETE FROM `weenie` WHERE `class_Id` = 490023;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490023, 'ace490023-diamondmorphgem', 38, '2022-01-29 01:15:03') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490023,   1,       2048) /* ItemType - Gem */
     , (490023,   5,         10) /* EncumbranceVal */
     , (490023,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (490023,  18,          1) /* UiEffects - Magical */
     , (490023,  19,         20) /* Value */
     , (490023,  65,        101) /* Placement - Resting */
     , (490023,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490023,  94,          8) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490023,   1, False) /* Stuck */
     , (490023,  11, True ) /* IgnoreCollisions */
     , (490023,  13, True ) /* Ethereal */
     , (490023,  14, True ) /* GravityStatus */
     , (490023,  19, True ) /* Attackable */
     , (490023,  22, True ) /* Inscribable */
     , (490023,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490023,   1, 'Foolproof Diamond') /* Name */
     , (490023,  14, 'Apply this bag of salvage to a trinket for a 100% chance to land.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490023,   1, 0x02000181) /* Setup */
     , (490023,   3, 0x20000014) /* SoundTable */
     , (490023,   6, 0x04000BEF) /* PaletteBase */
     , (490023,   7, 0x1000058A) /* ClothingBase */
     , (490023,   8, 0x060026C6) /* Icon */
     , (490023,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490023,  50, 0x060026F6) /* IconOverlay */
     , (490023,  52, 100689403) /* IconUnderlay */;

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
