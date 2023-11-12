DELETE FROM `weenie` WHERE `class_Id` = 490000;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490000, 'Hardened Bracelet', 1, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490000,   1,          8) /* ItemType - Jewelry */
     , (490000,   3,          1) /* PaletteTemplate - AquaBlue */
     , (490000,   5,         60) /* EncumbranceVal */
     , (490000,   8,         90) /* Mass */
     , (490000,   9,     196608) /* ValidLocations - WristWear */
     , (490000,  16,          1) /* ItemUseable - No */
     , (490000,  19,      25000) /* Value */
     , (490000,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (490000,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490000, 106,        350) /* ItemSpellcraft */
     , (490000, 107,       3000) /* ItemCurMana */
     , (490000, 108,       3000) /* ItemMaxMana */
     , (490000, 109,          0) /* ItemDifficulty */
     , (490000, 110,          0) /* ItemAllegianceRankLimit */
     , (490000, 151,          2) /* HookType - Wall */
     , (490000, 169,  118162702) /* TsysMutationData */
	 , (490000, 379,          3) /* GearMaxHealth */
	 , (490000, 376,          3) /* GearMaxHealth */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (490000,   4,          0) /* ItemTotalXp */
     , (490000,   5, 2000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490000,  11, True ) /* IgnoreCollisions */
     , (490000,  13, True ) /* Ethereal */
     , (490000,  14, True ) /* GravityStatus */
     , (490000,  19, True ) /* Attackable */
     , (490000,  22, True ) /* Inscribable */
     , (490000, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490000,   5,  -0.033) /* ManaRate */
     , (490000,  12,    0.66) /* Shade */
     , (490000,  39,     0.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490000,   1, 'Hardened Bracelet') /* Name */
     , (490000,  16, 'This bracelet grants the wearer protection against Bludgeoning and Flame. This bracelet looks well made, craftmanship that only could be obtained in the ancient empyrean forges.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490000,   1, 0x020000FB) /* Setup */
     , (490000,   3, 0x20000014) /* SoundTable */
     , (490000,   6, 0x04000BEF) /* PaletteBase */
     , (490000,   8, 0x06005BDE) /* Icon */
     , (490000,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490000,  36, 0x0E000012) /* MutateFilter */
     , (490000,  46, 0x38000032) /* TsysMutationFilter */
     , (490000,  52, 0x06005B0C) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (490000,  6081,      2)  /* Legendary Bludge Ward */
     , (490000,  6082,      2)  /* Legendary Flame Ward */
	 , (490000,  3204,      2)  /* Legendary Flame Ward */
	 , (490000,  4468,      2)  /* Legendary Flame Ward */
     , (490000,  4464,      2)  ;
