using FunFacts.Models;
using System.Collections.Generic;
using System.Linq;

namespace FunFacts.Repositories
{
    //public interface IFunFactsRepository<TModel> : IDisposable where TModel: class
    //{
    //    TModel Get(long id);
    //    TModel GetRandom();
    //    IQueryable<TModel> GetTop(int count);
    //    TModel Add(TModel model);
    //    void Delete(TModel model);
    //    void SaveChanges();
    //}

    public interface IFunFactsRepository
    {
        IFunFact Get(long id);
        IFunFact GetRandom();
        IEnumerable<IFunFact> GetTop(int count);
        IFunFact Add(IFunFact model);
        IFunFact Update(IFunFact model);
        bool Delete(long id);
    }
}
