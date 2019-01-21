USE `ace_world`;

ALTER TABLE `spell` 
ADD COLUMN `dot_Duration` INT(10) NULL DEFAULT NULL AFTER `number_Variance`;
