USE `ace_world`;

DROP TRIGGER IF EXISTS `landblock_instance_BEFORE_INSERT`;

DELIMITER $$
CREATE DEFINER = CURRENT_USER TRIGGER `landblock_instance_BEFORE_INSERT` BEFORE INSERT ON `landblock_instance` FOR EACH ROW
BEGIN
	IF !(NEW.guid >= 0x70000000 && NEW.guid <= 0x7FFFFFFF)
	THEN
		-- don't allow the insert to happen
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = "Cannot add row: guid column value must be between 0x70000000 and 0x7FFFFFFF";
	END IF;
END$$
DELIMITER ;

DROP TRIGGER IF EXISTS `landblock_instance_BEFORE_UPDATE`;

DELIMITER $$
CREATE DEFINER = CURRENT_USER TRIGGER `landblock_instance_BEFORE_UPDATE` BEFORE UPDATE ON `landblock_instance` FOR EACH ROW
BEGIN
	IF !(NEW.guid >= 0x70000000 && NEW.guid <= 0x7FFFFFFF)
	THEN
		-- don't allow the insert to happen
		SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = "Cannot update row: guid column value must be between 0x70000000 and 0x7FFFFFFF";
	END IF;
END$$
DELIMITER ;
