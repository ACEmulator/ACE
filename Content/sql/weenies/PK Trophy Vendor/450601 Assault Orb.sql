DELETE FROM `weenie` WHERE `class_Id` = 450601;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450601, 'orbtumerokwartailor3', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450601,   1,      32768) /* ItemType - Caster */
     , (450601,   3,         14) /* PaletteTemplate - Red */
     , (450601,   5,        0) /* EncumbranceVal */
     , (450601,   8,         50) /* Mass */
     , (450601,   9,   16777216) /* ValidLocations - Held */
     , (450601,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (450601,  18,          1) /* UiEffects - Magical */
     , (450601,  19,       20) /* Value */
     , (450601,  46,        512) /* DefaultCombatStyle - Magic */
     , (450601,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450601,  94,         16) /* TargetType - Creature */
     , (450601, 150,        103) /* HookPlacement - Hook */
     , (450601, 151,          2) /* HookType - Wall */
     , (450601, 353,          0) /* WeaponType - Undef */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450601,  11, True ) /* IgnoreCollisions */
     , (450601,  13, True ) /* Ethereal */
     , (450601,  14, True ) /* GravityStatus */
     , (450601,  15, True ) /* LightsStatus */
     , (450601,  19, True ) /* Attackable */
     , (450601,  22, True ) /* Inscribable */
     , (450601,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450601,   5,  -0.025) /* ManaRate */
     , (450601,  29,       1) /* WeaponDefense */
     , (450601,  39,     0.8) /* DefaultScale */
     , (450601,  77,       1) /* PhysicsScriptIntensity */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450601,   1, 'Assault Orb') /* Name */
     , (450601,  16, 'A reward for defeating the leaders of the Falcon Clan.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450601,   1, 0x02000EC3) /* Setup */
     , (450601,   3, 0x20000014) /* SoundTable */
     , (450601,   6, 0x04000BEF) /* PaletteBase */
     , (450601,   7, 0x100002E7) /* ClothingBase */
     , (450601,   8, 0x060020FD) /* Icon */
     , (450601,  19, 0x00000058) /* ActivationAnimation */
     , (450601,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450601,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450601,  30,         87) /* PhysicsScript - BreatheLightning */;


