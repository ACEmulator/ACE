DELETE FROM `weenie` WHERE `class_Id` = 490025;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490025, 'ace490025-Impenatrabilitymorphgem', 38, '2022-01-29 01:15:03') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490025,   1,       2048) /* ItemType - Gem */
     , (490025,   5,         10) /* EncumbranceVal */
     , (490025,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (490025,  18,          1) /* UiEffects - Magical */
     , (490025,  19,         10) /* Value */
     , (490025,  65,        101) /* Placement - Resting */
     , (490025,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490025,  94,          35215) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490025,   1, False) /* Stuck */
     , (490025,  11, True ) /* IgnoreCollisions */
     , (490025,  13, True ) /* Ethereal */
     , (490025,  14, True ) /* GravityStatus */
     , (490025,  19, True ) /* Attackable */
     , (490025,  22, True ) /* Inscribable */
     , (490025,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490025,   1, 'Impenatrability Morph Gem') /* Name */
     , (490025,  14, 'Applying this gem to loot generated armor has a 33% chance to add an additional spell, either Minor Impenatrability, Major Impenatrability or Epic Impenatrability. If this fails to land, the target will be destroyed.') /* Use */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490025,   1, 0x02000179) /* Setup */
     , (490025,   3, 0x20000014) /* SoundTable */
     , (490025,   6, 0x04000BEF) /* PaletteBase */
     , (490025,   7, 0x1000010B) /* ClothingBase */
     , (490025,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490025,  50, 100668271) /* IconOverlay */
     , (490025,  52, 100689403) /* IconUnderlay */;

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
