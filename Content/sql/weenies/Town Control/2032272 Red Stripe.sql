DELETE FROM `weenie` WHERE `class_Id` = 2032272;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (2032272, 'ace2032272-redstripe', 18, '2021-12-28 03:16:57') /* Food */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (2032272,   1,         32) /* ItemType - Food */
     , (2032272,   5,         50) /* EncumbranceVal */
     , (2032272,  11,        100) /* MaxStackSize */
     , (2032272,  12,          1) /* StackSize */
     , (2032272,  13,         50) /* StackUnitEncumbrance */
     , (2032272,  15,         10) /* StackUnitValue */
     , (2032272,  16,          8) /* ItemUseable - Contained */
     , (2032272,  18,          1) /* UiEffects - Magical */
     , (2032272,  19,         10) /* Value */
     , (2032272,  33,          1) /* Bonded - Bonded */
     , (2032272,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (2032272, 106,        250) /* ItemSpellcraft */
     , (2032272, 107,         60) /* ItemCurMana */
     , (2032272, 108,         60) /* ItemMaxMana */
     , (2032272, 109,          0) /* ItemDifficulty */
     , (2032272, 114,          1) /* Attuned - Attuned */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (2032272,  11, True ) /* IgnoreCollisions */
     , (2032272,  13, True ) /* Ethereal */
     , (2032272,  14, True ) /* GravityStatus */
     , (2032272,  19, True ) /* Attackable */
     , (2032272,  22, True ) /* Inscribable */
     , (2032272,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (2032272,   1, 'Red Stripe') /* Name */
     , (2032272,  14, 'Freshly Brewed in Jamaica, use this item to drink it.') /* Use */
     , (2032272,  16, 'A bottle of Red Stripe.') /* LongDesc */
     , (2032272,  20, 'Bottles of Red Stripe') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (2032272,   1, 0x02001258) /* Setup */
     , (2032272,   3, 0x20000014) /* SoundTable */
     , (2032272,   8, 0x06006273) /* Icon */
     , (2032272,  22, 0x3400002B) /* PhysicsEffectTable */
     , (2032272,  23,         65) /* UseSound - Drink1 */
     , (2032272,  28,       3863) /* Spell - Hunter's Hardiness */
     , (2032272,  50, 0x06005EC2) /* IconOverlay */
     , (2032272,  52, 0x06005EBB) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "Changelog": [
    {
      "created": "2021-12-28T01:46:27.2226862Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "IsDone": false
}
*/
