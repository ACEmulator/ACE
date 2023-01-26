DELETE FROM `quest` WHERE `name` = 'CorruptedEssenceTurnInTimer';

INSERT INTO `quest` (`name`, `min_Delta`, `max_Solves`, `message`, `last_Modified`)
VALUES ('CorruptedEssenceTurnInTimer', 0, -1, 'Corrupted Essence Turn In Timer', '2021-11-01 00:00:00');
