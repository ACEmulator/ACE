DELETE FROM `weenie` WHERE `class_Id` = 1027840;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1027840, 'ace1027840-singularityscepterofwarmagic', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1027840,   1,      32768) /* ItemType - Caster */
     , (1027840,   3,          2) /* PaletteTemplate - Blue */
     , (1027840,   5,          1) /* EncumbranceVal */
     , (1027840,   8,         90) /* Mass */
     , (1027840,   9,   16777216) /* ValidLocations - Held */
     , (1027840,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (1027840,  18,          1) /* UiEffects - Magical */
     , (1027840,  19,         20) /* Value */
     , (1027840,  46,        512) /* DefaultCombatStyle - Magic */
     , (1027840,  52,          1) /* ParentLocation - RightHand */
     , (1027840,  53,          1) /* PlacementPosition - RightHandCombat */
     , (1027840,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1027840,  94,         16) /* TargetType - Creature */
     , (1027840, 150,        103) /* HookPlacement - Hook */
     , (1027840, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1027840,  11, True ) /* IgnoreCollisions */
     , (1027840,  13, True ) /* Ethereal */
     , (1027840,  14, True ) /* GravityStatus */
     , (1027840,  19, True ) /* Attackable */
     , (1027840,  22, True ) /* Inscribable */
     , (1027840,  23, True ) /* DestroyOnSell */
     , (1027840,  84, True ) /* IgnoreCloIcons */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1027840,   1, 'Singularity Scepter of War Magic') /* Name */
     , (1027840,  15, 'A scepter imbued with Singularity energy.') /* ShortDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1027840,   1,   33558794) /* Setup */
     , (1027840,   6,   67111919) /* PaletteBase */
     , (1027840,   7,  268435755) /* ClothingBase */
     , (1027840,   8,  100676591) /* Icon */
     , (1027840,  22,  872415275) /* PhysicsEffectTable */
     , (1027840,  27, 1073742049) /* UseUserAnimation - UseMagicWand */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-05-29T23:23:40.0498649-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "custom",
  "IsDone": true
}
*/
