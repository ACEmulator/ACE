DELETE FROM `weenie` WHERE `class_Id` = 450550;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450550, 'orbtumerokwartailor2', 35, '2021-11-17 16:56:08') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450550,   1,      32768) /* ItemType - Caster */
     , (450550,   3,         14) /* PaletteTemplate - Red */
     , (450550,   5,        0) /* EncumbranceVal */
     , (450550,   8,         50) /* Mass */
     , (450550,   9,   16777216) /* ValidLocations - Held */
     , (450550,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (450550,  18,          1) /* UiEffects - Magical */
     , (450550,  19,       20) /* Value */
     , (450550,  46,        512) /* DefaultCombatStyle - Magic */
     , (450550,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450550,  94,         16) /* TargetType - Creature */
     , (450550, 150,        103) /* HookPlacement - Hook */
     , (450550, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450550,  11, True ) /* IgnoreCollisions */
     , (450550,  13, True ) /* Ethereal */
     , (450550,  14, True ) /* GravityStatus */
     , (450550,  15, True ) /* LightsStatus */
     , (450550,  19, True ) /* Attackable */
     , (450550,  22, True ) /* Inscribable */
     , (450550,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450550,   5,  -0.025) /* ManaRate */
     , (450550,  29,       1) /* WeaponDefense */
     , (450550,  39,     0.8) /* DefaultScale */
     , (450550,  77,       1) /* PhysicsScriptIntensity */
     , (450550, 138,     2.5) /* SlayerDamageBonus */
     , (450550, 144,       0) /* ManaConversionMod */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450550,   1, 'Assault Orb') /* Name */
     , (450550,  16, 'A reward for defeating the leaders of the Falcon Clan.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450550,   1, 0x02000EC3) /* Setup */
     , (450550,   3, 0x20000014) /* SoundTable */
     , (450550,   6, 0x04000BEF) /* PaletteBase */
     , (450550,   7, 0x100002E7) /* ClothingBase */
     , (450550,   8, 0x060020FD) /* Icon */
     , (450550,  19, 0x00000058) /* ActivationAnimation */
     , (450550,  22, 0x3400002B) /* PhysicsEffectTable */
     , (450550,  27, 0x400000E1) /* UseUserAnimation - UseMagicWand */
     , (450550,  30,         87) /* PhysicsScript - BreatheLightning */;


