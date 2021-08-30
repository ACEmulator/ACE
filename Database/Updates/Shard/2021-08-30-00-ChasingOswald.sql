/* Changes the completion quest stamp for "Chasing Oswald" to match the Contract stamp */
UPDATE
IGNORE
    `character_properties_quest_registry`
SET
    `quest_Name` = 'OswaldManualCompleted'
WHERE
    `quest_Name` = 'ChasingOswaldDone';
