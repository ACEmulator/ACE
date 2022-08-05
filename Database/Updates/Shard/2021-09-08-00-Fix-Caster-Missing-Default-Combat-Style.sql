INSERT INTO biota_properties_int
SELECT biota.id, 46 /* DefaultCombatStyle */, 512 /* Magic */  FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`=46 /* DefaultCombatStyle */
WHERE biota.weenie_Type=35 /* Caster */ AND bint.value IS NULL;
