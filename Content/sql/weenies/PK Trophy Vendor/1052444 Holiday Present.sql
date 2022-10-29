DELETE FROM `weenie` WHERE `class_Id` = 1052444;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1052444, 'ace1052444-holidaypresent', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1052444,   1,      32768) /* ItemType - Caster */
     , (1052444,   5,        250) /* EncumbranceVal */
     , (1052444,   9,   16777216) /* ValidLocations - Held */
     , (1052444,  16,          1) /* ItemUseable - No */
     , (1052444,  19,         20) /* Value */
     , (1052444,  52,          1) /* ParentLocation - RightHand */
     , (1052444,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1052444, 106,        400) /* ItemSpellcraft */
     , (1052444, 107,        867) /* ItemCurMana */
     , (1052444, 108,       1000) /* ItemMaxMana */
     , (1052444, 109,        100) /* ItemDifficulty */
     , (1052444, 151,          2) /* HookType - Wall */
     , (1052444, 158,          7) /* WieldRequirements - Level */
     , (1052444, 159,          1) /* WieldSkillType - Axe */
     , (1052444, 160,        150) /* WieldDifficulty */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1052444,  11, True ) /* IgnoreCollisions */
     , (1052444,  13, True ) /* Ethereal */
     , (1052444,  14, True ) /* GravityStatus */
     , (1052444,  19, True ) /* Attackable */
     , (1052444,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1052444,   5, -0.02500000037252903) /* ManaRate */
     , (1052444,  39, 0.17000000178813934) /* DefaultScale */
     , (1052444, 144, 0.10000000149011612) /* ManaConversionMod */
     , (1052444, 152, 1.0800000429153442) /* ElementalDamageMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1052444,   1, 'Holiday Present') /* Name */
     , (1052444,  14, 'Use this item to equip it.') /* Use */
     , (1052444,  16, 'A beautifully wrapped holiday present. You wonder what''s inside!') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1052444,   1,   33560155) /* Setup */
     , (1052444,   8,  100673909) /* Icon */;

INSERT INTO `weenie_properties_spell_book` (`object_Id`, `spell`, `probability`)
VALUES (1052444,  2227,      2)  /* Ketnan's Blessing */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-20T08:11:56.9265427-04:00",
  "ModifiedBy": "derek42588",
  "Changelog": [],
  "UserChangeSummary": "Ev Dalomar",
  "IsDone": false
}
*/
