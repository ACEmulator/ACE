USE `ace_world`;

DROP TABLE IF EXISTS `ace_recipe`;

CREATE TABLE IF NOT EXISTS `recipe` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe instance',
  `recipe_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe',
  `unknown_1` int(10) unsigned NOT NULL DEFAULT '0',
  `skill` int(10) unsigned NOT NULL DEFAULT '0',
  `difficulty` int(10) unsigned NOT NULL DEFAULT '0',
  `salvage_Type` int(10) unsigned NOT NULL DEFAULT '0',
  `success_W_C_I_D` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Weenie Class Id of object to create upon successful application of this recipe',
  `success_Amount` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Amount of objects to create upon successful application of this recipe',
  `success_Message` text NOT NULL,
  `fail_W_C_I_D` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Weenie Class Id of object to create upon failing application of this recipe',
  `fail_Amount` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Amount of objects to create upon failing application of this recipe',
  `fail_Message` text NOT NULL,
  `data_Id` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `recipe_uidx` (`recipe_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipes';

CREATE TABLE IF NOT EXISTS `recipe_component` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Component instance',
  `recipe_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe',
  `percent` double NOT NULL DEFAULT '0',
  `unknown_2` int(10) unsigned NOT NULL DEFAULT '0',
  `message` text NOT NULL,
  PRIMARY KEY (`id`),
  KEY `recipe_idx` (`recipe_Id`),
  CONSTRAINT `recipeId_component` FOREIGN KEY (`recipe_Id`) REFERENCES `recipe` (`recipe_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe Components';

CREATE TABLE IF NOT EXISTS `recipe_mod` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Mod instance',
  `recipe_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe',
  `mod_Set_Id` int(5) NOT NULL DEFAULT '0',
  `health` int(10) NOT NULL DEFAULT '0',
  `unknown_2` int(10) NOT NULL DEFAULT '0',
  `mana` int(10) NOT NULL DEFAULT '0',
  `unknown_4` int(10) NOT NULL DEFAULT '0',
  `unknown_5` int(10) NOT NULL DEFAULT '0',
  `unknown_6` int(10) NOT NULL DEFAULT '0',
  `unknown_7` bit(1) NOT NULL,
  `data_Id` int(10) NOT NULL DEFAULT '0',
  `unknown_9` int(10) NOT NULL DEFAULT '0',
  `instance_Id` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `recipe_modset_uidx` (`recipe_Id`,`mod_Set_Id`),
  KEY `recipe_idx` (`recipe_Id`),
  CONSTRAINT `recipeId_Mod` FOREIGN KEY (`recipe_Id`) REFERENCES `recipe` (`recipe_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe Mods';

CREATE TABLE IF NOT EXISTS `recipe_mods_bool` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Mod instance',
  `recipe_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe',
  `mod_Set_Id` int(5) NOT NULL DEFAULT '0',
  `stat` int(10) NOT NULL DEFAULT '0',
  `value` bit(1) NOT NULL,
  `enum` int(10) NOT NULL DEFAULT '0',
  `unknown_1` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `recipe_idx` (`recipe_Id`),
  CONSTRAINT `recipeId_mod_bool` FOREIGN KEY (`recipe_Id`) REFERENCES `recipe` (`recipe_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe Bool Mods';

CREATE TABLE IF NOT EXISTS `recipe_mods_d_i_d` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Mod instance',
  `recipe_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe',
  `mod_Set_Id` int(5) NOT NULL DEFAULT '0',
  `stat` int(10) NOT NULL DEFAULT '0',
  `value` int(10) unsigned NOT NULL DEFAULT '0',
  `enum` int(10) NOT NULL DEFAULT '0',
  `unknown_1` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `recipe_idx` (`recipe_Id`),
  CONSTRAINT `recipeId_mod_did` FOREIGN KEY (`recipe_Id`) REFERENCES `recipe` (`recipe_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe DID Mods';

CREATE TABLE IF NOT EXISTS `recipe_mods_float` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Mod instance',
  `recipe_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe',
  `mod_Set_Id` int(5) NOT NULL DEFAULT '0',
  `stat` int(10) NOT NULL DEFAULT '0',
  `value` double NOT NULL DEFAULT '0',
  `enum` int(10) NOT NULL DEFAULT '0',
  `unknown_1` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `recipe_idx` (`recipe_Id`),
  CONSTRAINT `recipeId_mod_float` FOREIGN KEY (`recipe_Id`) REFERENCES `recipe` (`recipe_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe Float Mods';

CREATE TABLE IF NOT EXISTS `recipe_mods_i_i_d` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Mod instance',
  `recipe_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe',
  `mod_Set_Id` int(5) NOT NULL DEFAULT '0',
  `stat` int(10) NOT NULL DEFAULT '0',
  `value` int(10) unsigned NOT NULL DEFAULT '0',
  `enum` int(10) NOT NULL DEFAULT '0',
  `unknown_1` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `recipe_idx` (`recipe_Id`),
  CONSTRAINT `recipeId_mod_iid` FOREIGN KEY (`recipe_Id`) REFERENCES `recipe` (`recipe_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe IID Mods';

CREATE TABLE IF NOT EXISTS `recipe_mods_int` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Mod instance',
  `recipe_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe',
  `mod_Set_Id` int(5) NOT NULL DEFAULT '0',
  `stat` int(10) NOT NULL DEFAULT '0',
  `value` int(10) NOT NULL DEFAULT '0',
  `enum` int(10) NOT NULL DEFAULT '0',
  `unknown_1` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `recipe_idx` (`recipe_Id`),
  CONSTRAINT `recipeId_mod_int` FOREIGN KEY (`recipe_Id`) REFERENCES `recipe` (`recipe_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe Int Mods';

CREATE TABLE IF NOT EXISTS `recipe_mods_string` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Mod instance',
  `recipe_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe',
  `mod_Set_Id` int(5) NOT NULL DEFAULT '0',
  `stat` int(10) NOT NULL DEFAULT '0',
  `value` text NOT NULL,
  `enum` int(10) NOT NULL DEFAULT '0',
  `unknown_1` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `recipe_idx` (`recipe_Id`),
  CONSTRAINT `recipeId_mod_string` FOREIGN KEY (`recipe_Id`) REFERENCES `recipe` (`recipe_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe String Mods';

CREATE TABLE IF NOT EXISTS `recipe_requirements_bool` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Requirement instance',
  `recipe_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe',
  `stat` int(10) NOT NULL DEFAULT '0',
  `value` bit(1) NOT NULL,
  `enum` int(10) NOT NULL DEFAULT '0',
  `message` text NOT NULL,
  PRIMARY KEY (`id`),
  KEY `recipe_idx` (`recipe_Id`),
  CONSTRAINT `recipeId_req_bool` FOREIGN KEY (`recipe_Id`) REFERENCES `recipe` (`recipe_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe Bool Requirments';

CREATE TABLE IF NOT EXISTS `recipe_requirements_d_i_d` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Requirement instance',
  `recipe_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe',
  `stat` int(10) NOT NULL DEFAULT '0',
  `value` int(10) unsigned NOT NULL DEFAULT '0',
  `enum` int(10) NOT NULL DEFAULT '0',
  `message` text NOT NULL,
  PRIMARY KEY (`id`),
  KEY `recipe_idx` (`recipe_Id`),
  CONSTRAINT `recipeId_req_did` FOREIGN KEY (`recipe_Id`) REFERENCES `recipe` (`recipe_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe DID Requirments';

CREATE TABLE IF NOT EXISTS `recipe_requirements_float` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Requirement instance',
  `recipe_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe',
  `stat` int(10) NOT NULL DEFAULT '0',
  `value` double NOT NULL DEFAULT '0',
  `enum` int(10) NOT NULL DEFAULT '0',
  `message` text NOT NULL,
  PRIMARY KEY (`id`),
  KEY `recipe_idx` (`recipe_Id`),
  CONSTRAINT `recipeId_req_float` FOREIGN KEY (`recipe_Id`) REFERENCES `recipe` (`recipe_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe Float Requirments';

CREATE TABLE IF NOT EXISTS `recipe_requirements_i_i_d` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Requirement instance',
  `recipe_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe',
  `stat` int(10) NOT NULL DEFAULT '0',
  `value` int(10) unsigned NOT NULL DEFAULT '0',
  `enum` int(10) NOT NULL DEFAULT '0',
  `message` text NOT NULL,
  PRIMARY KEY (`id`),
  KEY `recipe_idx` (`recipe_Id`),
  CONSTRAINT `recipeId_req_iid` FOREIGN KEY (`recipe_Id`) REFERENCES `recipe` (`recipe_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe IID Requirments';

CREATE TABLE IF NOT EXISTS `recipe_requirements_int` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Requirement instance',
  `recipe_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe',
  `stat` int(10) NOT NULL DEFAULT '0',
  `value` int(10) NOT NULL DEFAULT '0',
  `enum` int(10) NOT NULL DEFAULT '0',
  `message` text NOT NULL,
  PRIMARY KEY (`id`),
  KEY `recipe_idx` (`recipe_Id`),
  CONSTRAINT `recipeId_req_int` FOREIGN KEY (`recipe_Id`) REFERENCES `recipe` (`recipe_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe Int Requirments';

CREATE TABLE IF NOT EXISTS `recipe_requirements_string` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Requirement instance',
  `recipe_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe',
  `stat` int(10) NOT NULL DEFAULT '0',
  `value` text NOT NULL,
  `enum` int(10) NOT NULL DEFAULT '0',
  `message` text NOT NULL,
  PRIMARY KEY (`id`),
  KEY `recipe_idx` (`recipe_Id`),
  CONSTRAINT `recipeId_req_string` FOREIGN KEY (`recipe_Id`) REFERENCES `recipe` (`recipe_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe String Requirments';

CREATE TABLE IF NOT EXISTS `cook_book` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this cook book instance',
  `recipe_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe',
  `target_W_C_I_D` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Weenie Class Id of the target object for this recipe',
  `source_W_C_I_D` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Weenie Class Id of the source object for this recipe',
  PRIMARY KEY (`id`),
  UNIQUE KEY `recipe_target_source_uidx` (`recipe_Id`,`target_W_C_I_D`,`source_W_C_I_D`),
  KEY `source_idx` (`source_W_C_I_D`),
  KEY `target_idx` (`target_W_C_I_D`),
  CONSTRAINT `cookbook_recipe` FOREIGN KEY (`recipe_Id`) REFERENCES `recipe` (`recipe_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Cook Book for Recipes';
