USE `ace_world`;

ALTER TABLE `landblock_instance` 
DROP FOREIGN KEY `wcid_instance`;
ALTER TABLE `landblock_instance` 
DROP INDEX `wcid_instance` ;
;
