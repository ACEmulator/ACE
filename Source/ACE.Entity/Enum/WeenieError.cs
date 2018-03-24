namespace ACE.Entity.Enum
{
    // Corresponds to giant string switch in ClientCommunicationSystem::HandleFailureEvent
    public enum WERROR : uint
    {
        WERROR_NONE,
        WERROR_NOMEM,
        WERROR_BAD_PARAM,
        WERROR_DIV_ZERO,
        WERROR_SEGV,
        WERROR_UNIMPLEMENTED,
        WERROR_UNKNOWN_MESSAGE_TYPE,
        WERROR_NO_ANIMATION_TABLE,
        WERROR_NO_PHYSICS_OBJECT,
        WERROR_NO_BOOKIE_OBJECT,
        WERROR_NO_WSL_OBJECT,
        WERROR_NO_MOTION_INTERPRETER,
        WERROR_UNHANDLED_SWITCH,
        WERROR_DEFAULT_CONSTRUCTOR_CALLED,
        WERROR_INVALID_COMBAT_MANEUVER,
        WERROR_BAD_CAST,
        WERROR_MISSING_QUALITY,
        UNKNOWN__GUESSEDNAME17, // NOTE: Missing 1
        WERROR_MISSING_DATABASE_OBJECT,
        WERROR_NO_CALLBACK_SET,
        WERROR_CORRUPT_QUALITY,
        WERROR_BAD_CONTEXT,
        WERROR_NO_EPHSEQ_MANAGER,
        WERROR_BAD_MOVEMENT_EVENT,
        WERROR_CANNOT_CREATE_NEW_OBJECT,
        WERROR_NO_CONTROLLER_OBJECT,
        WERROR_CANNOT_SEND_EVENT,
        WERROR_PHYSICS_CANT_TRANSITION,
        WERROR_PHYSICS_MAX_DISTANCE_EXCEEDED,
        WERROR_ACTIONS_LOCKED,
        WERROR_EXTERNAL_ACTIONS_LOCKED,
        WERROR_CANNOT_SEND_MESSAGE,
        WERROR_ILLEGAL_INVENTORY_TRANSACTION,
        WERROR_EXTERNAL_WEENIE_OBJECT,
        WERROR_INTERNAL_WEENIE_OBJECT,
        WERROR_MOTION_FAILURE,
        WERROR_NO_CONTACT,
        WERROR_INQ_CYL_SPHERE_FAILURE,
        WERROR_BAD_COMMAND,
        WERROR_CARRYING_ITEM,
        WERROR_FROZEN,
        WERROR_STUCK,
        WERROR_OVERLOAD,
        WERROR_EXTERNAL_OVERLOAD,
        WERROR_BAD_CONTAIN,
        WERROR_BAD_PARENT,
        WERROR_BAD_DROP,
        WERROR_BAD_RELEASE,
        WERROR_MSG_BAD_MSG,
        WERROR_MSG_UNPACK_FAILED,
        WERROR_MSG_NO_MSG,
        WERROR_MSG_UNDERFLOW,
        WERROR_MSG_OVERFLOW,
        WERROR_MSG_CALLBACK_FAILED,
        WERROR_INTERRUPTED,
        WERROR_OBJECT_GONE,
        WERROR_NO_OBJECT,
        WERROR_CANT_GET_THERE,
        WERROR_DEAD,
        WERROR_I_LEFT_THE_WORLD,
        WERROR_I_TELEPORTED,
        WERROR_TOO_FAR,
        WERROR_STAMINA_TOO_LOW,
        WERROR_CANT_CROUCH_IN_COMBAT,
        WERROR_CANT_SIT_IN_COMBAT,
        WERROR_CANT_LIE_DOWN_IN_COMBAT,
        WERROR_CANT_CHAT_EMOTE_IN_COMBAT,
        WERROR_NO_MTABLE_DATA,
        WERROR_CANT_CHAT_EMOTE_NOT_STANDING,
        WERROR_TOO_MANY_ACTIONS,
        WERROR_HIDDEN,
        WERROR_GENERAL_MOVEMENT_FAILURE,
        WERROR_CANT_JUMP_POSITION,
        WERROR_CANT_JUMP_LOAD,
        WERROR_SELF_INFLICTED_DEATH,
        WERROR_MSG_RESPONSE_FAILURE,
        WERROR_OBJECT_IS_STATIC,
        WERROR_PK_INVALID_STATUS,
        WERROR_PK_PROTECTED_ATTACKER,
        WERROR_PK_PROTECTED_TARGET,
        WERROR_PK_UNPROTECTED_TARGET,
        WERROR_PK_NPK_ATTACKER,
        WERROR_PK_NPK_TARGET,
        WERROR_PK_WRONG_KIND,
        WERROR_PK_CROSS_HOUSE_BOUNDARY,
        // NOTE: Large skip
        WERROR_INVALID_XP_AMOUNT = 1001,
        WERROR_INVALID_PP_CALCULATION,
        WERROR_INVALID_CP_CALCULATION,
        WERROR_UNHANDLED_STAT_ANSWER,
        WERROR_HEART_ATTACK,
        WERROR_CLOSED,
        WERROR_GIVE_NOT_ALLOWED,
        WERROR_INVALID_INVENTORY_LOCATION,
        WERROR_CHANGE_COMBAT_MODE_FAILURE,
        WERROR_FULL_INVENTORY_LOCATION,
        WERROR_CONFLICTING_INVENTORY_LOCATION,
        WERROR_ITEM_NOT_PENDING,
        WERROR_BE_WIELDED_FAILURE,
        WERROR_BE_DROPPED_FAILURE,
        WERROR_COMBAT_FATIGUE,
        WERROR_COMBAT_OUT_OF_AMMO,
        WERROR_COMBAT_MISFIRE,
        WERROR_BAD_MISSILE_CALCULATIONS,
        WERROR_MAGIC_INCOMPLETE_ANIM_LIST,
        WERROR_MAGIC_INVALID_SPELL_TYPE,
        WERROR_MAGIC_INQ_POSITION_AND_VELOCITY_FAILURE,
        WERROR_MAGIC_UNLEARNED_SPELL,
        WERROR_MAGIC_BAD_TARGET_TYPE,
        WERROR_MAGIC_MISSING_COMPONENTS,
        WERROR_MAGIC_INSUFFICIENT_MANA,
        WERROR_MAGIC_FIZZLE,
        WERROR_MAGIC_MISSING_TARGET,
        WERROR_MAGIC_MISFIRED_PROJECTILE_SPELL,
        WERROR_MAGIC_SPELLBOOK_ADDSPELL_FAILURE,
        WERROR_MAGIC_TARGET_OUT_OF_RANGE,
        WERROR_MAGIC_NON_OUTDOOR_SPELL_CAST_OUTSIDE,
        WERROR_MAGIC_NON_INDOOR_SPELL_CAST_INSIDE,
        WERROR_MAGIC_GENERAL_FAILURE,
        WERROR_MAGIC_UNPREPARED,
        WERROR_ALLEGIANCE_PATRON_EXISTS,
        WERROR_ALLEGIANCE_INSUFFICIENT_CP,
        WERROR_ALLEGIANCE_IGNORING_REQUESTS,
        WERROR_ALLEGIANCE_SQUELCHED,
        WERROR_ALLEGIANCE_MAX_DISTANCE_EXCEEDED,
        WERROR_ALLEGIANCE_ILLEGAL_LEVEL,
        WERROR_ALLEGIANCE_BAD_CREATION,
        WERROR_ALLEGIANCE_PATRON_BUSY,
        WERROR_ALLEGIANCE_ADD_HIERARCHY_FAILURE,
        WERROR_ALLEGIANCE_NONEXISTENT,
        WERROR_ALLEGIANCE_REMOVE_HIERARCHY_FAILURE,
        WERROR_ALLEGIANCE_MAX_VASSALS,
        WERROR_FELLOWSHIP_IGNORING_REQUESTS,
        WERROR_FELLOWSHIP_SQUELCHED,
        WERROR_FELLOWSHIP_MAX_DISTANCE_EXCEEDED,
        WERROR_FELLOWSHIP_MEMBER,
        WERROR_FELLOWSHIP_ILLEGAL_LEVEL,
        WERROR_FELLOWSHIP_RECRUIT_BUSY,
        WERROR_FELLOWSHIP_NOT_LEADER,
        WERROR_FELLOWSHIP_FULL,
        WERROR_FELLOWSHIP_UNCLEAN_NAME,
        WERROR_LEVEL_TOO_LOW,
        WERROR_LEVEL_TOO_HIGH,
        WERROR_CHAN_INVALID,
        WERROR_CHAN_SECURITY,
        WERROR_CHAN_ALREADY_ACTIVE,
        WERROR_CHAN_NOT_ACTIVE,
        WERROR_ATTUNED_ITEM,
        WERROR_MERGE_BAD,
        WERROR_MERGE_ENCHANTED,
        WERROR_UNCONTROLLED_STACK,
        WERROR_CURRENTLY_ATTACKING,
        WERROR_MISSILE_ATTACK_NOT_OK,
        WERROR_TARGET_NOT_ACQUIRED,
        WERROR_IMPOSSIBLE_SHOT,
        WERROR_BAD_WEAPON_SKILL,
        WERROR_UNWIELD_FAILURE,
        WERROR_LAUNCH_FAILURE,
        WERROR_RELOAD_FAILURE,
        WERROR_CRAFT_UNABLE_TO_MAKE_CRAFTREQ,
        WERROR_CRAFT_ANIMATION_FAILED,
        WERROR_CRAFT_NO_MATCH_WITH_NUMPREOBJ,
        WERROR_CRAFT_GENERAL_ERROR_UI_MSG,
        WERROR_CRAFT_GENERAL_ERROR_NO_UI_MSG,
        WERROR_CRAFT_FAILED_REQUIREMENTS,
        WERROR_CRAFT_DONT_CONTAIN_EVERYTHING,
        WERROR_CRAFT_ALL_OBJECTS_NOT_FROZEN,
        WERROR_CRAFT_NOT_IN_PEACE_MODE,
        WERROR_CRAFT_NOT_HAVE_SKILL,
        WERROR_HANDS_NOT_FREE,
        WERROR_PORTAL_NOT_LINKABLE,
        WERROR_QUEST_SOLVED_TOO_RECENTLY,
        WERROR_QUEST_SOLVED_MAX_TIMES,
        WERROR_QUEST_UNKNOWN,
        WERROR_QUEST_TABLE_CORRUPT,
        WERROR_QUEST_BAD,
        WERROR_QUEST_DUPLICATE,
        WERROR_QUEST_UNSOLVED,
        WERROR_QUEST_RESRICTION_UNSOLVED,
        WERROR_QUEST_SOLVED_TOO_LONG_AGO,
        UNKNOWN__GUESSEDNAME1095, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1096, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1097, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1098, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1099, // NOTE: Missing 1
        WERROR_TRADE_IGNORING_REQUESTS,
        WERROR_TRADE_SQUELCHED,
        WERROR_TRADE_MAX_DISTANCE_EXCEEDED,
        WERROR_TRADE_ALREADY_TRADING,
        WERROR_TRADE_BUSY,
        WERROR_TRADE_CLOSED,
        WERROR_TRADE_EXPIRED,
        WERROR_TRADE_ITEM_BEING_TRADED,
        WERROR_TRADE_NON_EMPTY_CONTAINER,
        WERROR_TRADE_NONCOMBAT_MODE,
        WERROR_TRADE_INCOMPLETE,
        WERROR_TRADE_STAMP_MISMATCH,
        WERROR_TRADE_UNOPENED,
        WERROR_TRADE_EMPTY,
        WERROR_TRADE_ALREADY_ACCEPTED,
        WERROR_TRADE_OUT_OF_SYNC,
        WERROR_PORTAL_PK_NOT_ALLOWED,
        WERROR_PORTAL_NPK_NOT_ALLOWED,
        WERROR_HOUSE_ABANDONED,
        WERROR_HOUSE_EVICTED,
        WERROR_HOUSE_ALREADY_OWNED,
        WERROR_HOUSE_BUY_FAILED,
        WERROR_HOUSE_RENT_FAILED,
        WERROR_HOOKED,
        UNKNOWN__GUESSEDNAME1124, // NOTE: Missing 1
        WERROR_MAGIC_INVALID_POSITION,
        WERROR_PORTAL_ACDM_ONLY,
        WERROR_INVALID_AMMO_TYPE,
        WERROR_SKILL_TOO_LOW,
        WERROR_HOUSE_MAX_NUMBER_HOOKS_USED,
        WERROR_TRADE_AI_DOESNT_WANT,
        WERROR_HOOK_HOUSE_NOTE_OWNED,
        UNKNOWN__GUESSEDNAME1132, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1133, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1134, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1135, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1136, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1137, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1138, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1139, // NOTE: Missing 1
        WERROR_PORTAL_QUEST_RESTRICTED,
        UNKNOWN__GUESSEDNAME1141, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1142, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1143, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1144, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1145, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1146, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1147, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1148, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1149, // NOTE: Missing 1
        WERROR_HOUSE_NO_ALLEGIANCE,
        WERROR_NO_HOUSE,
        WERROR_HOUSE_NO_MANSION_NO_POSITION,
        WERROR_HOUSE_NOT_A_MANSION,
        WERROR_HOUSE_NOT_ALLOWED_IN,
        UNKNOWN__GUESSEDNAME1155, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1156, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1157, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1158, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1159, // NOTE: Missing 1
        WERROR_HOUSE_UNDER_MIN_LEVEL,
        WERROR_HOUSE_OVER_MAX_LEVEL,
        WERROR_HOUSE_NOT_A_MONARCH,
        WERROR_HOUSE_UNDER_MIN_RANK,
        WERROR_HOUSE_OVER_MAX_RANK,
        WERROR_ALLEGIANCE_DECLINED,
        WERROR_ALLEGIANCE_TIMEOUT,
        WERROR_CONFIRMATION_IN_PROGRESS,
        WERROR_MONARCH_ONLY,
        WERROR_ALLEGIANCE_BOOT_EMPTY_NAME,
        WERROR_ALLEGIANCE_BOOT_SELF,
        WERROR_NO_SUCH_CHARACTER,
        WERROR_ALLEGIANCE_TARGET_NOT_A_MEMBER,
        WERROR_ALLEGIANCE_REMOVE_NO_PATRON,
        WERROR_ALLEGIANCE_OFFLINE_DISSOLVED,
        WERROR_ALLEGIANCE_OFFLINE_DISMISSED,
        WERROR_MOVED_TOO_FAR,
        WERROR_TELETO_INVALID_POSITION,
        WERROR_ACDM_ONLY,
        WERROR_LIFESTONE_LINK_FAILED,
        WERROR_LIFESTONE_LINK_TOO_FAR,
        WERROR_LIFESTONE_LINK_SUCCESS,
        WERROR_LIFESTONE_RECALL_NO_LINK,
        WERROR_LIFESTONE_RECALL_FAILED,
        WERROR_PORTAL_LINK_FAILED,
        WERROR_PORTAL_LINK_SUCCESS,
        WERROR_PORTAL_RECALL_FAILED,
        WERROR_PORTAL_RECALL_NO_LINK,
        WERROR_PORTAL_SUMMON_FAILED,
        WERROR_PORTAL_SUMMON_NO_LINK,
        WERROR_PORTAL_TELEPORT_FAILED,
        WERROR_PORTAL_TOO_RECENTLY,
        WERROR_PORTAL_ADVOCATE_ONLY,
        WERROR_PORTAL_AIS_NOT_ALLOWED,
        WERROR_PORTAL_PLAYERS_NOT_ALLOWED,
        WERROR_PORTAL_LEVEL_TOO_LOW,
        WERROR_PORTAL_LEVEL_TOO_HIGH,
        WERROR_PORTAL_NOT_RECALLABLE,
        WERROR_PORTAL_NOT_SUMMONABLE,
        WERROR_LOCK_ALREADY_UNLOCKED,
        WERROR_LOCK_NOT_LOCKABLE,
        WERROR_LOCK_ALREADY_OPEN, // Chest or door is already in an open and unlocked state
        WERROR_LOCK_WRONG_KEY,
        WERROR_LOCK_USED_TOO_RECENTLY,
        WERROR_DONT_KNOW_LOCKPICKING,
        WERROR_ALLEGIANCE_INFO_EMPTY_NAME,
        WERROR_ALLEGIANCE_INFO_SELF,
        WERROR_ALLEGIANCE_INFO_TOO_RECENT,
        WERROR_ABUSE_NO_SUCH_CHARACTER,
        WERROR_ABUSE_REPORTED_SELF,
        WERROR_ABUSE_COMPLAINT_HANDLED,
        UNKNOWN__GUESSEDNAME1211, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1212, // NOTE: Missing 1
        WERROR_SALVAGE_DONT_OWN_TOOL,
        WERROR_SALVAGE_DONT_OWN_LOOT,
        WERROR_SALVAGE_NOT_SUITABLE,
        WERROR_SALVAGE_WRONG_MATERIAL,
        WERROR_SALVAGE_CREATION_FAILED,
        WERROR_SALVAGE_INVALID_LOOT_LIST,
        WERROR_SALVAGE_TRADING_LOOT,
        WERROR_PORTAL_HOUSE_RESTRICTED,
        WERROR_ACTIVATION_RANK_TOO_LOW,
        WERROR_ACTIVATION_WRONG_RACE,
        WERROR_ACTIVATION_ARCANE_LORE_TOO_LOW,
        WERROR_ACTIVATION_NOT_ENOUGH_MANA,
        WERROR_ACTIVATION_SKILL_TOO_LOW,
        WERROR_ACTIVATION_NOT_CRAFTSMAN,
        WERROR_ACTIVATION_NOT_SPECIALIZED,
        WERROR_PORTAL_PK_ATTACKED_TO_RECENTLY,
        WERROR_TRADE_AI_REFUSE_EMOTE,
        WERROR_TRADE_AI_REFUSE_EMOTE_FAILED_TOO_BUSY,
        WERROR_TRADE_AI_TOO_MANY,
        WERROR_SKILL_ALTERATION_FAILED,
        WERROR_SKILL_ALTERATION_RAISE_NOT_TRAINED,
        WERROR_SKILL_ALTERATION_RAISE_NOT_ENOUGH_CREDITS,
        WERROR_SKILL_ALTERATION_WRAP_AROUND,
        WERROR_SKILL_ALTERATION_LOWER_UNTRAINED,
        WERROR_SKILL_ALTERATION_ILLEGAL_WIELDED_ITEMS,
        WERROR_SKILL_ALTERATION_SPEC_SUCCEEDED,
        WERROR_SKILL_ALTERATION_UNSPEC_SUCCEEDED,
        WERROR_SKILL_ALTERATION_UNTRAIN_SUCCEEDED,
        WERROR_SKILL_ALTERATION_UNTRAIN_RACIAL_SUCCEEDED,
        WERROR_SKILL_ALTERATION_RAISE_TOO_MANY_SPEC_CREDITS,
        WERROR_FELLOWSHIP_DECLINED,
        WERROR_FELLOWSHIP_TIMEOUT,
        WERROR_ATTRIBUTE_ALTERATION_FAILED,
        WERROR_ATTRIBUTE_TRANSFER_FROM_TOO_LOW,
        WERROR_ATTRIBUTE_TRANSFER_TO_TOO_HIGH,
        WERROR_ATTRIBUTE_ALTERATION_ILLEGAL_WIELDED_ITEMS,
        WERROR_ATTRIBUTE_ALTERATION_SUCCEEDED,
        WERROR_HOUSE_DYNAMIC_HOOK_ADD,
        WERROR_HOUSE_WRONG_HOOK_TYPE,
        WERROR_HOUSE_DYNAMIC_STORAGE_ADD,
        WERROR_HOUSE_DYNAMIC_HOOK_CLOSE,
        WERROR_HOUSE_DYNAMIC_STORAGE_CLOSE,
        WERROR_ALLEGIANCE_OWNS_MANSION,
        WERROR_HOOK_ITEM_ON_HOOK_NOT_USEABLE,
        WERROR_HOOK_ITEM_ON_HOOK_NOT_USEABLE_OWNER,
        WERROR_HOOKER_NOT_USEABLE_OFF_HOOK,
        WERROR_MIDAIR,
        WERROR_PK_SWITCH_RECOVERING,
        WERROR_PK_SWITCH_ADVOCATE,
        WERROR_PK_SWITCH_MIN_LEVEL,
        WERROR_PK_SWITCH_MAX_LEVEL,
        WERROR_PK_SWITCH_RECENT_KILL,
        WERROR_PK_SWITCH_AUTO_PK,
        WERROR_PK_SWITCH_RESPITE,
        WERROR_PORTAL_PKLITE_NOT_ALLOWED,
        WERROR_PK_PROTECTED_ATTACKER_PASSIVE,
        WERROR_PK_PROTECTED_TARGET_PASSIVE,
        WERROR_PK_NPK_ATTACKER_PASSIVE,
        WERROR_PK_NPK_TARGET_PASSIVE,
        WERROR_PK_WRONG_KIND_PASSIVE,
        WERROR_PK_CROSS_HOUSE_BOUNDARY_PASSIVE,
        WERROR_MAGIC_INVALID_TARGET,
        WERROR_MAGIC_INVALID_TARGET_PASSIVE,
        WERROR_HEAL_NOT_TRAINED,
        WERROR_HEAL_DONT_OWN_KIT,
        WERROR_HEAL_CANT_HEAL_THAT,
        WERROR_HEAL_FULL_HEALTH,
        WERROR_HEAL_NOT_READY,
        WERROR_HEAL_PLAYERS_ONLY,
        WERROR_LIFESTONE_PROTECTION,
        WERROR_PORTAL_PROTECTION,
        WERROR_PK_SWITCH_PKLITE_OFF,
        WERROR_DEATH_TOO_CLOSE_TO_SANCTUARY,
        WERROR_TRADE_IN_PROGRESS,
        WERROR_PK_SWITCH_PKLITE_ON_PK,
        WERROR_PK_SWITCH_PKLITE_ON,
        WERROR_MAGIC_NO_SUITABLE_ALTERNATE_TARGET,
        WERROR_MAGIC_NO_SUITABLE_ALTERNATE_TARGET_PASSIVE,
        WERROR_FELLOWSHIP_NOW_OPEN,
        WERROR_FELLOWSHIP_NOW_CLOSED,
        WERROR_FELLOWSHIP_NEW_LEADER,
        WERROR_FELLOWSHIP_NO_LONGER_LEADER,
        WERROR_FELLOWSHIP_NO_FELLOWSHIP,
        WERROR_HOUSE_MAX_HOOK_GROUP_REACHED,
        WERROR_HOUSE_MAX_HOOK_GROUP_REACHED_SILENT,
        WERROR_HOUSE_NOW_USING_MAX_HOOKS,
        WERROR_HOUSE_NO_LONGER_USING_MAX_HOOKS,
        WERROR_HOUSE_NOW_USING_MAX_IN_HOOKGROUP,
        WERROR_HOUSE_NO_LONGER_USING_MAX_IN_HOOKGROUP,
        WERROR_HOOK_NOT_PERMITED_TO_USE_HOOK,
        WERROR_FELLOWSHIP_NOT_CLOSE_ENOUGH_LEVEL,
        WERROR_FELLOWSHIP_LOCKED_RECRUITER,
        WERROR_FELLOWSHIP_LOCKED_RECRUITEE,
        WERROR_ACTIVATION_NOT_ALLOWED_NO_NAME,
        WERROR_CHAT_ENTERED_TURBINE_CHAT_ROOM,
        WERROR_CHAT_LEFT_TURBINE_CHAT_ROOM,
        WERROR_CHAT_NOW_USING_TURBINE_CHAT,
        WERROR_ADMIN_IS_DEAF,
        WERROR_ADMIN_DEAF_TO_MESSAGE,
        WERROR_LOUD_LIST_IS_FULL,
        WERROR_CHARACTER_ADDED_LOUD_LIST,
        WERROR_CHARACTER_REMOVED_LOUD_LIST,
        WERROR_DEAF_MODE_ON,
        WERROR_DEAF_MODE_OFF,
        WERROR_FAILED_MUTE,
        WERROR_CRAFT_CHICKEN_OUT_MSG,
        WERROR_CRAFT_NO_CHANCE,
        WERROR_FELLOWSHIP_FELLOW_LOCKED_CAN_NOT_OPEN,
        WERROR_TRADE_COMPLETE,
        WERROR_SALVAGE_NOT_A_TOOL,
        WERROR_CHARACTER_NOT_AVAILABLE,
        WERROR_SNOOP_STARTED,
        WERROR_SNOOP_STOPED,
        WERROR_SNOOP_FAILED,
        WERROR_SNOOP_UNAUTHORIZED,
        WERROR_SNOOP_ALREADY_SNOOPED_ON,
        WERROR_CHARACTER_IN_LIMBO_MSG_NOT_RECIVED,
        WERROR_HOUSE_PURCHASE_TOO_SOON,
        WERROR_ALLEGIANCE_I_AM_BOOTED_FROM_CHAT,
        WERROR_ALLEGIANCE_TARGET_BOOTED_FROM_CHAT,
        WERROR_ALLEGIANCE_NOT_AUTHORIZED,
        WERROR_ALLEGIANCE_CHAR_ALREADY_BANNED,
        WERROR_ALLEGIANCE_CHAR_NOT_BANNED,
        WERROR_ALLEGIANCE_CHAR_NOT_UNBANNED,
        WERROR_ALLEGIANCE_CHAR_BANNED_SUCCESSFULLY,
        WERROR_ALLEGIANCE_CHAR_UNBANNED_SUCCESSFULLY,
        WERROR_ALLEGIANCE_LIST_BANNED_CHARACTERS,
        UNKNOWN__GUESSEDNAME1340, // NOTE: Missing 1
        UNKNOWN__GUESSEDNAME1341, // NOTE: Missing 1
        WERROR_ALLEGIANCE_BANNED,
        WERROR_ALLEGIANCE_YOU_ARE_BANNED,
        WERROR_ALLEGIANCE_BANNED_LIST_FULL,
        WERROR_ALLEGIANCE_OFFICER_SET,
        WERROR_ALLEGIANCE_OFFICER_NOT_SET,
        WERROR_ALLEGIANCE_OFFICER_REMOVED,
        WERROR_ALLEGIANCE_OFFICER_NOT_REMOVED,
        WERROR_ALLEGIANCE_OFFICER_FULL,
        WERROR_ALLEGIANCE_OFFICERS_CLEARED,
        WERROR_CHAT_MUST_WAIT_TO_COMMUNICATE,
        WERROR_CHAT_NO_JOIN_CHANNEL_WHILE_GAGGED,
        WERROR_ALLEGIANCE_YOU_ARE_NOW_AN_OFFICER,
        WERROR_ALLEGIANCE_YOU_ARE_NO_LONGER_AN_OFFICER,
        WERROR_ALLEGIANCE_OFFICER_ALREADY_OFFICER,
        WERROR_ALLEGIANCE_HOMETOWN_NOT_SET,
        WERROR_ALREADY_BEING_USED,
        WERROR_HOOK_EMPTY_NOT_OWNER,
        WERROR_HOOK_EMPTY_OWNER,
        WERROR_MISSILE_OUT_OF_RANGE,
        WERROR_CHAT_NOT_LISTENING_TO_CHANNEL,
        WERROR_ACTOD_ONLY,
        WERROR_ITEM_ACTOD_ONLY,
        WERROR_PORTAL_ACTOD_ONLY,
        WERROR_QUEST_ACTOD_ONLY,
        WERROR_AUGMENTATION_FAILED,
        WERROR_AUGMENTATION_TOO_MANY_TIMES,
        WERROR_AUGMENTATION_FAMILY_TOO_MANY_TIMES,
        WERROR_AUGMENTATION_NOT_ENOUGH_XP,
        WERROR_AUGMENTATION_SKILL_NOT_TRAINED,
        WERROR_AUGMENTATION_SUCCEEDED,
        WERROR_SKILL_ALTERATION_UNTRAIN_AUGMENTED_SUCCEEDED,
        WERROR_PORTAL_RECALLS_DISABLED,
        WERROR_AFK,
        WERROR_PK_ONLY,
        WERROR_PKL_ONLY,
        WERROR_FRIENDS_EXCEEDED_MAX,
        WERROR_FRIENDS_ALREADY_FRIEND,
        WERROR_FRIENDS_NOT_FRIEND,
        WERROR_HOUSE_NOT_OWNER,
        WERROR_ALLEGIANCE_NAME_EMPTY,
        WERROR_ALLEGIANCE_NAME_TOO_LONG,
        WERROR_ALLEGIANCE_NAME_BAD_CHARACTER,
        WERROR_ALLEGIANCE_NAME_NOT_APPROPRIATE,
        WERROR_ALLEGIANCE_NAME_IN_USE,
        WERROR_ALLEGIANCE_NAME_TIMER,
        WERROR_ALLEGIANCE_NAME_CLEARED,
        WERROR_ALLEGIANCE_NAME_SAME_NAME,
        WERROR_ALLEGIANCE_OFFICER_ALREADY_MONARCH,
        WERROR_ALLEGIANCE_OFFICER_TITLE_SET,
        WERROR_ALLEGIANCE_OFFICER_INVALID_LEVEL,
        WERROR_ALLEGIANCE_OFFICER_TITLE_NOT_APPROPRIATE,
        WERROR_ALLEGIANCE_OFFICER_TITLE_TOO_LONG,
        WERROR_ALLEGIANCE_OFFICER_TITLE_CLEARED,
        WERROR_ALLEGIANCE_OFFICER_TITLE_BAD_CHARACTER,
        WERROR_ALLEGIANCE_LOCK_DISPLAY,
        WERROR_ALLEGIANCE_LOCK_SET,
        WERROR_ALLEGIANCE_LOCK_PREVENTS_PATRON,
        WERROR_ALLEGIANCE_LOCK_PREVENTS_VASSAL,
        WERROR_ALLEGIANCE_LOCK_APPROVED_DISPLAY,
        WERROR_ALLEGIANCE_LOCK_NO_APPROVED,
        WERROR_ALLEGIANCE_TARGET_ALREADY_A_MEMBER,
        WERROR_ALLEGIANCE_APPROVED_SET,
        WERROR_ALLEGIANCE_APPROVED_CLEARED,
        WERROR_ALLEGIANCE_GAG_ALREADY,
        WERROR_ALLEGIANCE_GAG_NOT_ALREADY,
        WERROR_ALLEGIANCE_GAG_TARGET,
        WERROR_ALLEGIANCE_GAG_OFFICER,
        WERROR_ALLEGIANCE_GAG_AUTO_UNGAG,
        WERROR_ALLEGIANCE_GAG_UNGAG_TARGET,
        WERROR_ALLEGIANCE_GAG_UNGAG_OFFICER,
        WERROR_TOO_MANY_UNIQUE_ITEMS,
        WERROR_HERITAGE_REQUIRES_SPECIFIC_ARMOR,
        WERROR_SPECIFIC_ARMOR_REQUIRES_HERITAGE,
        WERROR_NOT_OLTHOI_INTERACTION,
        WERROR_NOT_OLTHOI_LIFESTONE,
        WERROR_NOT_OLTHOI_VENDOR,
        WERROR_NOT_OLTHOI_NPC,
        WERROR_NO_OLTHOI_FELLOWSHIP,
        WERROR_NO_OLTHOI_ALLEGIANCE,
        WERROR_ITEM_INTERACTION_RESTRICTED,
        WERROR_PERSON_INTERACTION_RESTRICTED,
        WERROR_PORTAL_ONLY_OLTHOI_PK,
        WERROR_PORTAL_NO_OLTHOI_PK,
        WERROR_PORTAL_NO_VITAE,
        WERROR_PORTAL_NO_NEW_ACCOUNTS,
        WERROR_BAD_OLTHOI_RECALL,
        WERROR_CONTRACT_ERROR
    }
}
