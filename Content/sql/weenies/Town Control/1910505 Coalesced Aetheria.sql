DELETE FROM `weenie` WHERE `class_Id` = 1910505;

INSERT INTO `weenie` (`class_Id`, `class_Name`, `type`, `last_Modified`)
VALUES (1910505, 'ace1910505-aetheria', 38, '2019-07-04 00:00:00') /* Gem */;

INSERT INTO `weenie_properties_int` (`object_Id`, `type`, `value`)
VALUES (1910505,   1,       2048) /* ItemType - Gem */
     , (1910505,   3,          2) /* PaletteTemplate - Blue */
     , (1910505,   5,         50) /* EncumbranceVal */
     , (1910505,   9,          0) /* ValidLocations - None */
     , (1910505,  11,          1) /* MaxStackSize */
     , (1910505,  12,          1) /* StackSize */
     , (1910505,  13,         50) /* StackUnitEncumbrance */
     , (1910505,  15,      1000) /* StackUnitValue */
     , (1910505,  16,          1) /* ItemUseable - No */
     , (1910505,  18,          1) /* UiEffects - Magical */
     , (1910505,  19,      1000) /* Value */
     , (1910505,  93,       1044) /* PhysicsState - Ethereal, IgnoreCollisions, Gravity */
     , (1910505, 158,          7) /* WieldRequirements - Level */
     , (1910505, 159,          1) /* WieldSkillType - Axe */
     , (1910505, 160,         75) /* WieldDifficulty */
     , (1910505, 319,          5) /* ItemMaxLevel */
     , (1910505, 320,          2) /* ItemXpStyle - ScalesWithLevel */;

INSERT INTO `weenie_properties_int64` (`object_Id`, `type`, `value`)
VALUES (1910505,   4,  750000000) /* ItemTotalXp */
     , (1910505,   5, 1000000000) /* ItemBaseXp */;

INSERT INTO `weenie_properties_bool` (`object_Id`, `type`, `value`)
VALUES (1910505,  22, True ) /* Inscribable */;

INSERT INTO `weenie_properties_string` (`object_Id`, `type`, `value`)
VALUES (1910505,   1, 'Coalesced Aetheria') /* Name */
     , (1910505,  16, 'A glowing ball of Coalesced Aetheria.') /* LongDesc */;

INSERT INTO `weenie_properties_d_i_d` (`object_Id`, `type`, `value`)
VALUES (1910505,   1,   33554809) /* Setup */
     , (1910505,   3,  536870932) /* SoundTable */
     , (1910505,   6,   67111919) /* PaletteBase */
     , (1910505,   7,  268435723) /* ClothingBase */
     , (1910505,   8,  100690954) /* Icon */
     , (1910505,  22,  872415275) /* PhysicsEffectTable */
     , (1910505,  50,  100691000) /* IconOverlay */;
