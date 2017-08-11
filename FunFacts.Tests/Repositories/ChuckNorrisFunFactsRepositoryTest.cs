using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using FunFacts.Repositories;
using FunFacts.Models;
using System.Data.Entity;

namespace FunFacts.Tests.Repositories
{
    [TestClass]
    public class ChuckNorrisFunFactsRepositoryTest
    {
        private IQueryable<ChuckNorrisFunFact> facts;
        private Mock<DbSet<ChuckNorrisFunFact>> mockSet;
        private Mock<FunFactsContext> mockContext;
        private Func<FunFactsContext> contextFactory;

        public Func<FunFactsContext> ContextFactory { get => contextFactory; set => contextFactory = value; }

        private void SetupData()
        {
            facts = new List<ChuckNorrisFunFact>() {
                new ChuckNorrisFunFact()
                {
                    Id = 2,
                    ModifiedBy = "Me",
                    ModifiedWhen = new DateTime(1000),
                    Fact = "Some Fact3",
                    Rating = 10
                },
                new ChuckNorrisFunFact()
                {
                    Id = 1,
                    ModifiedBy = "Me",
                    ModifiedWhen = new DateTime(3000),
                    Fact = "Some Fact2",
                    Rating = 5
                },
                new ChuckNorrisFunFact()
                {
                    Id = 3,
                    ModifiedBy = "Me",
                    ModifiedWhen = new DateTime(2000),
                    Fact = "Some Fact1",
                    Rating = 30
                }
            }.AsQueryable();
        }

        private void SetupEmptyData()
        {
            facts = new List<ChuckNorrisFunFact>().AsQueryable();
        }

        private void SetupMocks()
        {
            mockSet = new Mock<DbSet<ChuckNorrisFunFact>>();
            mockSet.As<IQueryable<ChuckNorrisFunFact>>().Setup(m => m.Provider).Returns(facts.Provider);
            mockSet.As<IQueryable<ChuckNorrisFunFact>>().Setup(m => m.Expression).Returns(facts.Expression);
            mockSet.As<IQueryable<ChuckNorrisFunFact>>().Setup(m => m.ElementType).Returns(facts.ElementType);
            mockSet.As<IQueryable<ChuckNorrisFunFact>>().Setup(m => m.GetEnumerator()).Returns(facts.GetEnumerator());

            mockContext = new Mock<FunFactsContext>();
            mockContext.Setup(c => c.ChuckNorrisFunFacts).Returns(mockSet.Object);

            contextFactory = () =>
            {
                return mockContext.Object;
            };
        }

        [TestMethod]
        public void Get_OK()
        {
            // Arrange
            SetupData();
            SetupMocks();
            mockSet.Setup(m => m.Find(It.Is<long>(i => i == 1))).Returns(facts.Skip(1).First());
            var service = new ChuckNorrisFunFactsRepository(contextFactory);

            // Act
            var fact = service.Get(1);

            // Assert
            Assert.AreEqual(1, fact.Id);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Get_InvalidId()
        {
            // Arrange
            SetupData();
            SetupMocks();
            var service = new ChuckNorrisFunFactsRepository(contextFactory);

            // Act
            var fact = service.Get(-1);

            // Assert
        }

        [TestMethod]
        public void GetTop_Ok()
        {
            // Arrange
            SetupData();
            SetupMocks();
            var service = new ChuckNorrisFunFactsRepository(contextFactory);

            // Act
            var result = service.GetTop(2).ToList();

            // Assert
            Assert.AreEqual(3, result.Select(f=>f.Id).First());
            Assert.AreEqual(2, result.Select(f => f.Id).Last());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void GetTop_Negative()
        {
            // Arrange
            SetupEmptyData();
            SetupMocks();
            var service = new ChuckNorrisFunFactsRepository(contextFactory);

            // Act
            var result = service.GetTop(-1);
        }

        [TestMethod]
        public void Add_OK()
        {
            // Arrange
            SetupData();
            SetupMocks();
            var fact = new ChuckNorrisFunFact()
            {
                Id = 4,
                ModifiedBy = "Me",
                ModifiedWhen = new DateTime(4000),
                Fact = "Some Fact4",
                Rating = 23
            };
            mockSet.Setup(m => m.Add(It.Is<ChuckNorrisFunFact>(i => i == fact))).Returns(fact);
            var service = new ChuckNorrisFunFactsRepository(contextFactory);

            // Act
            var start = DateTime.Now;
            var result = service.Add(fact);
            var end = DateTime.Now;
            // Assert
            Assert.AreEqual(result, fact);
            Assert.IsTrue(start <= result.ModifiedWhen && end >= result.ModifiedWhen);
            mockContext.Verify(m => m.SaveChanges());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Add_Null()
        {
            // Arrange
            SetupEmptyData();
            SetupMocks();
            var service = new ChuckNorrisFunFactsRepository(contextFactory);

            // Act
            var result = service.Add(null);
        }

        [TestMethod]
        public void Update_OK()
        {
            // Arrange
            SetupData();
            SetupMocks();
            var fact = new ChuckNorrisFunFact()
            {
                Id = 2,
                ModifiedBy = "Me",
                ModifiedWhen = new DateTime(4000),
                Fact = "New Fact",
                Rating = 23
            };
            mockContext.Setup(c => c.ModifiedEntity(It.Is<ChuckNorrisFunFact>(i => i == fact))).Returns(fact);
            var service = new ChuckNorrisFunFactsRepository(contextFactory);
            // Act
            var start = DateTime.Now;
            var result = service.Update(fact);
            var end = DateTime.Now;

            // Assert
            Assert.AreEqual(result, fact);
            Assert.IsTrue(start <= result.ModifiedWhen && end >= result.ModifiedWhen);
            mockContext.Verify(m => m.SaveChanges());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Update_Null()
        {
            // Arrange
            SetupEmptyData();
            SetupMocks();
            var service = new ChuckNorrisFunFactsRepository(contextFactory);

            // Act
            var result = service.Update(null);
        }

        [TestMethod]
        public void Delete_OK()
        {
            // Arrange
            SetupData();
            SetupMocks();
            var fact = facts.First();
            mockSet.Setup(m => m.Find(It.Is<long>(i => i == fact.Id))).Returns(fact);
            var service = new ChuckNorrisFunFactsRepository(contextFactory);
            // Act
            var result = service.Delete(fact.Id);

            // Assert
            Assert.IsTrue(result);
            mockSet.Verify(m => m.Remove(It.Is<ChuckNorrisFunFact>(i => i == fact)));
            mockContext.Verify(m => m.SaveChanges());
        }

        [TestMethod]
        public void Delete_NotFound()
        {
            // Arrange
            SetupData();
            SetupMocks();
            long badId = 1234; ;
            mockSet.Setup(m => m.Find(It.Is<long>(i => i == badId))).Returns<ChuckNorrisFunFact>(null);
            var service = new ChuckNorrisFunFactsRepository(contextFactory);
            // Act
            var result = service.Delete(badId);

            // Assert
            Assert.IsFalse(result);
            mockSet.Verify(m => m.Remove(It.IsAny<ChuckNorrisFunFact>()), Times.Never());
            mockContext.Verify(m => m.SaveChanges(), Times.Never());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void Delete_Negative()
        {
            // Arrange
            SetupEmptyData();
            SetupMocks();
            var service = new ChuckNorrisFunFactsRepository(contextFactory);

            // Act
            service.Delete(-1);
        }
    }
}
