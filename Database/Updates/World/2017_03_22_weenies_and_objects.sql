DROP VIEW IF EXISTS `vw_ace_object`;
DROP TABLE IF EXISTS `ace_object_palette_changes`;
DROP TABLE IF EXISTS `ace_object_texture_map_changes`;
DROP TABLE IF EXISTS `ace_object_animation_changes`;
DROP TABLE IF EXISTS `ace_object`;

DROP TABLE IF EXISTS `weenie_palette_changes`;
DROP TABLE IF EXISTS `weenie_animation_changes`;
DROP TABLE IF EXISTS `weenie_texture_map_changes`;
DROP TABLE IF EXISTS `weenie_creature_data`;
DROP TABLE IF EXISTS `weenie_class`;

DROP TABLE IF EXISTS `base_ace_object`;

CREATE TABLE `base_ace_object` (
  `baseAceObjectId` INT(10) UNSIGNED NOT NULL,
  `name` TEXT NOT NULL,
  `typeId` INT(10) UNSIGNED NOT NULL,
  `paletteId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  
  -- wdesc sourced data
  `ammoType` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `blipColor` TINYINT(3) UNSIGNED NOT NULL DEFAULT 0,
  `bitField` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `burden` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `combatUse` TINYINT(3) UNSIGNED NOT NULL DEFAULT 0,
  `cooldownDuration` DOUBLE NOT NULL DEFAULT 0,
  `cooldownId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `effects` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `containersCapacity` TINYINT(3) UNSIGNED NOT NULL DEFAULT 0,
  `header` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `hookTypeId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `iconId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `iconOverlayId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `iconUnderlayId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `hookItemTypes` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `itemsCapacity` TINYINT(3) UNSIGNED NOT NULL DEFAULT 0,
  `location` TINYINT(3) UNSIGNED NOT NULL DEFAULT 0,
  `materialType` TINYINT(3) UNSIGNED NOT NULL DEFAULT 0,
  `maxStackSize` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `maxStructure` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `radar` TINYINT(3) UNSIGNED NOT NULL DEFAULT 0,
  `pscript` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `spellId` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `stackSize` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `structure` SMALLINT(5) UNSIGNED NOT NULL DEFAULT 0,
  `targetTypeId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `usability` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `useRadius` FLOAT NOT NULL DEFAULT 0,
  `validLocations` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `value` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `workmanship` FLOAT NOT NULL DEFAULT 0,
  
  -- physicsdesc sourced data
  `animationFrameId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `defaultScript` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `defaultScriptIntensity` FLOAT NOT NULL DEFAULT 0,
  `elasticity` FLOAT NOT NULL DEFAULT 0,
  `friction` FLOAT NOT NULL DEFAULT 0,
  `locationId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `modelTableId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `motionTableId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `objectScale` FLOAT NOT NULL DEFAULT 0,
  `physicsBitField` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `physicsState` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `physicsTableId` INT(10) UNSIGNED NOT NULL DEFAULT 0,  
  `soundTableId` INT(10) UNSIGNED NOT NULL DEFAULT 0,
  `translucency` FLOAT NOT NULL DEFAULT 0,
  PRIMARY KEY (`baseAceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

CREATE TABLE `weenie_class` (
  `weenieClassId` SMALLINT(5) UNSIGNED NOT NULL,
  `baseAceObjectId` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`weenieClassId`),
  CONSTRAINT `FK_weenie_class__baseAceObjectId` FOREIGN KEY (`baseAceObjectId`) REFERENCES `base_ace_object` (`baseAceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `weenie_palette_changes` (
  `weenieClassId` SMALLINT(5) UNSIGNED NOT NULL,
  `subPaletteId` INT(10) UNSIGNED NOT NULL,
  `offset` SMALLINT(5) UNSIGNED NOT NULL,
  `length` SMALLINT(5) UNSIGNED NOT NULL,
  PRIMARY KEY (`weenieClassId`,`subPaletteId`),
  CONSTRAINT `FK_weenie_palette_data__weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `weenie_class` (`weenieClassId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `weenie_texture_map_changes` (
  `weenieClassId` SMALLINT(5) UNSIGNED NOT NULL,
  `index` TINYINT(3) UNSIGNED NOT NULL,
  `oldId` INT(10) UNSIGNED NOT NULL,
  `newId` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`weenieClassId`,`index`),
  CONSTRAINT `FK_weenie_texture_map_changes__weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `weenie_class` (`weenieClassId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `weenie_animation_changes` (
  `weenieClassId` SMALLINT(5) UNSIGNED NOT NULL,
  `index` TINYINT(3) UNSIGNED NOT NULL,
  `animationId` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`weenieClassId`,`index`),
  CONSTRAINT `FK_weenie_animation_changes__weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `weenie_class` (`weenieClassId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `weenie_creature_data` (
  `weenieClassId` SMALLINT(5) UNSIGNED NOT NULL,
  `level` INT(10) UNSIGNED NOT NULL,
  `strength` INT(10) UNSIGNED NOT NULL,
  `endurance` INT(10) UNSIGNED NOT NULL,
  `coordination` INT(10) UNSIGNED NOT NULL,
  `quickness` INT(10) UNSIGNED NOT NULL,
  `focus` INT(10) UNSIGNED NOT NULL,
  `self` INT(10) UNSIGNED NOT NULL,
  `health` INT(10) UNSIGNED NOT NULL,
  `stamina` INT(10) UNSIGNED NOT NULL,
  `mana` INT(10) UNSIGNED NOT NULL,
  `baseExperience` INT(10) UNSIGNED NOT NULL,
  `luminance` TINYINT(3) UNSIGNED NOT NULL DEFAULT 0,
  `lootTier` TINYINT(3) UNSIGNED NOT NULL DEFAULT 1,
  PRIMARY KEY (`weenieClassId`),
  CONSTRAINT `FK_weenie_creature_data__weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `weenie_class` (`weenieClassId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `ace_object` (
  `baseAceObjectId` INT(10) UNSIGNED NOT NULL,
  `weenieClassId` SMALLINT(5) UNSIGNED NOT NULL,
  `landblock` SMALLINT(5) UNSIGNED NULL,
  `cell` SMALLINT(5) UNSIGNED NULL,
  `posX` FLOAT NULL,
  `posY` FLOAT NULL,
  `posZ` FLOAT NULL,
  `qW` FLOAT NULL,
  `qX` FLOAT NULL,
  `qY` FLOAT NULL,
  `qZ` FLOAT NULL,
  PRIMARY KEY (`baseAceObjectId`),
  CONSTRAINT `FK_ace_object__weenieClassId` FOREIGN KEY (`weenieClassId`) REFERENCES `weenie_class` (`weenieClassId`),  
  CONSTRAINT `FK_ace_object__baseAceObjectId` FOREIGN KEY (`baseAceObjectId`) REFERENCES `base_ace_object` (`baseAceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `ace_object_palette_changes` (
  `baseAceObjectId` INT(10) UNSIGNED NOT NULL,
  `subPaletteId` INT(10) UNSIGNED NOT NULL,
  `offset` SMALLINT(5) UNSIGNED NOT NULL,
  `length` SMALLINT(5) UNSIGNED NOT NULL,
  PRIMARY KEY (`baseAceObjectId`,`subPaletteId`),
  CONSTRAINT `FK_ace_object_palette_data__baseAceObjectId` FOREIGN KEY (`baseAceObjectId`) REFERENCES `ace_object` (`baseAceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `ace_object_texture_map_changes` (
  `baseAceObjectId` INT(10) UNSIGNED NOT NULL,
  `index` TINYINT(3) UNSIGNED NOT NULL,
  `oldId` INT(10) UNSIGNED NOT NULL,
  `newId` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`baseAceObjectId`,`index`),
  CONSTRAINT `FK_ace_object_texture_map_changes__baseAceObjectId` FOREIGN KEY (`baseAceObjectId`) REFERENCES `ace_object` (`baseAceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

CREATE TABLE IF NOT EXISTS `ace_object_animation_changes` (
  `baseAceObjectId` INT(10) UNSIGNED NOT NULL,
  `index` TINYINT(3) UNSIGNED NOT NULL,
  `animationId` INT(10) UNSIGNED NOT NULL,
  PRIMARY KEY (`baseAceObjectId`,`index`),
  CONSTRAINT `FK_ace_object_animation_changes__baseAceObjectId` FOREIGN KEY (`baseAceObjectId`) REFERENCES `ace_object` (`baseAceObjectId`)
) ENGINE=INNODB DEFAULT CHARSET=utf8;

CREATE OR REPLACE VIEW `vw_ace_object` AS (
  SELECT BAO.*, AO.weenieClassId, AO.landblock, AO.cell, AO.posX, AO.posY, AO.posZ, AO.qW, AO.qX, AO.qY, AO.qZ 
  FROM ace_object AO
  INNER JOIN base_ace_object BAO ON AO.baseAceObjectId = BAO.baseAceObjectId);
