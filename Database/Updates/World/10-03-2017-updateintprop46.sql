# Clear out the old - none existed on 46 before this script
# Since I am not using the primary key, I need to turn safe deletes off to clear these out.
SET SQL_SAFE_UPDATES = 0;
DELETE
FROM
   ace_world.ace_object_properties_int
WHERE intPropertyId = 46;
SET SQL_SAFE_UPDATES = 1;

#ID the UA items
 INSERT INTO ace_world.ace_object_properties_int (
   aceObjectId
   , intPropertyId
   , propertyValue
)
SELECT
   weenieClassId
   , 46
   , CONV('8000003c', 16, 10)
FROM
   vw_ace_weenie_class wc
   LEFT OUTER JOIN ace_object_properties_int aopi
      ON wc.aceObjectId = aopi.aceObjectId
      AND aopi.intPropertyId = 51
WHERE (
      weenieclassdescription LIKE '%Katar%'
      OR weenieclassdescription LIKE '%Cestus%'
      OR weenieclassdescription LIKE '%Knuckles%'
      OR weenieclassdescription LIKE '%Claw%'
      OR weenieclassdescription LIKE '%Nekode%'
      OR weenieclassdescription LIKE '%Fist%'
      OR weenieclassdescription LIKE '%Korua%'
      OR weenieclassdescription LIKE '%Matihao%'
      OR weenieclassdescription LIKE '%SteelButterfly%'
      OR weenieclassdescription LIKE '%CrescentMoons%'
      OR weenieclassdescription LIKE '%Gauraloi%'
      OR weenieclassdescription LIKE '%Skulpuncher%'
      OR weenieclassdescription LIKE '%Needletooth%'
      OR weenieclassdescription LIKE '%Handwraps%'
      OR weenieClassDescription LIKE '%Basalt%'
   )
   AND wc.itemtype = 1
   AND aopi.propertyValue NOT IN (3, 4, 5)
ORDER BY weenieClassid;

# ID the two handed weapons sword stance
INSERT INTO ace_world.ace_object_properties_int (
   aceObjectId
   , intPropertyId
   , propertyValue
)
 SELECT
    weenieClassId
   , 46
   , CONV('80000044', 16, 10)
FROM
   vw_ace_weenie_class wc
   LEFT OUTER JOIN ace_object_properties_int aopi
      ON wc.aceObjectId = aopi.aceObjectId
      AND aopi.intPropertyId = 51
WHERE wc.itemType = 1
   AND aopi.propertyValue = 5
   AND weenieclassdescription NOT LIKE '%mace%' AND weenieclassdescription NOT LIKE '%Tetsubo'
ORDER BY wc.weenieClassId;

# ID the two handed weapons staff stance
INSERT INTO ace_world.ace_object_properties_int (
   aceObjectId
   , intPropertyId
   , propertyValue
)
 SELECT
    weenieClassId
   , 46
   , CONV('80000044', 16, 10)
FROM
   vw_ace_weenie_class wc
   LEFT OUTER JOIN ace_object_properties_int aopi
      ON wc.aceObjectId = aopi.aceObjectId
      AND aopi.intPropertyId = 51
WHERE wc.itemType = 1
   AND aopi.propertyValue = 5
   AND weenieclassdescription LIKE '%mace%' OR weenieclassdescription LIKE '%Tetsubo'
ORDER BY wc.weenieClassId;

#ID the crossbows
 INSERT INTO ace_world.ace_object_properties_int (
   aceObjectId
   , intPropertyId
   , propertyValue
)
SELECT
   weenieClassId
   , 46
   , CONV('80000041', 16, 10)
FROM
   vw_ace_weenie_class wc
   LEFT OUTER JOIN ace_object_properties_int aopi
      ON wc.aceObjectId = aopi.aceObjectId
      AND aopi.intPropertyId = 51
WHERE (
      weenieclassdescription LIKE '%crossbow%'
      OR weenieclassdescription LIKE '%Arbalest%'
      OR weenieclassdescription LIKE '%IronBull%'
      OR weenieclassdescription LIKE '%FeatheredRazor%'
      OR weenieclassdescription LIKE '%AudetaungasKalindanoftheMountains%'
      OR weenieclassdescription LIKE '%Kalindan%'
      OR weenieclassdescription LIKE '%Palauloi%'
      OR weenieclassdescription LIKE '%VortexThorn%'
      OR weenieclassdescription LIKE '%ZefirsBreath%'
   )
   AND itemtype = 256
   AND aopi.propertyValue NOT IN (3, 4, 5)
ORDER BY weenieClassid;

#ID the Atlatl
 INSERT INTO ace_world.ace_object_properties_int (
   aceObjectId
   , intPropertyId
   , propertyValue
)
SELECT
   weenieClassId
   , 46
   , CONV('80000138', 16, 10)
FROM
   vw_ace_weenie_class wc
   LEFT OUTER JOIN ace_object_properties_int aopi
      ON wc.aceObjectId = aopi.aceObjectId
      AND aopi.intPropertyId = 51
WHERE (
      weenieclassdescription LIKE '%atlatl%'
      OR weenieClassDescription LIKE '%DartFlinger%'
      OR weenieClassDescription LIKE '%CrimsonBraceofPain%'
      OR weenieClassDescription LIKE '%Eyeslayer%'
   )
   AND itemtype = 256
   AND aopi.propertyValue NOT IN (3, 4, 5)
ORDER BY weenieClassid;

#ID Thrown
 INSERT INTO ace_world.ace_object_properties_int (
   aceObjectId
   , intPropertyId
   , propertyValue
)
SELECT
   weenieClassId
   , 46
   , CONV('80000047', 16, 10)
FROM
   vw_ace_weenie_class wc
   LEFT OUTER JOIN ace_object_properties_int aopi
      ON wc.aceObjectId = aopi.aceObjectId
      AND aopi.intPropertyId = 51
WHERE (
      weenieclassdescription LIKE '%throwing%'
      OR weenieClassDescription LIKE '%phial%'
      OR weenieClassDescription LIKE '%spike%'
      OR weenieClassDescription LIKE '%pumpkin%'
      OR weenieClassDescription LIKE '%lantern%'
      OR weenieClassDescription LIKE '%skull%'
      OR weenieClassDescription LIKE '%slingshot%'
      OR weenieClassDescription LIKE '%lumpof%'
      OR weenieClassDescription LIKE '%hatchet%'
      OR weenieClassDescription LIKE '%Javelin%'
      OR weenieClassDescription LIKE '%Ball%'
      OR weenieClassDescription LIKE '%brace%'
      OR weenieClassDescription LIKE '%shard%'
      OR weenieClassDescription LIKE '%coconut%'
      OR weenieClassDescription LIKE '%rock%'
      OR weenieClassDescription LIKE '%axe%'
      OR weenieClassDescription = 'bowl'
      OR weenieClassDescription LIKE '%chalice%'
      OR weenieClassDescription LIKE '%bouquet%'
      OR weenieClassDescription LIKE '%discus%'
      OR weenieClassDescription LIKE '%flagon%'
      OR weenieClassDescription LIKE '%cup%'
      OR weenieClassDescription LIKE '%granade%'
      OR weenieClassDescription LIKE '%goblet%'
      OR weenieClassDescription LIKE '%boulder%'
      OR weenieClassDescription LIKE '%tankard%'
      OR weenieClassDescription LIKE '%ewer%'
      OR weenieClassDescription LIKE '%plate%'
      OR weenieClassDescription LIKE '%djar%'
      OR weenieClassDescription LIKE '%mug%'
      OR weenieClassDescription LIKE '%nannerpie%'
   )
   AND itemtype = 256
   AND aopi.propertyValue NOT IN (3, 4, 5)
ORDER BY weenieClassid;

#By process of elimination this is the bows.
 INSERT INTO ace_world.ace_object_properties_int (
   aceObjectId
   , intPropertyId
   , propertyValue
)
SELECT
   weenieClassId
   , 46
   , CONV('8000003f', 16, 10)
FROM
   vw_ace_weenie_class wc
   LEFT OUTER JOIN ace_object_properties_int aopi
      ON wc.aceObjectId = aopi.aceObjectId
      AND aopi.intPropertyId = 46
   LEFT OUTER JOIN ace_object_properties_int aopi2
      ON wc.aceObjectId = aopi2.aceObjectId
      AND aopi2.intPropertyId = 51
WHERE wc.itemType = 256
   AND aopi.aceObjectId IS NULL
   AND aopi2.propertyValue NOT IN (3, 4, 5)
ORDER BY wc.weenieClassId;

#By process of elimination this is the melee weapons.
 INSERT INTO ace_world.ace_object_properties_int (
   aceObjectId
   , intPropertyId
   , propertyValue
)
SELECT
   weenieClassId
   , 46
   , CONV('8000003e', 16, 10)
   FROM
   vw_ace_weenie_class wc
   LEFT OUTER JOIN ace_object_properties_int aopi
      ON wc.aceObjectId = aopi.aceObjectId
      AND aopi.intPropertyId = 46
LEFT OUTER JOIN ace_object_properties_int aopi2
      ON wc.aceObjectId = aopi2.aceObjectId
      AND aopi2.intPropertyId = 51      
WHERE wc.itemType = 1
   AND aopi.aceObjectId IS NULL  
   AND aopi2.propertyValue NOT IN (3, 4, 5) 
ORDER BY wc.weenieClassId;

