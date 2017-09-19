
DROP TABLE IF EXISTS `ace_recipe`;

CREATE TABLE `ace_recipe` (
  `recipeId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT COMMENT 'surrogate key',
  `recipeType` TINYINT(3) UNSIGNED NOT NULL COMMENT 'see RecipeType enum in code',
  `sourceWcid` INT(10) UNSIGNED NOT NULL COMMENT 'the object being used',
  `targetWcid` INT(10) UNSIGNED NOT NULL COMMENT 'the target of use',
  `skillId` SMALLINT(6) UNSIGNED DEFAULT NULL COMMENT 'skill required for the formula, if any',
  `skillDifficulty` SMALLINT(6) UNSIGNED DEFAULT NULL COMMENT 'skill value required for 50% success',
  `partialFailDifficulty` SMALLINT(6) UNSIGNED DEFAULT NULL COMMENT 'skill value for a partial botch (dyed clothing)',
  `successMessage` TEXT DEFAULT NULL,
  `failMessage` TEXT DEFAULT NULL,
  `alternateMessage` TEXT DEFAULT NULL,
  `resultFlags` INT(10) UNSIGNED DEFAULT NULL COMMENT 'bitmask of what happens.  see RecipeResults enum in code',
  `successItem1Wcid` INT(10) UNSIGNED DEFAULT NULL,
  `successItem2Wcid` INT(10) UNSIGNED DEFAULT NULL,
  `failureItem1Wcid` INT(10) UNSIGNED DEFAULT NULL,
  `failureItem2Wcid` INT(10) UNSIGNED DEFAULT NULL,
  `healingAttribute` SMALLINT(6) UNSIGNED DEFAULT NULL COMMENT 'used by recipeType = Healing. health = 64, stam = 128, mana = 256. if null, will default to health. source enum: ACE.Entity.Enum.Ability',
  PRIMARY KEY (`recipeId`)
) ENGINE=INNODB AUTO_INCREMENT=3 DEFAULT CHARSET=latin1;

