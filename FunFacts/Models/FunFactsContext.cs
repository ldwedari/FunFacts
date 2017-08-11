using System.Data.Entity;

namespace FunFacts.Models
{
    /// <summary>
    /// Entity Framework custom DbContext. There is no interface for this class.
    /// Creating an interface would require to have DbContext composed. It is an extra
    /// complication that would not produce a big gain.
    /// </summary>
    public class FunFactsContext : DbContext
    {
        //public FunFactsContext() : base("name=FunFactsContext")
        public FunFactsContext() : base("name=FunFactsAzureContext")
        {
        }

        public virtual DbSet<FunFacts.Models.ChuckNorrisFunFact> ChuckNorrisFunFacts { get; set; }

        /// <summary>
        /// Retrieves an EntityEntry and sets it State to Modified.
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <returns></returns>
        public virtual TModel ModifiedEntity<TModel>(TModel model) where TModel : class
        {
            var entry = Entry(model);
            entry.State = EntityState.Modified;
            return entry.Entity;
        }
    }
}
