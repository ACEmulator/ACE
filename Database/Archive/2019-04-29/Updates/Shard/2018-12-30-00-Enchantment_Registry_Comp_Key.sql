USE `ace_shard`;

ALTER TABLE `biota_properties_enchantment_registry` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`object_Id`, `enchantment_Category`, `spell_Id`, `layer_Id`);
;
