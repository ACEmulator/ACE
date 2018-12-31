USE `ace_shard`;

ALTER TABLE `biota_properties_enchantment_registry` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`object_Id`, `enchantment_Category`, `spell_Id`, `layer_Id`),
ADD INDEX `layer_Id_idx` (`layer_Id` ASC),
ADD INDEX `spell_Category_idx` (`spell_Category` ASC),
ADD INDEX `power_Level_idx` (`power_Level` ASC);
;
