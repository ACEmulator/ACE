DELETE FROM `weenie` WHERE `class_Id` = 490003;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490003, 'ringoftheheart', 1, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490003,   1,          8) /* ItemType - Jewelry */
     , (490003,   3,          1) /* PaletteTemplate - AquaBlue */
     , (490003,   5,         60) /* EncumbranceVal */
     , (490003,   8,         90) /* Mass */
     , (490003,   9,     786432) /* ValidLocations - FingerWear */
     , (490003,  16,          1) /* ItemUseable - No */
     , (490003,  19,      25000) /* Value */
     , (490003,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (490003,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490003, 106,        350) /* ItemSpellcraft */
     , (490003, 107,       3000) /* ItemCurMana */
     , (490003, 108,       3000) /* ItemMaxMana */
     , (490003, 109,          0) /* ItemDifficulty */
     , (490003, 110,          0) /* ItemAllegianceRankLimit */
     , (490003, 151,          2) /* HookType - Wall */
     , (490003, 169,  118162702) /* TsysMutationData */
	 , (490003, 379,          3) /* GearMaxHealth */
	 , (490003, 376,          3) /* GearMaxHealth */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490003,  11, True ) /* IgnoreCollisions */
     , (490003,  13, True ) /* Ethereal */
     , (490003,  14, True ) /* GravityStatus */
     , (490003,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490003,   5,  -0.033) /* ManaRate */
     , (490003,  12,    0.66) /* Shade */
     , (490003,  39,     0.8) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490003,   1, 'Ring of the Heart') /* Name */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490003,   1, 0x02000103) /* Setup */
     , (490003,   3, 0x20000014) /* SoundTable */
     , (490003,   6, 0x04000BEF) /* PaletteBase */
     , (490003,   8, 0x06005BEB) /* Icon */
     , (490003,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490003,  36, 0x0E000012) /* MutateFilter */
     , (490003,  46, 0x38000032) /* TsysMutationFilter */
     , (490003,  52, 0x06005B0C) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (490003,  6107,      2)  /* Epic Strength */
     , (490003,  6104,      2)  /* Epic Endurance */
	 , (490003,  2980,      2)  /* Incantation of Endurance Self */
     , (490003,  4299,      2)  /* Incantation of Endurance Self */
     , (490003,  4325,      2)  /* Incantation of Strength Self */;
