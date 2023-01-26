DELETE FROM `weenie` WHERE `class_Id` = 2029104;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (2029104, 'ace2029104-corona', 18, '2021-12-28 03:16:47') /* Food */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (2029104,   1,         32) /* ItemType - Food */
     , (2029104,   5,         50) /* EncumbranceVal */
     , (2029104,  11,        100) /* MaxStackSize */
     , (2029104,  12,          1) /* StackSize */
     , (2029104,  13,         50) /* StackUnitEncumbrance */
     , (2029104,  15,         10) /* StackUnitValue */
     , (2029104,  16,          8) /* ItemUseable - Contained */
     , (2029104,  18,          1) /* UiEffects - Magical */
     , (2029104,  19,         10) /* Value */
     , (2029104,  33,          1) /* Bonded - Bonded */
     , (2029104,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (2029104, 106,        250) /* ItemSpellcraft */
     , (2029104, 107,         60) /* ItemCurMana */
     , (2029104, 108,         60) /* ItemMaxMana */
     , (2029104, 109,          0) /* ItemDifficulty */
     , (2029104, 114,          1) /* Attuned - Attuned */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (2029104,  11, True ) /* IgnoreCollisions */
     , (2029104,  13, True ) /* Ethereal */
     , (2029104,  14, True ) /* GravityStatus */
     , (2029104,  19, True ) /* Attackable */
     , (2029104,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (2029104,   1, 'Corona') /* Name */
     , (2029104,  14, 'Freshly Brewed in Mexico, use this item to drink it.') /* Use */
     , (2029104,  16, 'A bottle of Corona.') /* LongDesc */
     , (2029104,  20, 'Bottles of Corona') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (2029104,   1, 0x02001258) /* Setup */
     , (2029104,   3, 0x20000014) /* SoundTable */
     , (2029104,   8, 0x06005A65) /* Icon */
     , (2029104,  22, 0x3400002B) /* PhysicsEffectTable */
     , (2029104,  23,         65) /* UseSound - Drink1 */
     , (2029104,  28,       3533) /* Spell - Brighteyes' Favor */
     , (2029104,  50, 0x06005EC2) /* IconOverlay */
     , (2029104,  52, 0x06005EBB) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "Changelog": [
    {
      "created": "2021-12-28T01:45:26.2770041Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "IsDone": false
}
*/
