delete biota from biota
inner join biota_properties_int bint on bint.object_Id=biota.id and bint.`type`=171 /* NumTimesTinkered */
inner join biota_properties_spell_book sb on sb.object_Id=biota.id
where biota.weenie_Class_Id >= 41483 and biota.weenie_Class_Id <= 41488 /* Trinket */
and (sb.spell=2579 /* CantripCoordination */ or sb.spell=2582 /* CantripQuickness */ or sb.spell=2626 /* CantripHealthGain */
or sb.spell=2008 /* WarriorsVigor */ or sb.spell=2627 /* CantripManaGain */ or sb.spell=2584 /* CantripWillpower */
or sb.spell=2004 /* WarriorsVitality */ or sb.spell=2628 /* CantripStaminaGain */ or sb.spell=2583 /* CantripStrength */
or sb.spell=2580 /* CantripEndurance */ or sb.spell=2012 /* WizardsIntellect */ or sb.spell=2581 /* CantripFocus */);
