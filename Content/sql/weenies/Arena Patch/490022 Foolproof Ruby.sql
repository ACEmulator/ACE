DELETE FROM `weenie` WHERE `class_Id` = 490022;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490022, 'ace490022-rubymorphgem', 38, '2022-01-29 01:15:03') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490022,   1,       2048) /* ItemType - Gem */
     , (490022,   5,         10) /* EncumbranceVal */
     , (490022,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (490022,  18,          1) /* UiEffects - Magical */
     , (490022,  19,         20) /* Value */
     , (490022,  65,        101) /* Placement - Resting */
     , (490022,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490022,  94,          8) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490022,   1, False) /* Stuck */
     , (490022,  11, True ) /* IgnoreCollisions */
     , (490022,  13, True ) /* Ethereal */
     , (490022,  14, True ) /* GravityStatus */
     , (490022,  19, True ) /* Attackable */
     , (490022,  22, True ) /* Inscribable */
     , (490022,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490022,   1, 'Foolproof Ruby') /* Name */
     , (490022,  14, 'Apply this bag of salvage to a trinket for a 100% chance to land.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490022,   1, 0x02000181) /* Setup */
     , (490022,   3, 0x20000014) /* SoundTable */
     , (490022,   6, 0x04000BEF) /* PaletteBase */
     , (490022,   7, 0x1000058A) /* ClothingBase */
     , (490022,   8, 0x060026C6) /* Icon */
     , (490022,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490022,  50, 0x0600270F) /* IconOverlay */
     , (490022,  52, 100689403) /* IconUnderlay */;

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
