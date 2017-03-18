ALTER TABLE `character_stats` 
CHANGE COLUMN `quicknessXpSspent` `quicknessXpSpent` INT(10) UNSIGNED NOT NULL DEFAULT '0';

ALTER TABLE `character_skills` 
ADD COLUMN `skillXpSpent` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `skillPoints`;