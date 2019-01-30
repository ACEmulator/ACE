CREATE TABLE `character_transfer` (
  `id` int(10) unsigned NOT NULL AUTO_INCREMENT,
  `transfer_Type` int(11) NOT NULL,
  `account_Id` int(10) NOT NULL,
  `package_source_Id` int(10) DEFAULT NULL,
  `source_Id` int(10) DEFAULT NULL,
  `target_Id` int(10) DEFAULT NULL,
  `transfer_Time` bigint(10) unsigned NOT NULL,
  `cancel_Time` bigint(10) unsigned DEFAULT NULL,
  `download_Time` bigint(10) unsigned DEFAULT NULL,
  `cookie` varchar(15) DEFAULT NULL,
  `source_Thumbprint` varchar(45) DEFAULT NULL,
  `source_Base_Url` varchar(80) DEFAULT NULL,
  PRIMARY KEY (`id`)
) ENGINE=InnoDB AUTO_INCREMENT=4 DEFAULT CHARSET=utf8;
