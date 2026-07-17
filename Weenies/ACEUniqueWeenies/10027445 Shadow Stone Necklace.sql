DELETE FROM `weenie` WHERE `class_Id` = 10027445;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (10027445, 'EGnecklaceshadowstone', 1, '2005-02-09 10:00:00') /* Generic */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (10027445,   1,          8) /* ItemType - Jewelry */
     , (10027445,   3,         39) /* PaletteTemplate - Black */
     , (10027445,   5,         40) /* EncumbranceVal */
     , (10027445,   8,         30) /* Mass */
     , (10027445,   9,      32768) /* ValidLocations - NeckWear */
     , (10027445,  16,          1) /* ItemUseable - No */
     , (10027445,  18,          1) /* UiEffects - Magical */
     , (10027445,  19,       6000) /* Value */
     , (10027445,  33,          1) /* Bonded - Bonded */
     , (10027445,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (10027445, 106,        580) /* ItemSpellcraft */
     , (10027445, 107,        800) /* ItemCurMana */
     , (10027445, 108,        800) /* ItemMaxMana */
     , (10027445, 109,          1) /* ItemDifficulty */
     , (10027445, 150,        103) /* HookPlacement - Hook */
     , (10027445, 151,          2) /* HookType - Wall */
     , (10027445, 376,          3) /* GearHealingBoost */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (10027445,  11, True ) /* IgnoreCollisions */
     , (10027445,  13, True ) /* Ethereal */
     , (10027445,  14, True ) /* GravityStatus */
     , (10027445,  19, True ) /* Attackable */
     , (10027445,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (10027445,   5,       0) /* ManaRate */
     , (10027445, 156,    0.15) /* ProcSpellRate */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (10027445,   1, 'Shadow Stone Necklace') /* Name */
     , (10027445,  16, 'The focus of the Consumed Wraith''s power, the Shadow Stone crackles with abyssal energy.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (10027445,   1, 0x020000F8) /* Setup */
     , (10027445,   3, 0x20000014) /* SoundTable */
     , (10027445,   6, 0x04000BEF) /* PaletteBase */
     , (10027445,   7, 0x1000027F) /* ClothingBase */
     , (10027445,   8, 0x06003343) /* Icon */
     , (10027445,  22, 0x3400002B) /* PhysicsEffectTable */
     , (10027445,  55,       5386) /* ProcSpell - Incantation of Weakening Curse */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (10027445,  2182,      2)  /* Battlemage's Boon */
     , (10027445,  2184,      2)  /* Hydra's Head */;
