DELETE FROM `weenie` WHERE `class_Id` = 490049;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490049, 'Reinforced Bracelet', 1, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490049,   1,          8) /* ItemType - Jewelry */
     , (490049,   3,          1) /* PaletteTemplate - AquaBlue */
     , (490049,   5,         60) /* EncumbranceVal */
     , (490049,   8,         90) /* Mass */
     , (490049,   9,     196608) /* ValidLocations - WristWear */
     , (490049,  16,          1) /* ItemUseable - No */
     , (490049,  19,      25000) /* Value */
     , (490049,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (490049,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490049, 106,        350) /* ItemSpellcraft */
     , (490049, 107,       3000) /* ItemCurMana */
     , (490049, 108,       3000) /* ItemMaxMana */
     , (490049, 109,          0) /* ItemDifficulty */
     , (490049, 110,          0) /* ItemAllegianceRankLimit */
     , (490049, 151,          2) /* HookType - Wall */
     , (490049, 169,  118162702) /* TsysMutationData */
	 , (490049, 379,          3) /* GearMaxHealth */
	 , (490049, 376,          3) /* GearMaxHealth */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (490049,   4,          0) /* ItemTotalXp */
     , (490049,   5, 2000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490049,  11, True ) /* IgnoreCollisions */
     , (490049,  13, True ) /* Ethereal */
     , (490049,  14, True ) /* GravityStatus */
     , (490049,  19, True ) /* Attackable */
     , (490049,  22, True ) /* Inscribable */
     , (490049, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490049,   5,  -0.033) /* ManaRate */
     , (490049,  12,    0.66) /* Shade */
     , (490049,  39,     0.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490049,   1, 'Reinforced Bracelet') /* Name */
     , (490049,  16, 'This bracelet grants the wearer protection against Bludgeoning and Frost. This bracelet looks well made, craftmanship that only could be obtained in the ancient empyrean forges.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490049,   1, 0x020000FB) /* Setup */
     , (490049,   3, 0x20000014) /* SoundTable */
     , (490049,   6, 0x04000BEF) /* PaletteBase */
     , (490049,   8, 0x06005BEC) /* Icon */
     , (490049,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490049,  36, 0x0E000012) /* MutateFilter */
     , (490049,  46, 0x38000032) /* TsysMutationFilter */
     , (490049,  52, 0x06005B0C) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (490049,  6081,      2)  /* Legendary Bludge Ward */
     , (490049,  6083,      2)  /* Legendary Flame Ward */
	 , (490049,  4464,      2)  /* Legendary Flame Ward */
     , (490049,  4466,      2)  ;
