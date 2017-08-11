using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using FunFacts.Controllers;
using Moq;
using FunFacts.Repositories;
using FunFacts.Models;
using System.Web.Script.Serialization;
using System.Web.Http.Results;
using System.Data.Entity.Infrastructure;

namespace FunFacts.Tests.Controllers
{
    [TestClass]
    public class ChuckNorrisFunFactsControllerTest
    {
        [TestMethod]
        public void GetRamdom_OK()
        {
            // Arrange
            var fact = new ChuckNorrisFunFact()
            {
                Id = 1,
                ModifiedBy = "Me",
                ModifiedWhen = new DateTime(1000),
                Fact = "Some Fact",
                Rating = 10
            };

            var repository = new Mock<IFunFactsRepository>();
            repository.Setup(ffr => ffr.GetRandom()).Returns(fact);
            ChuckNorrisFunFactsController controller = new ChuckNorrisFunFactsController(repository.Object);

            // Act
            var response = controller.GetRamdom();

            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<IFunFact>));
            var result = (response as OkNegotiatedContentResult<IFunFact>).Content;
            var serializer = new JavaScriptSerializer();

            Assert.AreEqual(serializer.Serialize(fact), serializer.Serialize(result));
        }

        [TestMethod]
        public void GetRamdom_NotFound()
        {
            // Arrange
            var repository = new Mock<IFunFactsRepository>();
            repository.Setup(ffr => ffr.GetRandom()).Returns<IFunFact>(null);
            ChuckNorrisFunFactsController controller = new ChuckNorrisFunFactsController(repository.Object);

            // Act
            var response = controller.GetRamdom();

            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }

        [TestMethod]
        public void GetTop_OK()
        {
            // Arrange
            var facts = new List<ChuckNorrisFunFact>() {
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
                    Rating = 3
                }
            };

            var repository = new Mock<IFunFactsRepository>();
            repository.Setup(ffr => ffr.GetTop(It.Is<int>(i => i  == 3))).Returns(facts.AsQueryable());
            ChuckNorrisFunFactsController controller = new ChuckNorrisFunFactsController(repository.Object);

            // Act
            var response = controller.GetTop(3);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<IEnumerable<IFunFact>>));
            var result = (response as OkNegotiatedContentResult<IEnumerable<IFunFact>>).Content;
            var serializer = new JavaScriptSerializer();

            Assert.AreEqual(serializer.Serialize(facts), serializer.Serialize(result));
        }

        [TestMethod]
        public void GetTop_NegativeCount()
        {
            // Arrange
            var repository = new Mock<IFunFactsRepository>();
            ChuckNorrisFunFactsController controller = new ChuckNorrisFunFactsController(repository.Object);

            // Act
            var response = controller.GetTop(-1);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Put_OK()
        {
            // Arrange
            var fact = new ChuckNorrisFunFact()
            {
                Id = 1,
                Fact = "Some Fact",
                Rating = 10
            };

            var fact2 = new ChuckNorrisFunFact()
            {
                Id = 1,
                Fact = "Some Fact",
                ModifiedBy = "Me",
                ModifiedWhen = DateTime.Now,
                Rating = 10
            };

            var repository = new Mock<IFunFactsRepository>();
            repository.Setup(ffr => ffr.Update(It.Is<IFunFact>(i => i == fact))).Returns(fact2);
            ChuckNorrisFunFactsController controller = new ChuckNorrisFunFactsController(repository.Object);

            // Act
            var response = controller.Put(1, fact);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(OkNegotiatedContentResult<IFunFact>));
            var result = (response as OkNegotiatedContentResult<IFunFact>).Content;
            var serializer = new JavaScriptSerializer();

            Assert.AreEqual(fact2, result);
            repository.VerifyAll();
        }

        [TestMethod]
        public void Put_NotValid()
        {
            // Arrange
            var fact = new ChuckNorrisFunFact()
            {
                Id = 1,
                ModifiedBy = null,
                ModifiedWhen = new DateTime(1000),
                Fact = null,
                Rating = 10
            };

            var repository = new Mock<IFunFactsRepository>();
            ChuckNorrisFunFactsController controller = new ChuckNorrisFunFactsController(repository.Object);
            controller.ModelState.AddModelError("Fact", "test");

            // Act
            var response = controller.Put(1, fact);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public void Put_Null()
        {
            // Arrange

            var repository = new Mock<IFunFactsRepository>();
            ChuckNorrisFunFactsController controller = new ChuckNorrisFunFactsController(repository.Object);

            // Act
            var response = controller.Put(1, null);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(BadRequestErrorMessageResult));
        }

        [TestMethod]
        public void Put_Id()
        {
            // Arrange
            var fact = new ChuckNorrisFunFact()
            {
                Id = 1,
                ModifiedBy = null,
                ModifiedWhen = new DateTime(1000),
                Fact = null,
                Rating = 10
            };

            var repository = new Mock<IFunFactsRepository>();
            ChuckNorrisFunFactsController controller = new ChuckNorrisFunFactsController(repository.Object);

            // Act
            var response = controller.Put(2, fact);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(BadRequestResult));
        }

        [TestMethod]
        public void Put_ConcurrencyNotExist()
        {
            // Arrange
            var fact = new ChuckNorrisFunFact()
            {
                Id = 1,
                ModifiedBy = null,
                ModifiedWhen = new DateTime(1000),
                Fact = null,
                Rating = 10
            };

            var repository = new Mock<IFunFactsRepository>();
            repository.Setup(ffr => ffr.Update(It.Is<IFunFact>(i => i == fact))).Throws<DbUpdateConcurrencyException>();
            repository.Setup(ffr => ffr.Get(It.Is<long>(i => i == 1))).Returns<IFunFact>(null);
            ChuckNorrisFunFactsController controller = new ChuckNorrisFunFactsController(repository.Object);

            // Act
            var response = controller.Put(1, fact);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateConcurrencyException))]
        public void Put_ConcurrencyExist()
        {
            // Arrange
            var fact = new ChuckNorrisFunFact()
            {
                Id = 1,
                ModifiedBy = null,
                ModifiedWhen = new DateTime(1000),
                Fact = null,
                Rating = 10
            };

            var repository = new Mock<IFunFactsRepository>();
            repository.Setup(ffr => ffr.Update(It.Is<IFunFact>(i => i == fact))).Throws<DbUpdateConcurrencyException>();
            repository.Setup(ffr => ffr.Get(It.Is<long>(i => i == 1))).Returns(fact);
            ChuckNorrisFunFactsController controller = new ChuckNorrisFunFactsController(repository.Object);

            // Act
            var response = controller.Put(1, fact);

            // Assert
        }

        [TestMethod]
        public void Post_OK()
        {
            // Arrange
            var fact = new ChuckNorrisFunFact()
            {
                ModifiedBy = "Me",
                ModifiedWhen = new DateTime(1000),
                Fact = "Some Fact",
                Rating = 10
            };
            var fact2 = new ChuckNorrisFunFact()
            {
                Id = 2,
                ModifiedBy = fact.ModifiedBy,
                ModifiedWhen = fact.ModifiedWhen,
                Fact = fact.Fact,
                Rating = fact.Rating
            };

            var repository = new Mock<IFunFactsRepository>();
            repository.Setup(ffr => ffr.Add(It.Is<IFunFact>(i => i == fact)))
                .Callback<IFunFact>(f => f.Id = 2)
                .Returns(fact);
            ChuckNorrisFunFactsController controller = new ChuckNorrisFunFactsController(repository.Object);

            // Act
            var response = controller.Post(fact);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(CreatedAtRouteNegotiatedContentResult<ChuckNorrisFunFact>));
            var createdRoute = (response as CreatedAtRouteNegotiatedContentResult<ChuckNorrisFunFact>);
            var result = createdRoute.Content;
            Assert.AreEqual("DefaultApi", createdRoute.RouteName);
            Assert.AreEqual("id", createdRoute.RouteValues.First().Key);
            Assert.AreEqual(2, (long)createdRoute.RouteValues.First().Value);

            var serializer = new JavaScriptSerializer();

            Assert.AreEqual(serializer.Serialize(fact2), serializer.Serialize(result));
            repository.VerifyAll();
        }

        [TestMethod]
        public void Post_NotValid()
        {
            // Arrange
            var fact = new ChuckNorrisFunFact()
            {
                Id = 1,
                ModifiedBy = null,
                ModifiedWhen = new DateTime(1000),
                Fact = null,
                Rating = 10
            };

            var repository = new Mock<IFunFactsRepository>();
            ChuckNorrisFunFactsController controller = new ChuckNorrisFunFactsController(repository.Object);
            controller.ModelState.AddModelError("Fact", "test");

            // Act
            var response = controller.Post(fact);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(InvalidModelStateResult));
        }

        [TestMethod]
        public void Delete_OK()
        {
            // Arrange
            var repository = new Mock<IFunFactsRepository>();
            repository.Setup(ffr => ffr.Delete(It.Is<long>(i => i == 2))).Returns(true);
            ChuckNorrisFunFactsController controller = new ChuckNorrisFunFactsController(repository.Object);

            // Act
            var response = controller.Delete(2);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(OkResult));
            repository.VerifyAll();
        }

        [TestMethod]
        public void Delete_NotFound()
        {
            // Arrange
            var repository = new Mock<IFunFactsRepository>();
            repository.Setup(ffr => ffr.Delete(It.Is<long>(i => i == 2))).Returns(false);
            ChuckNorrisFunFactsController controller = new ChuckNorrisFunFactsController(repository.Object);

            // Act
            var response = controller.Delete(2);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
            repository.VerifyAll();
        }

        [TestMethod]
        public void Delete_Negative()
        {
            // Arrange
            var repository = new Mock<IFunFactsRepository>();
            var fact = new ChuckNorrisFunFact();
            repository.Setup(ffr => ffr.Get(It.Is<long>(i => i == 2))).Returns<IFunFact>(null);
            ChuckNorrisFunFactsController controller = new ChuckNorrisFunFactsController(repository.Object);

            // Act
            var response = controller.Delete(2);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsInstanceOfType(response, typeof(NotFoundResult));
        }
    }
}
