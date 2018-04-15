USE `ace_world`;

CREATE TABLE IF NOT EXISTS `encounter` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Encounter',
  `landblock` int(5) NOT NULL DEFAULT '0' COMMENT 'Landblock for this Encounter',
  `weenie_Class_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Weenie Class Id of generator/object to spawn for Encounter',
  `cell_X` int(5) NOT NULL DEFAULT '0' COMMENT 'CellX position of this Encounter',
  `cell_Y` int(5) NOT NULL DEFAULT '0' COMMENT 'CellY position of this Encounter',
  PRIMARY KEY (`id`),
  UNIQUE KEY `landblock_cellx_celly_uidx` (`landblock`,`cell_X`,`cell_Y`),
  KEY `landblock_idx` (`landblock`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Encounters';
