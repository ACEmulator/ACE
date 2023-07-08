DELETE FROM `weenie` WHERE `class_Id` = 52010;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (52010, 'ace52010-legendarykey', 22, '2021-11-01 00:00:00') /* Key */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (52010,   1,      16384) /* ItemType - Key */
     , (52010,   5,         30) /* EncumbranceVal */
     , (52010,  16,    2097160) /* ItemUseable - SourceContainedTargetRemote */
     , (52010,  18,         64) /* UiEffects - Lightning */
     , (52010,  19,      10000) /* Value */
     , (52010,  33,          0) /* Bonded - Normal */
     , (52010,  91,          5) /* MaxStructure */
     , (52010,  92,          5) /* Structure */
     , (52010,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (52010,  94,        640) /* TargetType - LockableMagicTarget */
     , (52010, 114,          0) /* Attuned - Normal */
     , (52010, 267,      86400) /* Lifespan */
     , (52010, 369,        150) /* UseRequiresLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (52010,  22, True ) /* Inscribable */
     , (52010,  69, False) /* IsSellable */
     , (52010,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (52010,   1, 'Legendary Key') /* Name */
     , (52010,  13, 'legarmormagicweaponchest') /* KeyCode */
     , (52010,  14, 'Use this key to open a Legendary Armor, Magic, or Weapon Chest.') /* Use */
     , (52010,  16, 'A key only heard about in whispers and myths.') /* LongDesc */
     , (52010,  33, 'RTWKeyPickup_1013') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (52010,   1, 0x02000160) /* Setup */
     , (52010,   3, 0x20000014) /* SoundTable */
     , (52010,   8, 0x06007409) /* Icon */
     , (52010,  22, 0x3400002B) /* PhysicsEffectTable */;
