using FunFacts.Models;
using FunFacts.Repositories;

namespace FunFacts.Controllers
{
    /// <summary>
    /// This class provides the REST API for the Chuck Norris fun facts.
    /// </summary>
    public class ChuckNorrisFunFactsController : FunFactsGenericController<ChuckNorrisFunFact>
    {
        public ChuckNorrisFunFactsController(IFunFactsRepository repository) : base(repository)
        {
        }
    }
}