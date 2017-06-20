ALTER TABLE `ace_world`.`ace_position`
  DROP COLUMN `landblock` ,
  ADD COLUMN `landblock` INT(5) UNSIGNED AS ( ((landblockraw >> 16) & 0xFFFF)) STORED,
  DROP INDEX `idx_landblock`,
  ADD  KEY `idx_landblock` (`landblock`);
