USE `ace_world`;

ALTER TABLE `landblock_instance_link` 
ADD UNIQUE INDEX `parent_child_uidx` (`parent_GUID` ASC, `child_GUID` ASC),
ADD INDEX `child_idx` (`child_GUID` ASC),
DROP INDEX `parent_child_guuidx` ;
;
