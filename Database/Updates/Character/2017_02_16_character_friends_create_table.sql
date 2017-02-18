CREATE TABLE `character_friends` (
	`id` INT(10) UNSIGNED NOT NULL,
	`friendId` INT(10) UNSIGNED NOT NULL,
	PRIMARY KEY (`id`, `friendId`),
	INDEX `FK_character_friends_friendId_character_guid` (`friendId`),
	CONSTRAINT `FK_character_friends_friendId_character_guid` FOREIGN KEY (`friendId`) REFERENCES `character` (`guid`),
	CONSTRAINT `FK_character_friends_id_character_guid` FOREIGN KEY (`id`) REFERENCES `character` (`guid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
