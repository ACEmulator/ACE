DELETE FROM `weenie` WHERE `class_Id` = 480022;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480022, 'lockpickrareeternalpeerlesspk', 23, '2021-11-01 00:00:00') /* Lockpick */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480022,   1,      16384) /* ItemType - Key */
     , (480022,   5,          5) /* EncumbranceVal */
     , (480022,  16,    2097160) /* ItemUseable - SourceContainedTargetRemote */
     , (480022,  17,        149) /* RareId */
     , (480022,  19,          250) /* Value */
     , (480022,  33,         1) /* Bonded - Slippery */
	 , (480022, 114,          1) /* Attuned - Attuned */
     , (480022,  88,         20) /* LockpickMod */
     , (480022,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480022,  94,        640) /* TargetType - LockableMagicTarget */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480022,  22, True ) /* Inscribable */
     , (480022,  63, True ) /* UnlimitedUse */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480022,  39,       2) /* DefaultScale */
     , (480022,  40,       1) /* LockpickMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480022,   1, 'Limitless Lockpick') /* Name */
     , (480022,  14, 'Use this item on a locked door or chest to pick the lock.') /* Use */
     , (480022,  16, 'This Peerless Lockpick will never run out of uses.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480022,   1, 0x02000166) /* Setup */
     , (480022,   8, 0x06005B71) /* Icon */
     , (480022,  52, 0x06005B0C) /* IconUnderlay */;
