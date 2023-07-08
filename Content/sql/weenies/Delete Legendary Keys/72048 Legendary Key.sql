DELETE FROM `weenie` WHERE `class_Id` = 72048;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (72048, 'ace72048-legendarykey', 22, '2021-11-01 00:00:00') /* Key */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (72048,   1,      16384) /* ItemType - Key */
     , (72048,   5,         30) /* EncumbranceVal */
     , (72048,  16,    2097160) /* ItemUseable - SourceContainedTargetRemote */
     , (72048,  18,         64) /* UiEffects - Lightning */
     , (72048,  19,      10000) /* Value */
     , (72048,  33,          0) /* Bonded - Normal */
     , (72048,  91,          1) /* MaxStructure */
     , (72048,  92,          1) /* Structure */
     , (72048,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (72048,  94,        640) /* TargetType - LockableMagicTarget */
     , (72048, 114,          0) /* Attuned - Normal */
     , (72048, 267,      86400) /* Lifespan */
     , (72048, 369,        150) /* UseRequiresLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (72048,  22, True ) /* Inscribable */
     , (72048,  69, False) /* IsSellable */
     , (72048,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (72048,   1, 'Legendary Key') /* Name */
     , (72048,  13, 'legarmormagicweaponchest') /* KeyCode */
     , (72048,  14, 'Use this key to open a Legendary Armor, Magic, or Weapon Chest.') /* Use */
     , (72048,  16, 'A key only heard about in whispers and myths.') /* LongDesc */
     , (72048,  33, 'EODKeyPickup_1013') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (72048,   1, 0x02000160) /* Setup */
     , (72048,   3, 0x20000014) /* SoundTable */
     , (72048,   8, 0x06007409) /* Icon */
     , (72048,  22, 0x3400002B) /* PhysicsEffectTable */;
