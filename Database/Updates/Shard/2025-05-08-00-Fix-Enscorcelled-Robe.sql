/* Add spell 5437 (Corruptor's Boon) to WCIDs 29542 (Enscorcelled Robe) that don't already have that spell */

INSERT INTO biota_properties_spell_book
(SELECT DISTINCT b1.id, 5437, 2 FROM biota b1
WHERE b1.weenie_Class_Id = 29542 
AND b1.id NOT IN (SELECT DISTINCT b2.id FROM biota b2
INNER JOIN biota_properties_spell_book bpsb on bpsb.object_Id = b2.ID
WHERE b2.weenie_Class_Id = 29542 AND bpsb.spell = 5437)
);
