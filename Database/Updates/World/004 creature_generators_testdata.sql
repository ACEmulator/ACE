-- --------------------------------------------------------
-- Server version:               10.1.21-MariaDB-1~xenial - mariadb.org binary distribution
-- Server OS:                    debian-linux-gnu
-- HeidiSQL Version:             9.4.0.5125
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;

-- Dumping data for table ace_world.ace_creature_generators: ~2 rows (approximately)
/*!40000 ALTER TABLE `ace_creature_generators` DISABLE KEYS */;
INSERT INTO `ace_creature_generators` (`generatorid`, `name`) VALUES
	(3, 'Drduges (Black)'),
	(8, 'Drduges (Normal)');
/*!40000 ALTER TABLE `ace_creature_generators` ENABLE KEYS */;

-- Dumping data for table ace_world.ace_creature_generator_data: ~2 rows (approximately)
/*!40000 ALTER TABLE `ace_creature_generator_data` DISABLE KEYS */;
INSERT INTO `ace_creature_generator_data` (`generatorid`, `weenieClassId`, `probability`) VALUES
	(8, 35440, 50),
	(8, 35441, 40),
	(8, 42437, 10);
/*!40000 ALTER TABLE `ace_creature_generator_data` ENABLE KEYS */;

-- Dumping data for table ace_world.ace_creature_generator_locations: ~0 rows (approximately)
/*!40000 ALTER TABLE `ace_creature_generator_locations` DISABLE KEYS */;
INSERT INTO `ace_creature_generator_locations` (`id`, `generatorId`, `quantity`, `landblock`, `cell`, `posX`, `posY`, `posZ`, `qW`, `qX`, `qY`, `qZ`) VALUES
	(1, 8, 3, 4344, 25, 82.1302, 9.98661, 94.005, 0.983917, 0, 0, 0.178627);
/*!40000 ALTER TABLE `ace_creature_generator_locations` ENABLE KEYS */;

-- Dumping data for table ace_world.ace_creature_static_locations: ~1 rows (approximately)
/*!40000 ALTER TABLE `ace_creature_static_locations` DISABLE KEYS */;
INSERT INTO `ace_creature_static_locations` (`id`, `weenieClassId`, `landblock`, `cell`, `posX`, `posY`, `posZ`, `qW`, `qX`, `qY`, `qZ`) VALUES
	(2, 35442, 43443, 15, 47.7673, 154.315, 94.005, 0.278842, 0, 0, 0.960337);
/*!40000 ALTER TABLE `ace_creature_static_locations` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
