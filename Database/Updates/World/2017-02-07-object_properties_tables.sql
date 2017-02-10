
 CREATE TABLE IF NOT EXISTS `object_properties_bool` (
  object_guid INT UNSIGNED NOT NULL,
  property_id SMALLINT UNSIGNED NOT NULL,
  property_value BOOL NOT NULL,
  INDEX (object_guid),
  UNIQUE `object_guid__property_id` (object_guid, property_id)
) ENGINE = InnoDB;

 CREATE TABLE IF NOT EXISTS `object_properties_int` (
  object_guid INT UNSIGNED NOT NULL,
  property_id SMALLINT UNSIGNED NOT NULL,
  property_value INT UNSIGNED NOT NULL,
  INDEX (object_guid),
  UNIQUE `object_guid__property_id` (object_guid, property_id)
) ENGINE = InnoDB;

 CREATE TABLE IF NOT EXISTS `object_properties_bigint` (
  object_guid INT UNSIGNED NOT NULL,
  property_id SMALLINT UNSIGNED NOT NULL,
  property_value BIGINT UNSIGNED NOT NULL,
  INDEX (object_guid),
  UNIQUE `object_guid__property_id` (object_guid, property_id)
) ENGINE = InnoDB;

 CREATE TABLE IF NOT EXISTS `object_properties_double` (
  object_guid INT UNSIGNED NOT NULL,
  property_id SMALLINT UNSIGNED NOT NULL,
  property_value DOUBLE NOT NULL,
  INDEX (object_guid),
  UNIQUE `object_guid__property_id` (object_guid, property_id)
) ENGINE = InnoDB;

 CREATE TABLE IF NOT EXISTS `object_properties_string` (
  object_guid INT UNSIGNED NOT NULL,
  property_id SMALLINT UNSIGNED NOT NULL,
  property_value TEXT NOT NULL,
  INDEX (object_guid),
  UNIQUE `object_guid__property_id` (object_guid, property_id)
) ENGINE = InnoDB;


