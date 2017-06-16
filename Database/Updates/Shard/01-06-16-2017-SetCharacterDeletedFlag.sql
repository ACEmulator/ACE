DELIMITER $$

CREATE DEFINER=`root`@`localhost` EVENT `SetCharacterDeletedFlag` ON SCHEDULE EVERY 15 MINUTE STARTS '2017-06-16 16:15:35' ON COMPLETION NOT PRESERVE ENABLE DO BEGIN
 UPDATE
      ace_object_properties_bool aopb
      INNER JOIN ace_object_properties_bigint aopbi
         ON (
            aopb.aceObjectId = aopbi.aceObjectId
            AND aopbi.bigIntPropertyId = 9001
         ) SET aopb.propertyValue = 1
   WHERE aopb.boolPropertyId = 9001
      AND aopbi.propertyValue != 0
      AND (FROM_UNIXTIME(aopbi.propertyValue) < NOW());
      

   END$$

DELIMITER ;