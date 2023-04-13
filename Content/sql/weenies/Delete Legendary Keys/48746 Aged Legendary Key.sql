DELETE FROM `weenie` WHERE `class_Id` = 48746;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (48746, 'ace48746-agedlegendarykey', 22, '2021-11-01 00:00:00') /* Key */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (48746,   1,      16384) /* ItemType - Key */
     , (48746,   5,         30) /* EncumbranceVal */
     , (48746,  16,    2097160) /* ItemUseable - SourceContainedTargetRemote */
     , (48746,  18,         64) /* UiEffects - Lightning */
     , (48746,  19,      10000) /* Value */
     , (48746,  33,          0) /* Bonded - Normal */
     , (48746,  91,          1) /* MaxStructure */
     , (48746,  92,          1) /* Structure */
     , (48746,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (48746,  94,        640) /* TargetType - LockableMagicTarget */
     , (48746, 114,          0) /* Attuned - Normal */
     , (48746, 369,        150) /* UseRequiresLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (48746,  22, True ) /* Inscribable */
     , (48746,  69, False) /* IsSellable */
     , (48746,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (48746,   1, 'Aged Legendary Key') /* Name */
     , (48746,  13, 'keychestleg') /* KeyCode */
     , (48746,  14, 'Use this key to open a Legendary Chest.') /* Use */
     , (48746,  16, 'This key has seen better days.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (48746,   1, 0x02000160) /* Setup */
     , (48746,   3, 0x20000014) /* SoundTable */
     , (48746,   8, 0x06007409) /* Icon */
     , (48746,  22, 0x3400002B) /* PhysicsEffectTable */;
