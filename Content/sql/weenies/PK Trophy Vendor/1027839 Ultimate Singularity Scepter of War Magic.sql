DELETE FROM `weenie` WHERE `class_Id` = 1027839;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1027839, 'ace1027839-ultimatesingularityscepterofwarmagic', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1027839,   1,      32768) /* ItemType - Caster */
     , (1027839,   3,          8) /* PaletteTemplate - Green */
     , (1027839,   5,          1) /* EncumbranceVal */
     , (1027839,   8,         90) /* Mass */
     , (1027839,   9,   16777216) /* ValidLocations - Held */
     , (1027839,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (1027839,  18,          1) /* UiEffects - Magical */
     , (1027839,  19,         20) /* Value */
     , (1027839,  46,        512) /* DefaultCombatStyle - Magic */
     , (1027839,  52,          1) /* ParentLocation - RightHand */
     , (1027839,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1027839,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1027839,  94,         16) /* TargetType - Creature */
     , (1027839, 150,        103) /* HookPlacement - Hook */
     , (1027839, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1027839,  11, True ) /* IgnoreCollisions */
     , (1027839,  13, True ) /* Ethereal */
     , (1027839,  14, True ) /* GravityStatus */
     , (1027839,  19, True ) /* Attackable */
     , (1027839,  22, True ) /* Inscribable */
     , (1027839,  23, True ) /* DestroyOnSell */
     , (1027839,  84, True ) /* IgnoreCloIcons */
     , (1027839,  85, True ) /* AppraisalHasAllowedWielder */
     , (1027839,  99, True ) /* Ivoryable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1027839,   1, 'Ultimate Singularity Scepter of War Magic') /* Name */
     , (1027839,  15, 'A scepter imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1027839,   1,   33558796) /* Setup */
     , (1027839,   6,   67111919) /* PaletteBase */
     , (1027839,   7,  268435755) /* ClothingBase */
     , (1027839,   8,  100676589) /* Icon */
     , (1027839,  22,  872415275) /* PhysicsEffectTable */
     , (1027839,  27, 1073742049) /* UseUserAnimation - UseMagicWand */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-29T23:23:49.2882975-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
