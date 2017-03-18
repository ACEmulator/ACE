ALTER TABLE `character_stats` 
CHANGE COLUMN `coordinationXpSpent` `coordinationXpSpent` INT(10) UNSIGNED NOT NULL DEFAULT '0' ,
CHANGE COLUMN `healthCurrent` `healthCurrent` SMALLINT(2) UNSIGNED NOT NULL DEFAULT '0' ,
CHANGE COLUMN `staminaCurrent` `staminaCurrent` SMALLINT(2) UNSIGNED NOT NULL DEFAULT '0' ,
CHANGE COLUMN `manaCurrent` `manaCurrent` SMALLINT(2) UNSIGNED NOT NULL DEFAULT '0' ;