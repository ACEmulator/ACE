DELETE FROM `weenie` WHERE `class_Id` = 490026;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490026, 'ace490026-Hiltgem', 38, '2022-01-29 01:15:03') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490026,   1,       2048) /* ItemType - Gem */
     , (490026,   5,         10) /* EncumbranceVal */
     , (490026,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (490026,  18,          1) /* UiEffects - Magical */
     , (490026,  19,         250) /* Value */
     , (490026,  65,        101) /* Placement - Resting */
     , (490026,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490026,  94,          33025) /* TargetType - Vestements */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490026,   1, False) /* Stuck */
     , (490026,  11, True ) /* IgnoreCollisions */
     , (490026,  13, True ) /* Ethereal */
     , (490026,  14, True ) /* GravityStatus */
     , (490026,  19, True ) /* Attackable */
     , (490026,  22, True ) /* Inscribable */
     , (490026,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490026,   1, 'Blade Hilt') /* Name */
     , (490026,  15, 'A blade hilt.') /* ShortDesc */
     , (490026,  16, 'A well-balanced blade hilt. Affix this hilt to a loot-generated dagger or light sword to give the weapon multi-strike capability.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490026,   1, 0x02000179) /* Setup */
     , (490026,   3, 0x20000014) /* SoundTable */
     , (490026,   6, 0x04000BEF) /* PaletteBase */
     , (490026,   7, 0x1000010B) /* ClothingBase */
     , (490026,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490026,  50, 0x06001F5F) /* IconOverlay */
     , (490026,  52, 0x060065FB) /* IconUnderlay */;

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
