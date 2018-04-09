USE `ace_world`;

CREATE TABLE IF NOT EXISTS `encounter` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Encounter',
  `index` int(10) NOT NULL COMMENT 'Index of Enounter',
  `weenie_Class_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Weenie Class Id of generator/object to spawn for Encounter',
  `encounter_Map` text,
  PRIMARY KEY (`id`),
  KEY `encounter_idx` (`index`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Encounters';
