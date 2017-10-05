ALTER TABLE `ace_auth`.`account`   
  ADD COLUMN `email` VARCHAR(280) NULL AFTER `salt`,
  ADD COLUMN `githubSshKey` VARCHAR(1000) NULL AFTER `email`,
  ADD COLUMN `githubEmail` VARCHAR(280) NULL AFTER `githubSshKey`,
  ADD COLUMN `githubProviderKey` VARCHAR(100) NULL AFTER `githubEmail`;
