USE `ace_auth`;

ALTER TABLE `account` 
CHANGE COLUMN `passwordSalt` `passwordSalt` VARCHAR(88) NOT NULL DEFAULT 'use bcrypt' COMMENT 'This is no longer used, except to indicate if bcrypt is being employed for migration purposes. Previously: base64 encoded version of the password salt.  512 byte salts (88 characters when base64 encoded) are recommend for SHA512.' ;
