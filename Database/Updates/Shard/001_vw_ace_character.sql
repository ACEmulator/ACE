
CREATE
    /*[ALGORITHM = {UNDEFINED | MERGE | TEMPTABLE}]
    [DEFINER = { user | CURRENT_USER }]
    [SQL SECURITY { DEFINER | INVOKER }]*/
    VIEW `ace_shard`.`vw_ace_character` 
    AS
(SELECT
  ao.aceObjectId AS guid
  , aopiacc.propertyValue AS accountId
  , aops.propertyValue AS NAME
  , aopb.propertyValue AS deleted
  , '00:00:00' AS deleteTime
  , ao.weenieClassId AS weenieClassId
  , awc.weenieClassDescription AS weenieClassDescription
  , ao.aceObjectDescriptionFlags AS aceObjectDescriptionFlags
  , ao.physicsDescriptionFlag AS physicsDescriptionFlag
  , ao.weenieHeaderFlags AS weenieHeaderFlags
  , aopi.propertyValue AS itemType
  , ap.positionId AS positionId
  , ap.positionType AS positionType
  , ap.landblockRaw AS LandblockRaw
  , ap.posX AS posX
  , ap.posY AS posY
  , ap.posZ AS posZ
  , ap.qW AS qW
  , ap.qX AS qX
  , ap.qY AS qY
  , ap.qZ AS qZ
FROM
  ace_object ao
  JOIN ace_weenie_class awc
    ON ao.weenieClassId = awc.weenieClassId
  JOIN ace_object_properties_string aops
    ON ao.aceObjectId = aops.aceObjectId
    AND aops.strPropertyId = 1
  JOIN ace_object_properties_bool aopb
    ON ao.aceObjectId = aopb.aceObjectId
    AND aopb.boolPropertyId = 9001
  JOIN ace_object_properties_int aopi
    ON ao.aceObjectId = aopi.aceObjectId
    AND aopi.intPropertyId = 1
  JOIN ace_position ap
    ON ao.aceObjectId = ap.aceObjectId
    AND ap.positionType = 1
  JOIN ace_object_properties_int aopiacc
    ON ao.aceObjectId = aopiacc.aceObjectId
    AND aopiacc.intPropertyId = 9001);
