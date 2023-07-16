USE `ace_log`;

ALTER TABLE pk_kills_log
ADD killer_arena_player_id INT;

ALTER TABLE pk_kills_log
ADD victim_arena_player_id INT;

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
-- Table structure for table `arena_player`
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
/*!40101 SET character_set_client = @saved_cs_client */;

/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

