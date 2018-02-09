using System.Collections.Generic;
using System.Linq;

using ACE.Entity;
using ACE.Entity.Enum;

namespace ACE.Database
{
    public abstract class CommonDatabase : Database
    {
        private enum AceObjectPreparedStatement
        {
            GetPaletteOverridesByObject = 9000, // just needs to not conflict with subclassed enum types
            GetAnimationOverridesByObject,
            GetTextureOverridesByObject,
            GetAceObjectPropertiesInt,
            GetAceObjectPropertiesBigInt,
            GetAceObjectPropertiesDouble,
            GetAceObjectPropertiesBool,
            GetAceObjectPropertiesString,
            GetAceObjectPropertiesDid,
            GetAceObjectPropertiesIid,
            GetAceObjectPropertiesPositions,
            GetAceObjectPropertiesAttributes,
            GetAceObjectPropertiesAttributes2nd,
            GetAceObjectPropertiesSkills,
            GetAceObjectPropertiesSpell,
            GetAceObjectPropertiesBook,

            GetAceObjectsByContainerId,
            GetAceObjectsByWielderId,

            SaveAceObject,
            DeleteAceObject,
            GetAceObject,
            UpdateAceObject,

            DeletePaletteOverridesByObject,
            DeleteAnimationOverridesByObject,
            DeleteTextureOverridesByObject,
            DeleteAceObjectPropertiesInt,
            DeleteAceObjectPropertiesBigInt,
            DeleteAceObjectPropertiesDouble,
            DeleteAceObjectPropertiesBool,
            DeleteAceObjectPropertiesString,
            DeleteAceObjectPropertiesDid,
            DeleteAceObjectPropertiesIid,
            DeleteAceObjectPropertiesPositions,
            DeleteAceObjectPropertiesAttributes,
            DeleteAceObjectPropertiesAttributes2nd,
            DeleteAceObjectPropertiesSkills,
            DeleteAceObjectPropertiesSpell,
            DeleteAceObjectPropertiesBook,

            InsertPaletteOverridesByObject,
            InsertAnimationOverridesByObject,
            InsertTextureOverridesByObject,
            InsertAceObjectPropertiesInt,
            InsertAceObjectPropertiesBigInt,
            InsertAceObjectPropertiesDouble,
            InsertAceObjectPropertiesBool,
            InsertAceObjectPropertiesString,
            InsertAceObjectPropertiesDid,
            InsertAceObjectPropertiesIid,
            InsertAceObjectPropertiesPositions,
            InsertAceObjectPropertiesAttributes,
            InsertAceObjectPropertiesAttributes2nd,
            InsertAceObjectPropertiesSkills,
            InsertAceObjectPropertiesSpells,
            InsertAceObjectPropertiesBook,

            // note, this section is all "Property" singular
            UpdateAceObjectPropertyInt,
            UpdateAceObjectPropertyBigInt,
            UpdateAceObjectPropertyDouble,
            UpdateAceObjectPropertyBool,
            UpdateAceObjectPropertyString,
            UpdateAceObjectPropertyDid,
            UpdateAceObjectPropertyIid,
            UpdateAceObjectPropertyPosition,
            UpdateAceObjectPropertyAttribute,
            UpdateAceObjectPropertyAttribute2nd,
            UpdateAceObjectPropertySkill,

            DeleteAceObjectPropertyInt,
            DeleteAceObjectPropertyBigInt,
            DeleteAceObjectPropertyDouble,
            DeleteAceObjectPropertyBool,
            DeleteAceObjectPropertyString,
            DeleteAceObjectPropertyDid,
            DeleteAceObjectPropertyIid,
            DeleteAceObjectPropertyPosition,
            DeleteAceObjectPropertyAttribute,
            DeleteAceObjectPropertyAttribute2nd,
            DeleteAceObjectPropertySkill
        }

        protected override void InitializePreparedStatements()
        {
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObject, typeof(AceObject), ConstructedStatementType.Delete);
            ConstructStatement(AceObjectPreparedStatement.SaveAceObject, typeof(AceObject), ConstructedStatementType.Insert);
            ConstructStatement(AceObjectPreparedStatement.GetAceObject, typeof(AceObject), ConstructedStatementType.Get);
            ConstructStatement(AceObjectPreparedStatement.UpdateAceObject, typeof(AceObject), ConstructedStatementType.Update);

            ConstructStatement(AceObjectPreparedStatement.GetAceObjectsByContainerId, typeof(CachedInventoryObject), ConstructedStatementType.GetList);
            ConstructStatement(AceObjectPreparedStatement.GetAceObjectsByWielderId, typeof(CachedWieldedObject), ConstructedStatementType.GetList);

            ConstructStatement(AceObjectPreparedStatement.GetAceObjectPropertiesBigInt, typeof(AceObjectPropertiesInt64), ConstructedStatementType.GetList);
            ConstructStatement(AceObjectPreparedStatement.GetAceObjectPropertiesInt, typeof(AceObjectPropertiesInt), ConstructedStatementType.GetList);
            ConstructStatement(AceObjectPreparedStatement.GetAceObjectPropertiesBool, typeof(AceObjectPropertiesBool), ConstructedStatementType.GetList);
            ConstructStatement(AceObjectPreparedStatement.GetAceObjectPropertiesDouble, typeof(AceObjectPropertiesDouble), ConstructedStatementType.GetList);
            ConstructStatement(AceObjectPreparedStatement.GetAceObjectPropertiesString, typeof(AceObjectPropertiesString), ConstructedStatementType.GetList);
            ConstructStatement(AceObjectPreparedStatement.GetAceObjectPropertiesIid, typeof(AceObjectPropertiesInstanceId), ConstructedStatementType.GetList);
            ConstructStatement(AceObjectPreparedStatement.GetAceObjectPropertiesDid, typeof(AceObjectPropertiesDataId), ConstructedStatementType.GetList);
            
            ConstructStatement(AceObjectPreparedStatement.GetTextureOverridesByObject, typeof(TextureMapOverride), ConstructedStatementType.GetList);
            ConstructStatement(AceObjectPreparedStatement.GetPaletteOverridesByObject, typeof(PaletteOverride), ConstructedStatementType.GetList);
            ConstructStatement(AceObjectPreparedStatement.GetAnimationOverridesByObject, typeof(AnimationOverride), ConstructedStatementType.GetList);

            ConstructStatement(AceObjectPreparedStatement.GetAceObjectPropertiesPositions, typeof(AceObjectPropertiesPosition), ConstructedStatementType.GetList);

            ConstructStatement(AceObjectPreparedStatement.GetAceObjectPropertiesAttributes, typeof(AceObjectPropertiesAttribute), ConstructedStatementType.GetList);
            ConstructStatement(AceObjectPreparedStatement.GetAceObjectPropertiesAttributes2nd, typeof(AceObjectPropertiesAttribute2nd), ConstructedStatementType.GetList);
            ConstructStatement(AceObjectPreparedStatement.GetAceObjectPropertiesSkills, typeof(AceObjectPropertiesSkill), ConstructedStatementType.GetList);
            ConstructStatement(AceObjectPreparedStatement.GetAceObjectPropertiesSpell, typeof(AceObjectPropertiesSpell), ConstructedStatementType.GetList);
            ConstructStatement(AceObjectPreparedStatement.GetAceObjectPropertiesBook, typeof(AceObjectPropertiesBook), ConstructedStatementType.GetList);

            // Delete statements
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertiesInt, typeof(AceObjectPropertiesInt), ConstructedStatementType.DeleteList);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertiesBigInt, typeof(AceObjectPropertiesInt64), ConstructedStatementType.DeleteList);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertiesBool, typeof(AceObjectPropertiesBool), ConstructedStatementType.DeleteList);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertiesDouble, typeof(AceObjectPropertiesDouble), ConstructedStatementType.DeleteList);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertiesString, typeof(AceObjectPropertiesString), ConstructedStatementType.DeleteList);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertiesIid, typeof(AceObjectPropertiesInstanceId), ConstructedStatementType.DeleteList);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertiesDid, typeof(AceObjectPropertiesDataId), ConstructedStatementType.DeleteList);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertiesPositions, typeof(AceObjectPropertiesPosition), ConstructedStatementType.DeleteList);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertiesSkills, typeof(AceObjectPropertiesSkill), ConstructedStatementType.DeleteList);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertiesAttributes, typeof(AceObjectPropertiesAttribute), ConstructedStatementType.DeleteList);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertiesAttributes2nd, typeof(AceObjectPropertiesAttribute2nd), ConstructedStatementType.DeleteList);
            ConstructStatement(AceObjectPreparedStatement.DeleteTextureOverridesByObject, typeof(TextureMapOverride), ConstructedStatementType.DeleteList);
            ConstructStatement(AceObjectPreparedStatement.DeletePaletteOverridesByObject, typeof(PaletteOverride), ConstructedStatementType.DeleteList);
            ConstructStatement(AceObjectPreparedStatement.DeleteAnimationOverridesByObject, typeof(AnimationOverride), ConstructedStatementType.DeleteList);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertiesSpell, typeof(AceObjectPropertiesSpell), ConstructedStatementType.DeleteList);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertiesBook, typeof(AceObjectPropertiesBook), ConstructedStatementType.DeleteList);

            // Insert statements
            ConstructStatement(AceObjectPreparedStatement.InsertAceObjectPropertiesInt, typeof(AceObjectPropertiesInt), ConstructedStatementType.InsertList);
            ConstructStatement(AceObjectPreparedStatement.InsertAceObjectPropertiesBigInt, typeof(AceObjectPropertiesInt64), ConstructedStatementType.InsertList);
            ConstructStatement(AceObjectPreparedStatement.InsertAceObjectPropertiesBool, typeof(AceObjectPropertiesBool), ConstructedStatementType.InsertList);
            ConstructStatement(AceObjectPreparedStatement.InsertAceObjectPropertiesDouble, typeof(AceObjectPropertiesDouble), ConstructedStatementType.InsertList);
            ConstructStatement(AceObjectPreparedStatement.InsertAceObjectPropertiesString, typeof(AceObjectPropertiesString), ConstructedStatementType.InsertList);
            ConstructStatement(AceObjectPreparedStatement.InsertAceObjectPropertiesIid, typeof(AceObjectPropertiesInstanceId), ConstructedStatementType.InsertList);
            ConstructStatement(AceObjectPreparedStatement.InsertAceObjectPropertiesDid, typeof(AceObjectPropertiesDataId), ConstructedStatementType.InsertList);
            ConstructStatement(AceObjectPreparedStatement.InsertAceObjectPropertiesPositions, typeof(AceObjectPropertiesPosition), ConstructedStatementType.InsertList);
            ConstructStatement(AceObjectPreparedStatement.InsertAceObjectPropertiesSkills, typeof(AceObjectPropertiesSkill), ConstructedStatementType.InsertList);
            ConstructStatement(AceObjectPreparedStatement.InsertAceObjectPropertiesAttributes, typeof(AceObjectPropertiesAttribute), ConstructedStatementType.InsertList);
            ConstructStatement(AceObjectPreparedStatement.InsertAceObjectPropertiesAttributes2nd, typeof(AceObjectPropertiesAttribute2nd), ConstructedStatementType.InsertList);
            ConstructStatement(AceObjectPreparedStatement.InsertTextureOverridesByObject, typeof(TextureMapOverride), ConstructedStatementType.InsertList);
            ConstructStatement(AceObjectPreparedStatement.InsertPaletteOverridesByObject, typeof(PaletteOverride), ConstructedStatementType.InsertList);
            ConstructStatement(AceObjectPreparedStatement.InsertAnimationOverridesByObject, typeof(AnimationOverride), ConstructedStatementType.InsertList);
            ConstructStatement(AceObjectPreparedStatement.InsertAceObjectPropertiesSpells, typeof(AceObjectPropertiesSpell), ConstructedStatementType.InsertList);
            ConstructStatement(AceObjectPreparedStatement.InsertAceObjectPropertiesBook, typeof(AceObjectPropertiesBook), ConstructedStatementType.InsertList);

            // Updates
            ConstructStatement(AceObjectPreparedStatement.UpdateAceObjectPropertyInt, typeof(AceObjectPropertiesInt), ConstructedStatementType.Update);
            ConstructStatement(AceObjectPreparedStatement.UpdateAceObjectPropertyBigInt, typeof(AceObjectPropertiesInt64), ConstructedStatementType.Update);
            ConstructStatement(AceObjectPreparedStatement.UpdateAceObjectPropertyBool, typeof(AceObjectPropertiesBool), ConstructedStatementType.Update);
            ConstructStatement(AceObjectPreparedStatement.UpdateAceObjectPropertyDouble, typeof(AceObjectPropertiesDouble), ConstructedStatementType.Update);
            ConstructStatement(AceObjectPreparedStatement.UpdateAceObjectPropertyString, typeof(AceObjectPropertiesString), ConstructedStatementType.Update);
            ConstructStatement(AceObjectPreparedStatement.UpdateAceObjectPropertyIid, typeof(AceObjectPropertiesInstanceId), ConstructedStatementType.Update);
            ConstructStatement(AceObjectPreparedStatement.UpdateAceObjectPropertyDid, typeof(AceObjectPropertiesDataId), ConstructedStatementType.Update);
            ConstructStatement(AceObjectPreparedStatement.UpdateAceObjectPropertyPosition, typeof(AceObjectPropertiesPosition), ConstructedStatementType.Update);
            ConstructStatement(AceObjectPreparedStatement.UpdateAceObjectPropertySkill, typeof(AceObjectPropertiesSkill), ConstructedStatementType.Update);
            ConstructStatement(AceObjectPreparedStatement.UpdateAceObjectPropertyAttribute, typeof(AceObjectPropertiesAttribute), ConstructedStatementType.Update);
            ConstructStatement(AceObjectPreparedStatement.UpdateAceObjectPropertyAttribute2nd, typeof(AceObjectPropertiesAttribute2nd), ConstructedStatementType.Update);

            // deletes for properties
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertyInt, typeof(AceObjectPropertiesInt), ConstructedStatementType.Delete);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertyBigInt, typeof(AceObjectPropertiesInt64), ConstructedStatementType.Delete);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertyBool, typeof(AceObjectPropertiesBool), ConstructedStatementType.Delete);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertyDouble, typeof(AceObjectPropertiesDouble), ConstructedStatementType.Delete);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertyString, typeof(AceObjectPropertiesString), ConstructedStatementType.Delete);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertyIid, typeof(AceObjectPropertiesInstanceId), ConstructedStatementType.Delete);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertyDid, typeof(AceObjectPropertiesDataId), ConstructedStatementType.Delete);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertyPosition, typeof(AceObjectPropertiesPosition), ConstructedStatementType.Delete);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertySkill, typeof(AceObjectPropertiesSkill), ConstructedStatementType.Delete);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertyAttribute, typeof(AceObjectPropertiesAttribute), ConstructedStatementType.Delete);
            ConstructStatement(AceObjectPreparedStatement.DeleteAceObjectPropertyAttribute2nd, typeof(AceObjectPropertiesAttribute2nd), ConstructedStatementType.Delete);
        }

        public AceObject GetObject(uint objId)
        {
            AceObject ret = new AceObject();
            var criteria = new Dictionary<string, object> { { "aceObjectId", objId } };
            bool success = ExecuteConstructedGetStatement<AceObject, AceObjectPreparedStatement>(AceObjectPreparedStatement.GetAceObject, criteria, ret);
            if (!success)
            {
                return null;
            }

            LoadIntoObject(ret);
            ret.ClearDirtyFlags();
            return ret;
        }

        public bool SaveObject(AceObject aceObject)
        {
            return SaveOrReplaceObject(aceObject);
        }

        protected bool SaveOrReplaceObject(AceObject aceObject, bool replace = false)
        {
            DatabaseTransaction transaction = BeginTransaction();

            SaveObjectInternal(transaction, aceObject, replace);

            // Do we have any inventory to save - if not, we are done here?
            if (aceObject.Inventory.Count > 0)
            {
                foreach (AceObject invItem in aceObject.Inventory.Values)
                {
                    if (invItem.IsDirty)
                    {
                        DeleteObjectInternal(transaction, invItem);
                        invItem.SetDirtyFlags();
                    }
                    SaveObjectInternal(transaction, invItem);

                    // Was the item I just saved a container?   If so, we need to save the items in the container as well. Og II
                    if (invItem.WeenieType == (uint)WeenieType.Container && invItem.Inventory.Count > 0)
                    {
                        foreach (AceObject contInvItem in invItem.Inventory.Values)
                        {
                            if (contInvItem.IsDirty)
                            {
                                DeleteObjectInternal(transaction, contInvItem);
                                contInvItem.SetDirtyFlags();
                            }
                            SaveObjectInternal(transaction, contInvItem);
                        }
                    }
                }
            }

            // Do we have any wielded items to save - if so, let's save them.
            if (aceObject.WieldedItems.Count > 0)
            {
                foreach (AceObject wieldedItem in aceObject.WieldedItems.Values)
                {
                    if (wieldedItem.IsDirty)
                    {
                        DeleteObjectInternal(transaction, wieldedItem);
                        wieldedItem.SetDirtyFlags();
                    }
                    SaveObjectInternal(transaction, wieldedItem);
                }
            }

            return transaction.Commit().Result;
        }

        public bool DeleteObject(AceObject aceObject)
        {
            DatabaseTransaction transaction = BeginTransaction();

            DeleteObjectInternal(transaction, aceObject);

            // Do we have any  - if not, we are done here?
            if (aceObject.Inventory.Count <= 0)
                return transaction.Commit().Result;
            foreach (AceObject invItem in aceObject.Inventory.Values)
            {
                DeleteObjectInternal(transaction, invItem);
                // Was the item I just deleted a container?   If so, we need to delete the items in the container as well. Og II
                if (invItem.WeenieType != (uint)WeenieType.Container)
                    continue;
                foreach (AceObject contInvItem in invItem.Inventory.Values)
                {
                    DeleteObjectInternal(transaction, contInvItem);
                }
            }
            return transaction.Commit().Result;
        }

        protected virtual void LoadIntoObject(AceObject aceObject)
        {
            // this flag determines when the subsequentnt calls to "save" trigger an insert or an update
            aceObject.HasEverBeenSavedToDatabase = true;

            // TODO: still to implement - load spells, friends, allegiance info, spell comps
            aceObject.IntProperties = GetAceObjectPropertiesInt(aceObject.AceObjectId);
            aceObject.Int64Properties = GetAceObjectPropertiesBigInt(aceObject.AceObjectId);
            aceObject.BoolProperties = GetAceObjectPropertiesBool(aceObject.AceObjectId);
            aceObject.DoubleProperties = GetAceObjectPropertiesDouble(aceObject.AceObjectId);
            aceObject.StringProperties = GetAceObjectPropertiesString(aceObject.AceObjectId);
            aceObject.InstanceIdProperties = GetAceObjectPropertiesIid(aceObject.AceObjectId);
            aceObject.DataIdProperties = GetAceObjectPropertiesDid(aceObject.AceObjectId);
            aceObject.TextureOverrides = GetAceObjectTextureMaps(aceObject.AceObjectId);
            aceObject.AnimationOverrides = GetAceObjectAnimations(aceObject.AceObjectId);
            aceObject.PaletteOverrides = GetAceObjectPalettes(aceObject.AceObjectId);
            aceObject.AceObjectPropertiesPositions = GetAceObjectPostions(aceObject.AceObjectId).ToDictionary(x => (PositionType)x.DbPositionType, x => new Position(x));
            aceObject.AceObjectPropertiesAttributes = GetAceObjectPropertiesAttribute(aceObject.AceObjectId).ToDictionary(x => (Ability)x.AttributeId, x => new CreatureAbility(x));
            aceObject.AceObjectPropertiesAttributes2nd = GetAceObjectPropertiesAttribute2nd(aceObject.AceObjectId).ToDictionary(x => (Ability)x.Attribute2ndId, x => new CreatureVital(aceObject, x));
            aceObject.AceObjectPropertiesSkills = GetAceObjectPropertiesSkill(aceObject.AceObjectId).ToDictionary(x => (Skill)x.SkillId, x => new CreatureSkill(aceObject, x));
            aceObject.SpellIdProperties = GetAceObjectPropertiesSpell(aceObject.AceObjectId);
            aceObject.BookProperties = GetAceObjectPropertiesBook(aceObject.AceObjectId).ToDictionary(x => x.Page);

            aceObject.Inventory = GetInventoryByContainerId(aceObject.AceObjectId);
            // Ok now, check to see if we loaded any containers that themselves may have items ... Og II
            foreach (var invItem in aceObject.Inventory)
            {
                if (invItem.Value.WeenieType == (uint)WeenieType.Container)
                    invItem.Value.Inventory = GetInventoryByContainerId(invItem.Key.Full);
            }
            aceObject.WieldedItems = GetItemsByWielderId(aceObject.AceObjectId);
        }
        
        protected virtual bool DeleteObjectInternal(DatabaseTransaction transaction, AceObject aceObject)
        {
            // TODO: Database is designed to cascade delete so in reality we only need to call DeleteAceObjectBase
            // We need to decide to either let the db do the work or if we are going to keep doing it on the application side
            // should we remove the cascade deletes? Og II
            DeleteAceObjectPropertiesInt(transaction, aceObject.AceObjectId, aceObject.IntProperties);
            DeleteAceObjectPropertiesBigInt(transaction, aceObject.AceObjectId, aceObject.Int64Properties);
            DeleteAceObjectPropertiesBool(transaction, aceObject.AceObjectId, aceObject.BoolProperties);
            DeleteAceObjectPropertiesDouble(transaction, aceObject.AceObjectId, aceObject.DoubleProperties);
            DeleteAceObjectPropertiesString(transaction, aceObject.AceObjectId, aceObject.StringProperties);
            DeleteAceObjectPropertiesIid(transaction, aceObject.AceObjectId, aceObject.InstanceIdProperties);
            DeleteAceObjectPropertiesDid(transaction, aceObject.AceObjectId, aceObject.DataIdProperties);
            DeleteAceObjectTextureMaps(transaction, aceObject.AceObjectId, aceObject.TextureOverrides);
            DeleteAceObjectAnimations(transaction, aceObject.AceObjectId, aceObject.AnimationOverrides);
            DeleteAceObjectPalettes(transaction, aceObject.AceObjectId, aceObject.PaletteOverrides);

            DeleteAceObjectPropertiesPositions(transaction, aceObject.AceObjectId);
            DeleteAceObjectPropertiesAttributes(transaction, aceObject.AceObjectId);
            DeleteAceObjectPropertiesAttribute2nd(transaction, aceObject.AceObjectId);
            DeleteAceObjectPropertiesSkill(transaction, aceObject.AceObjectId);
            DeleteAceObjectPropertiesSpells(transaction, aceObject.AceObjectId);

            if (!DeleteObjectDependencies(transaction, aceObject)) return false;

            DeleteAceObjectBase(transaction, aceObject);
            return true;
        }

        protected virtual bool DeleteObjectDependencies(DatabaseTransaction transaction, AceObject aceObject)
        {
            return true;
        }

        private bool SaveObjectInternal(DatabaseTransaction transaction, AceObject aceObject, bool replace = false)
        {
            // Update the character table -- save the AceObject to ace_object.
            SaveAceObjectBase(transaction, aceObject);

            if (replace) DeleteAceObjectPropertiesInt(transaction, aceObject.AceObjectId, aceObject.IntProperties);
            SaveAceObjectPropertiesInt(transaction, aceObject.AceObjectId, aceObject.IntProperties);

            if (replace) DeleteAceObjectPropertiesBigInt(transaction, aceObject.AceObjectId, aceObject.Int64Properties);
            SaveAceObjectPropertiesBigInt(transaction, aceObject.AceObjectId, aceObject.Int64Properties);
            
            if (replace) DeleteAceObjectPropertiesBool(transaction, aceObject.AceObjectId, aceObject.BoolProperties);
            SaveAceObjectPropertiesBool(transaction, aceObject.AceObjectId, aceObject.BoolProperties);

            if (replace) DeleteAceObjectPropertiesDouble(transaction, aceObject.AceObjectId, aceObject.DoubleProperties);
            SaveAceObjectPropertiesDouble(transaction, aceObject.AceObjectId, aceObject.DoubleProperties);

            if (replace) DeleteAceObjectPropertiesString(transaction, aceObject.AceObjectId, aceObject.StringProperties);
            SaveAceObjectPropertiesString(transaction, aceObject.AceObjectId, aceObject.StringProperties);

            if (replace) DeleteAceObjectPropertiesIid(transaction, aceObject.AceObjectId, aceObject.InstanceIdProperties);
            SaveAceObjectPropertiesIid(transaction, aceObject.AceObjectId, aceObject.InstanceIdProperties);

            if (replace) DeleteAceObjectPropertiesDid(transaction, aceObject.AceObjectId, aceObject.DataIdProperties);
            SaveAceObjectPropertiesDid(transaction, aceObject.AceObjectId, aceObject.DataIdProperties);

            DeleteAceObjectTextureMaps(transaction, aceObject.AceObjectId, aceObject.TextureOverrides);
            SaveAceObjectTextureMaps(transaction, aceObject.AceObjectId, aceObject.TextureOverrides);

            DeleteAceObjectPropertiesSpells(transaction, aceObject.AceObjectId);
            SaveAceObjectPropertiesSpells(transaction, aceObject.AceObjectId, aceObject.SpellIdProperties);
            
            DeleteAceObjectAnimations(transaction, aceObject.AceObjectId, aceObject.AnimationOverrides);
            SaveAceObjectAnimations(transaction, aceObject.AceObjectId, aceObject.AnimationOverrides);

            DeleteAceObjectPalettes(transaction, aceObject.AceObjectId, aceObject.PaletteOverrides);
            SaveAceObjectPalettes(transaction, aceObject.AceObjectId, aceObject.PaletteOverrides);

            if (replace) DeleteAceObjectPropertiesAttributes(transaction, aceObject.AceObjectId);
            SaveAceObjectPropertiesAttributes(transaction, aceObject.AceObjectId, aceObject.AceObjectPropertiesAttributes);
            
            if (replace) DeleteAceObjectPropertiesAttribute2nd(transaction, aceObject.AceObjectId);
            SaveAceObjectPropertiesAttribute2nd(transaction, aceObject.AceObjectId, aceObject.AceObjectPropertiesAttributes2nd);

            if (replace) DeleteAceObjectPropertiesSkill(transaction, aceObject.AceObjectId);
            SaveAceObjectPropertiesSkill(transaction, aceObject.AceObjectId, aceObject.AceObjectPropertiesSkills);

            DeleteAceObjectPropertiesBook(transaction, aceObject.AceObjectId);
            SaveAceObjectPropertiesBook(transaction, aceObject.AceObjectId, aceObject.BookProperties);
            
            DeleteAceObjectPropertiesPositions(transaction, aceObject.AceObjectId);
            SaveAceObjectPropertiesPositions(transaction, aceObject.AceObjectId,
                aceObject.AceObjectPropertiesPositions.Select(x => x.Value.GetAceObjectPosition(aceObject.AceObjectId, x.Key)).ToList());

            if (!SaveObjectDependencies(transaction, aceObject)) return false;

            return true;
        }

        protected virtual bool SaveObjectDependencies(DatabaseTransaction transaction, AceObject aceObject)
        {
            return true;
        }

        public Dictionary<ObjectGuid, AceObject> GetInventoryByContainerId(uint containerId)
        {
            var criteria = new Dictionary<string, object> { { "containerId", containerId } };
            var objects = ExecuteConstructedGetListStatement<AceObjectPreparedStatement, CachedInventoryObject>(AceObjectPreparedStatement.GetAceObjectsByContainerId, criteria);
            return objects.ToDictionary(x => new ObjectGuid(x.AceObjectId), x => GetObject(x.AceObjectId));
        }

        public Dictionary<ObjectGuid, AceObject> GetItemsByWielderId(uint wielderId)
        {
            var criteria = new Dictionary<string, object> { { "wielderId", wielderId } };
            var objects = ExecuteConstructedGetListStatement<AceObjectPreparedStatement, CachedWieldedObject>(AceObjectPreparedStatement.GetAceObjectsByWielderId, criteria);
            return objects.ToDictionary(x => new ObjectGuid(x.AceObjectId), x => GetObject(x.AceObjectId));
        }

        private List<AceObjectPropertiesPosition> GetAceObjectPostions(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<AceObjectPreparedStatement, AceObjectPropertiesPosition>(AceObjectPreparedStatement.GetAceObjectPropertiesPositions, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesSkill> GetAceObjectPropertiesSkill(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<AceObjectPreparedStatement, AceObjectPropertiesSkill>(AceObjectPreparedStatement.GetAceObjectPropertiesSkills, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesAttribute> GetAceObjectPropertiesAttribute(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<AceObjectPreparedStatement, AceObjectPropertiesAttribute>(AceObjectPreparedStatement.GetAceObjectPropertiesAttributes, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        // ReSharper disable once InconsistentNaming
        private List<AceObjectPropertiesAttribute2nd> GetAceObjectPropertiesAttribute2nd(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<AceObjectPreparedStatement, AceObjectPropertiesAttribute2nd>(AceObjectPreparedStatement.GetAceObjectPropertiesAttributes2nd, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesInt> GetAceObjectPropertiesInt(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<AceObjectPreparedStatement, AceObjectPropertiesInt>(AceObjectPreparedStatement.GetAceObjectPropertiesInt, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesInt64> GetAceObjectPropertiesBigInt(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<AceObjectPreparedStatement, AceObjectPropertiesInt64>(AceObjectPreparedStatement.GetAceObjectPropertiesBigInt, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesBool> GetAceObjectPropertiesBool(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<AceObjectPreparedStatement, AceObjectPropertiesBool>(AceObjectPreparedStatement.GetAceObjectPropertiesBool, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesDouble> GetAceObjectPropertiesDouble(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<AceObjectPreparedStatement, AceObjectPropertiesDouble>(AceObjectPreparedStatement.GetAceObjectPropertiesDouble, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesString> GetAceObjectPropertiesString(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<AceObjectPreparedStatement, AceObjectPropertiesString>(AceObjectPreparedStatement.GetAceObjectPropertiesString, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesDataId> GetAceObjectPropertiesDid(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<AceObjectPreparedStatement, AceObjectPropertiesDataId>(AceObjectPreparedStatement.GetAceObjectPropertiesDid, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesInstanceId> GetAceObjectPropertiesIid(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<AceObjectPreparedStatement, AceObjectPropertiesInstanceId>(AceObjectPreparedStatement.GetAceObjectPropertiesIid, criteria);
            objects.ForEach(o =>
            {
                o.HasEverBeenSavedToDatabase = true;
                o.IsDirty = false;
            });
            return objects;
        }

        private List<AceObjectPropertiesSpell> GetAceObjectPropertiesSpell(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<AceObjectPreparedStatement, AceObjectPropertiesSpell>(AceObjectPreparedStatement.GetAceObjectPropertiesSpell, criteria);
            return objects;
        }
        
        private List<AceObjectPropertiesBook> GetAceObjectPropertiesBook(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<AceObjectPreparedStatement, AceObjectPropertiesBook>(AceObjectPreparedStatement.GetAceObjectPropertiesBook, criteria);
            return objects;
        }

        private List<TextureMapOverride> GetAceObjectTextureMaps(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<AceObjectPreparedStatement, TextureMapOverride>(AceObjectPreparedStatement.GetTextureOverridesByObject, criteria);
            return objects;
        }

        private List<PaletteOverride> GetAceObjectPalettes(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<AceObjectPreparedStatement, PaletteOverride>(AceObjectPreparedStatement.GetPaletteOverridesByObject, criteria);
            return objects;
        }

        private List<AnimationOverride> GetAceObjectAnimations(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<AceObjectPreparedStatement, AnimationOverride>(AceObjectPreparedStatement.GetAnimationOverridesByObject, criteria);
            return objects;
        }

        private bool SaveAceObjectBase(DatabaseTransaction transaction, AceObject obj)
        {
            if (obj.IsDirty)
            {
                if (!obj.HasEverBeenSavedToDatabase)
                    transaction.AddPreparedInsertStatement(AceObjectPreparedStatement.SaveAceObject, obj);
                else
                    transaction.AddPreparedUpdateStatement(AceObjectPreparedStatement.UpdateAceObject, obj);
            }

            return true;
        }

        // FIXME(ddevec): These are a lot of functions that essentially do the same thing... but the SharedPreparedStatement.--- makes them a pain to template/reduce
        private bool SaveAceObjectPropertiesInt(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesInt> properties)
        {
            // calling ToList forces the enumerable evaluation so that we can call .Remove from within the loop
            var theDirtyOnes = properties.Where(p => p.IsDirty).ToList();
            theDirtyOnes.ForEach(p => p.AceObjectId = aceObjectId);

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    if (prop.PropertyValue == null)
                    {
                        // delete it
                        transaction.AddPreparedDeleteStatement<AceObjectPreparedStatement, AceObjectPropertiesInt>(AceObjectPreparedStatement.DeleteAceObjectPropertyInt, prop);
                        properties.Remove(prop);
                    }
                    else
                    {
                        // update it
                        transaction.AddPreparedUpdateStatement(AceObjectPreparedStatement.UpdateAceObjectPropertyInt, prop);
                    }
                }
                else if (prop.PropertyValue != null)
                {
                    transaction.AddPreparedInsertStatement(AceObjectPreparedStatement.InsertAceObjectPropertiesInt, prop);
                }
            }

            return true;
        }

        private bool SaveAceObjectPropertiesBigInt(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesInt64> properties)
        {
            // calling ToList forces the enumerable evaluation so that we can call .Remove from within the loop
            var theDirtyOnes = properties.Where(p => p.IsDirty).ToList();
            theDirtyOnes.ForEach(p => p.AceObjectId = aceObjectId);

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    if (prop.PropertyValue == null)
                    {
                        // delete it
                        transaction.AddPreparedDeleteStatement<AceObjectPreparedStatement, AceObjectPropertiesInt64>(AceObjectPreparedStatement.DeleteAceObjectPropertyBigInt, prop);
                        properties.Remove(prop);
                    }
                    else
                    {
                        // update it
                        transaction.AddPreparedUpdateStatement<AceObjectPreparedStatement, AceObjectPropertiesInt64>(AceObjectPreparedStatement.UpdateAceObjectPropertyBigInt, prop);
                    }
                }
                else if (prop.PropertyValue != null)
                {
                    transaction.AddPreparedInsertStatement<AceObjectPreparedStatement, AceObjectPropertiesInt64>(AceObjectPreparedStatement.InsertAceObjectPropertiesBigInt, prop);
                }
            }

            return true;
        }

        private bool SaveAceObjectPropertiesBool(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesBool> properties)
        {
            // calling ToList forces the enumerable evaluation so that we can call .Remove from within the loop
            var theDirtyOnes = properties.Where(p => p.IsDirty).ToList();
            theDirtyOnes.ForEach(p => p.AceObjectId = aceObjectId);

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    if (prop.PropertyValue == null)
                    {
                        // delete it
                        transaction.AddPreparedDeleteStatement<AceObjectPreparedStatement, AceObjectPropertiesBool>(AceObjectPreparedStatement.DeleteAceObjectPropertyBool, prop);
                        properties.Remove(prop);
                    }
                    else
                    {
                        // update it
                        transaction.AddPreparedUpdateStatement<AceObjectPreparedStatement, AceObjectPropertiesBool>(AceObjectPreparedStatement.UpdateAceObjectPropertyBool, prop);
                    }
                }
                else if (prop.PropertyValue != null)
                {
                    transaction.AddPreparedInsertStatement<AceObjectPreparedStatement, AceObjectPropertiesBool>(AceObjectPreparedStatement.InsertAceObjectPropertiesBool, prop);
                }
            }

            return true;
        }

        private bool SaveAceObjectPropertiesDouble(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesDouble> properties)
        {
            // calling ToList forces the enumerable evaluation so that we can call .Remove from within the loop
            var theDirtyOnes = properties.Where(p => p.IsDirty).ToList();
            theDirtyOnes.ForEach(p => p.AceObjectId = aceObjectId);

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    if (prop.PropertyValue == null)
                    {
                        // delete it
                        transaction.AddPreparedDeleteStatement<AceObjectPreparedStatement, AceObjectPropertiesDouble>(AceObjectPreparedStatement.DeleteAceObjectPropertyDouble, prop);
                        properties.Remove(prop);
                    }
                    else
                    {
                        // update it
                        transaction.AddPreparedUpdateStatement<AceObjectPreparedStatement, AceObjectPropertiesDouble>(AceObjectPreparedStatement.UpdateAceObjectPropertyDouble, prop);
                    }
                }
                else if (prop.PropertyValue != null)
                {
                    transaction.AddPreparedInsertStatement<AceObjectPreparedStatement, AceObjectPropertiesDouble>(AceObjectPreparedStatement.InsertAceObjectPropertiesDouble, prop);
                }
            }

            return true;
        }

        private bool SaveAceObjectPropertiesString(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesString> properties)
        {
            // calling ToList forces the enumerable evaluation so that we can call .Remove from within the loop
            var theDirtyOnes = properties.Where(p => p.IsDirty).ToList();
            theDirtyOnes.ForEach(p => p.AceObjectId = aceObjectId);

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    if (string.IsNullOrWhiteSpace(prop.PropertyValue))
                    {
                        // delete it
                        transaction.AddPreparedDeleteStatement<AceObjectPreparedStatement, AceObjectPropertiesString>(AceObjectPreparedStatement.DeleteAceObjectPropertyString, prop);
                        properties.Remove(prop);
                    }
                    else
                    {
                        // update it
                        transaction.AddPreparedUpdateStatement<AceObjectPreparedStatement, AceObjectPropertiesString>(AceObjectPreparedStatement.UpdateAceObjectPropertyString, prop);
                    }
                }
                else if (!string.IsNullOrWhiteSpace(prop.PropertyValue))
                {
                    transaction.AddPreparedInsertStatement<AceObjectPreparedStatement, AceObjectPropertiesString>(AceObjectPreparedStatement.InsertAceObjectPropertiesString, prop);
                }
            }

            return true;
        }

        private bool SaveAceObjectPropertiesDid(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesDataId> properties)
        {
            // calling ToList forces the enumerable evaluation so that we can call .Remove from within the loop
            var theDirtyOnes = properties.Where(p => p.IsDirty).ToList();
            theDirtyOnes.ForEach(p => p.AceObjectId = aceObjectId);

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    if (prop.PropertyValue == null)
                    {
                        // delete it
                        transaction.AddPreparedDeleteStatement<AceObjectPreparedStatement, AceObjectPropertiesDataId>(AceObjectPreparedStatement.DeleteAceObjectPropertyDid, prop);
                        properties.Remove(prop);
                    }
                    else
                    {
                        // update it
                        transaction.AddPreparedUpdateStatement<AceObjectPreparedStatement, AceObjectPropertiesDataId>(AceObjectPreparedStatement.UpdateAceObjectPropertyDid, prop);
                    }
                }
                else if (prop.PropertyValue != null)
                {
                    transaction.AddPreparedInsertStatement<AceObjectPreparedStatement, AceObjectPropertiesDataId>(AceObjectPreparedStatement.InsertAceObjectPropertiesDid, prop);
                }
            }

            return true;
        }

        private bool SaveAceObjectPropertiesIid(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesInstanceId> properties)
        {
            // calling ToList forces the enumerable evaluation so that we can call .Remove from within the loop
            var theDirtyOnes = properties.Where(p => p.IsDirty).ToList();
            theDirtyOnes.ForEach(p => p.AceObjectId = aceObjectId);

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    if (prop.PropertyValue == null)
                    {
                        // delete it
                        transaction.AddPreparedDeleteStatement<AceObjectPreparedStatement, AceObjectPropertiesInstanceId>(AceObjectPreparedStatement.DeleteAceObjectPropertyIid, prop);
                        properties.Remove(prop);
                    }
                    else
                    {
                        // update it
                        transaction.AddPreparedUpdateStatement<AceObjectPreparedStatement, AceObjectPropertiesInstanceId>(AceObjectPreparedStatement.UpdateAceObjectPropertyIid, prop);
                    }
                }
                else if (prop.PropertyValue != null)
                {
                    transaction.AddPreparedInsertStatement<AceObjectPreparedStatement, AceObjectPropertiesInstanceId>(AceObjectPreparedStatement.InsertAceObjectPropertiesIid, prop);
                }
            }

            return true;
        }

        private bool SaveAceObjectTextureMaps(DatabaseTransaction transaction, uint aceObjectId, List<TextureMapOverride> properties)
        {
            properties.ForEach(a => a.AceObjectId = aceObjectId);
            transaction.AddPreparedInsertListStatement<AceObjectPreparedStatement, TextureMapOverride>(AceObjectPreparedStatement.InsertTextureOverridesByObject, properties);
            return true;
        }

        private bool SaveAceObjectPropertiesSpells(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesSpell> properties)
        {
            properties.ForEach(a => a.AceObjectId = aceObjectId);
            transaction.AddPreparedInsertListStatement<AceObjectPreparedStatement, AceObjectPropertiesSpell>(AceObjectPreparedStatement.InsertAceObjectPropertiesSpells, properties);
            return true;
        }
        
        private bool SaveAceObjectPalettes(DatabaseTransaction transaction, uint aceObjectId, List<PaletteOverride> properties)
        {
            properties.ForEach(a => a.AceObjectId = aceObjectId);
            transaction.AddPreparedInsertListStatement<AceObjectPreparedStatement, PaletteOverride>(AceObjectPreparedStatement.InsertPaletteOverridesByObject, properties);
            return true;
        }

        private bool SaveAceObjectAnimations(DatabaseTransaction transaction, uint aceObjectId, List<AnimationOverride> properties)
        {
            properties.ForEach(a => a.AceObjectId = aceObjectId);
            transaction.AddPreparedInsertListStatement<AceObjectPreparedStatement, AnimationOverride>(AceObjectPreparedStatement.InsertAnimationOverridesByObject, properties);
            return true;
        }

        private bool SaveAceObjectPropertiesPositions(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesPosition> properties)
        {
            properties.ForEach(p => p.AceObjectId = aceObjectId);
            transaction.AddPreparedInsertListStatement<AceObjectPreparedStatement, AceObjectPropertiesPosition>(AceObjectPreparedStatement.InsertAceObjectPropertiesPositions, properties);
            return true;
        }

        private bool SaveAceObjectPropertiesAttributes(DatabaseTransaction transaction, uint aceObjectId, Dictionary<Ability, CreatureAbility> attributes)
        {
            var attribs = attributes.Values.Select(x => x.GetAttribute()).ToList();

            // setting AceObjectId doesn't trigger dirty
            attribs.ForEach(a => a.AceObjectId = aceObjectId);

            var theDirtyOnes = attribs.Where(x => x.IsDirty).ToList();

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    // update it
                    transaction.AddPreparedUpdateStatement<AceObjectPreparedStatement, AceObjectPropertiesAttribute>(AceObjectPreparedStatement.UpdateAceObjectPropertyAttribute, prop);
                }
                else
                {
                    transaction.AddPreparedInsertStatement<AceObjectPreparedStatement, AceObjectPropertiesAttribute>(AceObjectPreparedStatement.InsertAceObjectPropertiesAttributes, prop);
                }
            }

            return true;
        }

        private bool SaveAceObjectPropertiesAttribute2nd(DatabaseTransaction transaction, uint aceObjectId, Dictionary<Ability, CreatureVital> properties)
        {
            var attribs = properties.Values.Select(x => x.GetVital()).ToList();

            // setting AceObjectId doesn't trigger dirty
            attribs.ForEach(a => a.AceObjectId = aceObjectId);

            var theDirtyOnes = attribs.Where(x => x.IsDirty).ToList();

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    // update it
                    transaction.AddPreparedUpdateStatement<AceObjectPreparedStatement, AceObjectPropertiesAttribute2nd>(AceObjectPreparedStatement.UpdateAceObjectPropertyAttribute2nd, prop);
                }
                else
                {
                    transaction.AddPreparedInsertStatement<AceObjectPreparedStatement, AceObjectPropertiesAttribute2nd>(AceObjectPreparedStatement.InsertAceObjectPropertiesAttributes2nd, prop);
                }
            }

            return true;
        }

        // SaveAceObjectPropertiesSkill(transaction, aceObject.AceObjectPropertiesSkills);
        private bool SaveAceObjectPropertiesSkill(DatabaseTransaction transaction, uint aceObjectId, Dictionary<Skill, CreatureSkill> skills)
        {
            var tempSkills = skills.Values.Select(x => x.GetAceObjectSkill()).ToList();

            // setting AceObjectId doesn't trigger dirty
            tempSkills.ForEach(a => a.AceObjectId = aceObjectId);

            // calling ToList forces the enumerable evaluation so that we can call .Remove from within the loop
            var theDirtyOnes = tempSkills.Where(x => x.IsDirty).ToList();

            foreach (var prop in theDirtyOnes)
            {
                if (prop.HasEverBeenSavedToDatabase)
                {
                    if (prop.SkillStatus == (ushort)SkillStatus.Untrained)
                    {
                        // delete it.  this is possibly (likely?) a no-op
                        transaction.AddPreparedDeleteStatement<AceObjectPreparedStatement, AceObjectPropertiesSkill>(AceObjectPreparedStatement.DeleteAceObjectPropertiesSkills, prop);
                    }
                    else
                    {
                        // update it
                        transaction.AddPreparedUpdateStatement<AceObjectPreparedStatement, AceObjectPropertiesSkill>(AceObjectPreparedStatement.UpdateAceObjectPropertySkill, prop);
                    }
                }
                else
                {
                    transaction.AddPreparedInsertStatement<AceObjectPreparedStatement, AceObjectPropertiesSkill>(AceObjectPreparedStatement.InsertAceObjectPropertiesSkills, prop);
                }
            }

            return true;
        }

        private bool SaveAceObjectPropertiesBook(DatabaseTransaction transaction, uint aceObjectId, Dictionary<uint, AceObjectPropertiesBook> properties)
        {
            var pages = properties.Values.ToList();
            pages.ForEach(a => a.AceObjectId = aceObjectId);

            foreach (var page in pages)
                transaction.AddPreparedInsertStatement<AceObjectPreparedStatement, AceObjectPropertiesBook>(AceObjectPreparedStatement.InsertAceObjectPropertiesBook, page);
            return true;
        }
        
        private bool DeleteAceObjectBase(DatabaseTransaction transaction, AceObject obj)
        {
            transaction.AddPreparedDeleteStatement<AceObjectPreparedStatement, AceObject>(AceObjectPreparedStatement.DeleteAceObject, obj);
            return true;
        }

        // FIXME(ddevec): These are a lot of functions that essentially do the same thing... but the SharedPreparedStatement.--- makes them a pain to tempalte/reduce
        private bool DeleteAceObjectPropertiesInt(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesInt> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<AceObjectPreparedStatement, AceObjectPropertiesInt>(AceObjectPreparedStatement.DeleteAceObjectPropertiesInt, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesSpells(DatabaseTransaction transaction, uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<AceObjectPreparedStatement, AceObjectPropertiesSpell>(AceObjectPreparedStatement.DeleteAceObjectPropertiesSpell, criteria);
            return true;
        }
        
        private bool DeleteAceObjectPropertiesBigInt(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesInt64> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<AceObjectPreparedStatement, AceObjectPropertiesInt64>(AceObjectPreparedStatement.DeleteAceObjectPropertiesBigInt, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesBool(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesBool> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<AceObjectPreparedStatement, AceObjectPropertiesBool>(AceObjectPreparedStatement.DeleteAceObjectPropertiesBool, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesDouble(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesDouble> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<AceObjectPreparedStatement, AceObjectPropertiesDouble>(AceObjectPreparedStatement.DeleteAceObjectPropertiesDouble, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesString(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesString> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<AceObjectPreparedStatement, AceObjectPropertiesString>(AceObjectPreparedStatement.DeleteAceObjectPropertiesString, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesDid(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesDataId> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<AceObjectPreparedStatement, AceObjectPropertiesDataId>(AceObjectPreparedStatement.DeleteAceObjectPropertiesDid, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesIid(DatabaseTransaction transaction, uint aceObjectId, List<AceObjectPropertiesInstanceId> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<AceObjectPreparedStatement, AceObjectPropertiesInstanceId>(AceObjectPreparedStatement.DeleteAceObjectPropertiesIid, critera);
            return true;
        }

        private bool DeleteAceObjectTextureMaps(DatabaseTransaction transaction, uint aceObjectId, List<TextureMapOverride> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<AceObjectPreparedStatement, TextureMapOverride>(AceObjectPreparedStatement.DeleteTextureOverridesByObject, critera);
            return true;
        }

        private bool DeleteAceObjectPalettes(DatabaseTransaction transaction, uint aceObjectId, List<PaletteOverride> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<AceObjectPreparedStatement, PaletteOverride>(AceObjectPreparedStatement.DeletePaletteOverridesByObject, critera);
            return true;
        }

        private bool DeleteAceObjectAnimations(DatabaseTransaction transaction, uint aceObjectId, List<AnimationOverride> properties)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<AceObjectPreparedStatement, AnimationOverride>(AceObjectPreparedStatement.DeleteAnimationOverridesByObject, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesPositions(DatabaseTransaction transaction, uint aceObjectId)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<AceObjectPreparedStatement, AceObjectPropertiesPosition>(AceObjectPreparedStatement.DeleteAceObjectPropertiesPositions, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesAttributes(DatabaseTransaction transaction, uint aceObjectId)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<AceObjectPreparedStatement, AceObjectPropertiesAttribute>(AceObjectPreparedStatement.DeleteAceObjectPropertiesAttributes, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesAttribute2nd(DatabaseTransaction transaction, uint aceObjectId)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<AceObjectPreparedStatement, AceObjectPropertiesAttribute2nd>(AceObjectPreparedStatement.DeleteAceObjectPropertiesAttributes2nd, critera);
            return true;
        }
        
        private bool DeleteAceObjectPropertiesSkill(DatabaseTransaction transaction, uint aceObjectId)
        {
            var critera = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<AceObjectPreparedStatement, AceObjectPropertiesSkill>(AceObjectPreparedStatement.DeleteAceObjectPropertiesSkills, critera);
            return true;
        }

        private bool DeleteAceObjectPropertiesBook(DatabaseTransaction transaction, uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            transaction.AddPreparedDeleteListStatement<AceObjectPreparedStatement, AceObjectPropertiesBook>(AceObjectPreparedStatement.DeleteAceObjectPropertiesBook, criteria);
            return true;
        }

        public abstract List<AceObject> GetObjectsByLandblock(ushort landblock);
    }
}
