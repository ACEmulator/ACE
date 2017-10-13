
ALTER TABLE `ace_object`   
  ADD COLUMN `userModified` TINYINT(1) DEFAULT 0 NOT NULL COMMENT 'flag indicating whether or not this has record has been altered since deployment' AFTER `weenieClassId`;
