-- --------------------------------------------------------
-- Host:                         192.168.123.140
-- Server version:               10.1.21-MariaDB-1~xenial - mariadb.org binary distribution
-- Server OS:                    debian-linux-gnu
-- HeidiSQL Version:             9.4.0.5125
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping data for table ace_world.creature_data: ~1 rows (approximately)
DELETE FROM `creature_data`;
/*!40000 ALTER TABLE `creature_data` DISABLE KEYS */;
INSERT INTO `creature_data` (`id`, `level`, `strength`, `endurance`, `coordination`, `quickness`, `focus`, `self`, `health`, `stamina`, `mana`, `xpgranted`, `luminance`, `loottier`) VALUES
	(1, 8, 30, 35, 50, 35, 30, 15, 33, 85, 15, 1000, 0, 1);
/*!40000 ALTER TABLE `creature_data` ENABLE KEYS */;

-- Dumping data for table ace_world.model_data: ~2 rows (approximately)
DELETE FROM `model_data`;
/*!40000 ALTER TABLE `model_data` DISABLE KEYS */;
INSERT INTO `model_data` (`id`, `index`, `resourceid`) VALUES
	(1, 9, 16784289),
	(1, 12, 16784289);
/*!40000 ALTER TABLE `model_data` ENABLE KEYS */;

-- Dumping data for table ace_world.object_data: ~1 rows (approximately)
DELETE FROM `object_data`;
/*!40000 ALTER TABLE `object_data` DISABLE KEYS */;
INSERT INTO `object_data` (`id`, `wcid`, `name`, `pluralname`, `type`, `iconid`, `iconoverlayid`, `iconunderlayid`, `setupid`, `animframeid`, `phstableid`, `stableid`, `itemscapacity`, `effects`, `ammotype`, `combatuse`, `maxstructure`, `maxstacksize`, `validlocations`, `targettype`, `burden`, `spellid`, `hooktype`, `materialtype`, `cooldownid`, `cooldownduration`, `objectdescription`, `physicsstate`, `containerscapacity`, `paletteid`) VALUES
	(1, 35442, 'Drudge Sneaker', '', 16, 100667445, 0, 0, 33556445, 0, 872415258, 536870919, 255, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 20, 1032, 255, 67112812);
/*!40000 ALTER TABLE `object_data` ENABLE KEYS */;

-- Dumping data for table ace_world.palette_data: ~1 rows (approximately)
DELETE FROM `palette_data`;
/*!40000 ALTER TABLE `palette_data` DISABLE KEYS */;
INSERT INTO `palette_data` (`id`, `paletteid`, `offset`, `length`) VALUES
	(1, 67112812, 0, 255);
/*!40000 ALTER TABLE `palette_data` ENABLE KEYS */;

-- Dumping data for table ace_world.texture_data: ~2 rows (approximately)
DELETE FROM `texture_data`;
/*!40000 ALTER TABLE `texture_data` DISABLE KEYS */;
INSERT INTO `texture_data` (`id`, `index`, `oldid`, `newid`) VALUES
	(1, 9, 83892467, 83892468),
	(1, 12, 83892467, 83892468);
/*!40000 ALTER TABLE `texture_data` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
