/* Legendary Seed of Mornings - change casting animation */ 
 UPDATE ace_shard.biota_properties_d_i_d
 SET value = 0x400000E1
 WHERE type = 27
 AND object_Id IN
 (
   SELECT id
   FROM ace_shard.biota 
   WHERE weenie_class_id = 48938 
 );