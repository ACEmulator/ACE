UPDATE ace_shard.biota_properties_enchantment_registry enchantment
INNER JOIN ace_world.spell ON spell.id=enchantment.spell_Id
AND spell.stat_Mod_Type IS NOT NULL AND spell.stat_Mod_Key IS NOT NULL
SET enchantment.stat_Mod_Type=spell.stat_Mod_Type, enchantment.stat_Mod_Key=spell.stat_Mod_Key
WHERE enchantment.object_Id > 0;
