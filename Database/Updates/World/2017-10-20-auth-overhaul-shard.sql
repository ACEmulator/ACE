ALTER TABLE `ace_world`.`ace_recipe`   
  CHANGE `recipeId` `recipeGuid` BINARY(16) NOT NULL COMMENT 'surrogate key';

DROP TABLE IF EXISTS `ace_content_weenie`;
DROP TABLE IF EXISTS `ace_content_resource`;
DROP TABLE IF EXISTS `ace_content_landblock`;
DROP TABLE IF EXISTS `ace_content_link`;
DROP TABLE IF EXISTS `ace_content`;
DROP VIEW IF EXISTS `vw_weenie_search`;

CREATE TABLE `ace_content` (
  `contentGuid` BINARY(16) NOT NULL,
  `contentName` TEXT NOT NULL,
  `contentType` INT(3) UNSIGNED NULL COMMENT 'ACE.Entity.Enum.ContentType',
  `userModified` TINYINT(1) NOT NULL DEFAULT 0,
  PRIMARY KEY (`contentGuid`)
) ENGINE=INNODB DEFAULT CHARSET=latin1;

CREATE TABLE `ace_content_landblock` (
  `contentLandblockGuid` BINARY(16) NOT NULL,
  `contentGuid` BINARY(16) NOT NULL,
  `landblockId` INT(10) UNSIGNED NOT NULL COMMENT '0x####0000.  lower word should be all 0s.',
  `comment` TEXT DEFAULT NULL,
  PRIMARY KEY (`contentLandblockGuid`),
  KEY (`contentGuid`,`landblockId`),
  CONSTRAINT `ace_content_landblock_ibfk_1` FOREIGN KEY (`contentGuid`) REFERENCES `ace_content` (`contentGuid`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=latin1;

CREATE TABLE `ace_content_link` (
  `contentGuid1` BINARY(16) NOT NULL,
  `contentGuid2` BINARY(16) NOT NULL,
  PRIMARY KEY (`contentGuid1`,`contentGuid2`),
  KEY `ace_content_link_ibfk_2` (`contentGuid2`),
  CONSTRAINT `ace_content_link_ibfk_1` FOREIGN KEY (`contentGuid1`) REFERENCES `ace_content` (`contentGuid`) ON DELETE CASCADE,
  CONSTRAINT `ace_content_link_ibfk_2` FOREIGN KEY (`contentGuid2`) REFERENCES `ace_content` (`contentGuid`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=latin1;

CREATE TABLE `ace_content_resource` (
  `contentResourceGuid` BINARY(16) NOT NULL,
  `contentGuid` BINARY(16) NOT NULL,
  `name` TEXT NOT NULL,
  `resourceUri` TEXT NOT NULL,
  `comment` TEXT DEFAULT NULL,
  PRIMARY KEY (`contentResourceGuid`),
  CONSTRAINT `ace_content_resource_ibfk_1` FOREIGN KEY (`contentGuid`) REFERENCES `ace_content` (`contentGuid`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=latin1;

CREATE TABLE `ace_content_weenie` (
  `contentWeenieGuid` BINARY(16) NOT NULL,
  `contentGuid` BINARY(16) NOT NULL,
  `weenieId` INT(10) UNSIGNED NOT NULL,
  `comment` TEXT DEFAULT NULL,
  PRIMARY KEY (`contentWeenieGuid`),
  CONSTRAINT `ace_content_weenie_ibfk_1` FOREIGN KEY (`contentGuid`) REFERENCES `ace_content` (`contentGuid`) ON DELETE CASCADE,
  CONSTRAINT `ace_content_weenie_ibfk_2` FOREIGN KEY (`weenieId`) REFERENCES `ace_object` (`aceObjectId`) ON DELETE CASCADE
) ENGINE=INNODB DEFAULT CHARSET=latin1;

ALTER TABLE `ace_world`.`ace_object`   
  ADD COLUMN `userModified` TINYINT(1) DEFAULT 0 NOT NULL COMMENT 'flag indicating whether or not this has record has been altered since deployment' AFTER `weenieClassId`;

ALTER TABLE `ace_world`.`ace_recipe`   
  ADD COLUMN `userModified` TINYINT(1) DEFAULT 0 NOT NULL COMMENT 'flag indicating whether or not this has record has been altered since deployment' AFTER `recipeType`;


CREATE VIEW `ace_world`.`vw_weenie_search` 
    AS
(SELECT AO.aceObjectId, AO.userModified, AO.weenieClassId, WC.weenieClassDescription, `names`.propertyValue `name`, `itemType`.propertyValue `itemType`, `weenieType`.propertyValue `weenieType`
FROM `ace_object` AO
    LEFT JOIN `ace_weenie_class` WC ON AO.aceObjectId = WC.weenieClassId
    LEFT JOIN ace_object_properties_string `names` ON AO.aceObjectId = `names`.aceObjectId AND `names`.strPropertyId = 1
    LEFT JOIN ace_object_properties_int `weenieType` ON AO.aceObjectId = `weenieType`.aceObjectId AND `weenieType`.intPropertyId = 9007
    LEFT JOIN ace_object_properties_int `itemType` ON AO.aceObjectId = `itemType`.aceObjectId AND `itemType`.intPropertyId = 1
WHERE AO.aceObjectId = AO.weenieClassId`vw_weenie_search`
);
`ace_shard`