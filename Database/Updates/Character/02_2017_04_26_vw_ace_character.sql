CREATE OR REPLACE VIEW `ace_character`.`vw_ace_character` AS
    SELECT 
        `ace_character`.`character`.`guid` AS `guid`,
        `ace_character`.`character`.`accountId` AS `accountId`,
        `ace_character`.`character`.`name` AS `name`,
        `ace_character`.`character`.`templateOption` AS `templateOption`,
        `ace_character`.`character`.`startArea` AS `startArea`,
        `ace_character`.`character`.`birth` AS `birth`,
        `ace_character`.`character`.`deleteTime` AS `deleteTime`,
        `ace_character`.`character`.`deleted` AS `deleted`,
        `ace_character`.`character`.`totalLogins` AS `totalLogins`,
        `ace_character`.`character`.`characterOptions1` AS `characterOptions1`,
        `ace_character`.`character`.`characterOptions2` AS `characterOptions2`,
        `ace_character`.`character_appearance`.`eyes` AS `eyes`,
        `ace_character`.`character_appearance`.`nose` AS `nose`,
        `ace_character`.`character_appearance`.`mouth` AS `mouth`,
        `ace_character`.`character_appearance`.`eyeColor` AS `eyeColor`,
        `ace_character`.`character_appearance`.`hairColor` AS `hairColor`,
        `ace_character`.`character_appearance`.`hairStyle` AS `hairStyle`,
        `ace_character`.`character_appearance`.`hairHue` AS `hairHue`,
        `ace_character`.`character_appearance`.`skinHue` AS `skinHue`,
        `ace_character`.`character_stats`.`strength` AS `strength`,
        `ace_character`.`character_stats`.`strengthXpSpent` AS `strengthXpSpent`,
        `ace_character`.`character_stats`.`strengthRanks` AS `strengthRanks`,
        `ace_character`.`character_stats`.`endurance` AS `endurance`,
        `ace_character`.`character_stats`.`enduranceXpSpent` AS `enduranceXpSpent`,
        `ace_character`.`character_stats`.`enduranceRanks` AS `enduranceRanks`,
        `ace_character`.`character_stats`.`coordination` AS `coordination`,
        `ace_character`.`character_stats`.`coordinationXpSpent` AS `coordinationXpSpent`,
        `ace_character`.`character_stats`.`coordinationRanks` AS `coordinationRanks`,
        `ace_character`.`character_stats`.`quickness` AS `quickness`,
        `ace_character`.`character_stats`.`quicknessXpSpent` AS `quicknessXpSpent`,
        `ace_character`.`character_stats`.`quicknessRanks` AS `quicknessRanks`,
        `ace_character`.`character_stats`.`focus` AS `focus`,
        `ace_character`.`character_stats`.`focusXpSpent` AS `focusXpSpent`,
        `ace_character`.`character_stats`.`focusRanks` AS `focusRanks`,
        `ace_character`.`character_stats`.`self` AS `self`,
        `ace_character`.`character_stats`.`selfXpSpent` AS `selfXpSpent`,
        `ace_character`.`character_stats`.`healthXpSpent` AS `healthXpSpent`,
        `ace_character`.`character_stats`.`staminaXpSpent` AS `staminaXpSpent`,
        `ace_character`.`character_stats`.`manaXpSpent` AS `manaXpSpent`,
        `ace_character`.`character_stats`.`selfRanks` AS `selfRanks`,
        `ace_character`.`character_stats`.`healthRanks` AS `healthRanks`,
        `ace_character`.`character_stats`.`healthCurrent` AS `healthCurrent`,
        `ace_character`.`character_stats`.`staminaRanks` AS `staminaRanks`,
        `ace_character`.`character_stats`.`staminaCurrent` AS `staminaCurrent`,
        `ace_character`.`character_stats`.`manaRanks` AS `manaRanks`,
        `ace_character`.`character_stats`.`manaCurrent` AS `manaCurrent`
    FROM
        ((`ace_character`.`character`
        JOIN `ace_character`.`character_appearance` ON ((`ace_character`.`character`.`guid` = `ace_character`.`character_appearance`.`id`)))
        JOIN `ace_character`.`character_stats` ON ((`ace_character`.`character`.`guid` = `ace_character`.`character_stats`.`id`)));
