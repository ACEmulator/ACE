-- MySQL dump 10.13  Distrib 8.0.22, for Linux (x86_64)
--
-- Host: localhost    Database: ace_log
-- ------------------------------------------------------
-- Server version	8.0.22

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!50503 SET NAMES utf8mb4 */;
/*!40103 SET @OLD_TIME_ZONE=@@TIME_ZONE */;
/*!40103 SET TIME_ZONE='+00:00' */;
/*!40014 SET @OLD_UNIQUE_CHECKS=@@UNIQUE_CHECKS, UNIQUE_CHECKS=0 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;
/*!40111 SET @OLD_SQL_NOTES=@@SQL_NOTES, SQL_NOTES=0 */;

--
-- Current Database: `ace_log`
--

/*!40000 DROP DATABASE IF EXISTS `ace_log`*/;

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `ace_log` /*!40100 DEFAULT CHARACTER SET utf8mb4 */ /*!80016 DEFAULT ENCRYPTION='N' */;

USE `ace_log`;

--
-- Table structure for table `tinker_log`
--

DROP TABLE IF EXISTS `tinker_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `tinker_log` (
  `tinkerLogId` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `characterId` INT UNSIGNED NOT NULL,
  `characterName` VARCHAR(50) NOT NULL,
  `itemBiotaId` INT UNSIGNED,
  `tinkDateTime` DATETIME,
  `successChance` FLOAT,
  `roll` FLOAT,
  `isSuccess` BOOLEAN,
  `itemNumPreviousTinks` INT,
  `itemWorkmanship` INT,
  `salvageType` VARCHAR(50),
  `salvageWorkmanship` INT,
  PRIMARY KEY (`tinkerLogId`)  
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `account_session_log`
--

DROP TABLE IF EXISTS `account_session_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `account_session_log` (
  `sessionLogId` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `accountId` INT UNSIGNED NOT NULL,
  `accountName` VARCHAR(50) NOT NULL,  
  `sessionIP` VARCHAR(45),
  `loginDateTime` DATETIME,  
  `logoutDateTime` DATETIME,
  PRIMARY KEY (`sessionLogId`)  
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `character_login_log`
--

DROP TABLE IF EXISTS `character_login_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `character_login_log` (
  `characterLoginLogId` INT UNSIGNED NOT NULL AUTO_INCREMENT,
  `accountId` INT UNSIGNED NOT NULL,
  `accountName` VARCHAR(50) NOT NULL, 
  `characterId` INT UNSIGNED NOT NULL,
  `characterName` VARCHAR(50) NOT NULL,   
  `sessionIP` VARCHAR(45),
  `loginDateTime` DATETIME,  
  `logoutDateTime` DATETIME,
  PRIMARY KEY (`characterLoginLogId`)  
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `pk_kills_log`
--

DROP TABLE IF EXISTS `pk_kills_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `pk_kills_log` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of this Kill',
  `killer_id` INT UNSIGNED NOT NULL COMMENT 'Unique Id of Killer Character',
  `victim_id` INT UNSIGNED NOT NULL COMMENT 'Unique Id of Victim Character',
  `killer_monarch_id` INT UNSIGNED,
  `victim_monarch_id` INT UNSIGNED,
  `kill_datetime` DATETIME NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `killer_arena_player_id` INT,
  `victim_arena_player_id` INT,
  PRIMARY KEY (`id`)
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;


/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

--
-- Table structure for table `arena_event
--

DROP TABLE IF EXISTS `arena_event`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `arena_event` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of the arena event',  
  `event_type` VARCHAR(16),
  `location` INT UNSIGNED,
  `status` INT,
  `start_datetime` DATETIME,
  `end_datetime` DATETIME,
  `winning_team_guid` VARCHAR(36),
  `cancel_reason` VARCHAR(500),
  `is_overtime` BIT NOT NULL DEFAULT(0),
  `create_datetime` DATETIME,
  PRIMARY KEY (`id`)
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `arena_player`
--

DROP TABLE IF EXISTS `arena_player`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `arena_player` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of the arena player instance',  
  `character_id` INT UNSIGNED,
  `character_name` VARCHAR(255),
  `character_level` INT UNSIGNED,
  `event_type`  VARCHAR(16),  
  `monarch_id` INT UNSIGNED,
  `monarch_name` VARCHAR(255), 
  `event_id` INT UNSIGNED,
  `team_guid` CHAR(36),
  `is_eliminated` BIT,
  `finish_place` INT,
  `total_deaths` INT UNSIGNED,
  `total_kills` INT UNSIGNED,
  `total_dmg_dealt` INT UNSIGNED,
  `total_dmg_received` INT UNSIGNED,
  `create_datetime` DATETIME,
  `player_ip` VARCHAR(25),  
  PRIMARY KEY (`id`)
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;


--
-- Table structure for table `arena_character_stats`
--

DROP TABLE IF EXISTS `arena_character_stats`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `arena_character_stats` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of the arena stats instance',  
  `character_id` INT UNSIGNED,
  `character_name` VARCHAR(255),
  `event_type` VARCHAR(12),
  `total_matches` INT UNSIGNED,
  `total_wins` INT UNSIGNED,
  `total_losses` INT UNSIGNED,
  `total_draws` INT UNSIGNED,
  `total_disqualified` INT UNSIGNED,  
  `total_deaths` INT UNSIGNED,
  `total_kills` INT UNSIGNED,
  `total_dmg_dealt` INT UNSIGNED,
  `total_dmg_received` INT UNSIGNED,
  `rank_points` INT UNSIGNED,
  PRIMARY KEY (`id`)
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;

/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

USE `ace_log`;

--
-- Table structure for table rare_log
--

DROP TABLE IF EXISTS `rare_log`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `rare_log` (
  `id` INT UNSIGNED NOT NULL AUTO_INCREMENT COMMENT 'Unique Id of the arena event',  
  `characterName` VARCHAR(255),
  `characterId` INT UNSIGNED,
  `itemName` VARCHAR(255),
  `itemBiotaId` INT UNSIGNED,
  `itemWeenieId` INT UNSIGNED,
  `createDateTime` DATETIME,  
  PRIMARY KEY (`id`)
) ENGINE=INNODB DEFAULT CHARSET=utf8mb4;
/*!40101 SET character_set_client = @saved_cs_client */;

/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;
