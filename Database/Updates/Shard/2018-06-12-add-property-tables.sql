USE ace_shard;

CREATE TABLE IF NOT EXISTS `config_properties_string`(
	`key`	        varchar(45) PRIMARY KEY,
    `value`         varchar(45) NOT NULL,
    `description`   varchar(45)
);

CREATE TABLE IF NOT EXISTS `config_properties_long`(
	`key`	        varchar(45) PRIMARY KEY,
    `value`         bigint NOT NULL,
    `description`   varchar(45)
);

CREATE TABLE IF NOT EXISTS `config_properties_double`(
	`key`	        varchar(45) PRIMARY KEY,
    `value`	        real NOT NULL,
    `description`   varchar(45)
);

CREATE TABLE IF NOT EXISTS `config_properties_boolean`(
	`key`	        varchar(45) PRIMARY KEY,
    `value`         bit(1) NOT NULL,
    `description`   varchar(45)
);
