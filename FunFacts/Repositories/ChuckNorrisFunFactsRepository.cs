using FunFacts.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace FunFacts.Repositories
{
    /// <summary>
    /// Business logic for Chuck Norris fun facts.
    /// </summary>
    public class ChuckNorrisFunFactsRepository : IFunFactsRepository
    {
        private Func<FunFactsContext> dbFactory;
        private Random random;

        public ChuckNorrisFunFactsRepository(Func<FunFactsContext> contextFactory)
        {
            dbFactory = contextFactory;
            random = new Random();
        }

        /// <summary>
        /// Get Fun fact by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IFunFact Get(long id)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException("Invalid id.");
            using (var db = dbFactory())
            {
                return dbFactory().ChuckNorrisFunFacts.Find(id);
            }
        }

        /// <summary>
        /// Get a ramdom fun fact from the database.
        /// </summary>
        /// <returns></returns>
        public IFunFact GetRandom()
        {
            using (var db = dbFactory())
            {
                int count = db.ChuckNorrisFunFacts.Count();
                int skip = random.Next(0, count);
                return db.ChuckNorrisFunFacts
                    .OrderBy(f => f.Id)
                    .Skip(skip)
                    .FirstOrDefault();
            }
        }

        /// <summary>
        /// Get the top count facts ordered by rating.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public IEnumerable<IFunFact> GetTop(int count)
        {
            if (count < 0)
                throw new ArgumentOutOfRangeException("Invalid count value.");
            using (var db = dbFactory())
            {
                return db.ChuckNorrisFunFacts.OrderByDescending(m => m.Rating).Take(count).ToList();
            }
        }

        /// <summary>
        /// Add a new fun fact to the database.
        /// </summary>
        /// <param name="fact"></param>
        /// <returns></returns>
        public IFunFact Add(IFunFact fact)
        {
            if (fact == null)
                throw new ArgumentNullException("Model is null.");
            using (var db = dbFactory())
            {
                fact.ModifiedWhen = DateTime.Now;
                fact.ModifiedBy = GetIdentity();
                var result = db.ChuckNorrisFunFacts.Add(fact as ChuckNorrisFunFact);
                db.SaveChanges();
                return result;
            }
        }

        private string GetIdentity()
        {
            var result = HttpContext.Current?.User?.Identity?.Name;
            if (string.IsNullOrEmpty(result))
            {
                result = "Anonymous";
            }
            return result;
        }

        /// <summary>
        /// Modify an existing fun fact 
        /// </summary>
        /// <param name="fact"></param>
        /// <returns></returns>
        public IFunFact Update(IFunFact fact)
        {
            if (fact == null)
                throw new ArgumentNullException("Model is null.");
            using (var db = dbFactory())
            {
                fact.ModifiedWhen = DateTime.Now;
                fact.ModifiedBy = GetIdentity();
                var entity = db.ModifiedEntity(fact as ChuckNorrisFunFact);
                db.SaveChanges();
                return entity;
            }
        }

        /// <summary>
        /// Delete an existing fun fact.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public bool Delete(long id)
        {
            if (id < 0)
                throw new ArgumentOutOfRangeException();
            using (var db = dbFactory())
            {
                var fact = db.ChuckNorrisFunFacts.Find(id);
                if (fact == null)
                    return false;
                db.ChuckNorrisFunFacts.Remove(fact);
                db.SaveChanges();
                return true;
            }
        }
    }
}
