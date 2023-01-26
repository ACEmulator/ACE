DELETE FROM `weenie` WHERE `class_Id` = 1910507;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1910507, 'ace1910507-aetheria', 38, '2019-07-04 00:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1910507,   1,       2048) /* ItemType - Gem */
     , (1910507,   3,         83) /* PaletteTemplate - Amber */
     , (1910507,   5,         50) /* EncumbranceVal */
     , (1910507,   9,          0) /* ValidLocations - None */
     , (1910507,  11,          1) /* MaxStackSize */
     , (1910507,  12,          1) /* StackSize */
     , (1910507,  13,         50) /* StackUnitEncumbrance */
     , (1910507,  15,      1000) /* StackUnitValue */
     , (1910507,  16,          1) /* ItemUseable - No */
     , (1910507,  18,          1) /* UiEffects - Magical */
     , (1910507,  19,      1000) /* Value */
     , (1910507,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1910507, 158,          7) /* WieldRequirements - Level */
     , (1910507, 159,          1) /* WieldSkillType - Axe */
     , (1910507, 160,        150) /* WieldDifficulty */
     , (1910507, 319,          5) /* ItemMaxLevel */
     , (1910507, 320,          2) /* ItemXpStyle - ScalesWithLevel */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (1910507,   4,          0) /* ItemTotalXp */
     , (1910507,   5, 1000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1910507,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1910507,   1, 'Coalesced Aetheria') /* Name */
     , (1910507,  16, 'A glowing ball of Coalesced Aetheria.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1910507,   1,   33554809) /* Setup */
     , (1910507,   3,  536870932) /* SoundTable */
     , (1910507,   6,   67111919) /* PaletteBase */
     , (1910507,   7,  268435723) /* ClothingBase */
     , (1910507,   8,  100690956) /* Icon */
     , (1910507,  22,  872415275) /* PhysicsEffectTable */
     , (1910507,  50,  100691000) /* IconOverlay */;
