USE ace_shard;

ALTER TABLE `ace_shard`.`biota_properties_int` 
DROP COLUMN `id`,
DROP PRIMARY KEY,
ADD PRIMARY KEY (`object_Id`, `type`),
DROP INDEX `wcid_int_type_uidx` ;
;