CREATE DATABASE  IF NOT EXISTS `ace_auth` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `ace_auth`;
-- MySQL dump 10.13  Distrib 5.7.17, for Win64 (x86_64)
--
-- Host: localhost    Database: ace_auth
-- ------------------------------------------------------
-- Server version	5.7.19-log

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
-- Table structure for table `accesslevel`
--

DROP TABLE IF EXISTS `accesslevel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `accesslevel` (
  `level` int(10) unsigned NOT NULL DEFAULT '0',
  `name` varchar(45) NOT NULL,
  `prefix` varchar(45) DEFAULT '',
  PRIMARY KEY (`level`),
  UNIQUE KEY `level` (`level`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `accesslevel`
--

LOCK TABLES `accesslevel` WRITE;
/*!40000 ALTER TABLE `accesslevel` DISABLE KEYS */;
INSERT INTO `accesslevel` VALUES (0,'Player',''),(1,'Advocate',''),(2,'Sentinel','Sentinel'),(3,'Envoy','Envoy'),(4,'Developer',''),(5,'Admin','Admin');
/*!40000 ALTER TABLE `accesslevel` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `account`
--

DROP TABLE IF EXISTS `account`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `account` (
  `accountId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `accountGuid` binary(16) NOT NULL,
  `accountName` varchar(50) NOT NULL,
  `displayName` varchar(50) NOT NULL,
  `passwordHash` varchar(88) NOT NULL COMMENT 'base64 encoded version of the hashed passwords.  88 characters are needed to base64 encode SHA512 output.',
  `passwordSalt` varchar(88) NOT NULL COMMENT 'base64 encoded version of the password salt.  512 byte salts (88 characters when base64 encoded) are recommend for SHA512.',
  `email` varchar(280) DEFAULT NULL,
  `githubSshKey` varchar(1000) DEFAULT NULL,
  `githubEmail` varchar(280) DEFAULT NULL,
  `githubProviderKey` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`accountId`),
  UNIQUE KEY `accountName_uidx` (`accountName`),
  UNIQUE KEY `accountGuid_uidx` (`accountGuid`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `account`
--

LOCK TABLES `account` WRITE;
/*!40000 ALTER TABLE `account` DISABLE KEYS */;
/*!40000 ALTER TABLE `account` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Table structure for table `subscription`
--

DROP TABLE IF EXISTS `subscription`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!40101 SET character_set_client = utf8 */;
CREATE TABLE `subscription` (
  `subscriptionId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `subscriptionGuid` binary(16) NOT NULL,
  `accountGuid` binary(16) NOT NULL COMMENT 'no foreign key because server might use a different auth server',
  `subscriptionName` varchar(100) NOT NULL,
  `accessLevel` int(10) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`subscriptionId`),
  UNIQUE KEY `subscriptionGuid_uidx` (`subscriptionGuid`),
  KEY `accesslevel_idx` (`accessLevel`),
  CONSTRAINT `accesslevel` FOREIGN KEY (`accessLevel`) REFERENCES `accesslevel` (`level`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Dumping data for table `subscription`
--

LOCK TABLES `subscription` WRITE;
/*!40000 ALTER TABLE `subscription` DISABLE KEYS */;
/*!40000 ALTER TABLE `subscription` ENABLE KEYS */;
UNLOCK TABLES;

--
-- Temporary view structure for view `vw_account_by_name`
--

DROP TABLE IF EXISTS `vw_account_by_name`;
/*!50001 DROP VIEW IF EXISTS `vw_account_by_name`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `vw_account_by_name` AS SELECT 
 1 AS `accountId`,
 1 AS `accountGuid`,
 1 AS `accountName`,
 1 AS `displayName`,
 1 AS `passwordHash`,
 1 AS `passwordSalt`,
 1 AS `email`,
 1 AS `githubSshKey`,
 1 AS `githubEmail`,
 1 AS `githubProviderKey`*/;
SET character_set_client = @saved_cs_client;

--
-- Temporary view structure for view `vw_subscription_by_account`
--

DROP TABLE IF EXISTS `vw_subscription_by_account`;
/*!50001 DROP VIEW IF EXISTS `vw_subscription_by_account`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `vw_subscription_by_account` AS SELECT 
 1 AS `subscriptionId`,
 1 AS `subscriptionGuid`,
 1 AS `accountGuid`,
 1 AS `subscriptionName`,
 1 AS `accessLevel`*/;
SET character_set_client = @saved_cs_client;

--
-- Final view structure for view `vw_account_by_name`
--

/*!50001 DROP VIEW IF EXISTS `vw_account_by_name`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_account_by_name` AS (select `account`.`accountId` AS `accountId`,`account`.`accountGuid` AS `accountGuid`,`account`.`accountName` AS `accountName`,`account`.`displayName` AS `displayName`,`account`.`passwordHash` AS `passwordHash`,`account`.`passwordSalt` AS `passwordSalt`,`account`.`email` AS `email`,`account`.`githubSshKey` AS `githubSshKey`,`account`.`githubEmail` AS `githubEmail`,`account`.`githubProviderKey` AS `githubProviderKey` from `account`) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;

--
-- Final view structure for view `vw_subscription_by_account`
--

/*!50001 DROP VIEW IF EXISTS `vw_subscription_by_account`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_subscription_by_account` AS (select `subscription`.`subscriptionId` AS `subscriptionId`,`subscription`.`subscriptionGuid` AS `subscriptionGuid`,`subscription`.`accountGuid` AS `accountGuid`,`subscription`.`subscriptionName` AS `subscriptionName`,`subscription`.`accessLevel` AS `accessLevel` from `subscription`) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
/*!40103 SET TIME_ZONE=@OLD_TIME_ZONE */;

/*!40101 SET SQL_MODE=@OLD_SQL_MODE */;
/*!40014 SET FOREIGN_KEY_CHECKS=@OLD_FOREIGN_KEY_CHECKS */;
/*!40014 SET UNIQUE_CHECKS=@OLD_UNIQUE_CHECKS */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
/*!40111 SET SQL_NOTES=@OLD_SQL_NOTES */;

-- Dump completed on 2017-10-29 16:04:45
