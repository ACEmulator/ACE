USE `ace_auth`;

ALTER TABLE `account` 
DROP COLUMN `githubProviderKey`,
DROP COLUMN `githubEmail`,
DROP COLUMN `githubSshKey`,
DROP COLUMN `email`,
DROP COLUMN `displayName`,
DROP COLUMN `accountGuid`,
DROP INDEX `accountGuid_uidx` ;

CREATE 
     OR REPLACE ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `vw_account_by_name` AS
    (SELECT 
        `account`.`accountId` AS `accountId`,
        `account`.`accountName` AS `accountName`,
        `account`.`passwordHash` AS `passwordHash`,
        `account`.`passwordSalt` AS `passwordSalt`
    FROM
        `account`);
                
DROP VIEW `vw_subscription_by_account`;

DROP TABLE `subscription`;
