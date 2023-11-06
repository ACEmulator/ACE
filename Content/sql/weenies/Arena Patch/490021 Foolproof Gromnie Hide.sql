DELETE FROM `weenie` WHERE `class_Id` = 490021;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490021, 'ace490021-gromniehidemorphgem', 38, '2022-01-29 01:15:03') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490021,   1,       2048) /* ItemType - Gem */
     , (490021,   5,         10) /* EncumbranceVal */
     , (490021,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (490021,  18,          1) /* UiEffects - Magical */
     , (490021,  19,         20) /* Value */
     , (490021,  65,        101) /* Placement - Resting */
     , (490021,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490021,  94,          8) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490021,   1, False) /* Stuck */
     , (490021,  11, True ) /* IgnoreCollisions */
     , (490021,  13, True ) /* Ethereal */
     , (490021,  14, True ) /* GravityStatus */
     , (490021,  19, True ) /* Attackable */
     , (490021,  22, True ) /* Inscribable */
     , (490021,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490021,   1, 'Foolproof Gromnie Hide') /* Name */
     , (490021,  14, 'Apply this bag of salvage to a trinket for a 100% chance to land.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490021,   1, 0x02000181) /* Setup */
     , (490021,   3, 0x20000014) /* SoundTable */
     , (490021,   6, 0x04000BEF) /* PaletteBase */
     , (490021,   7, 0x1000058A) /* ClothingBase */
     , (490021,   8, 0x060026C6) /* Icon */
     , (490021,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490021,  50, 0x060026FC) /* IconOverlay */
     , (490021,  52, 100689403) /* IconUnderlay */;

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
