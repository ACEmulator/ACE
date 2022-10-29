DELETE FROM `weenie` WHERE `class_Id` = 1030376;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1030376, 'ace1030376-orboftheironsea', 35, '2021-11-20 00:19:18') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1030376,   1,      32768) /* ItemType - Caster */
     , (1030376,   3,          4) /* PaletteTemplate - Brown */
     , (1030376,   5,        100) /* EncumbranceVal */
     , (1030376,   8,         90) /* Mass */
     , (1030376,   9,   16777216) /* ValidLocations - Held */
     , (1030376,  16,    6291460) /* ItemUseable - SourceWieldedTargetRemoteNeverWalk */
     , (1030376,  19,         20) /* Value */
     , (1030376,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (1030376,  52,          1) /* ParentLocation - RightHand */
     , (1030376,  53,        101) /* PlacementPosition - Resting */
     , (1030376,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1030376,  94,         16) /* TargetType - Creature */
     , (1030376, 151,          2) /* HookType - Wall */
     , (1030376, 169,  118162702) /* TsysMutationData */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1030376,  11, True ) /* IgnoreCollisions */
     , (1030376,  13, True ) /* Ethereal */
     , (1030376,  14, True ) /* GravityStatus */
     , (1030376,  19, True ) /* Attackable */
     , (1030376,  22, True ) /* Inscribable */
     , (1030376, 100, False) /* Dyable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (1030376,   5, -0.03333330154418945) /* ManaRate */
     , (1030376,  12, 0.6600000262260437) /* Shade */
     , (1030376,  39,       1) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1030376,   1, 'Orb of the Ironsea') /* Name */
     , (1030376,  16, 'Although this jewel looks solid, one has only to touch it to realize otherwise. The surface ripples like water when disturbed and yet somehow still manages to hold its spherical shape. Legend has it that this water comes from the deepest parts of the Ironsea and can only be retrieved by coaxing the denizens that live there to the surface. Such water is highly sought after by mages as it seems to help them cast their spells with more power and efficiency.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1030376,   1,   33559424) /* Setup */
     , (1030376,   3,  536870932) /* SoundTable */
     , (1030376,   6,   67111919) /* PaletteBase */
     , (1030376,   8,  100686851) /* Icon */
     , (1030376,  22,  872415275) /* PhysicsEffectTable */
     , (1030376,  27, 1073742049) /* UseUserAnimation - UseMagicWand */
     , (1030376,  28,       2132) /* Spell - The Spike */
     , (1030376,  36,  234881042) /* MutateFilter */
     , (1030376,  46,  939524146) /* TsysMutationFilter */
     , (1030376,  52,  100686604) /* IconUnderlay */;

/* Lifestoned Changelog:
{
  "LastModified": "2020-03-19T21:49:28.1491481-04:00",
  "ModifiedBy": "Tindale",
  "Changelog": [],
  "UserChangeSummary": "Updated Slayer Bonus Damage to 1.25",
  "IsDone": true
}
*/
