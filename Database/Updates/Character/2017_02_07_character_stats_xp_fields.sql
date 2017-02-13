ALTER TABLE character_stats
ADD COLUMN strengthXpSpent INTEGER UNSIGNED NOT NULL DEFAULT 0 AFTER strength;

ALTER TABLE character_stats
ADD COLUMN enduranceXpSpent INTEGER UNSIGNED NOT NULL DEFAULT 0 AFTER endurance;

ALTER TABLE character_stats
ADD COLUMN coordinationXpSpent INTEGER NOT NULL DEFAULT 0 AFTER coordination;

ALTER TABLE character_stats
ADD COLUMN quicknessXpSspent INTEGER UNSIGNED NOT NULL DEFAULT 0 AFTER quickness;

ALTER TABLE character_stats
ADD COLUMN focusXpSpent INTEGER UNSIGNED NOT NULL DEFAULT 0 AFTER focus;

ALTER TABLE character_stats
ADD COLUMN selfXpSpent INTEGER UNSIGNED NOT NULL DEFAULT 0 AFTER self;

ALTER TABLE character_stats
ADD COLUMN healthXpSpent INTEGER UNSIGNED NOT NULL DEFAULT 0 AFTER selfXpSpent;

ALTER TABLE character_stats
ADD COLUMN staminaXpSpent INTEGER UNSIGNED NOT NULL DEFAULT 0 AFTER healthXpSpent;

ALTER TABLE character_stats
ADD COLUMN manaXpSpent INTEGER UNSIGNED NOT NULL DEFAULT 0 AFTER staminaXpSpent;

ALTER TABLE `character`
DROP COLUMN isAdmin;

ALTER TABLE `character`
DROP COLUMN isEnvoy;

ALTER TABLE `character_appearance`
DROP COLUMN race;

ALTER TABLE `character_appearance`
DROP COLUMN gender;
