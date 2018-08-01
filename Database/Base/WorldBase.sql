-- MySQL dump 10.13  Distrib 5.7.20, for Win64 (x86_64)
--
-- Host: localhost    Database: ace_world
-- ------------------------------------------------------
-- Server version	5.7.20-log

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `ace_world`
--

/*!40000 DROP DATABASE IF EXISTS `ace_world`*/;

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `ace_world` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `ace_world`;

--
-- Table structure for table `cook_book`
--

DROP TABLE IF EXISTS `cook_book`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `cook_book` (
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `encounter`
--

DROP TABLE IF EXISTS `encounter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `encounter` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Encounter',
  `landblock` int(5) NOT NULL DEFAULT '0' COMMENT 'Landblock for this Encounter',
  `weenie_Class_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Weenie Class Id of generator/object to spawn for Encounter',
  `cell_X` int(5) NOT NULL DEFAULT '0' COMMENT 'CellX position of this Encounter',
  `cell_Y` int(5) NOT NULL DEFAULT '0' COMMENT 'CellY position of this Encounter',
  PRIMARY KEY (`id`),
  UNIQUE KEY `landblock_cellx_celly_uidx` (`landblock`,`cell_X`,`cell_Y`),
  KEY `landblock_idx` (`landblock`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Encounters';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `event`
--

DROP TABLE IF EXISTS `event`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `event` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Event',
  `name` varchar(255) NOT NULL COMMENT 'Unique Event of Quest',
  `start_Time` int(10) NOT NULL DEFAULT '-1' COMMENT 'Unixtime of Event Start',
  `end_Time` int(10) NOT NULL DEFAULT '-1' COMMENT 'Unixtime of Event End',
  `state` int(10) NOT NULL DEFAULT '0' COMMENT 'State of Event (GameEventState)',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Events';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `house_portal`
--

DROP TABLE IF EXISTS `house_portal`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `house_portal` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this House Portal',
  `house_Id` int(10) unsigned NOT NULL COMMENT 'Unique Id of House',
  `obj_Cell_Id` int(10) unsigned NOT NULL,
  `origin_X` float NOT NULL,
  `origin_Y` float NOT NULL,
  `origin_Z` float NOT NULL,
  `angles_W` float NOT NULL,
  `angles_X` float NOT NULL,
  `angles_Y` float NOT NULL,
  `angles_Z` float NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `house_Id_UNIQUE` (`house_Id`,`obj_Cell_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='House Portal Destinations';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `landblock_instances`
--

DROP TABLE IF EXISTS `landblock_instances`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `landblock_instances` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Instance',
  `landblock` int(5) GENERATED ALWAYS AS ((`obj_Cell_Id` >> 16)) VIRTUAL,
  `weenie_Class_Id` int(10) unsigned NOT NULL COMMENT 'Weenie Class Id of object to spawn',
  `guid` int(10) unsigned NOT NULL DEFAULT '0',
  `obj_Cell_Id` int(10) unsigned NOT NULL,
  `origin_X` float NOT NULL,
  `origin_Y` float NOT NULL,
  `origin_Z` float NOT NULL,
  `angles_W` float NOT NULL,
  `angles_X` float NOT NULL,
  `angles_Y` float NOT NULL,
  `angles_Z` float NOT NULL,
  `link_Slot` int(5) DEFAULT NULL COMMENT 'Slot Id for this instance''s link',
  `link_Controller` bit(1) DEFAULT NULL COMMENT 'Is this the controller for the link?',
  PRIMARY KEY (`id`),
  UNIQUE KEY `guid_UNIQUE` (`guid`),
  KEY `wcid_instance_idx` (`weenie_Class_Id`),
  KEY `instance_landblock_idx` (`landblock`),
  CONSTRAINT `wcid_instance` FOREIGN KEY (`weenie_Class_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Weenie Instances for each Landblock';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `points_of_interest`
--

DROP TABLE IF EXISTS `points_of_interest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `points_of_interest` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this POI',
  `name` text NOT NULL COMMENT 'Name for POI',
  `weenie_Class_Id` int(10) unsigned NOT NULL COMMENT 'Weenie Class Id of portal weenie to reference for destination of POI',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`(100)),
  KEY `wcid_poi_idx` (`weenie_Class_Id`),
  CONSTRAINT `wcid_poi` FOREIGN KEY (`weenie_Class_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Points of Interest for @telepoi command';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `quest`
--

DROP TABLE IF EXISTS `quest`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `quest` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Quest',
  `name` varchar(255) NOT NULL COMMENT 'Unique Name of Quest',
  `min_Delta` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Minimum time between Quest completions',
  `max_Solves` int(10) NOT NULL DEFAULT '0' COMMENT 'Maximum number of times Quest can be completed',
  `message` text NOT NULL COMMENT 'Quest solved text - unused?',
  PRIMARY KEY (`id`),
  UNIQUE KEY `name_UNIQUE` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Quests';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recipe`
--

DROP TABLE IF EXISTS `recipe`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recipe` (
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recipe_component`
--

DROP TABLE IF EXISTS `recipe_component`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recipe_component` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Component instance',
  `recipe_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe',
  `destroy_Chance` double NOT NULL DEFAULT '0',
  `destroy_Amount` int(10) unsigned NOT NULL DEFAULT '0',
  `destroy_Message` text NOT NULL,
  PRIMARY KEY (`id`),
  KEY `recipe_idx` (`recipe_Id`),
  CONSTRAINT `recipeId_component` FOREIGN KEY (`recipe_Id`) REFERENCES `recipe` (`recipe_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe Components';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recipe_mod`
--

DROP TABLE IF EXISTS `recipe_mod`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recipe_mod` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Mod instance',
  `recipe_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe',
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
  KEY `recipe_idx` (`recipe_Id`),
  CONSTRAINT `recipeId_Mod` FOREIGN KEY (`recipe_Id`) REFERENCES `recipe` (`recipe_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe Mods';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recipe_mods_bool`
--

DROP TABLE IF EXISTS `recipe_mods_bool`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recipe_mods_bool` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Mod instance',
  `recipe_Mod_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe Mod',
  `stat` int(10) NOT NULL DEFAULT '0',
  `value` bit(1) NOT NULL,
  `enum` int(10) NOT NULL DEFAULT '0',
  `unknown_1` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `recipe_mod_idx` (`recipe_Mod_Id`),
  CONSTRAINT `recipeId_mod_bool` FOREIGN KEY (`recipe_Mod_Id`) REFERENCES `recipe_mod` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe Bool Mods';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recipe_mods_d_i_d`
--

DROP TABLE IF EXISTS `recipe_mods_d_i_d`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recipe_mods_d_i_d` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Mod instance',
  `recipe_Mod_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe Mod',
  `stat` int(10) NOT NULL DEFAULT '0',
  `value` int(10) unsigned NOT NULL DEFAULT '0',
  `enum` int(10) NOT NULL DEFAULT '0',
  `unknown_1` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `recipe_mod_idx` (`recipe_Mod_Id`),
  CONSTRAINT `recipeId_mod_did` FOREIGN KEY (`recipe_Mod_Id`) REFERENCES `recipe_mod` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe DID Mods';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recipe_mods_float`
--

DROP TABLE IF EXISTS `recipe_mods_float`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recipe_mods_float` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Mod instance',
  `recipe_Mod_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe Mod',
  `stat` int(10) NOT NULL DEFAULT '0',
  `value` double NOT NULL DEFAULT '0',
  `enum` int(10) NOT NULL DEFAULT '0',
  `unknown_1` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `recipe_mod_idx` (`recipe_Mod_Id`),
  CONSTRAINT `recipeId_mod_float` FOREIGN KEY (`recipe_Mod_Id`) REFERENCES `recipe_mod` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe Float Mods';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recipe_mods_i_i_d`
--

DROP TABLE IF EXISTS `recipe_mods_i_i_d`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recipe_mods_i_i_d` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Mod instance',
  `recipe_Mod_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe Mod',
  `stat` int(10) NOT NULL DEFAULT '0',
  `value` int(10) unsigned NOT NULL DEFAULT '0',
  `enum` int(10) NOT NULL DEFAULT '0',
  `unknown_1` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `recipe_mod_idx` (`recipe_Mod_Id`),
  CONSTRAINT `recipeId_mod_iid` FOREIGN KEY (`recipe_Mod_Id`) REFERENCES `recipe_mod` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe IID Mods';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recipe_mods_int`
--

DROP TABLE IF EXISTS `recipe_mods_int`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recipe_mods_int` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Mod instance',
  `recipe_Mod_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe Mod',
  `stat` int(10) NOT NULL DEFAULT '0',
  `value` int(10) NOT NULL DEFAULT '0',
  `enum` int(10) NOT NULL DEFAULT '0',
  `unknown_1` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `recipe_mod_idx` (`recipe_Mod_Id`),
  CONSTRAINT `recipeId_mod_int` FOREIGN KEY (`recipe_Mod_Id`) REFERENCES `recipe_mod` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe Int Mods';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recipe_mods_string`
--

DROP TABLE IF EXISTS `recipe_mods_string`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recipe_mods_string` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Recipe Mod instance',
  `recipe_Mod_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Unique Id of Recipe Mod',
  `stat` int(10) NOT NULL DEFAULT '0',
  `value` text NOT NULL,
  `enum` int(10) NOT NULL DEFAULT '0',
  `unknown_1` int(10) NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `recipe_mod_idx` (`recipe_Mod_Id`),
  CONSTRAINT `recipeId_mod_string` FOREIGN KEY (`recipe_Mod_Id`) REFERENCES `recipe_mod` (`id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Recipe String Mods';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recipe_requirements_bool`
--

DROP TABLE IF EXISTS `recipe_requirements_bool`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recipe_requirements_bool` (
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recipe_requirements_d_i_d`
--

DROP TABLE IF EXISTS `recipe_requirements_d_i_d`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recipe_requirements_d_i_d` (
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recipe_requirements_float`
--

DROP TABLE IF EXISTS `recipe_requirements_float`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recipe_requirements_float` (
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recipe_requirements_i_i_d`
--

DROP TABLE IF EXISTS `recipe_requirements_i_i_d`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recipe_requirements_i_i_d` (
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recipe_requirements_int`
--

DROP TABLE IF EXISTS `recipe_requirements_int`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recipe_requirements_int` (
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `recipe_requirements_string`
--

DROP TABLE IF EXISTS `recipe_requirements_string`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `recipe_requirements_string` (
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
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `spell`
--

DROP TABLE IF EXISTS `spell`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `spell` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Spell',
  `spell_Id` int(10) unsigned NOT NULL COMMENT 'Unique Spell Id',
  `name` text NOT NULL,
  `description` text NOT NULL,
  `school` int(10) NOT NULL DEFAULT '0',
  `icon_Id` int(10) unsigned NOT NULL DEFAULT '0',
  `category` int(10) unsigned NOT NULL DEFAULT '0',
  `bitfield` int(10) unsigned NOT NULL DEFAULT '0',
  `mana` int(10) unsigned NOT NULL DEFAULT '0',
  `range_Constant` float NOT NULL,
  `range_Mod` float NOT NULL,
  `power` int(10) unsigned NOT NULL DEFAULT '0',
  `economy_Mod` float NOT NULL,
  `formula_Version` int(10) unsigned NOT NULL DEFAULT '0',
  `component_Loss` float NOT NULL,
  `meta_Spell_Type` int(10) NOT NULL DEFAULT '0',
  `meta_Spell_Id` int(10) unsigned NOT NULL DEFAULT '0',
  `spell_Formula_Comp_1_Component_Id` int(10) unsigned NOT NULL DEFAULT '0',
  `spell_Formula_Comp_2_Component_Id` int(10) unsigned NOT NULL DEFAULT '0',
  `spell_Formula_Comp_3_Component_Id` int(10) unsigned NOT NULL DEFAULT '0',
  `spell_Formula_Comp_4_Component_Id` int(10) unsigned NOT NULL DEFAULT '0',
  `spell_Formula_Comp_5_Component_Id` int(10) unsigned NOT NULL DEFAULT '0',
  `spell_Formula_Comp_6_Component_Id` int(10) unsigned NOT NULL DEFAULT '0',
  `spell_Formula_Comp_7_Component_Id` int(10) unsigned NOT NULL DEFAULT '0',
  `spell_Formula_Comp_8_Component_Id` int(10) unsigned NOT NULL DEFAULT '0',
  `caster_Effect` int(10) unsigned NOT NULL DEFAULT '0',
  `target_Effect` int(10) unsigned NOT NULL DEFAULT '0',
  `fizzle_Effect` int(10) unsigned NOT NULL DEFAULT '0',
  `recovery_Interval` double NOT NULL,
  `recovery_Amount` float NOT NULL,
  `display_Order` int(10) unsigned NOT NULL DEFAULT '0',
  `non_Component_Target_Type` int(10) unsigned NOT NULL DEFAULT '0',
  `mana_Mod` int(10) unsigned NOT NULL DEFAULT '0',
  `duration` double DEFAULT NULL,
  `degrade_Modifier` float DEFAULT NULL,
  `degrade_Limit` float DEFAULT NULL,
  `stat_Mod_Type` int(10) unsigned DEFAULT NULL,
  `stat_Mod_Key` int(10) unsigned DEFAULT NULL,
  `stat_Mod_Val` float DEFAULT NULL,
  `e_Type` int(10) unsigned DEFAULT NULL,
  `base_Intensity` int(10) DEFAULT NULL,
  `variance` int(10) DEFAULT NULL,
  `wcid` int(10) unsigned DEFAULT NULL,
  `num_Projectiles` int(10) DEFAULT NULL,
  `num_Projectiles_Variance` int(10) DEFAULT NULL,
  `spread_Angle` float DEFAULT NULL,
  `vertical_Angle` float DEFAULT NULL,
  `default_Launch_Angle` float DEFAULT NULL,
  `non_Tracking` bit(1) DEFAULT NULL,
  `create_Offset_Origin_X` float DEFAULT NULL,
  `create_Offset_Origin_Y` float DEFAULT NULL,
  `create_Offset_Origin_Z` float DEFAULT NULL,
  `padding_Origin_X` float DEFAULT NULL,
  `padding_Origin_Y` float DEFAULT NULL,
  `padding_Origin_Z` float DEFAULT NULL,
  `dims_Origin_X` float DEFAULT NULL,
  `dims_Origin_Y` float DEFAULT NULL,
  `dims_Origin_Z` float DEFAULT NULL,
  `peturbation_Origin_X` float DEFAULT NULL,
  `peturbation_Origin_Y` float DEFAULT NULL,
  `peturbation_Origin_Z` float DEFAULT NULL,
  `imbued_Effect` int(10) unsigned DEFAULT NULL,
  `slayer_Creature_Type` int(10) DEFAULT NULL,
  `slayer_Damage_Bonus` float DEFAULT NULL,
  `crit_Freq` double DEFAULT NULL,
  `crit_Multiplier` double DEFAULT NULL,
  `ignore_Magic_Resist` int(10) DEFAULT NULL,
  `elemental_Modifier` double DEFAULT NULL,
  `drain_Percentage` float DEFAULT NULL,
  `damage_Ratio` float DEFAULT NULL,
  `damage_Type` int(10) DEFAULT NULL,
  `boost` int(10) DEFAULT NULL,
  `boost_Variance` int(10) DEFAULT NULL,
  `source` int(10) DEFAULT NULL,
  `destination` int(10) DEFAULT NULL,
  `proportion` float DEFAULT NULL,
  `loss_Percent` float DEFAULT NULL,
  `source_Loss` int(10) DEFAULT NULL,
  `transfer_Cap` int(10) DEFAULT NULL,
  `max_Boost_Allowed` int(10) DEFAULT NULL,
  `transfer_Bitfield` int(10) unsigned DEFAULT NULL,
  `index` int(10) DEFAULT NULL,
  `portal_Lifetime` double DEFAULT NULL,
  `link` int(10) DEFAULT NULL,
  `position_Obj_Cell_ID` int(10) unsigned DEFAULT NULL,
  `position_Origin_X` float DEFAULT NULL,
  `position_Origin_Y` float DEFAULT NULL,
  `position_Origin_Z` float DEFAULT NULL,
  `position_Angles_W` float DEFAULT NULL,
  `position_Angles_X` float DEFAULT NULL,
  `position_Angles_Y` float DEFAULT NULL,
  `position_Angles_Z` float DEFAULT NULL,
  `min_Power` int(10) DEFAULT NULL,
  `max_Power` int(10) DEFAULT NULL,
  `power_Variance` float DEFAULT NULL,
  `dispel_School` int(10) DEFAULT NULL,
  `align` int(10) DEFAULT NULL,
  `number` int(10) DEFAULT NULL,
  `number_Variance` float DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `spell_id_uidx` (`spell_Id`),
  UNIQUE KEY `metaspell_id_uidx` (`meta_Spell_Id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Spell Table Extended Data';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `treasure_death`
--

DROP TABLE IF EXISTS `treasure_death`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `treasure_death` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Treasure',
  `treasure_Type` int(10) unsigned NOT NULL COMMENT 'Type of Treasure for this instance',
  `tier` int(10) NOT NULL,
  `unknown_1` float NOT NULL,
  `unknown_2` int(10) NOT NULL,
  `unknown_3` int(10) NOT NULL,
  `unknown_4` int(10) NOT NULL,
  `unknown_5` int(10) NOT NULL,
  `unknown_6` int(10) NOT NULL,
  `unknown_7` int(10) NOT NULL,
  `unknown_8` int(10) NOT NULL,
  `unknown_9` int(10) NOT NULL,
  `unknown_10` int(10) NOT NULL,
  `unknown_11` int(10) NOT NULL,
  `unknown_12` int(10) NOT NULL,
  `unknown_13` int(10) NOT NULL,
  `unknown_14` int(10) NOT NULL,
  PRIMARY KEY (`id`),
  KEY `treasureType_idx` (`treasure_Type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Death Treasure';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `treasure_wielded`
--

DROP TABLE IF EXISTS `treasure_wielded`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `treasure_wielded` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Treasure',
  `treasure_Type` int(10) unsigned NOT NULL COMMENT 'Type of Treasure for this instance',
  `weenie_Class_Id` int(10) unsigned NOT NULL COMMENT 'Weenie Class Id of Treasure to Generate',
  `palette_Id` int(10) unsigned NOT NULL COMMENT 'Palette Color of Object Generated',
  `unknown_1` int(10) unsigned NOT NULL,
  `shade` float NOT NULL COMMENT 'Shade of Object generated''s Palette',
  `stack_Size` int(10) NOT NULL DEFAULT '1' COMMENT 'Stack Size of object to create (-1 = infinite)',
  `unknown_2` float NOT NULL,
  `probability` float NOT NULL,
  `unknown_3` int(10) unsigned NOT NULL,
  `unknown_4` int(10) unsigned NOT NULL,
  `unknown_5` int(10) unsigned NOT NULL,
  `unknown_6` bit(1) NOT NULL,
  `unknown_7` bit(1) NOT NULL,
  `unknown_8` bit(1) NOT NULL,
  `unknown_9` int(10) unsigned NOT NULL,
  `unknown_10` int(10) unsigned NOT NULL,
  `unknown_11` int(10) unsigned NOT NULL,
  `unknown_12` int(10) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `treasureType_idx` (`treasure_Type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Wielded Treasure';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie`
--

DROP TABLE IF EXISTS `weenie`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie` (
  `class_Id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Weenie Class Id (wcid) / (WCID) / (weenieClassId)',
  `class_Name` varchar(100) NOT NULL COMMENT 'Weenie Class Name (W_????_CLASS)',
  `type` int(5) NOT NULL DEFAULT '0' COMMENT 'WeenieType',
  PRIMARY KEY (`class_Id`),
  UNIQUE KEY `className_UNIQUE` (`class_Name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_anim_part`
--

DROP TABLE IF EXISTS `weenie_properties_anim_part`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_anim_part` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `index` tinyint(3) unsigned NOT NULL,
  `animation_Id` int(10) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `object_Id_index_uidx` (`object_Id`,`index`),
  CONSTRAINT `wcid_animpart` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Animation Part Changes (from PCAPs) of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_attribute`
--

DROP TABLE IF EXISTS `weenie_properties_attribute`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_attribute` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertyAttribute.????)',
  `init_Level` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'innate points',
  `level_From_C_P` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'points raised',
  `c_P_Spent` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'XP spent on this attribute',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_attribute_type_uidx` (`object_Id`,`type`),
  KEY `wcid_attribute_idx` (`object_Id`),
  CONSTRAINT `wcid_attribute` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Attribute Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_attribute_2nd`
--

DROP TABLE IF EXISTS `weenie_properties_attribute_2nd`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_attribute_2nd` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertyAttribute2nd.????)',
  `init_Level` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'innate points',
  `level_From_C_P` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'points raised',
  `c_P_Spent` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'XP spent on this attribute',
  `current_Level` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'current value of the vital',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_attribute2nd_type_uidx` (`object_Id`,`type`),
  KEY `wcid_attribute2nd_idx` (`object_Id`),
  CONSTRAINT `wcid_attribute2nd` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Attribute2nd (Vital) Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_body_part`
--

DROP TABLE IF EXISTS `weenie_properties_body_part`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_body_part` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `key` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertySkill.????)',
  `d_Type` int(10) NOT NULL DEFAULT '0',
  `d_Val` int(10) NOT NULL DEFAULT '0',
  `d_Var` float NOT NULL DEFAULT '0',
  `base_Armor` int(10) NOT NULL DEFAULT '0',
  `armor_Vs_Slash` int(10) NOT NULL DEFAULT '0',
  `armor_Vs_Pierce` int(10) NOT NULL DEFAULT '0',
  `armor_Vs_Bludgeon` int(10) NOT NULL DEFAULT '0',
  `armor_Vs_Cold` int(10) NOT NULL DEFAULT '0',
  `armor_Vs_Fire` int(10) NOT NULL DEFAULT '0',
  `armor_Vs_Acid` int(10) NOT NULL DEFAULT '0',
  `armor_Vs_Electric` int(10) NOT NULL DEFAULT '0',
  `armor_Vs_Nether` int(10) NOT NULL DEFAULT '0',
  `b_h` int(10) NOT NULL DEFAULT '0',
  `h_l_f` float NOT NULL DEFAULT '0',
  `m_l_f` float NOT NULL DEFAULT '0',
  `l_l_f` float NOT NULL DEFAULT '0',
  `h_r_f` float NOT NULL DEFAULT '0',
  `m_r_f` float NOT NULL DEFAULT '0',
  `l_r_f` float NOT NULL DEFAULT '0',
  `h_l_b` float NOT NULL DEFAULT '0',
  `m_l_b` float NOT NULL DEFAULT '0',
  `l_l_b` float NOT NULL DEFAULT '0',
  `h_r_b` float NOT NULL DEFAULT '0',
  `m_r_b` float NOT NULL DEFAULT '0',
  `l_r_b` float NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_bodypart_type_uidx` (`object_Id`,`key`),
  KEY `wcid_bodypart_idx` (`object_Id`),
  CONSTRAINT `wcid_bodypart` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Body Part Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_book`
--

DROP TABLE IF EXISTS `weenie_properties_book`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_book` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `max_Num_Pages` int(10) NOT NULL DEFAULT '1' COMMENT 'Maximum number of pages per book',
  `max_Num_Chars_Per_Page` int(10) NOT NULL DEFAULT '1000' COMMENT 'Maximum number of characters per page',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_bookdata_uidx` (`object_Id`),
  KEY `wcid_bookdata_idx` (`object_Id`),
  CONSTRAINT `wcid_bookdata` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Book Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_book_page_data`
--

DROP TABLE IF EXISTS `weenie_properties_book_page_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_book_page_data` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the Book object this page belongs to',
  `page_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the page number for this page',
  `author_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the Author of this page',
  `author_Name` varchar(255) NOT NULL DEFAULT '' COMMENT 'Character Name of the Author of this page',
  `author_Account` varchar(255) NOT NULL DEFAULT 'prewritten' COMMENT 'Account Name of the Author of this page',
  `ignore_Author` bit(1) NOT NULL COMMENT 'if this is true, any character in the world can change the page',
  `page_Text` text NOT NULL COMMENT 'Text of the Page',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_pageid_uidx` (`object_Id`,`page_Id`),
  KEY `wcid_pagedata_idx` (`object_Id`),
  CONSTRAINT `wcid_pagedata` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Page Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_bool`
--

DROP TABLE IF EXISTS `weenie_properties_bool`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_bool` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertyBool.????)',
  `value` bit(1) NOT NULL COMMENT 'Value of this Property',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_bool_type_uidx` (`object_Id`,`type`),
  KEY `wcid_bool_idx` (`object_Id`),
  CONSTRAINT `wcid_bool` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Bool Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_create_list`
--

DROP TABLE IF EXISTS `weenie_properties_create_list`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_create_list` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `destination_Type` tinyint(5) NOT NULL DEFAULT '0' COMMENT 'Type of Destination the value applies to (DestinationType.????)',
  `weenie_Class_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Weenie Class Id of object to Create',
  `stack_Size` int(10) NOT NULL DEFAULT '1' COMMENT 'Stack Size of object to create (-1 = infinite)',
  `palette` tinyint(5) NOT NULL DEFAULT '0' COMMENT 'Palette Color of Object',
  `shade` float NOT NULL DEFAULT '0' COMMENT 'Shade of Object''s Palette',
  `try_To_Bond` bit(1) NOT NULL COMMENT 'Unused?',
  PRIMARY KEY (`id`),
  KEY `wcid_createlist_idx` (`object_Id`),
  CONSTRAINT `wcid_createlist` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='CreateList Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_d_i_d`
--

DROP TABLE IF EXISTS `weenie_properties_d_i_d`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_d_i_d` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertyDataId.????)',
  `value` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Value of this Property',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_did_type_uidx` (`object_Id`,`type`),
  KEY `wcid_did_idx` (`object_Id`),
  KEY `wcid_did_type_idx` (`type`),
  CONSTRAINT `wcid_did` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='DataID Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_emote`
--

DROP TABLE IF EXISTS `weenie_properties_emote`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_emote` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `category` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'EmoteCategory',
  `probability` float NOT NULL DEFAULT '1' COMMENT 'Probability of this EmoteSet being chosen',
  `emote_Set_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Emote Set Id',
  `weenie_Class_Id` int(10) unsigned DEFAULT NULL,
  `style` int(10) unsigned DEFAULT NULL,
  `substyle` int(10) unsigned DEFAULT NULL,
  `quest` text,
  `vendor_Type` int(10) DEFAULT NULL,
  `min_Health` float DEFAULT NULL,
  `max_Health` float DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_category_emoteset_uidx` (`object_Id`,`category`,`emote_Set_Id`),
  KEY `wcid_emote_idx` (`object_Id`),
  KEY `category_idx` (`category`),
  KEY `emoteset_idx` (`emote_Set_Id`),
  CONSTRAINT `wcid_emote` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Emote Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_emote_action`
--

DROP TABLE IF EXISTS `weenie_properties_emote_action`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_emote_action` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `emote_Category` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'EmoteCategory',
  `emote_Set_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Emote Set Id',
  `type` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'EmoteType',
  `order` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Emote Action Sequence Order',
  `delay` float NOT NULL DEFAULT '1' COMMENT 'Time to wait before EmoteAction starts execution',
  `extent` float NOT NULL DEFAULT '1' COMMENT '?',
  `motion` int(10) DEFAULT NULL,
  `message` text,
  `test_String` text,
  `min` int(10) DEFAULT NULL,
  `max` int(10) DEFAULT NULL,
  `min_64` bigint(10) DEFAULT NULL,
  `max_64` bigint(10) DEFAULT NULL,
  `min_Dbl` double DEFAULT NULL,
  `max_Dbl` double DEFAULT NULL,
  `stat` int(10) DEFAULT NULL,
  `display` int(10) DEFAULT NULL,
  `amount` int(10) DEFAULT NULL,
  `amount_64` bigint(10) DEFAULT NULL,
  `hero_X_P_64` bigint(10) DEFAULT NULL,
  `percent` double DEFAULT NULL,
  `spell_Id` int(10) DEFAULT NULL,
  `wealth_Rating` int(10) DEFAULT NULL,
  `treasure_Class` int(10) DEFAULT NULL,
  `treasure_Type` int(10) DEFAULT NULL,
  `p_Script` int(10) DEFAULT NULL,
  `sound` int(10) DEFAULT NULL,
  `destination_Type` tinyint(5) DEFAULT NULL COMMENT 'Type of Destination the value applies to (DestinationType.????)',
  `weenie_Class_Id` int(10) unsigned DEFAULT NULL COMMENT 'Weenie Class Id of object to Create',
  `stack_Size` int(10) DEFAULT NULL COMMENT 'Stack Size of object to create (-1 = infinite)',
  `palette` int(10) DEFAULT NULL COMMENT 'Palette Color of Object',
  `shade` float DEFAULT NULL COMMENT 'Shade of Object''s Palette',
  `try_To_Bond` bit(1) DEFAULT NULL COMMENT 'Unused?',
  `obj_Cell_Id` int(10) unsigned DEFAULT NULL,
  `origin_X` float DEFAULT NULL,
  `origin_Y` float DEFAULT NULL,
  `origin_Z` float DEFAULT NULL,
  `angles_W` float DEFAULT NULL,
  `angles_X` float DEFAULT NULL,
  `angles_Y` float DEFAULT NULL,
  `angles_Z` float DEFAULT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_category_set_order_uidx` (`object_Id`,`emote_Category`,`emote_Set_Id`,`order`),
  KEY `wcid_emoteaction_idx` (`object_Id`),
  KEY `emotecategory_idx` (`emote_Category`),
  KEY `emotetype_idx` (`type`),
  KEY `emoteorder_idx` (`order`),
  CONSTRAINT `wcid_emoteaction` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION,
  CONSTRAINT `wcid_emoteset` FOREIGN KEY (`object_Id`, `emote_Category`, `emote_Set_Id`) REFERENCES `weenie_properties_emote` (`object_Id`, `category`, `emote_Set_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='EmoteAction Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_event_filter`
--

DROP TABLE IF EXISTS `weenie_properties_event_filter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_event_filter` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `event` int(10) NOT NULL DEFAULT '0' COMMENT 'Id of Event to filter',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_eventfilter_type_uidx` (`object_Id`,`event`),
  KEY `wcid_eventfilter_idx` (`object_Id`),
  CONSTRAINT `wcid_eventfilter` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='EventFilter Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_float`
--

DROP TABLE IF EXISTS `weenie_properties_float`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_float` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertyFloat.????)',
  `value` double NOT NULL DEFAULT '0' COMMENT 'Value of this Property',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_float_type_uidx` (`object_Id`,`type`),
  KEY `wcid_float_idx` (`object_Id`),
  CONSTRAINT `wcid_float` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Float Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_generator`
--

DROP TABLE IF EXISTS `weenie_properties_generator`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_generator` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `probability` float NOT NULL DEFAULT '1',
  `weenie_Class_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Weenie Class Id of object to generate',
  `delay` float DEFAULT '0' COMMENT 'Amount of delay before generation',
  `init_Create` int(10) unsigned NOT NULL DEFAULT '1' COMMENT 'Number of object to generate initially',
  `max_Create` int(10) unsigned NOT NULL DEFAULT '1' COMMENT 'Maximum amount of objects to generate',
  `when_Create` int(10) unsigned NOT NULL DEFAULT '2' COMMENT 'When to generate the weenie object',
  `where_Create` int(10) unsigned NOT NULL DEFAULT '4' COMMENT 'Where to generate the weenie object',
  `stack_Size` int(10) DEFAULT NULL COMMENT 'StackSize of object generated',
  `palette_Id` int(10) unsigned DEFAULT NULL COMMENT 'Palette Color of Object Generated',
  `shade` float DEFAULT NULL COMMENT 'Shade of Object generated''s Palette',
  `obj_Cell_Id` int(10) unsigned DEFAULT NULL,
  `origin_X` float DEFAULT NULL,
  `origin_Y` float DEFAULT NULL,
  `origin_Z` float DEFAULT NULL,
  `angles_W` float DEFAULT NULL,
  `angles_X` float DEFAULT NULL,
  `angles_Y` float DEFAULT NULL,
  `angles_Z` float DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `wcid_generator_idx` (`object_Id`),
  CONSTRAINT `wcid_generator` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Generator Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_i_i_d`
--

DROP TABLE IF EXISTS `weenie_properties_i_i_d`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_i_i_d` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertyInstanceId.????)',
  `value` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Value of this Property',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_iid_type_uidx` (`object_Id`,`type`),
  KEY `wcid_iid_idx` (`object_Id`),
  KEY `wcid_did_type_idx` (`type`),
  CONSTRAINT `wcid_iid` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='InstanceID Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_int`
--

DROP TABLE IF EXISTS `weenie_properties_int`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_int` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertyInt.????)',
  `value` int(10) NOT NULL DEFAULT '0' COMMENT 'Value of this Property',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_int_type_uidx` (`object_Id`,`type`),
  KEY `wcid_int_idx` (`object_Id`),
  CONSTRAINT `wcid_int` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Int Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_int64`
--

DROP TABLE IF EXISTS `weenie_properties_int64`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_int64` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertyInt64.????)',
  `value` bigint(10) NOT NULL DEFAULT '0' COMMENT 'Value of this Property',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_int64_type_uidx` (`object_Id`,`type`),
  KEY `wcid_int64_idx` (`object_Id`),
  CONSTRAINT `wcid_int64` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Int64 Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_palette`
--

DROP TABLE IF EXISTS `weenie_properties_palette`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_palette` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `sub_Palette_Id` int(10) unsigned NOT NULL,
  `offset` smallint(5) unsigned NOT NULL,
  `length` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `object_Id_subPaletteId_offset_length_uidx` (`object_Id`,`sub_Palette_Id`,`offset`,`length`),
  CONSTRAINT `wcid_palette` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Palette Changes (from PCAPs) of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_position`
--

DROP TABLE IF EXISTS `weenie_properties_position`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_position` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Position',
  `object_Id` int(10) unsigned NOT NULL COMMENT 'Id of the object this property belongs to',
  `position_Type` smallint(5) unsigned NOT NULL COMMENT 'Type of Position the value applies to (PositionType.????)',
  `obj_Cell_Id` int(10) unsigned NOT NULL,
  `origin_X` float NOT NULL,
  `origin_Y` float NOT NULL,
  `origin_Z` float NOT NULL,
  `angles_W` float NOT NULL,
  `angles_X` float NOT NULL,
  `angles_Y` float NOT NULL,
  `angles_Z` float NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_position_type_uidx` (`object_Id`,`position_Type`),
  KEY `wcid_position_idx` (`object_Id`),
  KEY `objCellId_idx` (`obj_Cell_Id`),
  CONSTRAINT `wcid_position` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Position Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_skill`
--

DROP TABLE IF EXISTS `weenie_properties_skill`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_skill` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertySkill.????)',
  `level_From_P_P` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'points raised',
  `adjust_P_P` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'If this is not 0, it appears to trigger the initLevel to be treated as extra XP applied to the skill',
  `s_a_c` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'skill state',
  `p_p` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'XP spent on this skill',
  `init_Level` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'starting point for advancement of the skill (eg bonus points)',
  `resistance_At_Last_Check` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'last use difficulty',
  `last_Used_Time` double NOT NULL DEFAULT '0' COMMENT 'time skill was last used',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_skill_type_uidx` (`object_Id`,`type`),
  KEY `wcid_skill_idx` (`object_Id`),
  CONSTRAINT `wcid_skill` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Skill Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_spell_book`
--

DROP TABLE IF EXISTS `weenie_properties_spell_book`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_spell_book` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `spell` int(10) NOT NULL DEFAULT '0' COMMENT 'Id of Spell',
  `probability` float NOT NULL DEFAULT '2' COMMENT 'Chance to cast this spell',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_spellbook_type_uidx` (`object_Id`,`spell`),
  KEY `wcid_spellbook_idx` (`object_Id`),
  CONSTRAINT `wcid_spellbook` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='SpellBook Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_string`
--

DROP TABLE IF EXISTS `weenie_properties_string`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_string` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertyString.????)',
  `value` text NOT NULL COMMENT 'Value of this Property',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_string_type_uidx` (`object_Id`,`type`),
  KEY `wcid_string_idx` (`object_Id`),
  CONSTRAINT `wcid_string` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='String Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `weenie_properties_texture_map`
--

DROP TABLE IF EXISTS `weenie_properties_texture_map`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `weenie_properties_texture_map` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `index` tinyint(3) unsigned NOT NULL,
  `old_Id` int(10) unsigned NOT NULL,
  `new_Id` int(10) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  UNIQUE KEY `object_Id_index_oldId_uidx` (`object_Id`,`index`,`old_Id`),
  CONSTRAINT `wcid_texturemap` FOREIGN KEY (`object_Id`) REFERENCES `weenie` (`class_Id`) ON DELETE CASCADE ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Texture Map Changes (from PCAPs) of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2018-07-22 13:10:11
