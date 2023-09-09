DELETE FROM ace_world.weenie_properties_create_list WHERE destination_type IN (16,32);

/* Buy item list for apartments */
INSERT INTO ace_world.weenie_properties_create_list
(object_Id, destination_type, weenie_class_Id, stack_Size, palette, shade, try_to_Bond)
VALUES
(9621, 16, 1000002, 25, 0, 0, 0);

/* Rent item list for apartments */
INSERT INTO ace_world.weenie_properties_create_list
(object_Id, destination_type, weenie_class_Id, stack_Size, palette, shade, try_to_Bond)
VALUES
(9621, 32, 1000002, 25, 0, 0, 0);

/* Buy item list for cottages */
INSERT INTO ace_world.weenie_properties_create_list
(object_Id, destination_type, weenie_class_Id, stack_Size, palette, shade, try_to_Bond)
SELECT class_id, 16, 1000002, 50, 0, 0, 0
FROM ace_world.weenie
WHERE `type` = 55
AND class_Name LIKE '%cottage%';

/* Rent item list for cottages */
INSERT INTO ace_world.weenie_properties_create_list
(object_Id, destination_type, weenie_class_Id, stack_Size, palette, shade, try_to_Bond)
SELECT class_id, 32, 1000002, 25, 0, 0, 0
FROM ace_world.weenie
WHERE `type` = 55
AND class_Name LIKE '%cottage%';

/* Buy item list for villas */
INSERT INTO ace_world.weenie_properties_create_list
(object_Id, destination_type, weenie_class_Id, stack_Size, palette, shade, try_to_Bond)
SELECT class_id, 16, 1000002, 100, 0, 0, 0
FROM ace_world.weenie
WHERE `type` = 55
AND class_Name LIKE '%villa%';

/* Rent item list for villas */
INSERT INTO ace_world.weenie_properties_create_list
(object_Id, destination_type, weenie_class_Id, stack_Size, palette, shade, try_to_Bond)
SELECT class_id, 32, 1000002, 50, 0, 0, 0
FROM ace_world.weenie
WHERE `type` = 55
AND class_Name LIKE '%villa%';

/* Buy item list for mansions */
INSERT INTO ace_world.weenie_properties_create_list
(object_Id, destination_type, weenie_class_Id, stack_Size, palette, shade, try_to_Bond)
SELECT class_id, 16, 1000002, 500, 0, 0, 0
FROM ace_world.weenie
WHERE `type` = 55
AND class_Name LIKE '%mansion%';

/* Rent item list for mansions */
INSERT INTO ace_world.weenie_properties_create_list
(object_Id, destination_type, weenie_class_Id, stack_Size, palette, shade, try_to_Bond)
SELECT class_id, 32, 1000002, 150, 0, 0, 0
FROM ace_world.weenie
WHERE `type` = 55
AND class_Name LIKE '%mansion%';