CREATE TABLE IF NOT EXISTS `character_properties_bool` (
  `guid` INT UNSIGNED NOT NULL DEFAULT '0',
  `propertyId` SMALLINT UNSIGNED NOT NULL DEFAULT '0',
  `propertyValue` BOOL NOT NULL DEFAULT '0',
  INDEX (`guid`),
  UNIQUE `character_guid__property_id` (`guid`, `propertyId`)
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS `character_properties_int` (
  `guid` INT UNSIGNED NOT NULL DEFAULT '0',
  `propertyId` SMALLINT UNSIGNED NOT NULL DEFAULT '0',
  `propertyValue` INT UNSIGNED NOT NULL DEFAULT '0',
  INDEX (`guid`),
  UNIQUE `character_guid__property_id` (`guid`, `propertyId`)
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS `character_properties_bigint` (
  `guid` INT UNSIGNED NOT NULL DEFAULT '0',
  `propertyId` SMALLINT UNSIGNED NOT NULL DEFAULT '0',
  `propertyValue` BIGINT UNSIGNED NOT NULL DEFAULT '0',
  INDEX (`guid`),
  UNIQUE `character_guid__property_id` (`guid`, `propertyId`)
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS `character_properties_double` (
  `guid` INT UNSIGNED NOT NULL DEFAULT '0',
  `propertyId` SMALLINT UNSIGNED NOT NULL DEFAULT '0',
  `propertyValue` DOUBLE NOT NULL DEFAULT '0',
  INDEX (`guid`),
  UNIQUE `character_guid__property_id` (`guid`, `propertyId`)
) ENGINE = InnoDB;

CREATE TABLE IF NOT EXISTS `character_properties_string` (
  `guid` INT UNSIGNED NOT NULL DEFAULT '0',
  `propertyId` SMALLINT UNSIGNED NOT NULL DEFAULT '0',
  `propertyValue` TEXT NOT NULL,
  INDEX (`guid`),
  UNIQUE `character_guid__property_id` (`guid`, `propertyId`)
) ENGINE = InnoDB;
