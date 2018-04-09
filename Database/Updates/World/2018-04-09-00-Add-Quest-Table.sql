USE `ace_world`;

CREATE TABLE IF NOT EXISTS `quest` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Quest',
  `name` varchar(255) NOT NULL COMMENT 'Unique Name of Quest',
  `min_Delta` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Minimum time between Quest completions',
  `max_Solves` int(10) NOT NULL DEFAULT '0' COMMENT 'Maximum number of times Quest can be completed',
  `message` text NOT NULL COMMENT 'Quest solved text - unused?',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Quests';
