DELETE FROM `weenie` WHERE `class_Id` = 1910506;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1910506, 'ace1910506-aetheria', 38, '2019-07-04 00:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1910506,   1,       2048) /* ItemType - Gem */
     , (1910506,   3,         14) /* PaletteTemplate - Red */
     , (1910506,   5,         50) /* EncumbranceVal */
     , (1910506,   9,          0) /* ValidLocations - None */
     , (1910506,  11,          1) /* MaxStackSize */
     , (1910506,  12,          1) /* StackSize */
     , (1910506,  13,         50) /* StackUnitEncumbrance */
     , (1910506,  15,      1000) /* StackUnitValue */
     , (1910506,  16,          1) /* ItemUseable - No */
     , (1910506,  18,          1) /* UiEffects - Magical */
     , (1910506,  19,      1000) /* Value */
     , (1910506,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1910506, 158,          7) /* WieldRequirements - Level */
     , (1910506, 159,          1) /* WieldSkillType - Axe */
     , (1910506, 160,        225) /* WieldDifficulty */
     , (1910506, 319,          5) /* ItemMaxLevel */
     , (1910506, 320,          2) /* ItemXpStyle - ScalesWithLevel */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (1910506,   4,          0) /* ItemTotalXp */
     , (1910506,   5, 1000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1910506,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1910506,   1, 'Coalesced Aetheria') /* Name */
     , (1910506,  16, 'A glowing ball of Coalesced Aetheria.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1910506,   1,   33554809) /* Setup */
     , (1910506,   3,  536870932) /* SoundTable */
     , (1910506,   6,   67111919) /* PaletteBase */
     , (1910506,   7,  268435723) /* ClothingBase */
     , (1910506,   8,  100690955) /* Icon */
     , (1910506,  22,  872415275) /* PhysicsEffectTable */
     , (1910506,  50,  100691000) /* IconOverlay */;
