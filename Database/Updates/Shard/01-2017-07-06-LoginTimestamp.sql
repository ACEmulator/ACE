--
-- Temporary view structure for view `vw_ace_character`
--

DROP TABLE IF EXISTS `vw_ace_character`;
/*!50001 DROP VIEW IF EXISTS `vw_ace_character`*/;
SET @saved_cs_client     = @@character_set_client;
SET character_set_client = utf8;
/*!50001 CREATE VIEW `vw_ace_character` AS SELECT 
 1 AS `guid`,
 1 AS `accountId`,
 1 AS `NAME`,
 1 AS `deleted`,
 1 AS `deleteTime`,
 1 AS `weenieClassId`,
 1 AS `weenieClassDescription`,
 1 AS `aceObjectDescriptionFlags`,
 1 AS `physicsDescriptionFlag`,
 1 AS `weenieHeaderFlags`,
 1 AS `itemType`,
 1 AS `loginTimestamp`*/;
SET character_set_client = @saved_cs_client;

--
-- Final view structure for view `vw_ace_character`
--

/*!50001 DROP VIEW IF EXISTS `vw_ace_character`*/;
/*!50001 SET @saved_cs_client          = @@character_set_client */;
/*!50001 SET @saved_cs_results         = @@character_set_results */;
/*!50001 SET @saved_col_connection     = @@collation_connection */;
/*!50001 SET character_set_client      = utf8 */;
/*!50001 SET character_set_results     = utf8 */;
/*!50001 SET collation_connection      = utf8_general_ci */;
/*!50001 CREATE ALGORITHM=UNDEFINED */
/*!50013 DEFINER=`root`@`localhost` SQL SECURITY DEFINER */
/*!50001 VIEW `vw_ace_character` AS select `ao`.`aceObjectId` AS `guid`,`aopiidacc`.`propertyValue` AS `accountId`,`aops`.`propertyValue` AS `NAME`,`aopb`.`propertyValue` AS `deleted`,`aopbi`.`propertyValue` AS `deleteTime`,`ao`.`weenieClassId` AS `weenieClassId`,`awc`.`weenieClassDescription` AS `weenieClassDescription`,`ao`.`aceObjectDescriptionFlags` AS `aceObjectDescriptionFlags`,`ao`.`physicsDescriptionFlag` AS `physicsDescriptionFlag`,`ao`.`weenieHeaderFlags` AS `weenieHeaderFlags`,`aopi`.`propertyValue` AS `itemType`, `aopd`.`propertyValue` as `loginTimestamp` from ((((((`ace_object` `ao` join `ace_weenie_class` `awc` on((`ao`.`weenieClassId` = `awc`.`weenieClassId`))) join `ace_object_properties_string` `aops` on(((`ao`.`aceObjectId` = `aops`.`aceObjectId`) and (`aops`.`strPropertyId` = 1)))) join `ace_object_properties_bool` `aopb` on(((`ao`.`aceObjectId` = `aopb`.`aceObjectId`) and (`aopb`.`boolPropertyId` = 9001)))) join `ace_object_properties_int` `aopi` on(((`ao`.`aceObjectId` = `aopi`.`aceObjectId`) and (`aopi`.`intPropertyId` = 1)))) join `ace_object_properties_bigint` `aopbi` on(((`ao`.`aceObjectId` = `aopbi`.`aceObjectId`) and (`aopbi`.`bigIntPropertyId` = 9001)))) join `ace_object_properties_iid` `aopiidacc` on(((`ao`.`aceObjectId` = `aopiidacc`.`aceObjectId`) and (`aopiidacc`.`iidPropertyId` = 9001))) join `ace_object_properties_double` `aopd` on(((`ao`.`aceObjectId` = `aopd`.`aceObjectId`) and (`aopd`.`dblPropertyId` = 48)))) */;
/*!50001 SET character_set_client      = @saved_cs_client */;
/*!50001 SET character_set_results     = @saved_cs_results */;
/*!50001 SET collation_connection      = @saved_col_connection */;
