/* Changes the completion quest stamp for "Chasing Oswald" to match the Contract stamp */
UPDATE
    `character_properties_quest_registry`
SET
    `quest_Name` = 'OswaldManualCompleted'
WHERE
    `character_properties_quest_registry`.`quest_Name` = 'ChasingOswald';
	