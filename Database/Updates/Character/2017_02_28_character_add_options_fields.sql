ALTER TABLE `character`
ADD COLUMN characterOptions1 INT(10) UNSIGNED NOT NULL DEFAULT 0 AFTER totalLogins;

ALTER TABLE `character`
ADD COLUMN characterOptions2 INT(10) UNSIGNED NOT NULL DEFAULT 0 AFTER characterOptions1;
