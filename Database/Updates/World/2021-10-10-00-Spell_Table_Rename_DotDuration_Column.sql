USE `ace_world`;

ALTER TABLE `spell` 
CHANGE COLUMN `dot_Duration` `duration_Override` DOUBLE;
