USE `ace_auth`;

-- These columns are not used by ACE and are optional
-- They can be useful if you're using a direct database tool like phpMyAdmin or MySQL Workbench to view records
-- These columns show the decimal IP address representations using the binary stores used and maintained by ACE

ALTER TABLE `ace_auth`.`account` 
DROP COLUMN `create_I_P_ntoa`,
DROP COLUMN `last_Login_I_P_ntoa`;

