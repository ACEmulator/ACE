ALTER TABLE character_stats
ADD COLUMN strength_ranks TINYINT(2) UNSIGNED NOT NULL DEFAULT 0 AFTER strength;

ALTER TABLE character_stats
ADD COLUMN endurance_ranks TINYINT(2) UNSIGNED NOT NULL DEFAULT 0 AFTER endurance;

ALTER TABLE character_stats
ADD COLUMN coordination_ranks TINYINT(2) UNSIGNED NOT NULL DEFAULT 0 AFTER coordination;

ALTER TABLE character_stats
ADD COLUMN quickness_ranks TINYINT(2) UNSIGNED NOT NULL DEFAULT 0 AFTER quickness;

ALTER TABLE character_stats
ADD COLUMN focus_ranks TINYINT(2) UNSIGNED NOT NULL DEFAULT 0 AFTER focus;

ALTER TABLE character_stats
ADD COLUMN self_ranks TINYINT(2) UNSIGNED NOT NULL DEFAULT 0 AFTER self;

ALTER TABLE character_stats
ADD COLUMN health_ranks TINYINT(2) UNSIGNED NOT NULL DEFAULT 0 AFTER self_ranks;

ALTER TABLE character_stats
ADD COLUMN stamina_ranks TINYINT(2) UNSIGNED NOT NULL DEFAULT 0 AFTER health_ranks;

ALTER TABLE character_stats
ADD COLUMN mana_ranks TINYINT(2) UNSIGNED NOT NULL DEFAULT 0 AFTER stamina_ranks;


ALTER TABLE character_stats
ADD COLUMN health_current TINYINT(2) UNSIGNED NOT NULL DEFAULT 0 AFTER health_ranks;

ALTER TABLE character_stats
ADD COLUMN stamina_current TINYINT(2) UNSIGNED NOT NULL DEFAULT 0 AFTER stamina_ranks;

ALTER TABLE character_stats
ADD COLUMN mana_current TINYINT(2) UNSIGNED NOT NULL DEFAULT 0 AFTER mana_ranks;
