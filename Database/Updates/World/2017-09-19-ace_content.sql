/*
SQLyog Community
MySQL - 10.2.8-MariaDB : Database - ace_world
*********************************************************************
*/

/*!40101 SET NAMES utf8 */;

/*!40101 SET SQL_MODE=''*/;

/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;
/*Table structure for table `ace_content` */

DROP TABLE IF EXISTS `ace_content`;

CREATE TABLE `ace_content` (
  `contentId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `contentName` text NOT NULL,
  `contentType` int(3) unsigned NOT NULL COMMENT 'ACE.Entity.Enum.ContentType',
  PRIMARY KEY (`contentId`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Data for the table `ace_content` */

/*Table structure for table `ace_content_landblock` */

DROP TABLE IF EXISTS `ace_content_landblock`;

CREATE TABLE `ace_content_landblock` (
  `contentId` int(10) unsigned NOT NULL,
  `landblockId` int(10) unsigned NOT NULL COMMENT '0x####0000.  lower word should be all 0s.',
  `comment` text DEFAULT NULL,
  PRIMARY KEY (`contentId`,`landblockId`),
  CONSTRAINT `ace_content_landblock_ibfk_1` FOREIGN KEY (`contentId`) REFERENCES `ace_content` (`contentId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Data for the table `ace_content_landblock` */

/*Table structure for table `ace_content_link` */

DROP TABLE IF EXISTS `ace_content_link`;

CREATE TABLE `ace_content_link` (
  `contentId1` int(10) unsigned NOT NULL,
  `contentId2` int(10) unsigned NOT NULL,
  PRIMARY KEY (`contentId1`,`contentId2`),
  KEY `ace_content_link_ibfk_2` (`contentId2`),
  CONSTRAINT `ace_content_link_ibfk_1` FOREIGN KEY (`contentId1`) REFERENCES `ace_content` (`contentId`) ON DELETE CASCADE,
  CONSTRAINT `ace_content_link_ibfk_2` FOREIGN KEY (`contentId2`) REFERENCES `ace_content` (`contentId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Data for the table `ace_content_link` */

/*Table structure for table `ace_content_resource` */

DROP TABLE IF EXISTS `ace_content_resource`;

CREATE TABLE `ace_content_resource` (
  `contentId` int(10) unsigned NOT NULL,
  `resourceUri` text NOT NULL,
  `comment` text DEFAULT NULL,
  KEY `contentId` (`contentId`),
  CONSTRAINT `ace_content_resource_ibfk_1` FOREIGN KEY (`contentId`) REFERENCES `ace_content` (`contentId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Data for the table `ace_content_resource` */

/*Table structure for table `ace_content_weenie` */

DROP TABLE IF EXISTS `ace_content_weenie`;

CREATE TABLE `ace_content_weenie` (
  `contentId` int(10) unsigned NOT NULL,
  `weenieId` int(10) unsigned NOT NULL,
  `comment` text DEFAULT NULL,
  PRIMARY KEY (`contentId`,`weenieId`),
  KEY `weenieId` (`weenieId`),
  CONSTRAINT `ace_content_weenie_ibfk_1` FOREIGN KEY (`contentId`) REFERENCES `ace_content` (`contentId`) ON DELETE CASCADE,
  CONSTRAINT `ace_content_weenie_ibfk_2` FOREIGN KEY (`weenieId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=latin1;

/*Data for the table `ace_content_weenie` */

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
