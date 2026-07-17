DELETE FROM `weenie` WHERE `class_Id` = 10030249;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10030249, 'healingkitrareeternalstaminabonded', 28, '2021-11-17 16:56:08') /* Healer */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10030249,   1,        128) /* ItemType - Misc */
     , (10030249,   5,          5) /* EncumbranceVal */
     , (10030249,   8,          5) /* Mass */
     , (10030249,  16,    2228232) /* ItemUseable - SourceContainedTargetRemoteOrSelf */
     , (10030249,  17,        147) /* RareId */
     , (10030249,  19,          0) /* Value */
     , (10030249,  33,          1) /* Bonded - Bonded */
     , (10030249,  89,          4) /* BoosterEnum - Stamina */
     , (10030249,  90,        100) /* BoostValue */
     , (10030249,  92,         -1) /* Structure */
     , (10030249,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10030249,  94,         16) /* TargetType - Creature */
     , (10030249, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10030249,  11, True ) /* IgnoreCollisions */
     , (10030249,  13, True ) /* Ethereal */
     , (10030249,  14, True ) /* GravityStatus */
     , (10030249,  19, True ) /* Attackable */
     , (10030249,  22, True ) /* Inscribable */
     , (10030249,  63, True ) /* UnlimitedUse */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10030249, 100,     1.6) /* HealkitMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10030249,   1, 'Eternal Stamina Kit') /* Name */
     , (10030249,  16, 'Use this item to recover your Stamina. It will never run out of uses. ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10030249,   1, 0x020002FA) /* Setup */
     , (10030249,   6, 0x040008B4) /* PaletteBase */
     , (10030249,   7, 0x10000416) /* ClothingBase */
     , (10030249,   8, 0x06005B6D) /* Icon */
     , (10030249,  52, 0x06005B0C) /* IconUnderlay */;
