USE `ace_world`;

CREATE TABLE IF NOT EXISTS `house_portal` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this House Portal',
  `house_Id` int(10) unsigned NOT NULL COMMENT 'Unique Id of House',
  `obj_Cell_Id` int(10) unsigned NOT NULL,
  `origin_X` float NOT NULL,
  `origin_Y` float NOT NULL,
  `origin_Z` float NOT NULL,
  `angles_W` float NOT NULL,
  `angles_X` float NOT NULL,
  `angles_Y` float NOT NULL,
  `angles_Z` float NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `house_Id_UNIQUE` (`house_Id`,`obj_Cell_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='House Portal Destinations';
