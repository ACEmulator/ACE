USE `ace_shard`;

DROP TABLE IF EXISTS `character_properties_squelch`;

CREATE TABLE `character_properties_squelch` (

	`character_Id` int(10) unsigned NOT NULL,
	`squelch_Character_Id` int(10) unsigned NOT NULL,
	`squelch_Account_Id` int(10) unsigned NOT NULL,
	`type` int(10) unsigned NOT NULL,
	
	PRIMARY KEY `character_Id_squelch_Character_Id_uidx` (`character_Id`, `squelch_Character_Id`),
	
	CONSTRAINT `squelch_character_Id_constraint` FOREIGN KEY (`character_Id`) REFERENCES `character` (`id`) ON DELETE CASCADE

) ENGINE=InnoDB DEFAULT CHARSET=utf8;
