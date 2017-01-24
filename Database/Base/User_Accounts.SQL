CREATE TABLE `User_Accounts` (
  `ID` int(11) NOT NULL AUTO_INCREMENT,
  `UserName` varchar(50) DEFAULT NULL,
  `Password` varchar(50) DEFAULT NULL,
  `Email` varchar(75) DEFAULT NULL,
  `salt` varchar(64) NOT NULL DEFAULT '',
  PRIMARY KEY (`ID`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;
