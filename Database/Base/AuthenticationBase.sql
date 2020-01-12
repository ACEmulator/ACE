-- MySQL dump 10.13  Distrib 8.0.12, for Win64 (x86_64)
--
-- Host: localhost    Database: ace_auth
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
-- Current Database: `ace_auth`
--

/*!40000 DROP DATABASE IF EXISTS `ace_auth`*/;

CREATE DATABASE /*!32312 IF NOT EXISTS*/ `ace_auth` /*!40100 DEFAULT CHARACTER SET utf8 */;

USE `ace_auth`;

--
-- Table structure for table `accesslevel`
--

DROP TABLE IF EXISTS `accesslevel`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `accesslevel` (
  `level` int(10) unsigned NOT NULL DEFAULT '0',
  `name` varchar(45) NOT NULL,
  `prefix` varchar(45) DEFAULT '',
  PRIMARY KEY (`level`),
  UNIQUE KEY `level` (`level`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `account`
--

DROP TABLE IF EXISTS `account`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
 SET character_set_client = utf8mb4 ;
CREATE TABLE `account` (
  `accountId` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `accountName` varchar(50) NOT NULL,
  `passwordHash` varchar(88) NOT NULL COMMENT 'base64 encoded version of the hashed passwords.  88 characters are needed to base64 encode SHA512 output.',
  `passwordSalt` varchar(88) NOT NULL DEFAULT 'use bcrypt' COMMENT 'This is no longer used, except to indicate if bcrypt is being employed for migration purposes. Previously: base64 encoded version of the password salt.  512 byte salts (88 characters when base64 encoded) are recommend for SHA512.',
  `accessLevel` int(10) unsigned NOT NULL DEFAULT '0',
  `email_Address` varchar(320) DEFAULT NULL,
  `create_Time` datetime NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `create_I_P` varbinary(16) DEFAULT NULL,
  `create_I_P_ntoa` varchar(45) GENERATED ALWAYS AS (inet6_ntoa(`create_I_P`)) VIRTUAL,
  `last_Login_Time` datetime DEFAULT NULL,
  `last_Login_I_P` varbinary(16) DEFAULT NULL,
  `last_Login_I_P_ntoa` varchar(45) GENERATED ALWAYS AS (inet6_ntoa(`last_Login_I_P`)) VIRTUAL,
  `total_Times_Logged_In` int(10) unsigned NOT NULL DEFAULT '0',
  `banned_Time` datetime DEFAULT NULL,
  `banned_By_Account_Id` int(10) unsigned DEFAULT NULL,
  `ban_Expire_Time` datetime DEFAULT NULL,
  `ban_Reason` varchar(1000) DEFAULT NULL,
  PRIMARY KEY (`accountId`),
  UNIQUE KEY `accountName_uidx` (`accountName`),
  KEY `accesslevel_idx` (`accessLevel`),
  CONSTRAINT `fk_accesslevel` FOREIGN KEY (`accessLevel`) REFERENCES `accesslevel` (`level`)
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

-- Dump completed on 2019-09-14 12:35:23
 
/*
-- Query: SELECT * FROM ace_auth.accesslevel
-- Date: 2018-02-14 13:19
*/
INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (0,'Player','');
INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (1,'Advocate','');
INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (2,'Sentinel','Sentinel');
INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (3,'Envoy','Envoy');
INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (4,'Developer','');
INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (5,'Admin','Admin');
