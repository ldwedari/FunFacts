using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Web.Http;
using System.Web.Http.Description;
using FunFacts.Models;
using FunFacts.Repositories;
using System.Linq;

namespace FunFacts.Controllers
{
    public class FunFactsGenericController<TModel> : ApiController where TModel : IFunFact
    {
        protected IFunFactsRepository _repository;

        public FunFactsGenericController(IFunFactsRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// GET action to obtain a random fun fact.
        /// </summary>
        /// <returns>Ok wrapping the fun fact or NotFound when the database is empty.</returns>
        [HttpGet]
        [ResponseType(typeof(IFunFact))]
        public IHttpActionResult GetRamdom()
        {
            var fact = _repository.GetRandom();
            if (fact == null)
            {
                return NotFound();
            }

            return Ok(fact);
        }

        /// <summary>
        /// Get the top count rated Fun Facts. If the database contains less items than count then all the existing
        /// items are returned.
        /// </summary>
        /// <param name="count">Number of fun facts to retrieve. Must be greater or equal than 0.</param>
        /// <returns>Bad Request if count is invalid. Otherwise Ok with the list of Items.</returns>
        [HttpGet]
        [ResponseType(typeof(IEnumerable<IFunFact>))]
        public IHttpActionResult GetTop(int count)
        {
            if (count < 0)
                return BadRequest();

            var facts = _repository.GetTop(count);

            return Ok(facts);
        }

        /// <summary>
        /// Action to update an existing fun fact. ModifiedBy and ModifiedWhen are updated automatically.
        /// </summary>
        /// <param name="id">Id of the fun fact to moodify.</param>
        /// <param name="fact">New values of the fact.</param>
        /// <returns>Bad Request when the fact is null, does not comply the model annotations or id and fact.Id don't match.
        /// NotFound if the fact is not in the database. Otherwise Ok with the update fun fact.</returns>
        //[Authorize]
        [ResponseType(typeof(IFunFact))]
        public IHttpActionResult Put(long id, TModel fact)
        {
            if (fact == null)
                return BadRequest("Fact cannot be null.");

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (id != fact.Id)
            {
                return BadRequest();
            }

            IFunFact result;
            try
            {
                // Note: Update method is not idempotent
                result = _repository.Update(fact);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FunFactExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(result);
        }

        /// <summary>
        /// Action to add a new fun fact.
        /// </summary>
        /// <param name="funFact">Fun fact to add.</param>
        /// <returns>Bad request if the fun fact does not comply the model annotations. Otherwise the route to the new fact and the fact.</returns>
        //[Authorize]
        [ResponseType(typeof(IFunFact))]
        public IHttpActionResult Post([FromBody]TModel funFact)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _repository.Add(funFact);

            return CreatedAtRoute("DefaultApi", new { id = funFact.Id }, funFact);
        }

        /// <summary>
        /// Action to delete an existing fun fact.
        /// </summary>
        /// <param name="id">Id of the fun fact to delete.</param>
        /// <returns>Bad Request if the id is less than 0. NotFound if the fun fact does not exist. Otherwise Ok.</returns>
        //[Authorize]
        [ResponseType(typeof(void))]
        public IHttpActionResult Delete(long id)
        {
            if (id < 0)
                return BadRequest();

            if (!_repository.Delete(id))
                return NotFound();
            return Ok();
        }

        private bool FunFactExists(long id)
        {
            return _repository.Get(id) != null;
        }
    }
 }