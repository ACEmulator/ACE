USE `ace_shard`;

ALTER TABLE `character` ADD COLUMN `spellbook_Filters` int(10) unsigned NOT NULL DEFAULT '16383' AFTER `gameplay_Options`;