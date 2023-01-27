DELETE FROM `weenie` WHERE `class_Id` = 2032270;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (2032270, 'ace2032270-guinness', 18, '2021-12-28 03:16:53') /* Food */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (2032270,   1,         32) /* ItemType - Food */
     , (2032270,   5,         50) /* EncumbranceVal */
     , (2032270,  11,        100) /* MaxStackSize */
     , (2032270,  12,          1) /* StackSize */
     , (2032270,  13,         50) /* StackUnitEncumbrance */
     , (2032270,  15,         10) /* StackUnitValue */
     , (2032270,  16,          8) /* ItemUseable - Contained */
     , (2032270,  18,          1) /* UiEffects - Magical */
     , (2032270,  19,         10) /* Value */
     , (2032270,  33,          1) /* Bonded - Bonded */
     , (2032270,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (2032270, 106,        250) /* ItemSpellcraft */
     , (2032270, 107,         60) /* ItemCurMana */
     , (2032270, 108,         60) /* ItemMaxMana */
     , (2032270, 109,          0) /* ItemDifficulty */
     , (2032270, 114,          1) /* Attuned - Attuned */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (2032270,  11, True ) /* IgnoreCollisions */
     , (2032270,  13, True ) /* Ethereal */
     , (2032270,  14, True ) /* GravityStatus */
     , (2032270,  19, True ) /* Attackable */
     , (2032270,  22, True ) /* Inscribable */
     , (2032270,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (2032270,   1, 'Guinness') /* Name */
     , (2032270,  14, 'Freshly Brewed in Ireland, use this item to drink it.') /* Use */
     , (2032270,  16, 'A bottle of Guinness.') /* LongDesc */
     , (2032270,  20, 'Bottles of Guinness') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (2032270,   1, 0x02001258) /* Setup */
     , (2032270,   3, 0x20000014) /* SoundTable */
     , (2032270,   8, 0x06006273) /* Icon */
     , (2032270,  22, 0x3400002B) /* PhysicsEffectTable */
     , (2032270,  23,         65) /* UseSound - Drink1 */
     , (2032270,  28,       3864) /* Spell - Zongo's Fist */
     , (2032270,  50, 0x06005EC2) /* IconOverlay */
     , (2032270,  52, 0x06005EBB) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2021-12-28T01:45:49.9512218Z",
  "ModifiedBy": "ACE.Adapter",
  "Changelog": [
    {
      "created": "2021-12-27T04:49:17.5064566Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    },
    {
      "created": "2021-12-28T01:45:49.9424571Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "UserChangeSummary": "Weenie exported from ACEmulator world database using ACE.Adapter",
  "IsDone": false
}
*/
