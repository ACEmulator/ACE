-- MySQL dump 10.13  Distrib 8.0.12, for Win64 (x86_64)
--
-- Host: localhost    Database: ace_shard
-- ------------------------------------------------------
-- Server version	8.0.12

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
 SET NAMES utf8mb4 ;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `ace_shard`
--

/*!40000 DROP DATABASE IF EXISTS `ace_shard`*/;

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `ace_shard` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `ace_shard`;

--
-- Table structure for table `biota`
--

DROP TABLE IF EXISTS `biota`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Object Id within the Shard',
  `weenie_Class_Id` int(10) unsigned NOT NULL COMMENT 'Weenie Class Id of the Weenie this Biota was created from',
  `weenie_Type` int(5) NOT NULL DEFAULT '0' COMMENT 'WeenieType for this Object',
  `populated_Collection_Flags` int(10) unsigned NOT NULL DEFAULT '4294967295',
  PRIMARY KEY (`id`),
  KEY `biota_wcid_idx` (`weenie_Class_Id`),
  KEY `biota_type_idx` (`weenie_Type`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Dynamic Weenies of a Shard/World';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_allegiance`
--

DROP TABLE IF EXISTS `biota_properties_allegiance`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_allegiance` (
  `allegiance_Id` int(10) unsigned NOT NULL,
  `character_Id` int(10) unsigned NOT NULL,
  `banned` bit(1) NOT NULL,
  `approved_Vassal` bit(1) NOT NULL,
  PRIMARY KEY (`allegiance_Id`,`character_Id`),
  KEY `FK_allegiance_character_Id` (`character_Id`),
  CONSTRAINT `FK_allegiance_biota_Id` FOREIGN KEY (`allegiance_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE,
  CONSTRAINT `FK_allegiance_character_Id` FOREIGN KEY (`character_Id`) REFERENCES `character` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_anim_part`
--

DROP TABLE IF EXISTS `biota_properties_anim_part`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_anim_part` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `index` tinyint(3) unsigned NOT NULL,
  `animation_Id` int(10) unsigned NOT NULL,
  `order` tinyint(3) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `wcid_animpart_idx` (`object_Id`),
  CONSTRAINT `wcid_animpart` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Animation Part Changes (from PCAPs) of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_attribute`
--

DROP TABLE IF EXISTS `biota_properties_attribute`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_attribute` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertyAttribute.????)',
  `init_Level` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'innate points',
  `level_From_C_P` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'points raised',
  `c_P_Spent` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'XP spent on this attribute',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_attribute_type_uidx` (`object_Id`,`type`),
  CONSTRAINT `wcid_attribute` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Attribute Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_attribute_2nd`
--

DROP TABLE IF EXISTS `biota_properties_attribute_2nd`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_attribute_2nd` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertyAttribute2nd.????)',
  `init_Level` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'innate points',
  `level_From_C_P` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'points raised',
  `c_P_Spent` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'XP spent on this attribute',
  `current_Level` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'current value of the vital',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_attribute2nd_type_uidx` (`object_Id`,`type`),
  CONSTRAINT `wcid_attribute2nd` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Attribute2nd (Vital) Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_body_part`
--

DROP TABLE IF EXISTS `biota_properties_body_part`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_body_part` (
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
  CONSTRAINT `wcid_bodypart` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Body Part Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_book`
--

DROP TABLE IF EXISTS `biota_properties_book`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_book` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `max_Num_Pages` int(10) NOT NULL DEFAULT '0' COMMENT 'Maximum number of pages per book',
  `max_Num_Chars_Per_Page` int(10) NOT NULL DEFAULT '0' COMMENT 'Maximum number of characters per page',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_bookdata_uidx` (`object_Id`),
  CONSTRAINT `wcid_bookdata` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Book Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_book_page_data`
--

DROP TABLE IF EXISTS `biota_properties_book_page_data`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_book_page_data` (
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
  CONSTRAINT `wcid_pagedata` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Page Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_bool`
--

DROP TABLE IF EXISTS `biota_properties_bool`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_bool` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertyBool.????)',
  `value` bit(1) NOT NULL COMMENT 'Value of this Property',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_bool_type_uidx` (`object_Id`,`type`),
  CONSTRAINT `wcid_bool` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Bool Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_create_list`
--

DROP TABLE IF EXISTS `biota_properties_create_list`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_create_list` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `destination_Type` tinyint(5) NOT NULL DEFAULT '0' COMMENT 'Type of Destination the value applies to (DestinationType.????)',
  `weenie_Class_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Weenie Class Id of object to Create',
  `stack_Size` int(10) NOT NULL DEFAULT '0' COMMENT 'Stack Size of object to create (-1 = infinite)',
  `palette` tinyint(5) NOT NULL DEFAULT '0' COMMENT 'Palette Color of Object',
  `shade` float NOT NULL DEFAULT '0' COMMENT 'Shade of Object''s Palette',
  `try_To_Bond` bit(1) NOT NULL COMMENT 'Unused?',
  PRIMARY KEY (`id`),
  KEY `wcid_createlist` (`object_Id`),
  CONSTRAINT `wcid_createlist` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='CreateList Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_d_i_d`
--

DROP TABLE IF EXISTS `biota_properties_d_i_d`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_d_i_d` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertyDataId.????)',
  `value` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Value of this Property',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_did_type_uidx` (`object_Id`,`type`),
  CONSTRAINT `wcid_did` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='DataID Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_emote`
--

DROP TABLE IF EXISTS `biota_properties_emote`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_emote` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `category` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'EmoteCategory',
  `probability` float NOT NULL DEFAULT '0' COMMENT 'Probability of this EmoteSet being chosen',
  `weenie_Class_Id` int(10) unsigned DEFAULT NULL,
  `style` int(10) unsigned DEFAULT NULL,
  `substyle` int(10) unsigned DEFAULT NULL,
  `quest` text,
  `vendor_Type` int(10) DEFAULT NULL,
  `min_Health` float DEFAULT NULL,
  `max_Health` float DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `wcid_emote` (`object_Id`),
  CONSTRAINT `wcid_emote` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Emote Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_emote_action`
--

DROP TABLE IF EXISTS `biota_properties_emote_action`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_emote_action` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `emote_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the emote this property belongs to',
  `order` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Emote Action Sequence Order',
  `type` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'EmoteType',
  `delay` float NOT NULL DEFAULT '0' COMMENT 'Time to wait before EmoteAction starts execution',
  `extent` float NOT NULL DEFAULT '0' COMMENT '?',
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
  `display` bit(1) DEFAULT NULL,
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
  UNIQUE KEY `wcid_category_set_order_uidx` (`emote_Id`,`order`),
  CONSTRAINT `emoteid_emoteaction` FOREIGN KEY (`emote_Id`) REFERENCES `biota_properties_emote` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='EmoteAction Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_enchantment_registry`
--

DROP TABLE IF EXISTS `biota_properties_enchantment_registry`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_enchantment_registry` (
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `enchantment_Category` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Which PackableList this Enchantment goes in (enchantmentMask)',
  `spell_Id` int(10) NOT NULL DEFAULT '0' COMMENT 'Id of Spell',
  `layer_Id` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of Layer',
  `has_Spell_Set_Id` bit(1) NOT NULL COMMENT 'Has Spell Set Id?',
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
  PRIMARY KEY (`object_Id`,`spell_Id`,`caster_Object_Id`,`layer_Id`),
  UNIQUE KEY `wcid_enchantmentregistry_objectId_spellId_layerId_uidx` (`object_Id`,`spell_Id`,`layer_Id`),
  CONSTRAINT `wcid_enchantmentregistry` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Enchantment Registry Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_event_filter`
--

DROP TABLE IF EXISTS `biota_properties_event_filter`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_event_filter` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `event` int(10) NOT NULL DEFAULT '0' COMMENT 'Id of Event to filter',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_eventfilter_type_uidx` (`object_Id`,`event`),
  CONSTRAINT `wcid_eventfilter` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='EventFilter Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_float`
--

DROP TABLE IF EXISTS `biota_properties_float`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_float` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertyFloat.????)',
  `value` double NOT NULL DEFAULT '0' COMMENT 'Value of this Property',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_float_type_uidx` (`object_Id`,`type`),
  CONSTRAINT `wcid_float` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Float Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_generator`
--

DROP TABLE IF EXISTS `biota_properties_generator`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_generator` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `probability` float NOT NULL DEFAULT '0',
  `weenie_Class_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Weenie Class Id of object to generate',
  `delay` float DEFAULT '0' COMMENT 'Amount of delay before generation',
  `init_Create` int(10) NOT NULL DEFAULT '0' COMMENT 'Number of object to generate initially',
  `max_Create` int(10) NOT NULL DEFAULT '0' COMMENT 'Maximum amount of objects to generate',
  `when_Create` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'When to generate the weenie object',
  `where_Create` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Where to generate the weenie object',
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
  KEY `wcid_generator` (`object_Id`),
  CONSTRAINT `wcid_generator` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Generator Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_i_i_d`
--

DROP TABLE IF EXISTS `biota_properties_i_i_d`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_i_i_d` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertyInstanceId.????)',
  `value` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Value of this Property',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_iid_type_uidx` (`object_Id`,`type`),
  KEY `type_value_idx` (`type`,`value`),
  CONSTRAINT `wcid_iid` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='InstanceID Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_int`
--

DROP TABLE IF EXISTS `biota_properties_int`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_int` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertyInt.????)',
  `value` int(10) NOT NULL DEFAULT '0' COMMENT 'Value of this Property',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_int_type_uidx` (`object_Id`,`type`),
  CONSTRAINT `wcid_int` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Int Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_int64`
--

DROP TABLE IF EXISTS `biota_properties_int64`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_int64` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertyInt64.????)',
  `value` bigint(10) NOT NULL DEFAULT '0' COMMENT 'Value of this Property',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_int64_type_uidx` (`object_Id`,`type`),
  CONSTRAINT `wcid_int64` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Int64 Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_palette`
--

DROP TABLE IF EXISTS `biota_properties_palette`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_palette` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `sub_Palette_Id` int(10) unsigned NOT NULL,
  `offset` smallint(5) unsigned NOT NULL,
  `length` smallint(5) unsigned NOT NULL,
  PRIMARY KEY (`id`),
  KEY `wcid_palette_idx` (`object_Id`),
  CONSTRAINT `wcid_palette` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Palette Changes (from PCAPs) of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_position`
--

DROP TABLE IF EXISTS `biota_properties_position`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_position` (
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
  KEY `type_cell_idx` (`position_Type`,`obj_Cell_Id`),
  CONSTRAINT `wcid_position` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Position Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_skill`
--

DROP TABLE IF EXISTS `biota_properties_skill`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_skill` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertySkill.????)',
  `level_From_P_P` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'points raised',
  `s_a_c` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'skill state',
  `p_p` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'XP spent on this skill',
  `init_Level` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'starting point for advancement of the skill (eg bonus points)',
  `resistance_At_Last_Check` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'last use difficulty',
  `last_Used_Time` double NOT NULL DEFAULT '0' COMMENT 'time skill was last used',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_skill_type_uidx` (`object_Id`,`type`),
  CONSTRAINT `wcid_skill` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Skill Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_spell_book`
--

DROP TABLE IF EXISTS `biota_properties_spell_book`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_spell_book` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `spell` int(10) NOT NULL DEFAULT '0' COMMENT 'Id of Spell',
  `probability` float NOT NULL DEFAULT '0' COMMENT 'Chance to cast this spell',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_spellbook_type_uidx` (`object_Id`,`spell`),
  CONSTRAINT `wcid_spellbook` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='SpellBook Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_string`
--

DROP TABLE IF EXISTS `biota_properties_string`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_string` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `type` smallint(5) unsigned NOT NULL DEFAULT '0' COMMENT 'Type of Property the value applies to (PropertyString.????)',
  `value` text NOT NULL COMMENT 'Value of this Property',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_string_type_uidx` (`object_Id`,`type`),
  CONSTRAINT `wcid_string` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='String Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `biota_properties_texture_map`
--

DROP TABLE IF EXISTS `biota_properties_texture_map`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `biota_properties_texture_map` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the object this property belongs to',
  `index` tinyint(3) unsigned NOT NULL,
  `old_Id` int(10) unsigned NOT NULL,
  `new_Id` int(10) unsigned NOT NULL,
  `order` tinyint(3) unsigned DEFAULT NULL,
  PRIMARY KEY (`id`),
  KEY `wcid_texturemap_idx` (`object_Id`),
  CONSTRAINT `wcid_texturemap` FOREIGN KEY (`object_Id`) REFERENCES `biota` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Texture Map Changes (from PCAPs) of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character`
--

DROP TABLE IF EXISTS `character`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `character` (
  `id` int(10) unsigned NOT NULL COMMENT 'Id of the Biota for this Character',
  `account_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the Biota for this Character',
  `name` varchar(255) NOT NULL COMMENT 'Name of Character',
  `is_Plussed` bit(1) NOT NULL,
  `is_Deleted` bit(1) NOT NULL COMMENT 'Is this Character deleted?',
  `delete_Time` bigint(10) unsigned NOT NULL DEFAULT '0' COMMENT 'The character will be marked IsDeleted=True after this timestamp',
  `last_Login_Timestamp` double NOT NULL DEFAULT '0' COMMENT 'Timestamp the last time this character entered the world',
  `total_Logins` int(10) NOT NULL DEFAULT '0',
  `character_Options_1` int(10) NOT NULL DEFAULT '0',
  `character_Options_2` int(10) NOT NULL DEFAULT '0',
  `gameplay_Options` blob,
  `spellbook_Filters` int(10) unsigned NOT NULL DEFAULT '16383',
  `hair_Texture` int(10) unsigned NOT NULL DEFAULT '0',
  `default_Hair_Texture` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`id`),
  KEY `character_account_idx` (`account_Id`),
  KEY `character_name_idx` (`name`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='Int Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_properties_contract_registry`
--

DROP TABLE IF EXISTS `character_properties_contract_registry`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `character_properties_contract_registry` (
  `character_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the character this property belongs to',
  `contract_Id` int(10) unsigned NOT NULL,
  `delete_Contract` bit(1) NOT NULL,
  `set_As_Display_Contract` bit(1) NOT NULL,
  PRIMARY KEY (`character_Id`,`contract_Id`),
  CONSTRAINT `wcid_contract` FOREIGN KEY (`character_Id`) REFERENCES `character` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_properties_fill_comp_book`
--

DROP TABLE IF EXISTS `character_properties_fill_comp_book`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `character_properties_fill_comp_book` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `character_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the character this property belongs to',
  `spell_Component_Id` int(10) NOT NULL DEFAULT '0' COMMENT 'Id of Spell Component',
  `quantity_To_Rebuy` int(10) NOT NULL DEFAULT '0' COMMENT 'Amount of this component to add to the buy list for repurchase',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_fillcompbook_type_uidx` (`character_Id`,`spell_Component_Id`),
  CONSTRAINT `wcid_fillcompbook` FOREIGN KEY (`character_Id`) REFERENCES `character` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='FillCompBook Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_properties_friend_list`
--

DROP TABLE IF EXISTS `character_properties_friend_list`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `character_properties_friend_list` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `character_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the character this property belongs to',
  `friend_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of Friend',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_friend_uidx` (`character_Id`,`friend_Id`),
  CONSTRAINT `wcid_friend` FOREIGN KEY (`character_Id`) REFERENCES `character` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='FriendList Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_properties_quest_registry`
--

DROP TABLE IF EXISTS `character_properties_quest_registry`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `character_properties_quest_registry` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `character_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the character this property belongs to',
  `quest_Name` varchar(255) NOT NULL COMMENT 'Unique Name of Quest',
  `last_Time_Completed` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Timestamp of last successful completion',
  `num_Times_Completed` int(10) NOT NULL DEFAULT '0' COMMENT 'Number of successful completions',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_questbook_name_uidx` (`character_Id`,`quest_Name`),
  CONSTRAINT `wcid_questbook` FOREIGN KEY (`character_Id`) REFERENCES `character` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='QuestBook Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_properties_shortcut_bar`
--

DROP TABLE IF EXISTS `character_properties_shortcut_bar`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `character_properties_shortcut_bar` (
  `character_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the character this property belongs to',
  `shortcut_Bar_Index` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Position (Slot) on the Shortcut Bar for this Object',
  `shortcut_Object_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Guid of the object at this Slot',
  PRIMARY KEY (`character_Id`,`shortcut_Bar_Index`),
  KEY `wcid_shortcutbar_idx` (`character_Id`),
  CONSTRAINT `wcid_shortcutbar` FOREIGN KEY (`character_Id`) REFERENCES `character` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='ShortcutBar Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_properties_spell_bar`
--

DROP TABLE IF EXISTS `character_properties_spell_bar`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `character_properties_spell_bar` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `character_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the character this property belongs to',
  `spell_Bar_Number` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of Spell Bar',
  `spell_Bar_Index` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Position (Slot) on this Spell Bar for this Spell',
  `spell_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of Spell on this Spell Bar at this Slot',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_spellbar_barId_spellId_uidx` (`character_Id`,`spell_Bar_Number`,`spell_Id`),
  CONSTRAINT `wcid_spellbar` FOREIGN KEY (`character_Id`) REFERENCES `character` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='SpellBar Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_properties_squelch`
--

DROP TABLE IF EXISTS `character_properties_squelch`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `character_properties_squelch` (
  `character_Id` int(10) unsigned NOT NULL,
  `squelch_Character_Id` int(10) unsigned NOT NULL,
  `squelch_Account_Id` int(10) unsigned NOT NULL,
  `type` int(10) unsigned NOT NULL,
  PRIMARY KEY (`character_Id`,`squelch_Character_Id`),
  CONSTRAINT `squelch_character_Id_constraint` FOREIGN KEY (`character_Id`) REFERENCES `character` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_properties_title_book`
--

DROP TABLE IF EXISTS `character_properties_title_book`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `character_properties_title_book` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Property',
  `character_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of the character this property belongs to',
  `title_Id` int(10) unsigned NOT NULL DEFAULT '0' COMMENT 'Id of Title',
  PRIMARY KEY (`id`),
  UNIQUE KEY `wcid_titlebook_type_uidx` (`character_Id`,`title_Id`),
  CONSTRAINT `wcid_titlebook` FOREIGN KEY (`character_Id`) REFERENCES `character` (`id`) ON DELETE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8 COMMENT='TitleBook Properties of Weenies';
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `config_properties_boolean`
--

DROP TABLE IF EXISTS `config_properties_boolean`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `config_properties_boolean` (
  `key` varchar(255) NOT NULL,
  `value` bit(1) NOT NULL,
  `description` text,
  PRIMARY KEY (`key`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `config_properties_double`
--

DROP TABLE IF EXISTS `config_properties_double`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `config_properties_double` (
  `key` varchar(255) NOT NULL,
  `value` double NOT NULL,
  `description` text,
  PRIMARY KEY (`key`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `config_properties_long`
--

DROP TABLE IF EXISTS `config_properties_long`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `config_properties_long` (
  `key` varchar(255) NOT NULL,
  `value` bigint(20) NOT NULL,
  `description` text,
  PRIMARY KEY (`key`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `config_properties_string`
--

DROP TABLE IF EXISTS `config_properties_string`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `config_properties_string` (
  `key` varchar(255) NOT NULL,
  `value` text NOT NULL,
  `description` text,
  PRIMARY KEY (`key`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `house_permission`
--

DROP TABLE IF EXISTS `house_permission`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
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
/*!40101 SET character_set_client = @saved_cs_client */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2019-09-14 12:36:31
