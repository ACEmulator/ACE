USE `ace_shard`;

CREATE TABLE IF NOT EXISTS `biota_properties_quest_registry` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `quest_Name` varchar(255) NOT NULL COMMENT 'Unique Name of Quest',
  `last_Time_Completed` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Timestamp of last successful completion',
  `num_Times_Completed` int(10) NOT NULL DEFAULT '0' COMMENT 'Number of successful completions',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_questbook_name_uidx` (`object_Id`,`quest_Name`),
  KEY `wcid_questbook_idx` (`object_Id`),
  CONSTRAINT `wcid_questbook` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='QuestBook Properties of Weenies';
