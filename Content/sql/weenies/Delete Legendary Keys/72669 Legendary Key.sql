DELETE FROM `weenie` WHERE `class_Id` = 72669;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (72669, 'ace72669-legendarykey', 22, '2022-12-28 05:57:21') /* Key */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (72669,   1,      16384) /* ItemType - Key */
     , (72669,   5,         30) /* EncumbranceVal */
     , (72669,  16,    2097160) /* ItemUseable - SourceContainedTargetRemote */
     , (72669,  18,         64) /* UiEffects - Lightning */
     , (72669,  19,      10000) /* Value */
     , (72669,  33,          1) /* Bonded - Bonded */
     , (72669,  91,          1) /* MaxStructure */
     , (72669,  92,          1) /* Structure */
     , (72669,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (72669,  94,        640) /* TargetType - LockableMagicTarget */
     , (72669, 267,      86400) /* Lifespan */
     , (72669, 369,        150) /* UseRequiresLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (72669,   1, False) /* Stuck */
     , (72669,  11, True ) /* IgnoreCollisions */
     , (72669,  13, True ) /* Ethereal */
     , (72669,  14, True ) /* GravityStatus */
     , (72669,  19, True ) /* Attackable */
     , (72669,  22, True ) /* Inscribable */
     , (72669,  69, False) /* IsSellable */
     , (72669,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (72669,   1, 'Legendary Key') /* Name */
     , (72669,  13, 'legarmormagicweaponchest') /* KeyCode */
     , (72669,  14, 'Use this key to open a Legendary Armor, Magic, or Weapon Chest.') /* Use */
     , (72669,  16, 'A key only heard about in whispers and myths.') /* LongDesc */
     , (72669,  33, 'NinjaAcademyKeyPickup') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (72669,   1, 0x02000160) /* Setup */
     , (72669,   3, 0x20000014) /* SoundTable */
     , (72669,   8, 0x06007409) /* Icon */
     , (72669,  22, 0x3400002B) /* PhysicsEffectTable */;
