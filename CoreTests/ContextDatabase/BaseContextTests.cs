using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Core.CustomAttribute;
using CoreTests.InitTests.ContextTest;
using CoreTests.InitTests.ModelsTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MongoDB.Driver;

namespace CoreTests.ContextDatabase
{
    [TestClass]
    public class ContextDatabaseTest
    {
        [TestInitialize]
        public void Init()
        {
            using var context = new ContextTest();
            context.DropDatabase();
        }

        [TestMethod]
        public void InsertOne_QueryCollection_GetEntity_Test()
        {
            using var context = new ContextTest();

            var allFamily = context.QueryCollection<Family>();
            Assert.AreEqual(0, allFamily.Count());

            var family1 = new Family
            {
                FamilyName = "Deligans"
            };
            context.Insert(family1);

            allFamily = context.QueryCollection<Family>();
            Assert.AreEqual(1, allFamily.Count());

            var getFamily = context.GetEntity<Family>(family1.Id);
            Assert.AreEqual(family1.Id, getFamily.Id);
            Assert.AreEqual(family1.FamilyName, getFamily.FamilyName);

        }

        [TestMethod]
        public void InsertAll_Test()
        {
            using var context = new ContextTest();

            var family1 = new Family
            {
                FamilyName = "Deligans"
            };
            context.Insert(family1);

            var listParent = new List<Parent>();

            var parent1 = new Parent
            {
                FamilyId = family1.Id,
                FirstName = family1.FamilyName,
                LastName = "Fabien",
            };

            var parent2 = new Parent
            {
                FamilyId = family1.Id,
                FirstName = family1.FamilyName,
                LastName = "Sandrine",
            };

            listParent.AddRange(new[] { parent1, parent2 });
            context.InsertAll(listParent);

            var listParentDatabase = context.QueryCollection<Parent>();
            Assert.AreEqual(2, listParentDatabase.Count());
        }

        [TestMethod]
        public void Update_Test()
        {
            using var context = new ContextTest();

            var family1 = new Family
            {
                FamilyName = "Deligans"
            };
            context.Insert(family1);

            var listParent = new List<Parent>();

            var parent1 = new Parent
            {
                FamilyId = family1.Id,
                FirstName = family1.FamilyName,
                LastName = "Fabien",
            };

            var parent2 = new Parent
            {
                FamilyId = family1.Id,
                FirstName = family1.FamilyName,
                LastName = "Sandrine",
            };

            listParent.AddRange(new[] { parent1, parent2 });
            context.InsertAll(listParent);

            var newFirstName = "Paulat";
            context.UpdateProperty(parent2, nameof(Parent.FirstName), newFirstName);

            var newParent2 = context.GetEntity<Parent>(parent2.Id);
            Assert.AreEqual(newFirstName, newParent2.FirstName);
        }

        [TestMethod]
        public void UpdateEntity_Test()
        {
            using var context = new ContextTest();

            var family1 = new Family
            {
                FamilyName = "Deligans"
            };
            context.Insert(family1);

            var listParent = new List<Parent>();

            var parent1 = new Parent
            {
                FamilyId = family1.Id,
                FirstName = family1.FamilyName,
                LastName = "Fabien",
            };

            var parent2 = new Parent
            {
                FamilyId = family1.Id,
                FirstName = family1.FamilyName,
                LastName = "Sandrine",
            };

            listParent.AddRange(new[] { parent1, parent2 });
            context.InsertAll(listParent);

            parent2.FirstName = "Paulat";
            context.UpdateEntity(parent2);

            var newParent2 = context.GetEntity<Parent>(parent2.Id);
            Assert.AreEqual(parent2.FirstName, newParent2.FirstName);
        }

        [TestMethod]
        public void RemoveOne_Test()
        {
            using var context = new ContextTest();

            var family1 = new Family
            {
                FamilyName = "Deligans"
            };
            context.Insert(family1);

            var listParent = new List<Parent>();

            var parent1 = new Parent
            {
                FamilyId = family1.Id,
                FirstName = family1.FamilyName,
                LastName = "Fabien",
            };

            var parent2 = new Parent
            {
                FamilyId = family1.Id,
                FirstName = family1.FamilyName,
                LastName = "Sandrine",
            };

            listParent.AddRange(new[] { parent1, parent2 });
            context.InsertAll(listParent);

            var listParentDatabase = context.QueryCollection<Parent>();
            Assert.AreEqual(2, listParentDatabase.Count());

            context.RemoveOne<Parent>(v => v.Id == parent2.Id);

            listParentDatabase = context.QueryCollection<Parent>();
            Assert.AreEqual(1, listParentDatabase.Count());
            Assert.AreEqual(parent1.Id, listParentDatabase.FirstOrDefault(v => v.Id == parent1.Id)?.Id);
        }

        [TestMethod]
        public void RemoveAll_Test()
        {
            using var context = new ContextTest();

            var family1 = new Family
            {
                FamilyName = "Deligans"
            };
            context.Insert(family1);

            var listParent = new List<Parent>();

            var parent1 = new Parent
            {
                FamilyId = family1.Id,
                FirstName = family1.FamilyName,
                LastName = "Fabien",
            };

            var parent2 = new Parent
            {
                FamilyId = family1.Id,
                FirstName = family1.FamilyName,
                LastName = "Sandrine",
            };

            listParent.AddRange(new[] { parent1, parent2 });
            context.InsertAll(listParent);

            var listParentDatabase = context.QueryCollection<Parent>();
            Assert.AreEqual(2, listParentDatabase.Count());

            context.RemoveAll<Parent>(v => v.FamilyId == family1.Id);

            listParentDatabase = context.QueryCollection<Parent>();
            Assert.AreEqual(0, listParentDatabase.Count());
        }

        [TestMethod]
        public void GetEntityOfForeignKey()
        {
            using var context = new ContextTest();

            var family1 = new Family
            {
                FamilyName = "Deligans"
            };
            context.Insert(family1);

            var listParent = new List<Parent>();

            var parent1 = new Parent
            {
                FamilyId = family1.Id,
                FirstName = family1.FamilyName,
                LastName = "Fabien",
            };

            var parent2 = new Parent
            {
                FamilyId = family1.Id,
                FirstName = family1.FamilyName,
                LastName = "Sandrine",
            };

            listParent.AddRange(new[] { parent1, parent2 });
            context.InsertAll(listParent);

            var parentDatabase = context.GetEntity<Parent>(parent1.Id);

            var entityAll = context.GetEntityOfForeignKey(parentDatabase);

            Assert.AreEqual(1, entityAll.Count);
            Assert.AreEqual(family1.Id, entityAll.First().Value.Id);


        }
    }
}
