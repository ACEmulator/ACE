DELETE FROM `weenie` WHERE `class_Id` = 1022080;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1022080, 'ace1022080-impiousstaff', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1022080,   1,      32768) /* ItemType - Caster */
     , (1022080,   3,         39) /* PaletteTemplate - Black */
     , (1022080,   5,          1) /* EncumbranceVal */
     , (1022080,   8,         25) /* Mass */
     , (1022080,   9,   16777216) /* ValidLocations - Held */
     , (1022080,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (1022080,  18,          1) /* UiEffects - Magical */
     , (1022080,  19,         20) /* Value */
     , (1022080,  46,        512) /* DefaultCombatStyle - Magic */
     , (1022080,  52,          1) /* ParentLocation - RightHand */
     , (1022080,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1022080,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1022080,  94,         16) /* TargetType - Creature */
     , (1022080, 150,        103) /* HookPlacement - Hook */
     , (1022080, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1022080,  22, True ) /* Inscribable */
     , (1022080,  23, True ) /* DestroyOnSell */
     , (1022080,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1022080,  39, 0.6000000238418579) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1022080,   1, 'Impious Staff') /* Name */
     , (1022080,  15, 'This staff is made from a metal alloy and carbonized iron.') /* ShortDesc */
     , (1022080,  16, 'Made from a metal alloy and carbonized iron. This staff once belonged to an ancient group of acolytes who possessed magical powers.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1022080,   1,   33557877) /* Setup */
     , (1022080,   3,  536870932) /* SoundTable */
     , (1022080,   6,   67111919) /* PaletteBase */
     , (1022080,   7,  268436442) /* ClothingBase */
     , (1022080,   8,  100673510) /* Icon */
     , (1022080,  22,  872415275) /* PhysicsEffectTable */
     , (1022080,  27, 1073742049) /* UseUserAnimation - UseMagicWand */
     , (1022080,  28,       2814) /* Spell - Priest's Curse */
     , (1022080,  37,         34) /* ItemSkillLimit */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-29T23:42:10.8985127-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": ".custom",
  "IsDone": false
}
*/
