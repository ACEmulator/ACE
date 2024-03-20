/* Seasoned Explorer Arbalest */
UPDATE `biota` SET `weenie_Class_Id` = '45908' WHERE `biota`.`weenie_Class_Id` = '70205';

/* Seasoned Explorer Knife */
UPDATE `biota` SET `weenie_Class_Id` = '45912' WHERE `biota`.`weenie_Class_Id` = '70211';

/* Seasoned Explorer Budiaq */
UPDATE `biota` SET `weenie_Class_Id` = '45916' WHERE `biota`.`weenie_Class_Id` = '70215';

/* Amateur Explorer Bastone */
UPDATE `biota` SET `weenie_Class_Id` = '45918' WHERE `biota`.`weenie_Class_Id` = '70218';

/* Amateur Explorer Nekode */
UPDATE `biota` SET `weenie_Class_Id` = '45922' WHERE `biota`.`weenie_Class_Id` = '70222';

/* Seasoned Explorer Dirk */
UPDATE `biota` SET `weenie_Class_Id` = '45926' WHERE `biota`.`weenie_Class_Id` = '70227';

/* Seasoned Explorer Morning Star */
UPDATE `biota` SET `weenie_Class_Id` = '45928' WHERE `biota`.`weenie_Class_Id` = '70251';

/* Seasoned Explorer Trident */
UPDATE `biota` SET `weenie_Class_Id` = '45930' WHERE `biota`.`weenie_Class_Id` = '70231';

/* Seasoned Explorer Stick */
UPDATE `biota` SET `weenie_Class_Id` = '45932' WHERE `biota`.`weenie_Class_Id` = '70233';

/* Seasoned Explorer Ken */
UPDATE `biota` SET `weenie_Class_Id` = '45934' WHERE `biota`.`weenie_Class_Id` = '70235';

/* Seasoned Explorer Nekode */
UPDATE `biota` SET `weenie_Class_Id` = '45936' WHERE `biota`.`weenie_Class_Id` = '70221';

/* Amateur Explorer Yari */
UPDATE `biota` SET `weenie_Class_Id` = '45945' WHERE `biota`.`weenie_Class_Id` = '70244';

/* Seasoned Explorer Shamshir */
UPDATE `biota` SET `weenie_Class_Id` = '45948' WHERE `biota`.`weenie_Class_Id` = '70247';

/* Seasoned Explorer Katar */
UPDATE `biota` SET `weenie_Class_Id` = '45950' WHERE `biota`.`weenie_Class_Id` = '70249';

/* Seasoned Explorer Greataxe */
UPDATE `biota` SET `weenie_Class_Id` = '45954' WHERE `biota`.`weenie_Class_Id` = '70253';

/* Seasoned Explorer Shield */
UPDATE `biota` SET `weenie_Class_Id` = '45973' WHERE `biota`.`weenie_Class_Id` = '70209';

/* Seasoned Explorer Axe Cast */
UPDATE `biota` SET `weenie_Class_Id` = '45982' WHERE `biota`.`weenie_Class_Id` = '70192';

/* Stamped Al Arqas Scarlet Red Letter */
UPDATE `biota` SET `weenie_Class_Id` = '45881' WHERE `biota`.`weenie_Class_Id` = '70302';

/* Stamped Nanto Scarlet Red Letter */
UPDATE `biota` SET `weenie_Class_Id` = '45885' WHERE `biota`.`weenie_Class_Id` = '70306';

/* Stamped Rithwic Lucky Gold Letter */
UPDATE `biota` SET `weenie_Class_Id` = '45886' WHERE `biota`.`weenie_Class_Id` = '70307';

/* Inactive Blank Golem - Update WeenieType */
UPDATE `biota` SET `weenie_Type` = '70' WHERE `biota`.`weenie_Class_Id` = '34916';

/* Inactive Blank Golem - Update Int.ItemUseable - Contained */
REPLACE INTO biota_properties_int
SELECT biota.id, 16, 8 FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`= 16 
WHERE biota.weenie_Class_Id = 34916 AND bint.value = 1;

/* Inactive Blank Golem - Add Int.PetClass */
INSERT INTO biota_properties_int
SELECT biota.id, 266, 34898 FROM biota
LEFT JOIN biota_properties_int bint ON bint.object_Id=biota.id AND bint.`type`= 266 
WHERE biota.weenie_Class_Id = 34916 AND bint.value IS NULL;
