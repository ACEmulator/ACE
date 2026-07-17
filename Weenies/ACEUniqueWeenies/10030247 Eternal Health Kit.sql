DELETE FROM `weenie` WHERE `class_Id` = 10030247;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10030247, 'unbondedhealingkitrareeternalhealthbonded', 28, '2021-11-17 16:56:08') /* Healer */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10030247,   1,        128) /* ItemType - Misc */
     , (10030247,   3,         61) /* PaletteTemplate - White */
     , (10030247,   5,          5) /* EncumbranceVal */
     , (10030247,   8,          5) /* Mass */
     , (10030247,  16,    2228232) /* ItemUseable - SourceContainedTargetRemoteOrSelf */
     , (10030247,  17,        146) /* RareId */
     , (10030247,  19,          0) /* Value */
     , (10030247,  33,          1) /* Bonded - Bonded */
     , (10030247,  89,          2) /* BoosterEnum - Health */
     , (10030247,  90,        100) /* BoostValue */
     , (10030247,  92,         -1) /* Structure */
     , (10030247,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10030247,  94,         16) /* TargetType - Creature */
     , (10030247, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10030247,  11, True ) /* IgnoreCollisions */
     , (10030247,  13, True ) /* Ethereal */
     , (10030247,  14, True ) /* GravityStatus */
     , (10030247,  19, True ) /* Attackable */
     , (10030247,  22, True ) /* Inscribable */
     , (10030247,  63, True ) /* UnlimitedUse */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10030247, 100,     1.6) /* HealkitMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10030247,   1, 'Eternal Health Kit') /* Name */
     , (10030247,  16, 'Use this item to recover your Health. It will never run out of uses. ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10030247,   1, 0x020002FA) /* Setup */
     , (10030247,   6, 0x040008B4) /* PaletteBase */
     , (10030247,   7, 0x100003B0) /* ClothingBase */
     , (10030247,   8, 0x06005B6B) /* Icon */
     , (10030247,  52, 0x06005B0C) /* IconUnderlay */;
