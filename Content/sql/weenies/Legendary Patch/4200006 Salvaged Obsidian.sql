DELETE FROM `recipe` WHERE `id` = 4200006;

INSERT INTO `recipe` (`id`, `unknown_1`, `skill`, `difficulty`, `salvage_Type`, `success_W_C_I_D`, `success_Amount`, `success_Message`, `fail_W_C_I_D`, `fail_Amount`, `fail_Message`, `success_Destroy_Source_Chance`, `success_Destroy_Source_Amount`, `success_Destroy_Source_Message`, `success_Destroy_Target_Chance`, `success_Destroy_Target_Amount`, `success_Destroy_Target_Message`, `fail_Destroy_Source_Chance`, `fail_Destroy_Source_Amount`, `fail_Destroy_Source_Message`, `fail_Destroy_Target_Chance`, `fail_Destroy_Target_Amount`, `fail_Destroy_Target_Message`, `data_Id`, `last_Modified`)
VALUES (4200006, 0, 30 /* MagicItemTinkering */, 0, 2, 0, 0, 'You apply the Obsidian.', 0, 0, 'You apply the Obsidian, but in the process you destroy the target.', 1, 1, NULL, 0, 0, NULL, 1, 1, NULL, 1, 1, NULL, 0, '2022-01-09 21:50:57');

INSERT INTO `recipe_requirements_int` (`recipe_Id`, `index`, `stat`, `value`, `enum`, `message`)
VALUES (4200006, 0, 105, 1, 2, 'The target item cannot be tinkered!') /* Target.ItemWorkmanship LessThan 1 */
     , (4200006, 0, 171, 10, 3, 'The target item has been tinkered too many times already!') /* Target.NumTimesTinkered GreaterThanEqual 10 */
     , (4200006, 0, 179, 1, 3, 'The target item has been imbued already!') /* Target.ImbuedEffect - CriticalStrike GreaterThanEqual 1 */
     , (4200006, 0, 108, 1, 2, 'The target item must be magical!') /* Target.ItemMaxMana LessThan 1 */
     , (4200006, 1,  92, 100, 2, 'The material is not complete!') /* Source.Structure LessThan 100 */;

INSERT INTO `recipe_mod` (`recipe_Id`, `executes_On_Success`, `health`, `stamina`, `mana`, `unknown_7`, `data_Id`, `unknown_9`, `instance_Id`)
VALUES (4200006, True, 0, 0, 0, False, 0x38000041, 0, 0);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `recipe_mods_int` (`recipe_Mod_Id`, `index`, `stat`, `value`, `enum`, `source`)
VALUES (@parent_id, 0, 109, 25, 2, 0) /* On Player.SuccessTarget Add ItemDifficulty 25 to Target */
     , (@parent_id, 0, 106, 25, 2, 0) /* On Player.SuccessTarget Add ItemSpellcraft 25 to Target */
     , (@parent_id, 0,  19, 15000, 2, 0) /* On Player.SuccessTarget Add Value 15000 to Target */
     , (@parent_id, 0, 3204, -1, 7, 0) /* On Player.SuccessTarget AddSpell GolemHunterHealthHigh to Target */;

INSERT INTO `recipe_mod` (`recipe_Id`, `executes_On_Success`, `health`, `stamina`, `mana`, `unknown_7`, `data_Id`, `unknown_9`, `instance_Id`)
VALUES (4200006, True, 0, 0, 0, False, 0, 0, 0);

SET @parent_id = LAST_INSERT_ID();

INSERT INTO `recipe_mods_string` (`recipe_Mod_Id`, `index`, `stat`, `value`, `enum`, `source`)
VALUES (@parent_id, 3,  40, NULL, 3, 0) /* On Player.SuccessResult CopyFromSourceToTarget ImbuerName to Result */;

DELETE FROM `cook_book` WHERE `recipe_Id` = 4200006;

INSERT INTO `cook_book` (`recipe_Id`, `source_W_C_I_D`, `target_W_C_I_D`, `last_Modified`)
VALUES (4200006, 21063 /* Salvaged Obsidian */,   294 /* Amulet */, '2022-01-09 21:50:57')
     , (4200006, 21063 /* Salvaged Obsidian */,   295 /* Bracelet */, '2022-01-09 21:50:57')
     , (4200006, 21063 /* Salvaged Obsidian */,   297 /* Ring */, '2022-01-09 21:50:57')
     , (4200006, 21063 /* Salvaged Obsidian */,   621 /* Heavy Bracelet */, '2022-01-09 21:50:57')
     , (4200006, 21063 /* Salvaged Obsidian */,   622 /* Necklace */, '2022-01-09 21:50:57')
     , (4200006, 21063 /* Salvaged Obsidian */,   623 /* Heavy Necklace */, '2022-01-09 21:50:57')
     , (4200006, 21063 /* Salvaged Obsidian */,   624 /* Ring */, '2022-01-09 21:50:57')
     , (4200006, 21063 /* Salvaged Obsidian */,  1443 /* Ring */, '2022-01-09 21:50:57')
     , (4200006, 21063 /* Salvaged Obsidian */,  2367 /* Gorget */, '2022-01-09 21:50:57');
