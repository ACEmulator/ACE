using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using MySql.Data.MySqlClient;
using ACE.Entity;
using ACE.Entity.Enum;
using System.Diagnostics;

namespace ACE.Database
{
    public class WorldDatabase : CommonDatabase, IWorldDatabase
    {
        private enum WorldPreparedStatement
        {
            GetPointsOfInterest,
            GetWeenieClass,
            GetObjectsByLandblock,
            GetCreaturesByLandblock,
            GetCreatureDataByWeenie,
            InsertCreatureStaticLocation,
            GetCreatureGeneratorByLandblock,
            GetCreatureGeneratorData,
            GetPortalObjectsByAceObjectId,
            GetItemsByTypeId,
            GetAceObject,
            GetAceObjectGeneratorLinks,
            GetAceObjectInventory,
            GetMaxId,

            GetWeenieInstancesByLandblock,            

            GetAllRecipes,
            CreateRecipe,
            UpdateRecipe,
            DeleteRecipe,

            GetAllContent,
            GetContent,
            CreateContent,
            UpdateContent,
            DeleteContent,

            GetContentWeenies,
            CreateContentWeenie,
            UpdateContentWeenie,
            DeleteContentWeenie,

            GetContentLandblocks,
            CreateContentLandblock,
            UpdateContentLandblock,
            DeleteContentLandblock,

            GetAssociatedContent,
            CreateAssociatedContent,
            DeleteAssociatedContent,

            GetContentResources,
            CreateContentResource,
            UpdateContentResource,
            DeleteContentResource,

            CreateWeenie,
            UpdateWeenie
        }
        
        private void ConstructMaxQueryStatement(WorldPreparedStatement id, string tableName, string columnName)
        {
            // NOTE: when moved to WordDatabase, ace_shard needs to be changed to ace_world
            AddPreparedStatement(id, $"SELECT MAX(`{columnName}`) FROM `{tableName}` WHERE `{columnName}` >= ? && `{columnName}` < ?",
                MySqlDbType.UInt32, MySqlDbType.UInt32);
        }

        protected override void InitializePreparedStatements()
        {
            base.InitializePreparedStatements();

            ConstructStatement(WorldPreparedStatement.GetPointsOfInterest, typeof(TeleportLocation), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetWeenieClass, typeof(AceObject), ConstructedStatementType.Get);
            HashSet<string> criteria1 = new HashSet<string> { "itemType" };
            ConstructGetListStatement(WorldPreparedStatement.GetItemsByTypeId, typeof(CachedWeenieClass), criteria1);
            HashSet<string> criteria2 = new HashSet<string> { "landblock" };
            ConstructGetListStatement(WorldPreparedStatement.GetObjectsByLandblock, typeof(CachedWorldObject), criteria2);
            
            ConstructStatement(WorldPreparedStatement.GetAceObjectGeneratorLinks, typeof(AceObjectGeneratorLink), ConstructedStatementType.GetList);

            ConstructStatement(WorldPreparedStatement.GetAceObjectInventory, typeof(AceObjectInventory), ConstructedStatementType.GetList);

            ConstructStatement(WorldPreparedStatement.GetAceObject, typeof(AceObject), ConstructedStatementType.Get);

            ConstructMaxQueryStatement(WorldPreparedStatement.GetMaxId, "ace_object", "aceObjectId");

            ConstructGetListStatement(WorldPreparedStatement.GetWeenieInstancesByLandblock, typeof(WeenieObjectInstance), criteria2);

            // recipes
            ConstructStatement(WorldPreparedStatement.GetAllRecipes, typeof(Recipe), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.CreateRecipe, typeof(Recipe), ConstructedStatementType.Insert);
            ConstructStatement(WorldPreparedStatement.UpdateRecipe, typeof(Recipe), ConstructedStatementType.Update);
            ConstructStatement(WorldPreparedStatement.DeleteRecipe, typeof(Recipe), ConstructedStatementType.Delete);

            // content
            ConstructStatement(WorldPreparedStatement.GetAllContent, typeof(Content), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.GetContent, typeof(Content), ConstructedStatementType.Get);
            ConstructStatement(WorldPreparedStatement.CreateContent, typeof(Content), ConstructedStatementType.Insert);
            ConstructStatement(WorldPreparedStatement.UpdateContent, typeof(Content), ConstructedStatementType.Update);
            ConstructStatement(WorldPreparedStatement.DeleteContent, typeof(Content), ConstructedStatementType.Delete);

            ConstructStatement(WorldPreparedStatement.GetContentWeenies, typeof(ContentWeenie), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.CreateContentWeenie, typeof(ContentWeenie), ConstructedStatementType.Insert);
            ConstructStatement(WorldPreparedStatement.UpdateContentWeenie, typeof(ContentWeenie), ConstructedStatementType.Update);
            ConstructStatement(WorldPreparedStatement.DeleteContentWeenie, typeof(ContentWeenie), ConstructedStatementType.Delete);

            ConstructStatement(WorldPreparedStatement.GetContentLandblocks, typeof(ContentLandblock), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.CreateContentLandblock, typeof(ContentLandblock), ConstructedStatementType.Insert);
            ConstructStatement(WorldPreparedStatement.UpdateContentLandblock, typeof(ContentLandblock), ConstructedStatementType.Update);
            ConstructStatement(WorldPreparedStatement.DeleteContentLandblock, typeof(ContentLandblock), ConstructedStatementType.Delete);

            ConstructStatement(WorldPreparedStatement.GetAssociatedContent, typeof(ContentLink), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.CreateAssociatedContent, typeof(ContentLink), ConstructedStatementType.InsertList);
            ConstructStatement(WorldPreparedStatement.DeleteAssociatedContent, typeof(ContentLink), ConstructedStatementType.DeleteList);

            ConstructStatement(WorldPreparedStatement.GetContentResources, typeof(ContentResource), ConstructedStatementType.GetList);
            ConstructStatement(WorldPreparedStatement.CreateContentResource, typeof(ContentResource), ConstructedStatementType.Insert);
            ConstructStatement(WorldPreparedStatement.UpdateContentResource, typeof(ContentResource), ConstructedStatementType.Update);
            ConstructStatement(WorldPreparedStatement.DeleteContentResource, typeof(ContentResource), ConstructedStatementType.Delete);
        }

        /// <summary>
        /// does a full object replacement, deleting all properties prior to insertion
        /// </summary>
        public bool ReplaceObject(AceObject aceObject)
        {
            return SaveOrReplaceObject(aceObject, true);
        }

        public List<CachedWeenieClass> GetRandomWeeniesOfType(uint itemType, uint numWeenies)
        {
            var criteria = new Dictionary<string, object> { { "itemType", itemType } };
            var weenieList = ExecuteConstructedGetListStatement<WorldPreparedStatement, CachedWeenieClass>(WorldPreparedStatement.GetItemsByTypeId, criteria);
            if (weenieList.Count <= 0) return null;
            Random rnd = new Random();
            int r = rnd.Next(weenieList.Count);
            var randomWeenieList = new List<CachedWeenieClass>();
            for (int i = 0; i < numWeenies; i++)
            {
                randomWeenieList.Add(weenieList[r]);
                r = rnd.Next(weenieList.Count);
            }
            return randomWeenieList;
        }

        public override List<AceObject> GetObjectsByLandblock(ushort landblock)
        {
            var criteria = new Dictionary<string, object> { { "landblock", landblock } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, CachedWorldObject>(WorldPreparedStatement.GetObjectsByLandblock, criteria);
            return objects.Select(cwo => GetObject(cwo.AceObjectId)).ToList();
        }

        protected override void LoadIntoObject(AceObject aceObject)
        {
            base.LoadIntoObject(aceObject);
            aceObject.GeneratorLinks = GetAceObjectGeneratorLinks(aceObject.AceObjectId);
            aceObject.CreateList = GetAceObjectInventory(aceObject.AceObjectId);
        }

        public List<AceObject> GetWeenieInstancesByLandblock(ushort landblock)
        {
            var criteria = new Dictionary<string, object> { { "landblock", landblock } };
            var instances = ExecuteConstructedGetListStatement<WorldPreparedStatement, WeenieObjectInstance>(WorldPreparedStatement.GetWeenieInstancesByLandblock, criteria);
            List<AceObject> ret = new List<AceObject>();
            instances.ForEach(instance =>
            {
                // Create a new AceObject from Weenie.
                AceObject ao = GetObject(instance.WeenieClassId);

                // Set the object's current location for this instance.
                ao.Location = new Position(instance.LandblockRaw, instance.PositionX, instance.PositionY, instance.PositionZ, instance.RotationX, instance.RotationY, instance.RotationZ, instance.RotationW);

                // Use the guid recorded by the PCAP.
                // This step could eventually be removed if we want to let the GuidManager handle assigning guids for static objects, ignoring the recorded guids.
                string cmsClone = ao.CurrentMotionState; // Make a copy of the CurrentMotionState for cloning
                ao = (AceObject)ao.Clone(instance.PreassignedGuid); // Clone AceObject and assign the recorded Guid
                ao.CurrentMotionState = cmsClone; // Restore CurrentMotionState from original weenie

                ret.Add(ao);
            });
            return ret;
        }
        
        private List<AceObjectGeneratorLink> GetAceObjectGeneratorLinks(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectGeneratorLink>(WorldPreparedStatement.GetAceObjectGeneratorLinks, criteria);
            return objects;
        }

        private List<AceObjectInventory> GetAceObjectInventory(uint aceObjectId)
        {
            var criteria = new Dictionary<string, object> { { "aceObjectId", aceObjectId } };
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, AceObjectInventory>(WorldPreparedStatement.GetAceObjectInventory, criteria);
            return objects;
        }

        public AceObject GetAceObjectByWeenie(uint weenieClassId)
        {
            return GetObject(weenieClassId);
        }
        
        public List<TeleportLocation> GetPointsOfInterest()
        {
            Dictionary<string, object> criteria = new Dictionary<string, object>();
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, TeleportLocation>(WorldPreparedStatement.GetPointsOfInterest, criteria);
            return objects;
        }

        private uint GetMaxId(WorldPreparedStatement id, uint min, uint max)
        {
            object[] critera = new object[] { min, max };
            MySqlResult res = SelectPreparedStatement<WorldPreparedStatement>(id, critera);
            var ret = res.Rows[0][0];
            if (ret is DBNull)
            {
                return uint.MaxValue;
            }

            return (uint)res.Rows[0][0];
        }

        public uint GetCurrentId(uint min, uint max)
        {
            return GetMaxId(WorldPreparedStatement.GetMaxId, min, max);
        }

        public List<Recipe> GetAllRecipes()
        {
            var objects = ExecuteConstructedGetListStatement<WorldPreparedStatement, Recipe>(WorldPreparedStatement.GetAllRecipes, new Dictionary<string, object>());
            return objects;
        }

        public List<Content> GetAllContent()
        {
            var results = ExecuteConstructedGetListStatement<WorldPreparedStatement, Content>(WorldPreparedStatement.GetAllContent, new Dictionary<string, object>());

            results.ForEach(c =>
            {
                var criteria = new Dictionary<string, object>();
                criteria.Add("contentGuid", c.ContentGuid.Value.ToByteArray());  // used for ContentWeenie, ContentResource, and ContentLandblock
                criteria.Add("contentGuid1", c.ContentGuid.Value.ToByteArray()); // ContentLink uses contentGuid1
                c.AssociatedContent = ExecuteConstructedGetListStatement<WorldPreparedStatement, ContentLink>(WorldPreparedStatement.GetAssociatedContent, criteria);
                c.Weenies = ExecuteConstructedGetListStatement<WorldPreparedStatement, ContentWeenie>(WorldPreparedStatement.GetContentWeenies, criteria);
                c.ExternalResources = ExecuteConstructedGetListStatement<WorldPreparedStatement, ContentResource>(WorldPreparedStatement.GetContentResources, criteria);
                c.AssociatedLandblocks = ExecuteConstructedGetListStatement<WorldPreparedStatement, ContentLandblock>(WorldPreparedStatement.GetContentLandblocks, criteria);
            });

            results.ForEach(r => r.ClearDirtyFlags());
            return results;
        }

        public void CreateRecipe(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public void UpdateRecipe(Recipe recipe)
        {
            throw new NotImplementedException();
        }

        public void DeleteRecipe(Guid recipeGuid)
        {
            throw new NotImplementedException();
        }

        public void CreateContent(Content content)
        {
            DatabaseTransaction transaction = BeginTransaction();
            content.ContentGuid = content.ContentGuid ?? Guid.NewGuid();

            transaction.AddPreparedInsertStatement<WorldPreparedStatement, Content>(WorldPreparedStatement.CreateContent, content);

            // forcibly propagate the content id
            content.Weenies.ForEach(w => w.ContentGuid = content.ContentGuid.Value);
            content.ExternalResources.ForEach(r => r.ContentGuid = content.ContentGuid.Value);
            content.AssociatedLandblocks.ForEach(l => l.ContentGuid = content.ContentGuid.Value);
            content.AssociatedContent.ForEach(c => c.ContentGuid = content.ContentGuid.Value);

            content.Weenies.ForEach(w => transaction.AddPreparedInsertStatement(WorldPreparedStatement.CreateContentWeenie, w));
            content.ExternalResources.ForEach(r => transaction.AddPreparedInsertStatement(WorldPreparedStatement.CreateContentResource, r));
            content.AssociatedLandblocks.ForEach(l => transaction.AddPreparedInsertStatement(WorldPreparedStatement.CreateContentLandblock, l));
            transaction.AddPreparedInsertListStatement(WorldPreparedStatement.CreateAssociatedContent, content.AssociatedContent);

            transaction.Commit().Wait();

            content.ClearDirtyFlags();
        }

        public void UpdateContent(Content content)
        {
            if (content?.ContentGuid == null)
                throw new ArgumentNullException("content", "Cannot update null content or content with a null ContentGuid.");

            DatabaseTransaction transaction = BeginTransaction();

            transaction.AddPreparedUpdateStatement(WorldPreparedStatement.UpdateContent, content);

            // forcibly propagate the content id
            content.Weenies.ForEach(w => w.ContentGuid = content.ContentGuid.Value);
            content.ExternalResources.ForEach(r => r.ContentGuid = content.ContentGuid.Value);
            content.AssociatedLandblocks.ForEach(l => l.ContentGuid = content.ContentGuid.Value);
            content.AssociatedContent.ForEach(c => c.ContentGuid = content.ContentGuid.Value);

            content.Weenies.Where(o => o.IsDirty).ToList().ForEach(w => transaction.AddPreparedUpdateStatement(WorldPreparedStatement.UpdateContentWeenie, w));
            content.ExternalResources.Where(o => o.IsDirty).ToList().ForEach(r => transaction.AddPreparedUpdateStatement(WorldPreparedStatement.UpdateContentResource, r));
            content.AssociatedLandblocks.Where(o => o.IsDirty).ToList().ForEach(l => transaction.AddPreparedUpdateStatement(WorldPreparedStatement.UpdateContentLandblock, l));

            // content resources are weak entities that cannot be updated.  always delete and reinsert the list
            var criteria = new Dictionary<string, object>();
            criteria.Add("contentGuid1", content.ContentGuid.Value.ToByteArray());
            transaction.AddPreparedDeleteListStatement<WorldPreparedStatement, ContentLink>(WorldPreparedStatement.DeleteAssociatedContent, criteria);
            transaction.AddPreparedInsertListStatement(WorldPreparedStatement.DeleteAssociatedContent, content.AssociatedContent);

            transaction.Commit().Wait();

            content.ClearDirtyFlags();
        }

        public void DeleteContent(Guid contentGuid)
        {
            // content cascades in the database by design.  no need to force it here.

            var criteria = new Dictionary<string, object>();
            criteria.Add("contentGuid", contentGuid.ToByteArray());
            var result = ExecuteConstructedDeleteStatement(WorldPreparedStatement.DeleteContent, typeof(Content), criteria);
        }
        
        public List<WeenieSearchResult> SearchWeenies(SearchWeeniesCriteria criteria)
        {
            List<WeenieSearchResult> results = new List<WeenieSearchResult>();
            List<MySqlParameter> mysqlParams = new List<MySqlParameter>();

            var properties = GetPropertyCache(typeof(WeenieSearchResult));
            var dbTable = GetDbTableAttribute(typeof(WeenieSearchResult));
            string sql = "SELECT " + string.Join(", ", properties.Select(p => "`v`." + p.Item2.DbFieldName)) + " FROM " + dbTable.DbTableName + " `v` ";
            string where = null;

            if (criteria?.ContentGuid != null)
            {
                where = where != null ? where + " AND " : "";
                where += "`v`.aceObjectId IN (SELECT weenieId FROM ace_content_weenie WHERE contentGuid = ?)";
                var p = new MySqlParameter("", MySqlDbType.Binary);
                p.Value = criteria.ContentGuid.Value.ToByteArray();
                mysqlParams.Add(p);
            }

            if (criteria?.ItemType != null)
            {
                where = where != null ? where + " AND " : "";
                where += "`itemType` = ?";
                var p = new MySqlParameter("", MySqlDbType.Int32);
                p.Value = (int)criteria.ItemType.Value;
                mysqlParams.Add(p);
            }

            if (criteria?.WeenieType != null)
            {
                where = where != null ? where + " AND " : "";
                where += "`weenieType` = ?";
                var p = new MySqlParameter("", MySqlDbType.Int32);
                p.Value = (int)criteria.WeenieType.Value;
                mysqlParams.Add(p);
            }

            if (criteria?.WeenieClassId != null)
            {
                where = where != null ? where + " AND " : "";
                where += "`v`.aceObjectId = ?";
                var p = new MySqlParameter("", MySqlDbType.UInt32);
                p.Value = (uint)criteria.WeenieClassId.Value;
                mysqlParams.Add(p);
            }

            if (criteria?.UserModified != null)
            {
                where = where != null ? where + " AND " : "";
                where += "`v`.userModified = ?";
                var p = new MySqlParameter("", MySqlDbType.Bit);
                p.Value = criteria.UserModified.Value;
                mysqlParams.Add(p);
            }

            if (!string.IsNullOrWhiteSpace(criteria?.PartialName))
            {
                where = where != null ? where + " AND " : "";
                where += "`v`.`name` LIKE '%" + EscapeStringLiteral(criteria.PartialName) + "%'";
            }

            int index = 0;
            if (criteria?.PropertyCriteria != null)
            {
                foreach (var prop in criteria.PropertyCriteria)
                {
                    // this should be the 99% use case
                    switch (prop.PropertyType)
                    {
                        case AceObjectPropertyType.PropertyBool:
                            sql += $" INNER JOIN ace_object_properties_bool `prop{index}` ON `v`.`aceObjectId` = `prop{index}`.aceObjectId " +
                                                                                              $"AND `prop{index}`.boolPropertyId = {prop.PropertyId} " +
                                                                                              $"AND `prop{index}`.propertyValue = {bool.Parse(prop.PropertyValue)}";
                            break;
                        case AceObjectPropertyType.PropertyString:
                            sql += $" INNER JOIN ace_object_properties_string `prop{index}` ON `v`.`aceObjectId` = `prop{index}`.aceObjectId " +
                                                                                              $"AND `prop{index}`.strPropertyId = {prop.PropertyId} " +
                                                                                              $"AND `prop{index}`.propertyValue like '%{EscapeStringLiteral(prop.PropertyValue)}%'";
                            break;
                        case AceObjectPropertyType.PropertyDouble:
                            float fnum = float.Parse(prop.PropertyValue);
                            sql += $" INNER JOIN ace_object_properties_double `prop{index}` ON `v`.`aceObjectId` = `prop{index}`.aceObjectId " +
                                                                                              $"AND `prop{index}`.dblPropertyId = {prop.PropertyId} " +
                                                                                              $"AND `prop{index}`.propertyValue = {fnum}";
                            break;
                        case AceObjectPropertyType.PropertyDataId:
                            uint did = uint.Parse(prop.PropertyValue);
                            sql += $" INNER JOIN ace_object_properties_did `prop{index}` ON `v`.`aceObjectId` = `prop{index}`.aceObjectId " +
                                                                                              $"AND `prop{index}`.didPropertyId = {prop.PropertyId} " +
                                                                                              $"AND `prop{index}`.propertyValue = {did}";
                            break;
                        case AceObjectPropertyType.PropertyInstanceId:
                            uint iid = uint.Parse(prop.PropertyValue);
                            sql += $" INNER JOIN ace_object_properties_iid `prop{index}` ON `v`.`aceObjectId` = `prop{index}`.aceObjectId " +
                                                                                              $"AND `prop{index}`.iidPropertyId = {prop.PropertyId} " +
                                                                                              $"AND `prop{index}`.propertyValue = {iid}";
                            break;
                        case AceObjectPropertyType.PropertyInt:
                            uint id = uint.Parse(prop.PropertyValue);
                            sql += $" INNER JOIN ace_object_properties_int `prop{index}` ON `v`.`aceObjectId` = `prop{index}`.aceObjectId " +
                                                                                              $"AND `prop{index}`.intPropertyId = {prop.PropertyId} " +
                                                                                              $"AND `prop{index}`.propertyValue = {id}";
                            break;
                        case AceObjectPropertyType.PropertyInt64:
                            ulong id64 = ulong.Parse(prop.PropertyValue);
                            sql += $" INNER JOIN ace_object_properties_int `prop{index}` ON `v`.`aceObjectId` = `prop{index}`.aceObjectId " +
                                                                                              $"AND `prop{index}`.intPropertyId = {prop.PropertyId} " +
                                                                                              $"AND `prop{index}`.propertyValue = {id64}";
                            break;
                        case AceObjectPropertyType.PropertyBook:
                            // TODO: implement
                            break;

                        case AceObjectPropertyType.PropertyPosition:
                            // TODO: implement.  this will look up the static weenie mappings of where things are supposed to be spawned
                            break;

                        default:
                            break;
                    }

                    index++;
                }
            }

            // note, "sql" may have had additional joins done on it with criteria

            if (where != null)
                sql += " WHERE " + where;

            sql += " ORDER BY aceObjectId";
            
            using (var connection = new MySqlConnection(connectionString))
            {
                using (var command = new MySqlCommand(sql, connection))
                {
                    mysqlParams.ForEach(p => command.Parameters.Add(p));

                    connection.Open();
                    using (var commandReader = command.ExecuteReader(CommandBehavior.Default))
                    {
                        while (commandReader.Read())
                        {
                            results.Add(ReadObject<WeenieSearchResult>(commandReader));
                        }
                    }
                }
            }

            return results;
        }

        public bool UserModifiedFlagPresent()
        {
            // seach weenies or look for a single user mod flag ?
            SearchWeeniesCriteria criteria = new SearchWeeniesCriteria();
            criteria.UserModified = true;
            var result = SearchWeenies(criteria);
            if (result.Count > 0)
                return true;
            return false;
        }
    }
}
