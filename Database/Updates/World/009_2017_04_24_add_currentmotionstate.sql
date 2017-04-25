ALTER TABLE `base_ace_object` 
ADD COLUMN `currentMotionState` TEXT NOT NULL AFTER `translucency`;

ALTER TABLE `base_ace_object` 
CHANGE COLUMN `location` `location` INT(10) UNSIGNED NOT NULL DEFAULT '0' ;


USE `ace_world`;
CREATE 
     OR REPLACE ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `ace_world`.`vw_ace_creature_object` AS
    (SELECT 
        `bao`.`baseAceObjectId` AS `baseAceObjectId`,
        `bao`.`name` AS `name`,
        `bao`.`typeId` AS `typeId`,
        `bao`.`paletteId` AS `paletteId`,
        `bao`.`ammoType` AS `ammoType`,
        `bao`.`blipColor` AS `blipColor`,
        `bao`.`bitField` AS `bitField`,
        `bao`.`burden` AS `burden`,
        `bao`.`combatUse` AS `combatUse`,
        `bao`.`cooldownDuration` AS `cooldownDuration`,
        `bao`.`cooldownId` AS `cooldownId`,
        `bao`.`effects` AS `effects`,
        `bao`.`containersCapacity` AS `containersCapacity`,
        `bao`.`header` AS `header`,
        `bao`.`hookTypeId` AS `hookTypeId`,
        `bao`.`iconId` AS `iconId`,
        `bao`.`iconOverlayId` AS `iconOverlayId`,
        `bao`.`iconUnderlayId` AS `iconUnderlayId`,
        `bao`.`hookItemTypes` AS `hookItemTypes`,
        `bao`.`itemsCapacity` AS `itemsCapacity`,
        `bao`.`location` AS `location`,
        `bao`.`materialType` AS `materialType`,
        `bao`.`maxStackSize` AS `maxStackSize`,
        `bao`.`maxStructure` AS `maxStructure`,
        `bao`.`radar` AS `radar`,
        `bao`.`pscript` AS `pscript`,
        `bao`.`spellId` AS `spellId`,
        `bao`.`stackSize` AS `stackSize`,
        `bao`.`structure` AS `structure`,
        `bao`.`targetTypeId` AS `targetTypeId`,
        `bao`.`usability` AS `usability`,
        `bao`.`useRadius` AS `useRadius`,
        `bao`.`validLocations` AS `validLocations`,
        `bao`.`value` AS `value`,
        `bao`.`workmanship` AS `workmanship`,
        `bao`.`animationFrameId` AS `animationFrameId`,
        `bao`.`defaultScript` AS `defaultScript`,
        `bao`.`defaultScriptIntensity` AS `defaultScriptIntensity`,
        `bao`.`elasticity` AS `elasticity`,
        `bao`.`friction` AS `friction`,
        `bao`.`locationId` AS `locationId`,
        `bao`.`modelTableId` AS `modelTableId`,
        `bao`.`motionTableId` AS `motionTableId`,
        `bao`.`objectScale` AS `objectScale`,
        `bao`.`physicsBitField` AS `physicsBitField`,
        `bao`.`physicsState` AS `physicsState`,
        `bao`.`physicsTableId` AS `physicsTableId`,
        `bao`.`soundTableId` AS `soundTableId`,
        `bao`.`translucency` AS `translucency`,
        `bao`.`currentMotionState` AS `currentMotionState`,
        `wcd`.`weenieClassId` AS `weenieClassId`,
        `wcd`.`level` AS `level`,
        `wcd`.`strength` AS `strength`,
        `wcd`.`endurance` AS `endurance`,
        `wcd`.`coordination` AS `coordination`,
        `wcd`.`quickness` AS `quickness`,
        `wcd`.`focus` AS `focus`,
        `wcd`.`self` AS `self`,
        `wcd`.`health` AS `health`,
        `wcd`.`stamina` AS `stamina`,
        `wcd`.`mana` AS `mana`,
        `wcd`.`baseExperience` AS `baseExperience`,
        `wcd`.`luminance` AS `luminance`,
        `wcd`.`lootTier` AS `lootTier`
    FROM
        ((`ace_world`.`weenie_creature_data` `wcd`
        JOIN `ace_world`.`weenie_class` `wc` ON ((`wcd`.`weenieClassId` = `wc`.`weenieClassId`)))
        JOIN `ace_world`.`base_ace_object` `bao` ON ((`wc`.`baseAceObjectId` = `bao`.`baseAceObjectId`))));
        
USE `ace_world`;
CREATE 
     OR REPLACE ALGORITHM = UNDEFINED 
    DEFINER = `root`@`localhost` 
    SQL SECURITY DEFINER
VIEW `ace_world`.`vw_ace_object` AS
    (SELECT 
        `bao`.`baseAceObjectId` AS `baseAceObjectId`,
        `bao`.`name` AS `name`,
        `bao`.`typeId` AS `typeId`,
        `bao`.`paletteId` AS `paletteId`,
        `bao`.`ammoType` AS `ammoType`,
        `bao`.`blipColor` AS `blipColor`,
        `bao`.`bitField` AS `bitField`,
        `bao`.`burden` AS `burden`,
        `bao`.`combatUse` AS `combatUse`,
        `bao`.`cooldownDuration` AS `cooldownDuration`,
        `bao`.`cooldownId` AS `cooldownId`,
        `bao`.`effects` AS `effects`,
        `bao`.`containersCapacity` AS `containersCapacity`,
        `bao`.`header` AS `header`,
        `bao`.`hookTypeId` AS `hookTypeId`,
        `bao`.`iconId` AS `iconId`,
        `bao`.`iconOverlayId` AS `iconOverlayId`,
        `bao`.`iconUnderlayId` AS `iconUnderlayId`,
        `bao`.`hookItemTypes` AS `hookItemTypes`,
        `bao`.`itemsCapacity` AS `itemsCapacity`,
        `bao`.`location` AS `location`,
        `bao`.`materialType` AS `materialType`,
        `bao`.`maxStackSize` AS `maxStackSize`,
        `bao`.`maxStructure` AS `maxStructure`,
        `bao`.`radar` AS `radar`,
        `bao`.`pscript` AS `pscript`,
        `bao`.`spellId` AS `spellId`,
        `bao`.`stackSize` AS `stackSize`,
        `bao`.`structure` AS `structure`,
        `bao`.`targetTypeId` AS `targetTypeId`,
        `bao`.`usability` AS `usability`,
        `bao`.`useRadius` AS `useRadius`,
        `bao`.`validLocations` AS `validLocations`,
        `bao`.`value` AS `value`,
        `bao`.`workmanship` AS `workmanship`,
        `bao`.`animationFrameId` AS `animationFrameId`,
        `bao`.`defaultScript` AS `defaultScript`,
        `bao`.`defaultScriptIntensity` AS `defaultScriptIntensity`,
        `bao`.`elasticity` AS `elasticity`,
        `bao`.`friction` AS `friction`,
        `bao`.`locationId` AS `locationId`,
        `bao`.`modelTableId` AS `modelTableId`,
        `bao`.`motionTableId` AS `motionTableId`,
        `bao`.`objectScale` AS `objectScale`,
        `bao`.`physicsBitField` AS `physicsBitField`,
        `bao`.`physicsState` AS `physicsState`,
        `bao`.`physicsTableId` AS `physicsTableId`,
        `bao`.`soundTableId` AS `soundTableId`,
        `bao`.`translucency` AS `translucency`,
        `bao`.`currentMotionState` AS `currentMotionState`,
        `ao`.`weenieClassId` AS `weenieClassId`,
        `ao`.`landblock` AS `landblock`,
        `ao`.`cell` AS `cell`,
        `ao`.`posX` AS `posX`,
        `ao`.`posY` AS `posY`,
        `ao`.`posZ` AS `posZ`,
        `ao`.`qW` AS `qW`,
        `ao`.`qX` AS `qX`,
        `ao`.`qY` AS `qY`,
        `ao`.`qZ` AS `qZ`
    FROM
        (`ace_world`.`ace_object` `ao`
        JOIN `ace_world`.`base_ace_object` `bao` ON ((`ao`.`baseAceObjectId` = `bao`.`baseAceObjectId`))));

