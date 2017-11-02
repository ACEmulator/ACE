using ACE.Common;
using ACE.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ACE.Database.Tests
{
    [TestClass]
    public class ContentTests
    {
        private static WorldDatabase worldDb;

        [ClassInitialize]
        public static void TestSetup(TestContext context)
        {
            // copy config.json
            File.Copy(Path.Combine(Environment.CurrentDirectory, "..\\..\\..\\..\\ACE\\Config.json"), ".\\Config.json", true);

            ConfigManager.Initialize();
            worldDb = new WorldDatabase();
            worldDb.Initialize(ConfigManager.Config.MySql.World.Host,
                          ConfigManager.Config.MySql.World.Port,
                          ConfigManager.Config.MySql.World.Username,
                          ConfigManager.Config.MySql.World.Password,
                          ConfigManager.Config.MySql.World.Database);
        }
        
        [TestMethod]
        public void CreateContent_NoChildData_SavesContent()
        {
            Content c = new Content();
            c.ContentGuid = Guid.NewGuid();
            c.ContentName = "CreateContent_NoChildData_SavesContent " + c.ContentGuid.Value.ToString();
                
            worldDb.CreateContent(c);

            // clean up after ourselves
            worldDb.DeleteContent(c.ContentGuid.Value);
        }

        [TestMethod]
        public void UpdateContent_NoChildData_SavesContent()
        {
            Content c = new Content();
            c.ContentGuid = Guid.NewGuid();
            c.ContentName = "UpdateContent_NoChildData_SavesContent 1  " + c.ContentGuid.Value.ToString();

            worldDb.CreateContent(c);

            c.ContentName = "UpdateContent_NoChildData_SavesContent 2 " + c.ContentGuid.Value.ToString();

            worldDb.UpdateContent(c);

            var allContent = worldDb.GetAllContent();
            var cCopy = allContent.FirstOrDefault(x => x.ContentGuid == c.ContentGuid);

            Assert.IsNotNull(cCopy, "cCopy is null");
            Assert.AreEqual(c.ContentName, cCopy.ContentName, "contentName is incorrect after an update.");

            // clean up after ourselves
            worldDb.DeleteContent(c.ContentGuid.Value);
        }

        [TestMethod]
        public void CreateContent_WithWeenieData_SavesContent()
        {
            Content c = new Content();
            c.ContentGuid = Guid.NewGuid();
            c.ContentName = "CreateContent_WithWeenieData_SavesContent " + c.ContentGuid.Value.ToString();
            c.ContentType = Entity.Enum.ContentType.Patch;
            c.Weenies.Add(new ContentWeenie() { ContentWeenieGuid = Guid.NewGuid(), WeenieId = 6353, Comment = "Pyreal Mote" });
            c.Weenies.Add(new ContentWeenie() { ContentWeenieGuid = Guid.NewGuid(), WeenieId = 6354, Comment = "Pyreal Nugget" });
            worldDb.CreateContent(c);

            var allContent = worldDb.GetAllContent();
            var cCopy = allContent.FirstOrDefault(x => x.ContentGuid == c.ContentGuid);

            Assert.IsNotNull(cCopy, "cCopy is null");
            Assert.IsTrue(cCopy.Weenies.Count == 2, "cCopy.Weenies is missing");

            // clean up after ourselves
            worldDb.DeleteContent(c.ContentGuid.Value);
        }

        [TestMethod]
        public void CreateContent_WithLandblock_SavesContent()
        {
            Content c = new Content();
            c.ContentGuid = Guid.NewGuid();
            c.ContentName = "CreateContent_WithLandblock_SavesContent " + c.ContentGuid.Value.ToString();
            c.ContentType = Entity.Enum.ContentType.Patch;
            c.AssociatedLandblocks.Add(new ContentLandblock() { ContentLandblockGuid = Guid.NewGuid(), LandblockId = new LandblockId(5, 5), Comment = "a landblock" });
            worldDb.CreateContent(c);

            var allContent = worldDb.GetAllContent();
            var cCopy = allContent.FirstOrDefault(x => x.ContentGuid == c.ContentGuid);

            Assert.IsNotNull(cCopy, "cCopy is null");
            Assert.IsTrue(cCopy.AssociatedLandblocks.Count > 0, "cCopy.AssociatedLandblocks is missing");

            // clean up after ourselves
            worldDb.DeleteContent(c.ContentGuid.Value);
        }

        [TestMethod]
        public void CreateContent_WithAssociatedContent_SavesContent()
        {
            Content c1 = new Content();
            c1.ContentGuid = Guid.NewGuid();
            c1.ContentName = "CreateContent_WithAssociatedContent_SavesContent 1 " + c1.ContentGuid.Value.ToString();
            c1.ContentType = Entity.Enum.ContentType.Patch;
            worldDb.CreateContent(c1);

            Content c2 = new Content();
            c2.ContentGuid = Guid.NewGuid();
            c2.ContentName = "CreateContent_WithAssociatedContent_SavesContent 2 " + c2.ContentGuid.Value.ToString();
            c2.ContentType = Entity.Enum.ContentType.Quest;
            c2.AssociatedContent.Add(new ContentLink() { AssociatedContentGuid = c1.ContentGuid.Value });
            worldDb.CreateContent(c2);

            var allContent = worldDb.GetAllContent();
            var c1Copy = allContent.FirstOrDefault(c => c.ContentGuid == c1.ContentGuid);
            var c2Copy = allContent.FirstOrDefault(c => c.ContentGuid == c2.ContentGuid);

            Assert.IsNotNull(c1Copy, "c1Copy is null");
            Assert.IsNotNull(c2Copy, "c2Copy is null");
            Assert.IsTrue(c2Copy.AssociatedContent.Count > 0, "c2Copy.AssociatedContent is missing");

            // clean up after ourselves
            worldDb.DeleteContent(c1.ContentGuid.Value);
            worldDb.DeleteContent(c2.ContentGuid.Value);
        }

        [TestMethod]
        public void CreateContent_WithExternalResource_SavesContent()
        {
            Content c = new Content();
            c.ContentGuid = Guid.NewGuid();
            c.ContentName = "CreateContent_WithExternalResource_SavesContent " + c.ContentGuid.Value.ToString();
            c.ContentType = Entity.Enum.ContentType.Patch;
            c.ExternalResources.Add(new ContentResource() { ContentResourceGuid = Guid.NewGuid(), Name = "ACPedia link", ResourceUri = "http://acpedia.com/something" });
            worldDb.CreateContent(c);

            var allContent = worldDb.GetAllContent();
            var cCopy = allContent.FirstOrDefault(x => x.ContentGuid == c.ContentGuid);

            Assert.IsNotNull(cCopy, "cCopy is null");
            Assert.IsTrue(cCopy.ExternalResources.Count > 0, "cCopy.ExternalResources is missing");

            // clean up after ourselves
            // worldDb.DeleteContent(c.ContentGuid);
        }
    }
}
