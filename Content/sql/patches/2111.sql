DELETE FROM `treasure_death` WHERE `id` = 2111;

INSERT INTO `treasure_death` (`id`, `treasure_Type`, `tier`, `loot_Quality_Mod`, `unknown_Chances`, `item_Chance`, `item_Min_Amount`, `item_Max_Amount`, `item_Treasure_Type_Selection_Chances`, `magic_Item_Chance`, `magic_Item_Min_Amount`, `magic_Item_Max_Amount`, `magic_Item_Treasure_Type_Selection_Chances`, `mundane_Item_Chance`, `mundane_Item_Min_Amount`, `mundane_Item_Max_Amount`, `mundane_Item_Type_Selection_Chances`, `last_Modified`) 
VALUES (2111 	/* id */
	 , 2111		/* treasure_Type */
	 , 7		/* tier */
	 , 0.5		/* loot_Quality_Mod */
	 , 19		/* unknown_Chances */
	 , 100		/* item_Chance */
	 , 2		/* item_Min_Amount */
	 , 3		/* item_Max_Amount */
	 , 9		/* item_Treasure_Type_Selection_Chances */
	 , 100		/* magic_Item_Chance */
	 , 7		/* magic_Item_Min_Amount */
	 , 10		/* magic_Item_Max_Amount */
	 , 8		/* magic_Item_Treasure_Type_Selection_Chances */
	 , 100		/* mundane_Item_Chance */
	 , 0		/* mundane_Item_Min_Amount */
	 , 1		/* mundane_Item_Max_Amount */
	 , 7		/* mundane_Item_Type_Selection_Chances */
	 , '2020-03-16 12:00:00');		/* last_Modified */