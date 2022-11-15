DELETE FROM `weenie` WHERE `class_Id` = 450602;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (450602, 'orbdarksorcerernewtailor', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (450602,   1,      32768) /* ItemType - Caster */
     , (450602,   5,         0) /* EncumbranceVal */
     , (450602,   8,         50) /* Mass */
     , (450602,   9,   16777216) /* ValidLocations - Held */
     , (450602,  16,    6291464) /* ItemUseable - SourceContainedTargetRemoteNeverWalk */
     , (450602,  18,          1) /* UiEffects - Magical */
     , (450602,  19,       20) /* Value */
     , (450602,  46,        512) /* DefaultCombatStyle - Magic */
     , (450602,  93,       3092) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity, LightingOn */
     , (450602,  94,         16) /* TargetType - Creature */
     , (450602, 150,        103) /* HookPlacement - Hook */
     , (450602, 151,          2) /* HookType - Wall */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (450602,  15, True ) /* LightsStatus */
     , (450602,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (450602,   5,   -0.05) /* ManaRate */
     , (450602,  39,     2.0) /* DefaultScale */
     , (450602,  29,       1) /* WeaponDefense */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (450602,   1, 'Dark Sorcerer''s Phylactery') /* Name */
     , (450602,  16, 'An orb with some sort of dark figure within.  Gazing into its depths, you see the wretched face of a Dark Sorcerer, an undead from the Vesayen Isles.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (450602,   1, 0x02000B5A) /* Setup */
     , (450602,   3, 0x20000014) /* SoundTable */
     , (450602,   8, 0x06002A2E) /* Icon */
     , (450602,  22, 0x3400002B) /* PhysicsEffectTable */;

