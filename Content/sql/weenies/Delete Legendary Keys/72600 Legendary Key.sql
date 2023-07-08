DELETE FROM `weenie` WHERE `class_Id` = 72600;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (72600, 'ace72600-legendarykey', 22, '2022-01-08 18:29:57') /* Key */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (72600,   1,      16384) /* ItemType - Key */
     , (72600,   5,         30) /* EncumbranceVal */
     , (72600,  16,    2097160) /* ItemUseable - SourceContainedTargetRemote */
     , (72600,  18,         64) /* UiEffects - Lightning */
     , (72600,  19,      10000) /* Value */
     , (72600,  33,          0) /* Bonded - Normal */
     , (72600,  91,          1) /* MaxStructure */
     , (72600,  92,          1) /* Structure */
     , (72600,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (72600,  94,        640) /* TargetType - LockableMagicTarget */
     , (72600, 114,          0) /* Attuned - Normal */
     , (72600, 267,      86400) /* Lifespan */
     , (72600, 369,        150) /* UseRequiresLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (72600,  22, True ) /* Inscribable */
     , (72600,  69, False) /* IsSellable */
     , (72600,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (72600,   1, 'Legendary Key') /* Name */
     , (72600,  13, 'legarmormagicweaponchest') /* KeyCode */
     , (72600,  14, 'Use this key to open a Legendary Armor, Magic, or Weapon Chest.') /* Use */
     , (72600,  16, 'A key only heard about in whispers and myths.') /* LongDesc */
     , (72600,  33, 'NanjouStockadeKeyPickup') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (72600,   1, 0x02000160) /* Setup */
     , (72600,   3, 0x20000014) /* SoundTable */
     , (72600,   8, 0x06007409) /* Icon */
     , (72600,  22, 0x3400002B) /* PhysicsEffectTable */;
