USE `ace_world`;

ALTER TABLE `spell` 
ADD COLUMN `dot_Duration` DOUBLE NULL DEFAULT NULL AFTER `number_Variance`;
