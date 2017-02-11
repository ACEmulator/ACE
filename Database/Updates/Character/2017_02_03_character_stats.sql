CREATE TABLE `character_appearance` (
  `id` int(10) unsigned NOT NULL,
  `race` tinyint(1) unsigned NOT NULL,
  `gender` tinyint(1) unsigned NOT NULL,
  `eyes` tinyint(1) unsigned NOT NULL,
  `nose` tinyint(1) unsigned NOT NULL,
  `mouth` tinyint(1) unsigned NOT NULL,
  `eyeColor` tinyint(1) unsigned NOT NULL,
  `hairColor` tinyint(1) unsigned NOT NULL,
  `hairStyle` tinyint(1) unsigned NOT NULL,
  `hairHue` double NOT NULL,
  `skinHue` double NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_character_appearance_id_character_guid` FOREIGN KEY (`id`) REFERENCES `character` (`guid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `character_stats` (
  `id` int(10) unsigned NOT NULL,
  `strength` tinyint(1) unsigned NOT NULL,
  `endurance` tinyint(1) unsigned NOT NULL,
  `coordination` tinyint(1) unsigned NOT NULL,
  `quickness` tinyint(1) unsigned NOT NULL,
  `focus` tinyint(1) unsigned NOT NULL,
  `self` tinyint(1) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_character_stats_id_character_guid` FOREIGN KEY (`id`) REFERENCES `character` (`guid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `character_skills` (
  `id` int(10) unsigned NOT NULL,
  `skillId` tinyint(1) unsigned NOT NULL,
  `skillStatus` tinyint(1) unsigned NOT NULL,
  `skillPoints` smallint(2) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`, `skillId`),
  CONSTRAINT `FK_character_skills_id_character_guid` FOREIGN KEY (`id`) REFERENCES `character` (`guid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

CREATE TABLE `character_startup_gear` (
  `id` int(10) unsigned NOT NULL,
  `headgearStyle` int(10) unsigned NOT NULL,
  `headgearColor` tinyint(1) unsigned NOT NULL,
  `headgearHue` double NOT NULL,
  `shirtStyle` tinyint(1) unsigned NOT NULL,
  `shirtColor` tinyint(1) unsigned NOT NULL,
  `shirtHue` double NOT NULL,
  `pantsStyle` tinyint(1) unsigned NOT NULL,
  `pantsColor` tinyint(1) unsigned NOT NULL,
  `pantsHue` double NOT NULL,
  `footwearStyle` tinyint(1) unsigned NOT NULL,
  `footwearColor` tinyint(1) unsigned NOT NULL,
  `footwearHue` double NOT NULL,
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_character_startupgear_id_character_guid` FOREIGN KEY (`id`) REFERENCES `character` (`guid`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
