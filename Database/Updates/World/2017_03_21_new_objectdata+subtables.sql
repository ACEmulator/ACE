-- --------------------------------------------------------
-- HeidiSQL Version:             9.4.0.5125
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Data exporting was unselected.
-- Dumping structure for table ace_world.object_data
CREATE TABLE IF NOT EXISTS `object_data` (
  `id` int(10) unsigned NOT NULL DEFAULT '0',
  `wcid` smallint(5) unsigned NOT NULL DEFAULT '0',
  `name` text NOT NULL,
  `pluralname` text NOT NULL,
  `type` int(10) unsigned NOT NULL DEFAULT '0',
  `iconid` int(10) unsigned NOT NULL DEFAULT '0',
  `iconoverlayid` int(10) unsigned NOT NULL DEFAULT '0',
  `iconunderlayid` int(10) unsigned NOT NULL DEFAULT '0',
  `setupid` int(10) unsigned NOT NULL DEFAULT '0',
  `animframeid` int(10) unsigned NOT NULL DEFAULT '0',
  `phstableid` int(10) unsigned NOT NULL DEFAULT '0',
  `stableid` int(10) unsigned NOT NULL DEFAULT '0',
  `itemscapacity` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `effects` int(10) unsigned NOT NULL DEFAULT '0',
  `ammotype` int(10) unsigned NOT NULL DEFAULT '0',
  `combatuse` int(10) unsigned NOT NULL DEFAULT '0',
  `maxstructure` int(10) unsigned NOT NULL DEFAULT '0',
  `maxstacksize` int(10) unsigned NOT NULL DEFAULT '0',
  `validlocations` int(10) unsigned NOT NULL DEFAULT '0',
  `targettype` int(10) unsigned NOT NULL DEFAULT '0',
  `burden` int(10) unsigned NOT NULL DEFAULT '0',
  `spellid` int(10) unsigned NOT NULL DEFAULT '0',
  `hooktype` int(10) unsigned NOT NULL DEFAULT '0',
  `materialtype` int(10) unsigned NOT NULL DEFAULT '0',
  `cooldownid` int(10) unsigned NOT NULL DEFAULT '0',
  `cooldownduration` int(10) unsigned NOT NULL DEFAULT '0',
  `objectdescription` int(10) unsigned NOT NULL DEFAULT '0',
  `physicsstate` int(10) unsigned NOT NULL DEFAULT '0',
  `containerscapacity` int(10) unsigned NOT NULL DEFAULT '0',
  `paletteid` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Dumping structure for table ace_world.creature_data
CREATE TABLE IF NOT EXISTS `creature_data` (
  `id` int(10) unsigned NOT NULL DEFAULT '0',
  `level` int(10) unsigned NOT NULL DEFAULT '0',
  `strength` int(10) unsigned NOT NULL DEFAULT '0',
  `endurance` int(10) unsigned NOT NULL DEFAULT '0',
  `coordination` int(10) unsigned NOT NULL DEFAULT '0',
  `quickness` int(10) unsigned NOT NULL DEFAULT '0',
  `focus` int(10) unsigned NOT NULL DEFAULT '0',
  `self` int(10) unsigned NOT NULL DEFAULT '0',
  `health` int(10) unsigned NOT NULL DEFAULT '0',
  `stamina` int(10) unsigned NOT NULL DEFAULT '0',
  `mana` int(10) unsigned NOT NULL DEFAULT '0',
  `xpgranted` int(10) unsigned NOT NULL DEFAULT '0',
  `luminance` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `loottier` tinyint(3) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  CONSTRAINT `FK_creature_data_id_object_data_id` FOREIGN KEY (`id`) REFERENCES `object_data` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table ace_world.model_data
CREATE TABLE IF NOT EXISTS `model_data` (
  `id` int(10) unsigned NOT NULL DEFAULT '0',
  `index` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `resourceid` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`,`index`),
  CONSTRAINT `FK_model_data_id_object_data_id` FOREIGN KEY (`id`) REFERENCES `object_data` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table ace_world.palette_data
CREATE TABLE IF NOT EXISTS `palette_data` (
  `id` int(10) unsigned NOT NULL DEFAULT '0',
  `paletteid` int(10) unsigned NOT NULL DEFAULT '0',
  `offset` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `length` tinyint(3) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`,`paletteid`),
  CONSTRAINT `FK_palette_data_id_object_data_id` FOREIGN KEY (`id`) REFERENCES `object_data` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
-- Dumping structure for table ace_world.texture_data
CREATE TABLE IF NOT EXISTS `texture_data` (
  `id` int(10) unsigned NOT NULL DEFAULT '0',
  `index` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `oldid` int(10) unsigned NOT NULL DEFAULT '0',
  `newid` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`,`index`),
  CONSTRAINT `FK_texture_data_id_object_data_id` FOREIGN KEY (`id`) REFERENCES `object_data` (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

-- Data exporting was unselected.
/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
