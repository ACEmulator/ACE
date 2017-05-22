/*
SQLyog Ultimate v12.4.1 (64 bit)
MySQL - 10.1.23-MariaDB : Database - ace_world
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
USE `ace_world`;

/*Table structure for table `ace_portal_object` */

DROP TABLE IF EXISTS `ace_portal_object`;

CREATE TABLE `ace_portal_object` (
  `aceObjectId` int(10) unsigned NOT NULL,
  `positionId` int(10) unsigned NOT NULL,
  `min_lvl` int(11) unsigned NOT NULL DEFAULT '0',
  `max_lvl` int(11) unsigned NOT NULL DEFAULT '0',
  `societyId` tinyint(3) unsigned NOT NULL DEFAULT '0',
  `isTieable` tinyint(1) unsigned NOT NULL DEFAULT '1',
  `isRecallable` tinyint(1) unsigned NOT NULL DEFAULT '1',
  `isSummonable` tinyint(1) unsigned NOT NULL DEFAULT '1',
  PRIMARY KEY (`aceObjectId`),
  KEY `FK_apo2po` (`positionId`),
  CONSTRAINT `FK_apo2ao` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`),
  CONSTRAINT `FK_apo2po` FOREIGN KEY (`positionId`) REFERENCES `ace_position` (`positionId`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
