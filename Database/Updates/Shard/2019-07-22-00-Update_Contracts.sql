USE `ace_shard`;

ALTER TABLE `character_properties_contract`
DROP COLUMN `id`,
DROP COLUMN `time_When_Repeats`,
DROP COLUMN `time_When_Done`,
DROP COLUMN `stage`,
DROP COLUMN `version`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`character_Id`, `contract_Id`),
DROP INDEX `wcid_contract_uidx` ;
;
ALTER TABLE `character_properties_contract`
RENAME TO `character_properties_contract_registry` ;
