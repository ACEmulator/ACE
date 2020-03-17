/* warning: only run this before installing the world database that adds support for armatures
if you run it afterwards, you might risk deleting trinkets that were imbued legitimately! */

delete biota from biota
inner join biota_properties_int bint on bint.object_id=biota.id and bint.`type`=171 /* NumTimesTinkered */
where biota.weenie_Class_Id >= 41483 and biota.weenie_Class_Id <= 41488; /* Trinket */
