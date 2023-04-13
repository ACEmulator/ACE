DELETE FROM `weenie` WHERE `class_Id` = 72628;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (72628, 'ace72628-legendarykey', 22, '2022-12-28 05:57:21') /* Key */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (72628,   1,      16384) /* ItemType - Key */
     , (72628,   5,         30) /* EncumbranceVal */
     , (72628,  16,    2097160) /* ItemUseable - SourceContainedTargetRemote */
     , (72628,  18,         64) /* UiEffects - Lightning */
     , (72628,  19,      10000) /* Value */
     , (72628,  33,          1) /* Bonded - Bonded */
     , (72628,  91,          1) /* MaxStructure */
     , (72628,  92,          1) /* Structure */
     , (72628,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (72628,  94,        640) /* TargetType - LockableMagicTarget */
     , (72628, 267,      86400) /* Lifespan */
     , (72628, 369,        150) /* UseRequiresLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (72628,   1, False) /* Stuck */
     , (72628,  11, True ) /* IgnoreCollisions */
     , (72628,  13, True ) /* Ethereal */
     , (72628,  14, True ) /* GravityStatus */
     , (72628,  19, True ) /* Attackable */
     , (72628,  22, True ) /* Inscribable */
     , (72628,  69, False) /* IsSellable */
     , (72628,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (72628,   1, 'Legendary Key') /* Name */
     , (72628,  13, 'legarmormagicweaponchest') /* KeyCode */
     , (72628,  14, 'Use this key to open a Legendary Armor, Magic, or Weapon Chest.') /* Use */
     , (72628,  16, 'A key only heard about in whispers and myths.') /* LongDesc */
     , (72628,  33, 'TanadaInterceptKeyPickup') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (72628,   1, 0x02000160) /* Setup */
     , (72628,   3, 0x20000014) /* SoundTable */
     , (72628,   8, 0x06007409) /* Icon */
     , (72628,  22, 0x3400002B) /* PhysicsEffectTable */;
