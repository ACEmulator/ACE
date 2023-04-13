DELETE FROM `weenie` WHERE `class_Id` = 51954;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (51954, 'ace51954-durablelegendarykey', 22, '2021-11-01 00:00:00') /* Key */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (51954,   1,      16384) /* ItemType - Key */
     , (51954,   5,         30) /* EncumbranceVal */
     , (51954,  16,    2097160) /* ItemUseable - SourceContainedTargetRemote */
     , (51954,  18,         64) /* UiEffects - Lightning */
     , (51954,  19,     100000) /* Value */
     , (51954,  33,          0) /* Bonded - Normal */
     , (51954,  91,         10) /* MaxStructure */
     , (51954,  92,         10) /* Structure */
     , (51954,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (51954,  94,        640) /* TargetType - LockableMagicTarget */
     , (51954, 114,          0) /* Attuned - Normal */
     , (51954, 369,        150) /* UseRequiresLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (51954,  22, True ) /* Inscribable */
     , (51954,  69, False) /* IsSellable */
     , (51954,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (51954,   1, 'Durable Legendary Key') /* Name */
     , (51954,  13, 'keychestleg') /* KeyCode */
     , (51954,  14, 'Use this key to open a Legendary Chest.') /* Use */
     , (51954,  16, 'This key has seen better days.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (51954,   1, 0x02000160) /* Setup */
     , (51954,   3, 0x20000014) /* SoundTable */
     , (51954,   8, 0x06007409) /* Icon */
     , (51954,  22, 0x3400002B) /* PhysicsEffectTable */;
