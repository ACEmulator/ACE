/* Mark Character as Deleted Event Script */
/*                                        */

SET GLOBAL event_scheduler = ON;
delimiter |
CREATE EVENT IF NOT EXISTS check_for_characters_to_delete
ON SCHEDULE EVERY 1 MINUTE
STARTS CURRENT_TIMESTAMP
DO
	BEGIN
		SET SQL_SAFE_UPDATES = 0;
		UPDATE `character` SET deleted = 1 WHERE deleted = 0 AND NOT deleteTime = 0 AND unix_timestamp() > deleteTime;
		SET SQL_SAFE_UPDATES = 1;
    END |  
delimiter ;