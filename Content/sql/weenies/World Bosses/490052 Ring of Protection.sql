DELETE FROM `weenie` WHERE `class_Id` = 490052;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490052, 'ringofprotection', 1, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490052,   1,          8) /* ItemType - Jewelry */
     , (490052,   3,          1) /* PaletteTemplate - AquaBlue */
     , (490052,   5,         60) /* EncumbranceVal */
     , (490052,   8,         90) /* Mass */
     , (490052,   9,     786432) /* ValidLocations - FingerWear */
     , (490052,  16,          1) /* ItemUseable - No */
     , (490052,  19,      25000) /* Value */
     , (490052,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (490052,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490052, 106,        350) /* ItemSpellcraft */
     , (490052, 107,       3000) /* ItemCurMana */
     , (490052, 108,       3000) /* ItemMaxMana */
     , (490052, 109,          0) /* ItemDifficulty */
     , (490052, 110,          0) /* ItemAllegianceRankLimit */
     , (490052, 151,          2) /* HookType - Wall */
     , (490052, 169,  118162702) /* TsysMutationData */
	 , (490052, 379,          3) /* GearMaxHealth */
	 , (490052, 376,          3) /* GearMaxHealth */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490052,  11, True ) /* IgnoreCollisions */
     , (490052,  13, True ) /* Ethereal */
     , (490052,  14, True ) /* GravityStatus */
     , (490052,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490052,   5,  -0.033) /* ManaRate */
     , (490052,  12,    0.66) /* Shade */
     , (490052,  39,     0.8) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490052,   1, 'Ring of Protection') /* Name */
     , (490052,  16, 'This Ring grants the wearer additional Armor and protection against Lightning damage. This bracelet looks well made, craftmanship that only could be obtained in the ancient empyrean forges.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490052,   1, 0x02000103) /* Setup */
     , (490052,   3, 0x20000014) /* SoundTable */
     , (490052,   6, 0x04000BEF) /* PaletteBase */
     , (490052,   8, 0x06005BEA) /* Icon */
     , (490052,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490052,  36, 0x0E000012) /* MutateFilter */
     , (490052,  46, 0x38000032) /* TsysMutationFilter */
     , (490052,  52, 0x06005B0C) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (490052,  6102,      2)  /* Epic Strength */
     , (490052,  6079,      2)  /* Epic Endurance */
	 , (490052,  4090,      2)  /* Incantation of Endurance Self */
     , (490052,  4470,      2)  /* Incantation of Endurance Self */;
