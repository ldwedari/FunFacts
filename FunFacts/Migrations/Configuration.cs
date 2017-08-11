namespace FunFacts.Migrations
{
    using FunFacts.Models;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<FunFacts.Models.FunFactsContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(FunFacts.Models.FunFactsContext context)
        {
            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            context.ChuckNorrisFunFacts.RemoveRange(context.ChuckNorrisFunFacts);
            context.ChuckNorrisFunFacts.AddOrUpdate(
              new ChuckNorrisFunFact()
                {
                    ModifiedBy = "Me",
                    ModifiedWhen = new DateTime(2017, 8, 1),
                    Fact = "Un dia Chuck Norris estaba comiendo carne de ternera y cansado de comer lo mismo dijo hágase un animal del cual yo podria alimentar sin volar de mis patadas giratorias... y al instante se le cayo un testiculo, y se hizo un huevo...¡Enigma del huevo y la gallina resuelto!",
                    Rating = 10
                },
                new ChuckNorrisFunFact()
                {
                    ModifiedBy = "Me",
                    ModifiedWhen = new DateTime(2017, 8, 2),
                    Fact = "A persar de lo que digan los gallinas...¡Chuck es bostero de corazón! y amigo del Rafa Di Zeo.",
                    Rating = 5
                },
                new ChuckNorrisFunFact()
                {
                    ModifiedBy = "Me",
                    ModifiedWhen = new DateTime(2017, 8, 3),
                    Fact = "Chuck norris puede matar dos tiros de un pájaro.",
                    Rating = 30
                }
            );
        }
    }
}
