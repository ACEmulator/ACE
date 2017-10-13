
DROP TABLE IF EXISTS `account`;

CREATE TABLE `account` (
  `accountId` INT(10) UNSIGNED NOT NULL AUTO_INCREMENT,
  `accountGuid` BINARY(16) NOT NULL,
  `accountName` VARCHAR(50) NOT NULL,
  `displayName` VARCHAR(50) NOT NULL,
  `accessLevel` INT(10) UNSIGNED NOT NULL DEFAULT '0',
  `passwordHash` VARCHAR(88) NOT NULL COMMENT 'base64 encoded version of the hashed passwords.  88 characters are needed to base64 encode SHA512 output.',
  `passwordSalt` VARCHAR(88) NOT NULL COMMENT 'base64 encoded version of the password salt.  512 byte salts (88 characters when base64 encoded) are recommend for SHA512.',
  `email` VARCHAR(280) NULL,
  `githubSshKey` VARCHAR(1000) NULL,
  `githubEmail` VARCHAR(280) NULL,
  `githubProviderKey` VARCHAR(100) NULL,
  PRIMARY KEY (`accountId`),
  UNIQUE KEY `accountName_uidx` (`accountName`),
  UNIQUE KEY `accountGuid_uidx` (`accountGuid`),
  KEY `accesslevel_idx` (`accesslevel`),
  CONSTRAINT `accesslevel` FOREIGN KEY (`accesslevel`) REFERENCES `accesslevel` (`level`) ON DELETE NO ACTION ON UPDATE NO ACTION
) ENGINE=INNODB DEFAULT CHARSET=utf8;

DROP VIEW IF EXISTS `vw_account_by_name` ;

CREATE VIEW `vw_account_by_name` 
    AS
(SELECT accountId, accountGuid, accountName, displayName, accessLevel, passwordHash, passwordSalt, email, githubSshKey, githubEmail, githubProviderKey FROM account);
