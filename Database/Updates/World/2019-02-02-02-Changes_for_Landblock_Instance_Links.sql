USE `ace_world`;

ALTER TABLE `landblock_instance_link` 
;
ALTER TABLE `landblock_instance_link` RENAME INDEX `parent_child_guuidx` TO `parent_child_uidx`;
/* ALTER TABLE `landblock_instance_link` ALTER INDEX `parent_child_uidx` INVISIBLE; */

/*
ALTER TABLE `landblock_instance_link` 
ADD UNIQUE INDEX `child_uidx` (`child_GUID` ASC) /*VISIBLE*/;
;

ALTER TABLE `landblock_instance_link` 
ADD INDEX `child_idx` (`child_GUID` ASC);
;

/*
ALTER TABLE `landblock_instance_link` 
DROP FOREIGN KEY `instance_link`;
ALTER TABLE `landblock_instance_link` 
DROP INDEX `parent_child_uidx` ;
;
*/
