DELETE FROM `weenie` WHERE `class_Id` = 480013;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480013, 'idoldarkfinishedpk', 44, '2005-02-09 10:00:00') /* CraftTool */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480013,   1,        128) /* ItemType - Misc */
     , (480013,   5,        700) /* EncumbranceVal */
     , (480013,   8,        700) /* Mass */
     , (480013,   9,          0) /* ValidLocations - None */
     , (480013,  11,          1) /* MaxStackSize */
     , (480013,  12,          1) /* StackSize */
     , (480013,  13,        700) /* StackUnitEncumbrance */
     , (480013,  14,        700) /* StackUnitMass */
     , (480013,  15,          1) /* StackUnitValue */
     , (480013,  16,     524296) /* ItemUseable - SourceContainedTargetContained */
     , (480013,  19,         250) /* Value */
     , (480013,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480013,  94,        256) /* TargetType - MissileWeapon */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480013,  22, True ) /* Inscribable */
     , (480013,  23, True ) /* DestroyOnSell */
     , (480013,  69, False) /* IsSellable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480013,  39,     0.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480013,   1, 'Fetish of the Dark Idols') /* Name */
     , (480013,  14, 'Combine this with any loot-generated atlatl, bow, or crossbow.  The Fetish of the Dark Idols will apply a Magic Absorbing property and a Melee Defense penalty to the weapon.  Although the weapon can be imbued before applying a Fetish of the Dark Idols, it cannot be imbued afterwards.  The weapon may have non-imbue tinkers applied either before or after application of the Fetish of the Dark Idols.') /* Use */
     , (480013,  16, 'This bizarre creation seems to pulse in your hands, sending powerful ripples of energy through your arms.  ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480013,   1, 0x020010FA) /* Setup */
     , (480013,   3, 0x20000014) /* SoundTable */
     , (480013,   8, 0x060033DB) /* Icon */
     , (480013,  22, 0x3400002B) /* PhysicsEffectTable */;
