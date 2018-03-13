USE `ace_shard`;

DROP TABLE IF EXISTS `biota_properties_fill_comp_book`;

CREATE TABLE `biota_properties_fill_comp_book` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `spell_Component_Id` int(10) NOT NULL DEFAULT '0' COMMENT 'Id of Spell Component',
  `quantity_To_Rebuy` int(10) NOT NULL DEFAULT '0' COMMENT 'Amount of this component to add to the buy list for repurchase',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_fillcompbook_type_uidx` (`object_Id`,`spell_Component_Id`),
  KEY `wcid_fillcompbook_idx` (`object_Id`),
  CONSTRAINT `wcid_fillcompbook` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='FillCompBook Properties of Weenies';
