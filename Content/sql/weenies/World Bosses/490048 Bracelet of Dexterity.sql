DELETE FROM `weenie` WHERE `class_Id` = 490048;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490048, 'Bracelet of Dexterity', 1, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490048,   1,          8) /* ItemType - Jewelry */
     , (490048,   3,          1) /* PaletteTemplate - AquaBlue */
     , (490048,   5,         60) /* EncumbranceVal */
     , (490048,   8,         90) /* Mass */
     , (490048,   9,     196608) /* ValidLocations - WristWear */
     , (490048,  16,          1) /* ItemUseable - No */
     , (490048,  19,      25000) /* Value */
     , (490048,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (490048,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490048, 106,        350) /* ItemSpellcraft */
     , (490048, 107,       3000) /* ItemCurMana */
     , (490048, 108,       3000) /* ItemMaxMana */
     , (490048, 109,          0) /* ItemDifficulty */
     , (490048, 110,          0) /* ItemAllegianceRankLimit */
     , (490048, 151,          2) /* HookType - Wall */
     , (490048, 169,  118162702) /* TsysMutationData */
	 , (490048, 379,          3) /* GearMaxHealth */
	 , (490048, 376,          3) /* GearMaxHealth */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (490048,   4,          0) /* ItemTotalXp */
     , (490048,   5, 2000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490048,  11, True ) /* IgnoreCollisions */
     , (490048,  13, True ) /* Ethereal */
     , (490048,  14, True ) /* GravityStatus */
     , (490048,  19, True ) /* Attackable */
     , (490048,  22, True ) /* Inscribable */
     , (490048, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490048,   5,  -0.033) /* ManaRate */
     , (490048,  12,    0.66) /* Shade */
     , (490048,  39,     0.5) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490048,   1, 'Bracelet of Dexterity') /* Name */
     , (490048,  16, 'This bracelet grants the wearer agility and speed. This bracelet looks well made, craftmanship that only could be obtained in the ancient empyrean forges.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490048,   1, 0x020000FB) /* Setup */
     , (490048,   3, 0x20000014) /* SoundTable */
     , (490048,   6, 0x04000BEF) /* PaletteBase */
     , (490048,   8, 0x06005BDE) /* Icon */
     , (490048,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490048,  36, 0x0E000012) /* MutateFilter */
     , (490048,  46, 0x38000032) /* TsysMutationFilter */
     , (490048,  52, 0x06005B0C) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (490048,  6106,      2)  /* Legendary Bludge Ward */
     , (490048,  6103,      2)  /* Legendary Flame Ward */
	 , (490048,  3849,      2)  /* Legendary Flame Ward */
	 , (490048,  4297,      2)  /* Legendary Flame Ward */
	 , (490048,  4319,      2)  /* Legendary Flame Ward */
	 , (490048,  4616,      2)  /* Legendary Flame Ward */;