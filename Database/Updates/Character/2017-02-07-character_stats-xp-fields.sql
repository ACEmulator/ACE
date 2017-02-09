
ALTER TABLE character_stats
ADD COLUMN strength_xp_spent INTEGER UNSIGNED NOT NULL DEFAULT 0 AFTER strength;

ALTER TABLE character_stats
ADD COLUMN endurance_xp_spent INTEGER UNSIGNED NOT NULL DEFAULT 0 AFTER endurance;

ALTER TABLE character_stats
ADD COLUMN coordination_xp_spent INTEGER NOT NULL DEFAULT 0 AFTER coordination;

ALTER TABLE character_stats
ADD COLUMN quickness_xp_spent INTEGER UNSIGNED NOT NULL DEFAULT 0 AFTER quickness;

ALTER TABLE character_stats
ADD COLUMN focus_xp_spent INTEGER UNSIGNED NOT NULL DEFAULT 0 AFTER focus;

ALTER TABLE character_stats
ADD COLUMN self_xp_spent INTEGER UNSIGNED NOT NULL DEFAULT 0 AFTER self;

ALTER TABLE character_stats
ADD COLUMN health_xp_spent INTEGER UNSIGNED NOT NULL DEFAULT 0 AFTER self_xp_spent;

ALTER TABLE character_stats
ADD COLUMN stamina_xp_spent INTEGER UNSIGNED NOT NULL DEFAULT 0 AFTER health_xp_spent;

ALTER TABLE character_stats
ADD COLUMN mana_xp_spent INTEGER UNSIGNED NOT NULL DEFAULT 0 AFTER stamina_xp_spent;

ALTER TABLE `character`
DROP COLUMN isAdmin;

ALTER TABLE `character`
DROP COLUMN isEnvoy;

ALTER TABLE `character_appearance`
DROP COLUMN race;

ALTER TABLE `character_appearance`
DROP COLUMN gender;