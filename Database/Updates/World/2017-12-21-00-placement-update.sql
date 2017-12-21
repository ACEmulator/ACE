USE `ace_world`;

SET SQL_SAFE_UPDATES = 0;
CREATE TEMPORARY TABLE tmp53 SELECT * from ace_object_properties_int WHERE intPropertyId = 53;
CREATE TEMPORARY TABLE tmp65 SELECT * from ace_object_properties_int WHERE intPropertyId = 65;
UPDATE tmp53 SET intPropertyId = 65;
UPDATE tmp65 SET intPropertyId = 53;
DELETE FROM ace_object_properties_int WHERE intPropertyId = 53 OR intPropertyId = 65;
INSERT INTO ace_object_properties_int SELECT tmp53.* FROM tmp53;
INSERT INTO ace_object_properties_int SELECT tmp65.* FROM tmp65;
DROP TEMPORARY TABLE tmp53;
DROP TEMPORARY TABLE tmp65;
SET SQL_SAFE_UPDATES = 1;
