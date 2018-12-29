USE `ace_shard`;

DROP TABLE IF EXISTS `house_permission`;

CREATE TABLE `house_permission` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `house_Id` int(10) unsigned NOT NULL COMMENT 'GUID of House Biota Object',
  `player_Guid` int(10) unsigned NOT NULL COMMENT 'GUID of Player Biota Object being granted permission to this house',
  `storage` bit(1) NOT NULL COMMENT 'Permission includes access to House Storage',
  PRIMARY KEY (`id`),
  UNIQUE KEY `biota_Id_house_Id_player_Guid_uidx` (`house_Id`,`player_Guid`),
  KEY `biota_Id_house_Id_idx` (`house_Id`),
  CONSTRAINT `biota_Id_house_Id` FOREIGN KEY (`house_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
