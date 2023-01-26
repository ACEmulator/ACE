DELETE FROM `weenie` WHERE `class_Id` = 480030;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (480030, 'healingkitrareeternalhealthpk', 28, '2021-11-17 16:56:08') /* Healer */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (480030,   1,        128) /* ItemType - Misc */
     , (480030,   3,         61) /* PaletteTemplate - White */
     , (480030,   5,          5) /* EncumbranceVal */
     , (480030,   8,          5) /* Mass */
     , (480030,  16,    2228232) /* ItemUseable - SourceContainedTargetRemoteOrSelf */
     , (480030,  19,          5) /* Value */
     , (420030, 114,          1) /* Attuned - Attuned */
     , (420030,  33,          1) /* Bonded - Bonded */
     , (420030, 267,     172800) /* Lifespan */
     , (480030,  89,          2) /* BoosterEnum - Health */
     , (480030,  90,        120) /* BoostValue */
     , (480030,  92,         -1) /* Structure */
     , (480030,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (480030,  94,         16) /* TargetType - Creature */
     , (480030, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (480030,  11, True ) /* IgnoreCollisions */
     , (480030,  13, True ) /* Ethereal */
     , (480030,  14, True ) /* GravityStatus */
     , (480030,  19, True ) /* Attackable */
     , (480030,  22, True ) /* Inscribable */
     , (480030,  63, True ) /* UnlimitedUse */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (480030, 100,     1.75) /* HealkitMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (480030,   1, 'Durable Health Kit') /* Name */
     , (480030,  16, 'Use this item to recover your Health. It will never run out of uses. ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (480030,   1, 0x020002FA) /* Setup */
     , (480030,   6, 0x040008B4) /* PaletteBase */
     , (480030,   7, 0x100003B0) /* ClothingBase */
     , (480030,   8, 0x06005B6B) /* Icon */
     , (480030,  52, 0x06005B0C) /* IconUnderlay */;
