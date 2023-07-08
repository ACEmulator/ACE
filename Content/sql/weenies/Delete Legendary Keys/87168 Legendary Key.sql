DELETE FROM `weenie` WHERE `class_Id` = 87168;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (87168, 'ace87168-legendarykey', 22, '2021-11-01 00:00:00') /* Key */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (87168,   1,      16384) /* ItemType - Key */
     , (87168,   5,         30) /* EncumbranceVal */
     , (87168,  16,    2097160) /* ItemUseable - SourceContainedTargetRemote */
     , (87168,  18,         64) /* UiEffects - Lightning */
     , (87168,  19,      40000) /* Value */
     , (87168,  33,          0) /* Bonded - Normal */
     , (87168,  91,          4) /* MaxStructure */
     , (87168,  92,          4) /* Structure */
     , (87168,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (87168,  94,        640) /* TargetType - LockableMagicTarget */
     , (87168, 114,          0) /* Attuned - Normal */
     , (87168, 267,      86400) /* Lifespan */
     , (87168, 369,        150) /* UseRequiresLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (87168,  22, True ) /* Inscribable */
     , (87168,  69, False) /* IsSellable */
     , (87168,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (87168,   1, 'Legendary Key') /* Name */
     , (87168,  13, 'legarmormagicweaponchest') /* KeyCode */
     , (87168,  14, 'Use this key to open a Legendary Armor, Magic, or Weapon Chest.') /* Use */
     , (87168,  16, 'A key only heard about in whispers and myths.') /* LongDesc */
     , (87168,  33, 'DericostLegKeyPickupTimer') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (87168,   1, 0x02000160) /* Setup */
     , (87168,   3, 0x20000014) /* SoundTable */
     , (87168,   8, 0x06007409) /* Icon */
     , (87168,  22, 0x3400002B) /* PhysicsEffectTable */;
