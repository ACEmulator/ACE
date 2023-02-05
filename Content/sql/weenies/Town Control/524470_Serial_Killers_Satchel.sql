DELETE FROM `weenie` WHERE `class_Id` = 524470;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (524470, 'ace524470-serialkillerssatchel', 21, '2021-12-24 02:37:21') /* Container */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (524470,   1,        512) /* ItemType - Container */
     , (524470,   5,         15) /* EncumbranceVal */
     , (524470,   6,         36) /* ItemsCapacity */
     , (524470,  16,         56) /* ItemUseable - ContainedViewedRemote */
     , (524470,  18,          8) /* UiEffects - BoostMana */
     , (524470,  19,        250) /* Value */
     , (524470,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (524470,   2, False) /* Open */
     , (524470,  22, True ) /* Inscribable */
     , (524470,  34, False) /* DefaultOpen */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (524470,  39,     1.2) /* DefaultScale */
     , (524470,  54,    0.25) /* UseRadius */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (524470,   1, 'Serial Killer''s Satchel') /* Name */
     , (524470,  14, 'Use this item to close it.') /* Use */
     , (524470,  16, 'A satchel made from the skin of your enemy''s. You find the skin stretches nicely to be able to carry a few more items.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (524470,   1, 0x02000181) /* Setup */
     , (524470,   3, 0x20000014) /* SoundTable */
     , (524470,   6, 0x04000BEF) /* PaletteBase */
     , (524470,   7, 0x10000178) /* ClothingBase */
     , (524470,   8, 0x06002B1D) /* Icon */
     , (524470,  22, 0x3400002B) /* PhysicsEffectTable */
     , (524470,  52, 0x06006E89) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2021-12-10T20:15:10.0871277Z",
  "ModifiedBy": "ACE.Adapter",
  "Changelog": [
    {
      "created": "2021-12-10T19:38:28.5691253Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    },
    {
      "created": "2021-12-10T19:45:28.7251761Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    },
    {
      "created": "2021-12-10T20:06:04.1840798Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    },
    {
      "created": "2021-12-10T20:07:53.4173434Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    },
    {
      "created": "2021-12-10T20:08:38.8308668Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    },
    {
      "created": "2021-12-10T20:09:21.6774308Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    },
    {
      "created": "2021-12-10T20:15:10.0864722Z",
      "author": "ACE.Adapter",
      "comment": "Weenie exported from ACEmulator world database using ACE.Adapter"
    }
  ],
  "UserChangeSummary": "Weenie exported from ACEmulator world database using ACE.Adapter",
  "IsDone": false
}
*/
