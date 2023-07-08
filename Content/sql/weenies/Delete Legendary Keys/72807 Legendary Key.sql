DELETE FROM `weenie` WHERE `class_Id` = 72807;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (72807, 'ace72807-legendarykey', 22, '2022-01-08 18:29:57') /* Key */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (72807,   1,      16384) /* ItemType - Key */
     , (72807,   5,         30) /* EncumbranceVal */
     , (72807,  16,    2097160) /* ItemUseable - SourceContainedTargetRemote */
     , (72807,  18,         64) /* UiEffects - Lightning */
     , (72807,  19,      20000) /* Value */
     , (72807,  33,          0) /* Bonded - Normal */
     , (72807,  91,          2) /* MaxStructure */
     , (72807,  92,          2) /* Structure */
     , (72807,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (72807,  94,        640) /* TargetType - LockableMagicTarget */
     , (72807, 114,          0) /* Attuned - Normal */
     , (72807, 267,      86400) /* Lifespan */
     , (72807, 369,        150) /* UseRequiresLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (72807,  22, True ) /* Inscribable */
     , (72807,  69, False) /* IsSellable */
     , (72807,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (72807,   1, 'Legendary Key') /* Name */
     , (72807,  13, 'legarmormagicweaponchest') /* KeyCode */
     , (72807,  14, 'Use this key to open a Legendary Armor, Magic, or Weapon Chest.') /* Use */
     , (72807,  16, 'A key only heard about in whispers and myths.') /* LongDesc */
     , (72807,  33, 'SerpentBurialGroundsKeyPickup') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (72807,   1, 0x02000160) /* Setup */
     , (72807,   3, 0x20000014) /* SoundTable */
     , (72807,   8, 0x06007409) /* Icon */
     , (72807,  22, 0x3400002B) /* PhysicsEffectTable */;
