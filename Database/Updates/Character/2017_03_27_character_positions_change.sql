ALTER TABLE `character_position`
CHANGE COLUMN `id` `character_id` INT(10) UNSIGNED NOT NULL DEFAULT '0' FIRST;
ALTER TABLE `character_position`
	CHANGE COLUMN `positionType` `positionType` TINYINT(3) UNSIGNED NOT NULL DEFAULT '0' BEFORE `cell`,
	DROP PRIMARY KEY,
	ADD PRIMARY KEY (`character_id`, `positionType`);
ALTER TABLE `character_position`
	ALTER `character_id` DROP DEFAULT,
	ALTER `positionType` DROP DEFAULT;
ALTER TABLE `character_position`
	CHANGE COLUMN `character_id` `character_id` INT(10) UNSIGNED NOT NULL FIRST,
	CHANGE COLUMN `positionType` `positionType` TINYINT(3) UNSIGNED NOT NULL BEFORE `cell`;