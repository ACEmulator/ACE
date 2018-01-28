USE `ace_auth`;

ALTER TABLE `account` 
DROP COLUMN `githubProviderKey`,
DROP COLUMN `githubEmail`,
DROP COLUMN `githubSshKey`,
DROP COLUMN `email`,
DROP COLUMN `displayName`,
DROP COLUMN `accountGuid`,
DROP INDEX `accountGuid_uidx` ;

ALTER TABLE `account` 
ADD COLUMN `accessLevel` INT(10) UNSIGNED NOT NULL DEFAULT '0' AFTER `passwordSalt`,
ADD INDEX `accesslevel_idx` (`accessLevel` ASC);
ALTER TABLE `account` 
ADD CONSTRAINT `fk_accesslevel`
  FOREIGN KEY (`accessLevel`)
  REFERENCES `accesslevel` (`level`)
  ON DELETE NO ACTION
  ON UPDATE NO ACTION;

CREATE 
     OR REPLACE ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `vw_account_by_name` AS
    (SELECT 
        `account`.`accountId` AS `accountId`,
        `account`.`accountName` AS `accountName`,
        `account`.`passwordHash` AS `passwordHash`,
        `account`.`passwordSalt` AS `passwordSalt`,
        `account`.`accessLevel` AS `accessLevel`
    FROM
        `account`);
                
DROP VIEW `vw_subscription_by_account`;

DROP TABLE `subscription`;

