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

INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (0,'NoAccess','');
INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (1,'Player','');
INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (2,'Advocate','');
INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (3,'Sentinel','Sentinel');
INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (4,'Envoy','Envoy');
INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (5,'Developer','');
INSERT INTO `accesslevel` (`level`,`name`,`prefix`) VALUES (6,'Admin','Admin');

UPDATE `account` SET `accesslevel` = 1 WHERE `accesslevel` = 0;
UPDATE `account` SET `accesslevel` = 2 WHERE `accesslevel` = 1;
UPDATE `account` SET `accesslevel` = 3 WHERE `accesslevel` = 2;
UPDATE `account` SET `accesslevel` = 4 WHERE `accesslevel` = 3;
UPDATE `account` SET `accesslevel` = 5 WHERE `accesslevel` = 4;
UPDATE `account` SET `accesslevel` = 6 WHERE `accesslevel` = 5;