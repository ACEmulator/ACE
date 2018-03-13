USE `ace_shard`;

DROP TABLE IF EXISTS `biota_properties_title_book`;

CREATE TABLE `biota_properties_title_book` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `title_Id` int(10) NOT NULL DEFAULT '0' COMMENT 'Id of Title',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_titlebook_type_uidx` (`object_Id`,`title_Id`),
  KEY `wcid_titlebook_idx` (`object_Id`),
  CONSTRAINT `wcid_titlebook` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='TitleBook Properties of Weenies';
