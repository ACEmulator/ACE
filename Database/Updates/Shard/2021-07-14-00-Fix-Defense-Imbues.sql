update biota_properties_int set value=0x400 /* ImbuedEffectType.MeleeDefense */ where `type`=179 /* PropertyInt.ImbuedEffect */ and value=6; /* Skill.MeleeDefense / ImbuedEffectType.CripplingBlow | ImbuedEffectType.ArmorRending */

update biota_properties_int set value=0x800 /* ImbuedEffectType.MissileDefense */ where `type`=179 /* PropertyInt.ImbuedEffect */ and value=7; /* Skill.MissileDefense / ImbuedEffectType.CriticalStrike | ImbuedEffectType.CripplingBlow | ImbuedEffectType.ArmorRending */

update biota_properties_int set value=0x1000 /* ImbuedEffectType.MagicDefense */ where `type`=179 /* PropertyInt.ImbuedEffect */ and value=15; /* Skill.MagicDefense / ImbuedEffectType.CriticalStrike | ImbuedEffectType.CripplingBlow | ImbuedEffectType.ArmorRending | ImbuedEffectType.SlashRending */
