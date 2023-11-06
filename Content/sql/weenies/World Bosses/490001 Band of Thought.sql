DELETE FROM `weenie` WHERE `class_Id` = 490001;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (490001, 'braceletofthought', 1, '2021-11-17 16:56:08') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (490001,   1,          8) /* ItemType - Jewelry */
     , (490001,   3,          4) /* PaletteTemplate - Brown */
     , (490001,   5,         60) /* EncumbranceVal */
     , (490001,   8,         90) /* Mass */
     , (490001,   9,     196608) /* ValidLocations - WristWear */
     , (490001,  16,          1) /* ItemUseable - No */
     , (490001,  19,      25000) /* Value */
     , (490001,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (490001,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (490001, 106,        350) /* ItemSpellcraft */
     , (490001, 107,       3000) /* ItemCurMana */
     , (490001, 108,       3000) /* ItemMaxMana */
     , (490001, 109,          0) /* ItemDifficulty */
     , (490001, 110,          0) /* ItemAllegianceRankLimit */
     , (490001, 151,          2) /* HookType - Wall */
     , (490001, 169,  118162702) /* TsysMutationData */
	 , (490001, 379,          3) /* GearMaxHealth */
	 , (490001, 376,          3) /* GearMaxHealth */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (490001,   4,          0) /* ItemTotalXp */
     , (490001,   5, 2000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (490001,  11, True ) /* IgnoreCollisions */
     , (490001,  13, True ) /* Ethereal */
     , (490001,  14, True ) /* GravityStatus */
     , (490001,  19, True ) /* Attackable */
     , (490001,  22, True ) /* Inscribable */
     , (490001, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (490001,   5,  -0.033) /* ManaRate */
     , (490001,  12,    0.66) /* Shade */
     , (490001,  39,       1) /* DefaultScale */
     , (490001, 110,       1) /* BulkMod */
     , (490001, 111,       1) /* SizeMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (490001,   1, 'Band of Thought') /* Name */
     , (490001,  16, 'This bracelet is a simple tool used by monks to meditate. Simply by staring at the designs carved on the bracelet, elevated concentration and harmony with one''s surroundings may be achieved. ') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (490001,   1, 0x020000FB) /* Setup */
     , (490001,   3, 0x20000014) /* SoundTable */
     , (490001,   6, 0x04000BEF) /* PaletteBase */
     , (490001,   8, 0x06005BE2) /* Icon */
     , (490001,  22, 0x3400002B) /* PhysicsEffectTable */
     , (490001,  36, 0x0E000012) /* MutateFilter */
     , (490001,  46, 0x38000032) /* TsysMutationFilter */
     , (490001,  52, 0x06005B0C) /* IconUnderlay */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (490001,  6105,      2)  /* Epic Focus */
     , (490001,  6101,      2)  /* Epic Willpower */
     , (490001,  4305,      2)  /* Incantation of Focus Self */
     , (490001,  4329,      2)  /* Incantation of Willpower Self */
	 , (490001,  2979,      2)  /* Incantation of Willpower Self */
     , (490001,  4494,      2)  /* Incantation of Mana Renewal Self */
     , (490001,  4498,      2)  /* Incantation of Rejuvenation Self */;
