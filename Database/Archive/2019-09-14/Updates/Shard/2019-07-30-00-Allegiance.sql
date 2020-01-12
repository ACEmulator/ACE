USE `ace_shard`;

DROP TABLE IF EXISTS `biota_properties_allegiance`;

CREATE TABLE `biota_properties_allegiance` (

	`allegiance_Id` int(10) unsigned NOT NULL,
	`character_Id` int(10) unsigned NOT NULL,
	`banned` bit NOT NULL,
	`approved_Vassal` bit NOT NULL,
	
	PRIMARY KEY `allegiance_Id_character_Id_uidx` (`allegiance_Id`, `character_Id`),
	
	CONSTRAINT `FK_allegiance_biota_Id` FOREIGN KEY (`allegiance_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE,
	CONSTRAINT `FK_allegiance_character_Id` FOREIGN KEY (`character_Id`) REFERENCES `character` (`id`) ON DELETE CASCADE

) ENGINE=InnoDB DEFAULT CHARSET=utf8;
