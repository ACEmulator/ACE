USE `ace_shard`;

ALTER TABLE `biota_properties_position` 
ADD INDEX `type_cell_idx` (`position_Type` ASC, `obj_Cell_Id` ASC);
;
