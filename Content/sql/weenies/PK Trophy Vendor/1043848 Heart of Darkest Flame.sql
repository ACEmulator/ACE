DELETE FROM `weenie` WHERE `class_Id` = 1043848;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1043848, 'ace1043848-heartofdarkestflame', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1043848,   1,      32768) /* ItemType - Caster */
     , (1043848,   3,          4) /* PaletteTemplate - Brown */
     , (1043848,   5,        100) /* EncumbranceVal */
     , (1043848,   8,         90) /* Mass */
     , (1043848,   9,   16777216) /* ValidLocations - Held */
     , (1043848,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (1043848,  19,         20) /* Value */
     , (1043848,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (1043848,  52,          1) /* ParentLocation - RightHand */
     , (1043848,  53,        101) /* PlacementPosition - Resting */
     , (1043848,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1043848,  94,         16) /* TargetType - Creature */
     , (1043848, 106,        500) /* ItemSpellcraft */
     , (1043848, 107,       8000) /* ItemCurMana */
     , (1043848, 108,       8000) /* ItemMaxMana */
     , (1043848, 109,          0) /* ItemDifficulty */
     , (1043848, 110,          0) /* ItemAllegianceRankLimit */
     , (1043848, 117,         30) /* ItemManaCost */
     , (1043848, 151,          2) /* HookType - Wall */
     , (1043848, 169,  118162702) /* TsysMutationData */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (1043848,   4,          0) /* ItemTotalXp */
     , (1043848,   5, 2000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1043848,  11, True ) /* IgnoreCollisions */
     , (1043848,  13, True ) /* Ethereal */
     , (1043848,  14, True ) /* GravityStatus */
     , (1043848,  19, True ) /* Attackable */
     , (1043848,  22, True ) /* Inscribable */
     , (1043848, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1043848,   5, -0.05000000074505806) /* ManaRate */
     , (1043848,  12, 0.6600000262260437) /* Shade */
     , (1043848,  39,       1) /* DefaultScale */
     , (1043848, 144, 0.0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1043848,   1, 'Heart of Darkest Flame') /* Name */
     , (1043848,  16, 'Due to the dark whispers that can be sometimes heard when the orb is wielded, it is often believed to be the heart of a slain Kemeroi. Whether or not this is belief is a true one, the Heart of Darkest Flame is a potent tool for those who wield the powers of the Void.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1043848,   1,   33561171) /* Setup */
     , (1043848,   3,  536870932) /* SoundTable */
     , (1043848,   6,   67111919) /* PaletteBase */
     , (1043848,   8,  100691783) /* Icon */
     , (1043848,  22,  872415275) /* PhysicsEffectTable */
     , (1043848,  27, 1073742049) /* UseUserAnimation - UseMagicWand */
     , (1043848,  28,       5355) /* Spell - Nether Bolt VII */
     , (1043848,  36,  234881042) /* MutateFilter */
     , (1043848,  46,  939524146) /* TsysMutationFilter */
     , (1043848,  52,  100686604) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-20T08:03:15.2626436-04:00",
  "ModifiedBy": "derek42588",
  "Changelog": [],
  "UserChangeSummary": "Updated - Done",
  "IsDone": true
}
*/
