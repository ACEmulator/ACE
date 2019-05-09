USE `ace_shard`;

ALTER TABLE `biota_properties_anim_part` 
DROP FOREIGN KEY `wcid_animpart`;
ALTER TABLE `biota_properties_anim_part` 
ADD INDEX `wcid_animpart_idx` (`object_Id` ASC),
DROP INDEX `object_Id_index_uidx` ;
;
ALTER TABLE `biota_properties_anim_part` 
ADD CONSTRAINT `wcid_animpart`
  FOREIGN KEY (`object_Id`)
  REFERENCES `biota` (`id`)
  ON DELETE CASCADE;
