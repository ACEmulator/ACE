DELETE FROM `weenie` WHERE `class_Id` = 450218;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450218, 'wandrarederulimbtailor', 35, '2021-11-01 00:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450218,   1,      32768) /* ItemType - Caster */
     , (450218,   3,          4) /* PaletteTemplate - Brown */
     , (450218,   5,        0) /* EncumbranceVal */
     , (450218,   8,         90) /* Mass */
     , (450218,   9,   16777216) /* ValidLocations - Held */
     , (450218,  18,         32) /* UiEffects - Fire */
     , (450218,  19,      20) /* Value */
     , (450218,  26,          1) /* AccountRequirements - AsheronsCall_Subscription */
     , (450218,  46,        512) /* DefaultCombatStyle - Magic */
     , (450218,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (450218,  94,         16) /* TargetType - Creature */
     , (450218, 151,          2) /* HookType - Wall */;


INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450218,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450218,   5,  -0.033) /* ManaRate */
     , (450218,  12,    0.66) /* Shade */
     , (450218,  29,     1.0) /* WeaponDefense */
     , (450218, 136,       0) /* CriticalMultiplier */
     , (450218, 144,     0.2) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450218,   1, 'Deru Limb') /* Name */
     , (450218,  16, 'All the Deru can trace their ancestry to the First Tree, because they all were once twigs on its great trunk. It is said that the First Tree has seen the beginning of the world and that when the First Tree perishes, so will the world. On rare occasions the First Tree will grant a blessing to those that show exceptional awareness of the cycles of life. This wand is one such gift. This wand must be cultivated from the First Tree in such a way that no knife is used. The petitioner must pray and coax a piece of the First Tree so that the limb will naturally fall off the tree, neither harming the First Tree nor the new limb.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450218,   1, 0x02001382) /* Setup */
     , (450218,   3, 0x20000014) /* SoundTable */
     , (450218,   6, 0x04000BEF) /* PaletteBase */
     , (450218,   8, 0x06005C07) /* Icon */
     , (450218,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450218,  27, 0x400000E0) /* UseUserAnimation - UseMagicStaff */
     , (450218,  36, 0x0E000012) /* MutateFilter */
     , (450218,  46, 0x38000032) /* TsysMutationFilter */
     , (450218,  52, 0x06005B0C) /* IconUnderlay */;

