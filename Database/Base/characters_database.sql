  CREATE TABLE `characters` (
  `Character_ID` int(11) NOT NULL AUTO_INCREMENT,
  `Owner_ID` int(11) unsigned DEFAULT NULL,
  `Name` varchar(50) DEFAULT NULL,
  `Birth` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  `TotalLogins` int(11) DEFAULT '0',
  `Deleted` int(11) DEFAULT '0',
  `DeleteTime` bigint(20) unsigned NOT NULL DEFAULT '0',
  PRIMARY KEY (`UA_ID`),
  UNIQUE KEY `Name` (`Name`)
) ENGINE=InnoDB AUTO_INCREMENT=50 DEFAULT CHARSET=utf8;

