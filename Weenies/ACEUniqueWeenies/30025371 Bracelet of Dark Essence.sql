DELETE FROM `weenie` WHERE `class_Id` = 30025371;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (30025371, 'EGbraceletdarkessenceCastonStrike', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (30025371,   1,       8) /* ItemType - Jewelry */
     , (30025371,   3,         82) /* PaletteTemplate - PinkPurple */
     , (30025371,   5,        150) /* EncumbranceVal */
     , (30025371,   8,         30) /* Mass */
     , (30025371,   9,     196608) /* ValidLocations - WristWear */
     , (30025371,  16,          1) /* ItemUseable - No */
     , (30025371,  18,          1) /* UiEffects - Magical */
     , (30025371,  19,       8000) /* Value */
     , (30025371,  33,          1) /* Bonded - Bonded */
     , (30025371,  36,       9999) /* ResistMagic */
     , (30025371,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (30025371, 106,        580) /* ItemSpellcraft */
     , (30025371, 107,       1200) /* ItemCurMana */
     , (30025371, 108,       1200) /* ItemMaxMana */
     , (30025371, 109,         50) /* ItemDifficulty */
     , (30025371, 158,          7) /* WieldRequirements - Level */
     , (30025371, 159,          1) /* WieldSkillType - Axe */
     , (30025371, 160,         60) /* WieldDifficulty */
     , (30025371, 376,          3) /* GearHealingBoost */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (30025371,  12, True ) /* ReportCollisions */
     , (30025371,  22, True ) /* Inscribable */
     , (30025371,  23, True ) /* DestroyOnSell */
     , (30025371,  99, True ) /* Ivoryable */
     , (30025371, 112, False) /* ProcSpellSelfTargeted */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (30025371,   5,  -0.033) /* ManaRate */
     , (30025371, 156,    0.15) /* ProcSpellRate */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (30025371,   1, 'Bracelet of Dark Essence') /* Name */
     , (30025371,  16, 'A bracelet, forged in pyreal, stained dark with the taint of corruption. Several diamonds and oddly shaped crystals adorn the thick band of this bracelet. A palpable taint of corruption emanates from within a large white opal that is set into the metal.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (30025371,   1, 0x020000FB) /* Setup */
     , (30025371,   3, 0x20000014) /* SoundTable */
     , (30025371,   6, 0x04000BEF) /* PaletteBase */
     , (30025371,   7, 0x1000033E) /* ClothingBase */
     , (30025371,   8, 0x06002D1D) /* Icon */
     , (30025371,  22, 0x3400002B) /* PhysicsEffectTable */
     , (30025371,  55,       5394) /* ProcSpell - Incantation of Corrosion */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (30025371,  2625,      2)  /* Major Stamina Gain */
     , (30025371,  1450,      2)  /* Willpower Self VI */
     , (30025371,  2623,      2)  /* Major Health Gain */;
