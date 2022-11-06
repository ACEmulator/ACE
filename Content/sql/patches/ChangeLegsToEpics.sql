UPDATE doctide_world.spell
SET stat_Mod_Val = 25
WHERE NAME LIKE '%Legendary%'
AND stat_Mod_Val = 35;

UPDATE doctide_world.spell
SET stat_Mod_Val = 60
WHERE NAME LIKE '%Legendary%'
AND stat_Mod_Val = 80;

UPDATE doctide_world.spell
SET stat_Mod_Val = -30
WHERE NAME LIKE '%Legendary Swift%'
AND stat_Mod_Val = -40;

UPDATE doctide_world.spell
SET stat_Mod_Val = 0.8
WHERE NAME LIKE '%Legendary%Ward%'
AND stat_Mod_Val = 0.75;

UPDATE doctide_world.spell
SET stat_Mod_Val = 1.25
WHERE NAME LIKE '%Legendary Hermetic%'
AND stat_Mod_Val = 1.3;

UPDATE doctide_world.spell
SET stat_Mod_Val = 7
WHERE NAME LIKE '%Legendary Blood%'
AND stat_Mod_Val = 10;

UPDATE doctide_world.spell
SET stat_Mod_Val = 0.07
WHERE NAME LIKE '%Legendary%'
AND stat_Mod_Val = 0.09;

UPDATE doctide_world.spell
SET stat_Mod_Val = 1.45
WHERE NAME LIKE '%Legendary%Gain%'
AND stat_Mod_Val = 1.6;

UPDATE doctide_world.spell
SET stat_Mod_Val = 0.2
WHERE NAME LIKE '%Legendary%Bane%'
AND stat_Mod_Val = 0.25;

UPDATE doctide_world.spell
SET stat_Mod_Val = 0.2
WHERE NAME LIKE '%Epic%Bane%'
AND stat_Mod_Val = 0.8;