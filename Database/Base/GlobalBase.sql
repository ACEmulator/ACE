CREATE TABLE `db_schemas` (
  `id` int(11) NOT NULL AUTO_INCREMENT,
  `node_name` varchar(255) DEFAULT NULL,
  `schema_name` varchar(255) DEFAULT NULL,
  `schema_revision` int(10) unsigned DEFAULT NULL,
  `date_updated` datetime DEFAULT CURRENT_TIMESTAMP ON UPDATE CURRENT_TIMESTAMP,
  PRIMARY KEY (`id`),
  UNIQUE KEY `node_name` (`node_name`)
) ENGINE=InnoDB DEFAULT CHARSET=latin1;
