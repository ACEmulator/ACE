DELETE FROM `weenie` WHERE `class_Id` = 1023774;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1023774, 'ace1023774-castingstein', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1023774,   1,      32768) /* ItemType - Caster */
     , (1023774,   5,         50) /* EncumbranceVal */
     , (1023774,   8,         10) /* Mass */
     , (1023774,   9,   16777216) /* ValidLocations - Held */
     , (1023774,  16,     655364) /* ItemUseable - 655364 */
     , (1023774,  18,          1) /* UiEffects - Magical */
     , (1023774,  19,         20) /* Value */
     , (1023774,  46,        512) /* DefaultCombatStyle - Magic */
     , (1023774,  53,        101) /* PlacementPosition - Resting */
     , (1023774,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (1023774,  94,         16) /* TargetType - Creature */
     , (1023774, 106,        200) /* ItemSpellcraft */
     , (1023774, 107,       1000) /* ItemCurMana */
     , (1023774, 108,       1000) /* ItemMaxMana */
     , (1023774, 109,         50) /* ItemDifficulty */
     , (1023774, 117,         75) /* ItemManaCost */
     , (1023774, 150,        103) /* HookPlacement - Hook */
     , (1023774, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1023774,  11, True ) /* IgnoreCollisions */
     , (1023774,  13, True ) /* Ethereal */
     , (1023774,  14, True ) /* GravityStatus */
     , (1023774,  15, True ) /* LightsStatus */
     , (1023774,  19, True ) /* Attackable */
     , (1023774,  22, True ) /* Inscribable */
     , (1023774,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1023774,   5,       0) /* ManaRate */
     , (1023774,  12,     0.5) /* Shade */
     , (1023774,  29,       1) /* WeaponDefense */
     , (1023774,  39, 1.2000000476837158) /* DefaultScale */
     , (1023774, 144, 0.07000000029802322) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1023774,   1, 'Casting Stein') /* Name */
     , (1023774,  16, 'A magical beer stein that can be used to focus magic.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1023774,   1,   33558217) /* Setup */
     , (1023774,   3,  536870932) /* SoundTable */
     , (1023774,   8,  100671129) /* Icon */
     , (1023774,  22,  872415275) /* PhysicsEffectTable */
     , (1023774,  27, 1073742049) /* UseUserAnimation - UseMagicWand */
     , (1023774,  28,       1679) /* Spell - Stamina to Mana Self IV */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-20T08:24:12.4078093-04:00",
  "ModifiedBy": "derek42588",
  "Changelog": [],
  "UserChangeSummary": "Ev Dalomar",
  "IsDone": false
}
*/
