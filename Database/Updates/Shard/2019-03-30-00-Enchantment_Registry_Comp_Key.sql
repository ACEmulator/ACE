USE `ace_shard`;

ALTER TABLE `biota_properties_enchantment_registry` 
DROP PRIMARY KEY,
ADD PRIMARY KEY (`object_Id`, `spell_Id`, `caster_Object_Id`, `layer_Id`);
;
