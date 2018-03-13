USE `ace_shard`;

DROP TABLE IF EXISTS `biota_properties_enchantment_registry`;

CREATE TABLE `biota_properties_enchantment_registry` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `enchantment_Category` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Which PackableList this Enchantment goes in (enchantmentMask)',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `spell_Id` int(10) NOT NULL DEFAULT '0' COMMENT 'Id of Spell',
  `layer_Id` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of Layer',
  `has_Spell_Set_Id` bit(1) NOT NULL DEFAULT '0' COMMENT 'Has Spell Set Id?',
  `spell_Category` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Category of Spell',
  `power_Level` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Power Level of Spell',
  `start_Time` double NOT NULL DEFAULT '0' COMMENT 'the amount of time this enchantment has been active',
  `duration` double NOT NULL DEFAULT '0' COMMENT 'the duration of the spell',
  `caster_Object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object that cast this spell',
  `degrade_Modifier` float NOT NULL DEFAULT '0' COMMENT '???',
  `degrade_Limit` float NOT NULL DEFAULT '0' COMMENT '???',
  `last_Time_Degraded` double NOT NULL DEFAULT '0' COMMENT 'the time when this enchantment was cast',
  `stat_Mod_Type` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'flags that indicate the type of effect the spell has',
  `stat_Mod_Key` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'along with flags, indicates which attribute is affected by the spell',
  `stat_Mod_Value` float NOT NULL DEFAULT '0' COMMENT 'the effect value/amount',
  `spell_Set_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the Spell Set for this spell',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_enchantmentregistry_objectId_spellId_layerId_uidx` (`object_Id`,`spell_Id`,`layer_Id`),
  KEY `wcid_enchantmentregistry_idx` (`object_Id`),
  CONSTRAINT `wcid_enchantmentregistry` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Enchantment Registry Properties of Weenies';
