DELETE FROM `weenie` WHERE `class_Id` = 48747;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (48747, 'ace48747-legendarykey', 22, '2021-11-01 00:00:00') /* Key */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (48747,   1,      16384) /* ItemType - Key */
     , (48747,   5,         30) /* EncumbranceVal */
     , (48747,  16,    2097160) /* ItemUseable - SourceContainedTargetRemote */
     , (48747,  18,         64) /* UiEffects - Lightning */
     , (48747,  19,      10000) /* Value */
     , (48747,  33,          0) /* Bonded - Normal */
     , (48747,  91,          1) /* MaxStructure */
     , (48747,  92,          1) /* Structure */
     , (48747,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (48747,  94,        640) /* TargetType - LockableMagicTarget */
     , (48747, 114,          0) /* Attuned - Normal */
     , (48747, 267,      86400) /* Lifespan */
     , (48747, 369,        150) /* UseRequiresLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (48747,  22, True ) /* Inscribable */
     , (48747,  69, False) /* IsSellable */
     , (48747,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (48747,   1, 'Legendary Key') /* Name */
     , (48747,  13, 'legarmormagicweaponchest') /* KeyCode */
     , (48747,  14, 'Use this key to open a Legendary Armor, Magic, or Weapon Chest.') /* Use */
     , (48747,  16, 'A key only heard about in whispers and myths.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (48747,   1, 0x02000160) /* Setup */
     , (48747,   3, 0x20000014) /* SoundTable */
     , (48747,   8, 0x06007409) /* Icon */
     , (48747,  22, 0x3400002B) /* PhysicsEffectTable */;
