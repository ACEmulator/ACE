DELETE FROM `weenie` WHERE `class_Id` = 450323;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450323, 'orbtumerokwartailor', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450323,   1,      32768) /* ItemType - Caster */
     , (450323,   3,         14) /* PaletteTemplate - Red */
     , (450323,   5,        0) /* EncumbranceVal */
     , (450323,   8,         50) /* Mass */
     , (450323,   9,   16777216) /* ValidLocations - Held */
     , (450323,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (450323,  18,          1) /* UiEffects - Magical */
     , (450323,  19,       20) /* Value */
     , (450323,  46,        512) /* DefaultCombatStyle - Magic */
     , (450323,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450323,  94,         16) /* TargetType - Creature */
     , (450323, 150,        103) /* HookPlacement - Hook */
     , (450323, 151,          2) /* HookType - Wall */
     , (450323, 353,          0) /* WeaponType - Undef */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450323,  11, True ) /* IgnoreCollisions */
     , (450323,  13, True ) /* Ethereal */
     , (450323,  14, True ) /* GravityStatus */
     , (450323,  15, True ) /* LightsStatus */
     , (450323,  19, True ) /* Attackable */
     , (450323,  22, True ) /* Inscribable */
     , (450323,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450323,   5,  -0.025) /* ManaRate */
     , (450323,  29,       1) /* WeaponDefense */
     , (450323,  39,     0.8) /* DefaultScale */
     , (450323,  77,       1) /* PhysicsScriptIntensity */
     , (450323, 138,     2.5) /* SlayerDamageBonus */
     , (450323, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450323,   1, 'Assault Orb') /* Name */
     , (450323,  16, 'A reward for defeating the leaders of the Falcon Clan.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450323,   1, 0x02000EC3) /* Setup */
     , (450323,   3, 0x20000014) /* SoundTable */
     , (450323,   6, 0x04000BEF) /* PaletteBase */
     , (450323,   7, 0x100002E7) /* ClothingBase */
     , (450323,   8, 0x060020FD) /* Icon */
     , (450323,  19, 0x00000058) /* ActivationAnimation */
     , (450323,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450323,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450323,  30,         87) /* PhysicsScript - BreatheLightning */;
