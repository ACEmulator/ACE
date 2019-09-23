USE `ace_shard`;

DELETE spells FROM biota
INNER JOIN biota_properties_spell_book spells ON biota.id=spells.object_Id
WHERE biota.weenie_Class_Id >= 42635 AND biota.weenie_Class_Id <= 42637;

UPDATE biota SET populated_Collection_Flags = populated_Collection_Flags & ~0x100000
WHERE biota.weenie_Class_Id >= 42635 AND biota.weenie_Class_Id <= 42637;
