USE `ace_shard`;

ALTER TABLE `biota_properties_texture_map` 
DROP FOREIGN KEY `wcid_texturemap`;
ALTER TABLE `biota_properties_texture_map` 
ADD INDEX `wcid_texturemap_idx` (`object_Id` ASC),
DROP INDEX `object_Id_index_oldId_uidx` ;
;
ALTER TABLE `biota_properties_texture_map` 
ADD CONSTRAINT `wcid_texturemap`
  FOREIGN KEY (`object_Id`)
  REFERENCES `biota` (`id`)
  ON DELETE CASCADE;
