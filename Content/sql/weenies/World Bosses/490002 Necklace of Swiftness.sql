DELETE FROM `weenie` WHERE `class_Id` = 490002;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490002, 'necklaceofswiftnesspk', 1, '2022-11-05 05:26:30') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490002,   1,          8) /* ItemType - Jewelry */
     , (490002,   3,          4) /* PaletteTemplate - Brown */
     , (490002,   5,        100) /* EncumbranceVal */
     , (490002,   8,         90) /* Mass */
     , (490002,   9,      32768) /* ValidLocations - NeckWear */
     , (490002,  16,          1) /* ItemUseable - No */
     , (490002,  19,      25000) /* Value */
     , (490002,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (490002,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490002, 106,        350) /* ItemSpellcraft */
     , (490002, 107,       3000) /* ItemCurMana */
     , (490002, 108,       3000) /* ItemMaxMana */
     , (490002, 109,          0) /* ItemDifficulty */
     , (490002, 110,          0) /* ItemAllegianceRankLimit */
     , (490002, 151,          2) /* HookType - Wall */
     , (490002, 169,  118162702) /* TsysMutationData */
	 , (490002, 379,          3) /* GearMaxHealth */
	 , (490002, 376,          3) /* GearMaxHealth */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (490002,   4,          0) /* ItemTotalXp */
     , (490002,   5, 2000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490002,  11, True ) /* IgnoreCollisions */
     , (490002,  13, True ) /* Ethereal */
     , (490002,  14, True ) /* GravityStatus */
     , (490002,  19, True ) /* Attackable */
     , (490002,  22, True ) /* Inscribable */
     , (490002, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490002,   5,  -0.033) /* ManaRate */
     , (490002,  12,    0.66) /* Shade */
     , (490002,  39,    0.67) /* DefaultScale */
     , (490002, 110,       1) /* BulkMod */
     , (490002, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490002,   1, 'Necklace of Swiftness') /* Name */
     , (490002,  16, 'This neclace grants the wearer agility and speed. This necklace looks well made, craftmanship that only could be obtained in the ancient empyrean forges.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490002,   1, 0x020000F8) /* Setup */
     , (490002,   3, 0x20000014) /* SoundTable */
     , (490002,   6, 0x04000BEF) /* PaletteBase */
     , (490002,   8, 0x06005BE5) /* Icon */
     , (490002,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490002,  36, 0x0E000012) /* MutateFilter */
     , (490002,  46, 0x38000032) /* TsysMutationFilter */
     , (490002,  52, 0x06005B0C) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (490002,  6106,      2)  /* Epic Quickness */
     , (490002,  6103,      2)  /* Epic Endurance */
	 , (490002,  6071,      2)  /* Epic Endurance */
     , (490002,  4297,      2)  /* Incantation of Coordination Self */
     , (490002,  4319,      2)  /* Incantation of Quickness Self */;
