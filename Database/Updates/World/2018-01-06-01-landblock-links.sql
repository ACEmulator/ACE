USE `ace_world`;

ALTER TABLE `ace_landblock` 
ADD COLUMN `linkSlot` INT(5) NULL DEFAULT NULL AFTER `qZ`,
ADD COLUMN `linkSource` TINYINT NULL DEFAULT NULL AFTER `linkSlot`,
ADD INDEX `linkSlot` (`linkSlot` ASC);
