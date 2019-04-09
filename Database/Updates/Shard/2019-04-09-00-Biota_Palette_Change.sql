USE `ace_shard`;

ALTER TABLE `biota_properties_palette` 
DROP FOREIGN KEY `wcid_palette`,
ADD INDEX `wcid_palette_idx` (`object_Id` ASC),
DROP INDEX `object_Id_subPaletteId_offset_length_uidx` ;
;
ALTER TABLE `biota_properties_palette` 
ADD CONSTRAINT `wcid_palette`
  FOREIGN KEY (`object_Id`)
  REFERENCES `ace_shard`.`biota` (`id`)
  ON DELETE CASCADE;
