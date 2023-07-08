DELETE FROM `weenie` WHERE `class_Id` = 44470;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (44470, 'ace44470-corruptedessence', 51, '2021-11-01 00:00:00') /* Stackable */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (44470,   1,        128) /* ItemType - Misc */
     , (44470,   5,         50) /* EncumbranceVal */
     , (44470,  11,         100) /* MaxStackSize */
     , (44470,  12,          1) /* StackSize */
     , (44470,  13,         50) /* StackUnitEncumbrance */
     , (44470,  15,          0) /* StackUnitValue */
     , (44470,  16,          1) /* ItemUseable - No */
     , (44470,  18,          8) /* UiEffects - BoostMana */
     , (44470,  19,          0) /* Value */
     , (44470,  33,          0) /* Bonded - Normal */
     , (44470,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (44470, 114,          0) /* Attuned - Normal */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (44470,   1, 'Corrupted Essence') /* Name */
     , (44470,  16, 'A corrupted essence from a Dark Isle creature.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (44470,   1, 0x02000181) /* Setup */
     , (44470,   3, 0x20000014) /* SoundTable */
     , (44470,   8, 0x0600704A) /* Icon */
     , (44470,  22, 0x3400002B) /* PhysicsEffectTable */;
