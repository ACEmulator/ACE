ALTER TABLE `account` DROP COLUMN password;
ALTER TABLE `account` DROP COLUMN salt;
ALTER TABLE `account` ADD COLUMN hashed_password CHAR(102) CHARACTER SET latin1 COLLATE latin1_bin;