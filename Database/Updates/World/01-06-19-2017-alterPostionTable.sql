ALTER TABLE `ace_world`.`ace_position`   
  DROP COLUMN `landblock` ,
  ADD COLUMN `landblock` INT(5) UNSIGNED AS ( ((landblockraw >> 16) & 0xFFFF)) VIRTUAL,
  DROP COLUMN `cell` ,
  DROP INDEX `idx_landblock`,
  DROP INDEX `idx_cell`,
  ADD  KEY `idx_landblock` (`landblock`);
