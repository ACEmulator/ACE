SET @OLD_SQL_SAFE_UPDATES=@@SQL_SAFE_UPDATES, SQL_SAFE_UPDATES=0;
UPDATE ace_shard.biota AS biota
INNER JOIN ace_world.weenie AS weenie ON biota.weenie_Class_Id=weenie.class_Id
SET biota.weenie_Type=weenie.`type`
WHERE biota.id > 0 AND biota.weenie_Class_Id <> 1 AND biota.weenie_Class_Id <> 4 AND biota.weenie_Class_Id <> 41 AND weenie.`type` <> biota.weenie_Type;
SET SQL_SAFE_UPDATES=@OLD_SQL_SAFE_UPDATES;
