DELETE FROM `weenie` WHERE `class_Id` = 4200173;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200173, 'tailor-aerlinthecynosure', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200173,   1,      32768) /* ItemType - Caster */
     , (4200173,   3,          2) /* PaletteTemplate - Blue */
     , (4200173,   5,         50) /* EncumbranceVal */
     , (4200173,   8,         50) /* Mass */
     , (4200173,   9,   16777216) /* ValidLocations - Held */
     , (4200173,  16,          1) /* ItemUseable - No */
     , (4200173,  18,          1) /* UiEffects - Magical */
     , (4200173,  19,         20) /* Value */
     , (4200173,  46,        512) /* DefaultCombatStyle - Magic */
     , (4200173,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200173,  94,         16) /* TargetType - Creature */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200173,  22, True ) /* Inscribable */
     , (4200173,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200173,   5,   -0.05) /* ManaRate */
     , (4200173,  29,       1) /* WeaponDefense */
     , (4200173,  39,     0.8) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200173,   1, 'Aerlinthe Cynosure') /* Name */
     , (4200173,  15, 'A spikey orb, crackling with arcane energy.') /* ShortDesc */
     , (4200173,  16, 'An irregularly carved obsidian sphere, enchanted with a permanent tie to the island of Aerlinthe. This item will cast its teleport spell on the caster when it is WIELDED. It will not be lost on death, cannot be given, and may only be taken once.') /* LongDesc */
     , (4200173,  33, 'aercyno') /* Quest */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200173,   1, 0x020009CF) /* Setup */
     , (4200173,   3, 0x20000014) /* SoundTable */
     , (4200173,   6, 0x04000BF8) /* PaletteBase */
     , (4200173,   7, 0x10000249) /* ClothingBase */
     , (4200173,   8, 0x06001F4B) /* Icon */
     , (4200173,  22, 0x3400002B) /* PhysicsEffectTable */
     , (4200173,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (4200173,  36, 0x0E000016) /* MutateFilter */
     , (4200173,  37,         32) /* ItemSkillLimit - ItemEnchantment */;
