DELETE FROM `weenie` WHERE `class_Id` = 490051;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490051, 'ringoffocusandcoord', 1, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490051,   1,          8) /* ItemType - Jewelry */
     , (490051,   3,          1) /* PaletteTemplate - AquaBlue */
     , (490051,   5,         60) /* EncumbranceVal */
     , (490051,   8,         90) /* Mass */
     , (490051,   9,     786432) /* ValidLocations - FingerWear */
     , (490051,  16,          1) /* ItemUseable - No */
     , (490051,  19,      25000) /* Value */
     , (490051,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (490051,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490051, 106,        350) /* ItemSpellcraft */
     , (490051, 107,       3000) /* ItemCurMana */
     , (490051, 108,       3000) /* ItemMaxMana */
     , (490051, 109,          0) /* ItemDifficulty */
     , (490051, 110,          0) /* ItemAllegianceRankLimit */
     , (490051, 151,          2) /* HookType - Wall */
     , (490051, 169,  118162702) /* TsysMutationData */
	 , (490051, 379,          3) /* GearMaxHealth */
	 , (490051, 376,          3) /* GearMaxHealth */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490051,  11, True ) /* IgnoreCollisions */
     , (490051,  13, True ) /* Ethereal */
     , (490051,  14, True ) /* GravityStatus */
     , (490051,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490051,   5,  -0.033) /* ManaRate */
     , (490051,  12,    0.66) /* Shade */
     , (490051,  39,     0.8) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490051,   1, 'Ring of Healing') /* Name */
     , (490051,  16, 'Practice is the key to any skill; left unused, skill is lost and knowledge is forgotten. The power that is contained within this ring focuses the bearer''s knowledge. Bits and pieces of forgotten lore are brought back into crystal clarity as if learned yesterday. ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490051,   1, 0x02000103) /* Setup */
     , (490051,   3, 0x20000014) /* SoundTable */
     , (490051,   6, 0x04000BEF) /* PaletteBase */
     , (490051,   8, 0x06005BE9) /* Icon */
     , (490051,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490051,  36, 0x0E000012) /* MutateFilter */
     , (490051,  46, 0x38000032) /* TsysMutationFilter */
     , (490051,  52, 0x06005B0C) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (490051,  6103,      2)  /* Epic Endurance */
	 , (490051,  6053,      2)  /* Epic Endurance */
	 , (490051,  4556,      2)  /* Incantation of Endurance Self */
     , (490051,  4297,      2)  /* Incantation of Endurance Self */
     , (490051,  4305,      2)  /* Incantation of Strength Self */;
