DELETE FROM `weenie` WHERE `class_Id` = 4200170;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (4200170, 'tailor-thrownskullflaming', 35, '2005-02-09 10:00:00') /* Caster */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (4200170,   1,        32768) /* ItemType - Caster */
     , (4200170,   5,         20) /* EncumbranceVal */
     , (4200170,   8,         20) /* Mass */
     , (4200170,   9,    16777216) /* ValidLocations - Held */
     , (4200170,  16,          1) /* ItemUseable - No */
     , (4200170,  19,         20) /* Value */
     , (4200170,  46,        512) /* DefaultCombatStyle - Magic */
     , (4200170,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (4200170, 150,        103) /* HookPlacement - Hook */
     , (4200170, 151,         11) /* HookType - Floor, Wall, Yard */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (4200170,  17, True ) /* Inelastic */
     , (4200170,  23, True ) /* DestroyOnSell */;

INSERT INTO `weenie_properties_float` (`object_Id`, `type`, `value`)
VALUES (4200170,   5,   -0.05) /* ManaRate */
     , (4200170,  29,       1) /* WeaponDefense */
     , (4200170,  39,     0.8) /* DefaultScale */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (4200170,   1, 'Flaming Skull') /* Name */
     , (4200170,  15, 'A skull, that''s on fire.') /* ShortDesc */
     , (4200170,  16, 'It''s a skull, that burns within with a strange supernatural flame.  You can sense a strange, latent magic within it.  When it is thrown in combat, it will mystically replenish itself, tapping into the magic until it is exhausted.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (4200170,   1, 0x02000B76) /* Setup */
     , (4200170,   3, 0x20000014) /* SoundTable */
     , (4200170,   8, 0x060022AE) /* Icon */
     , (4200170,  22, 0x3400002B) /* PhysicsEffectTable */;
