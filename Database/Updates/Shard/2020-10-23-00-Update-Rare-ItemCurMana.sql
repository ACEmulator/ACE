INSERT INTO biota_properties_int
SELECT biota.id, 107 /* ItemCurMana */, weenieMana.value FROM biota                                                                            
INNER JOIN biota_properties_int rareId ON rareId.object_Id=biota.id AND rareId.`type`=17 /* RareId */
INNER JOIN ace_world.weenie_properties_int weenieMana ON weenieMana.object_Id=biota.weenie_Class_Id AND weenieMana.`type`=107 /* ItemCurMana */
LEFT JOIN  biota_properties_int biotaMana ON biotaMana.object_Id=biota.id AND biotaMana.`type`=107 /* ItemCurMana */
WHERE biotaMana.value IS NULL;
