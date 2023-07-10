DELETE FROM `weenie` WHERE `class_Id` = 480608;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480608, 'ace480608-darkbeatsloststoragekey', 22, '2021-11-01 00:00:00') /* Key */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480608,   1,      16384) /* ItemType - Key */
     , (480608,   5,        500) /* EncumbranceVal */
	 , (480608,   8,          5) /* Mass */
     , (480608,  16,    2097160) /* ItemUseable - SourceContainedTargetRemote */
     , (480608,  19,          0) /* Value */
	 , (480608,  33,         -1) /* Bonded - Slippery */
     , (480608,  91,          1) /* MaxStructure */
     , (480608,  92,          1) /* Structure */
     , (480608,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480608,  94,        640) /* TargetType - LockableMagicTarget */
	 , (480608, 369,        150) /* UseRequiresLevel */
	 , (480608, 267,     14400) /* Lifespan */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480608,  22, True ) /* Inscribable */
     , (480608,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480608,  39,       3) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480608,   1, 'Darkbeat''s Lost Storage Key') /* Name */
     , (480608,  13, 'darkbeatkey') /* KeyCode */
     , (480608,  16, 'A finely crafted key made from human bone. As you look closely at the key you see the name Darkbeat engraved into its surface.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480608,   1, 0x02000160) /* Setup */
     , (480608,   3, 0x20000014) /* SoundTable */
     , (480608,   8, 0x060065FE) /* Icon */
     , (480608,  22, 0x3400002B) /* PhysicsEffectTable */
	 , (480608,  52,  100689403) /* IconUnderlay */;
