ALTER TABLE `character_position`
	ADD COLUMN `positionType` TINYINT UNSIGNED NULL DEFAULT '0' AFTER `cell`;