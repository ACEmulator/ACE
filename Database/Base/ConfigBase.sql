DROP DATABASE IF EXISTS ace_config;

CREATE DATABASE ace_config DEFAULT CHARACTER SET utf8;

USE ace_config;

CREATE TABLE `properties_string`(
	`key`	varchar(45) PRIMARY KEY,
    `value` varchar(45)
);

CREATE TABLE `properties_long`(
	`key`	varchar(45) PRIMARY KEY,
    `value` bigint
);

CREATE TABLE `properties_double`(
	`key`	varchar(45) PRIMARY KEY,
    `value`	real
);

CREATE TABLE `properties_boolean`(
	`key`	varchar(45) PRIMARY KEY,
    `value` bit(1)
);