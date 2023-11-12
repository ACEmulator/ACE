DELETE FROM `weenie` WHERE `class_Id` = 490042;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490042, 'eternalhealingkitplentiful', 28, '2005-02-09 10:00:00') /* Healer */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490042,   1,        128) /* ItemType - Misc */
     , (490042,   5,         50) /* EncumbranceVal */
     , (490042,   8,         25) /* Mass */
     , (490042,   9,          0) /* ValidLocations - None */
     , (490042,  16,    2228232) /* ItemUseable - SourceContainedTargetRemoteOrSelf */
     , (490042,  19,       50) /* Value */
	 , (490042,  33,          1) /* Bonded - Bonded */
     , (490042,  89,          2) /* BoosterEnum - Health */
     , (490042,  90,        100) /* BoostValue */
     , (490042,  91,        100) /* MaxStructure */
     , (490042,  92,        100) /* Structure */
     , (490042,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490042,  94,         16) /* TargetType - Creature */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490042,  22, True ) /* Inscribable */
     , (490042,  69, False) /* IsSellable */
	 , (490042,  63,      True ) /* UnlimitedUse */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490042, 100,     1.6) /* HealkitMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490042,   1, 'Eternal Plentiful Healing Kit') /* Name */
     , (490042,  15, 'A healing kit that has a heady scent.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490042,   1, 0x020002FA) /* Setup */
     , (490042,   6, 0x040008B4) /* PaletteBase */
     , (490042,   7, 0x10000416) /* ClothingBase */
     , (490042,   8, 0x06002908) /* Icon */
	 , (490042,  52, 0x06005B0C) /* IconUnderlay */;
