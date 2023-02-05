DELETE FROM `weenie` WHERE `class_Id` = 2032271;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (2032271, 'ace2032271-heineken', 18, '2021-12-28 03:16:55') /* Food */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (2032271,   1,         32) /* ItemType - Food */
     , (2032271,   5,         50) /* EncumbranceVal */
     , (2032271,  11,        100) /* MaxStackSize */
     , (2032271,  12,          1) /* StackSize */
     , (2032271,  13,         50) /* StackUnitEncumbrance */
     , (2032271,  15,         10) /* StackUnitValue */
     , (2032271,  16,          8) /* ItemUseable - Contained */
     , (2032271,  18,          1) /* UiEffects - Magical */
     , (2032271,  19,         10) /* Value */
     , (2032271,  33,          1) /* Bonded - Bonded */
     , (2032271,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (2032271, 106,        250) /* ItemSpellcraft */
     , (2032271, 107,         60) /* ItemCurMana */
     , (2032271, 108,         60) /* ItemMaxMana */
     , (2032271, 109,          0) /* ItemDifficulty */
     , (2032271, 114,          1) /* Attuned - Attuned */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (2032271,  11, True ) /* IgnoreCollisions */
     , (2032271,  13, True ) /* Ethereal */
     , (2032271,  14, True ) /* GravityStatus */
     , (2032271,  19, True ) /* Attackable */
     , (2032271,  22, True ) /* Inscribable */
     , (2032271,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (2032271,   1, 'Heineken') /* Name */
     , (2032271,  14, 'Freshly Brewed in The Netherlands, use this item to drink it.') /* Use */
     , (2032271,  16, 'A bottle of Heineken.') /* LongDesc */
     , (2032271,  20, 'Bottles of Heineken') /* PluralName */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (2032271,   1, 0x02001258) /* Setup */
     , (2032271,   3, 0x20000014) /* SoundTable */
     , (2032271,   8, 0x06006273) /* Icon */
     , (2032271,  22, 0x3400002B) /* PhysicsEffectTable */
     , (2032271,  23,         65) /* UseSound - Drink1 */
     , (2032271,  28,       3862) /* Spell - Duke Raoul's Pride */
     , (2032271,  50, 0x06005EC2) /* IconOverlay */
     , (2032271,  52, 0x06005EBB) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "Changelog": [
    {
      "created": "2021-12-28T01:46:07.635292Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "IsDone": false
}
*/
