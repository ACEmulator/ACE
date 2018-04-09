USE `ace_world`;

CREATE TABLE IF NOT EXISTS `event` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Event',
  `name` varchar(255) NOT NULL COMMENT 'Unique Event of Quest',
  `start_Time` int(10) NOT NULL DEFAULT '-1' COMMENT 'Unixtime of Event Start',
  `end_Time` int(10) NOT NULL DEFAULT '-1' COMMENT 'Unixtime of Event End',
  `state` int(10) NOT NULL DEFAULT '0' COMMENT 'State of Event (GameEventState)',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Events';
