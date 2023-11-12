DELETE FROM `weenie` WHERE `class_Id` = 490050;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490050, 'necklaceofelementsk', 1, '2022-11-05 05:26:30') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490050,   1,          8) /* ItemType - Jewelry */
     , (490050,   3,          4) /* PaletteTemplate - Brown */
     , (490050,   5,        100) /* EncumbranceVal */
     , (490050,   8,         90) /* Mass */
     , (490050,   9,      32768) /* ValidLocations - NeckWear */
     , (490050,  16,          1) /* ItemUseable - No */
     , (490050,  19,      25000) /* Value */
     , (490050,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (490050,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490050, 106,        350) /* ItemSpellcraft */
     , (490050, 107,       3000) /* ItemCurMana */
     , (490050, 108,       3000) /* ItemMaxMana */
     , (490050, 109,          0) /* ItemDifficulty */
     , (490050, 110,          0) /* ItemAllegianceRankLimit */
     , (490050, 151,          2) /* HookType - Wall */
     , (490050, 169,  118162702) /* TsysMutationData */
	 , (490050, 379,          3) /* GearMaxHealth */
	 , (490050, 376,          3) /* GearMaxHealth */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (490050,   4,          0) /* ItemTotalXp */
     , (490050,   5, 2000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490050,  11, True ) /* IgnoreCollisions */
     , (490050,  13, True ) /* Ethereal */
     , (490050,  14, True ) /* GravityStatus */
     , (490050,  19, True ) /* Attackable */
     , (490050,  22, True ) /* Inscribable */
     , (490050, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490050,   5,  -0.033) /* ManaRate */
     , (490050,  12,    0.66) /* Shade */
     , (490050,  39,    0.67) /* DefaultScale */
     , (490050, 110,       1) /* BulkMod */
     , (490050, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490050,   1, 'Necklace of the Elements') /* Name */
     , (490050,  16, 'This neclace grants the wearer protection against Flame and Cold. This necklace looks well made, craftmanship that only could be obtained in the ancient empyrean forges.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490050,   1, 0x020000F8) /* Setup */
     , (490050,   3, 0x20000014) /* SoundTable */
     , (490050,   6, 0x04000BEF) /* PaletteBase */
     , (490050,   8, 0x06005BE4) /* Icon */
     , (490050,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490050,  36, 0x0E000012) /* MutateFilter */
     , (490050,  46, 0x38000032) /* TsysMutationFilter */
     , (490050,  52, 0x06005B0C) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (490050,  4466,      2)  /* Incantation of Cold Protection Self */
     , (490050,  4468,      2)  /* Incantation of Fire Protection Self */
     , (490050,  6082,      2)  /* Epic Flame Ward */
     , (490050,  6083,      2)  /* Epic Frost Ward */;
