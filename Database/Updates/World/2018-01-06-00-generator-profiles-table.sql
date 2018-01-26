USE `ace_world`;

CREATE TABLE `ace_object_generator_profile` (
  `profileId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `aceObjectId` int(10) unsigned NOT NULL DEFAULT '0',
  `probability` float NOT NULL DEFAULT '1',
  `weenieClassId` int(10) unsigned NOT NULL DEFAULT '0',
  `delay` float DEFAULT '0',
  `initCreate` int(10) unsigned NOT NULL DEFAULT '1',
  `maxCreate` int(10) unsigned NOT NULL DEFAULT '1',
  `whenCreate` int(10) unsigned NOT NULL DEFAULT '2',
  `whereCreate` int(10) unsigned NOT NULL DEFAULT '4',
  `stackSize` int(10) DEFAULT NULL,
  `paletteId` int(10) unsigned DEFAULT NULL,
  `shade` float DEFAULT NULL,
  `landblockRaw` int(10) unsigned DEFAULT NULL,
  `posX` float DEFAULT NULL,
  `posY` float DEFAULT NULL,
  `posZ` float DEFAULT NULL,
  `qW` float DEFAULT NULL,
  `qX` float DEFAULT NULL,
  `qY` float DEFAULT NULL,
  `qZ` float DEFAULT NULL,
  PRIMARY KEY (`profileId`),
  KEY `fk_gen_ao_idx` (`aceObjectId`),
  KEY `fk_gen_weenie_idx` (`weenieClassId`),
  CONSTRAINT `fk_gen_ao` FOREIGN KEY (`aceObjectId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE,
  CONSTRAINT `fk_gen_weenie` FOREIGN KEY (`weenieClassId`) REFERENCES `ace_weenie_class` (`weenieClassId`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
