DELETE FROM `weenie` WHERE `class_Id` = 72338;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (72338, 'ace72338-legendarykey', 22, '2022-12-28 05:57:21') /* Key */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (72338,   1,      16384) /* ItemType - Key */
     , (72338,   5,         30) /* EncumbranceVal */
     , (72338,  16,    2097160) /* ItemUseable - SourceContainedTargetRemote */
     , (72338,  18,         64) /* UiEffects - Lightning */
     , (72338,  19,      30000) /* Value */
     , (72338,  33,          0) /* Bonded - Normal */
     , (72338,  91,          3) /* MaxStructure */
     , (72338,  92,          3) /* Structure */
     , (72338,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (72338,  94,        640) /* TargetType - LockableMagicTarget */
     , (72338, 114,          0) /* Attuned - Normal */
     , (72338, 267,      86400) /* Lifespan */
     , (72338, 369,        150) /* UseRequiresLevel */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (72338,   1, False) /* Stuck */
     , (72338,  11, True ) /* IgnoreCollisions */
     , (72338,  13, True ) /* Ethereal */
     , (72338,  14, True ) /* GravityStatus */
     , (72338,  19, True ) /* Attackable */
     , (72338,  22, True ) /* Inscribable */
     , (72338,  69, False) /* IsSellable */
     , (72338,  99, False) /* Ivoryable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (72338,   1, 'Legendary Key') /* Name */
     , (72338,  13, 'legarmormagicweaponchest') /* KeyCode */
     , (72338,  14, 'Use this key to open a Legendary Armor, Magic, or Weapon Chest.') /* Use */
     , (72338,  16, 'A key only heard about in whispers and myths.') /* LongDesc */
     , (72338,  33, 'GeraineStudyKeyPickup') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (72338,   1, 0x02000160) /* Setup */
     , (72338,   3, 0x20000014) /* SoundTable */
     , (72338,   8, 0x06007409) /* Icon */
     , (72338,  22, 0x3400002B) /* PhysicsEffectTable */;
