DELETE FROM `weenie` WHERE `class_Id` = 10030248;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10030248, 'healingkitrareeternalmanabonded', 28, '2021-11-17 16:56:08') /* Healer */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10030248,   1,        128) /* ItemType - Misc */
     , (10030248,   5,          5) /* EncumbranceVal */
     , (10030248,   8,          5) /* Mass */
     , (10030248,  16,    2228232) /* ItemUseable - SourceContainedTargetRemoteOrSelf */
     , (10030248,  17,        148) /* RareId */
     , (10030248,  19,          0) /* Value */
     , (10030248,  33,          1) /* Bonded - Bonded */
     , (10030248,  89,          6) /* BoosterEnum - Mana */
     , (10030248,  90,        100) /* BoostValue */
     , (10030248,  92,         -1) /* Structure */
     , (10030248,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10030248,  94,         16) /* TargetType - Creature */
     , (10030248, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10030248,  11, True ) /* IgnoreCollisions */
     , (10030248,  13, True ) /* Ethereal */
     , (10030248,  14, True ) /* GravityStatus */
     , (10030248,  19, True ) /* Attackable */
     , (10030248,  22, True ) /* Inscribable */
     , (10030248,  63, True ) /* UnlimitedUse */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10030248, 100,     1.6) /* HealkitMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10030248,   1, 'Eternal Mana Kit') /* Name */
     , (10030248,  16, 'Use this item to recover your Mana. It will never run out of uses. ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10030248,   1, 0x020002FA) /* Setup */
     , (10030248,   6, 0x040008B4) /* PaletteBase */
     , (10030248,   7, 0x10000416) /* ClothingBase */
     , (10030248,   8, 0x06005B6C) /* Icon */
     , (10030248,  52, 0x06005B0C) /* IconUnderlay */;
