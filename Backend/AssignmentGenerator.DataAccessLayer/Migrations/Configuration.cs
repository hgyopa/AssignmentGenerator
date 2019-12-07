namespace AssignmentGenerator.DataAccessLayer.Migrations
{
    using System.Collections.Generic;
    using System.Data.Entity.Migrations;
    using Entities;

    internal sealed class Configuration : DbMigrationsConfiguration<AssignmentGenerator.DataAccessLayer.Context.AssignmentGeneratorDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(AssignmentGenerator.DataAccessLayer.Context.AssignmentGeneratorDbContext context)
        {
            var questionTypeList = new List<QuestionType>
            {
                new QuestionType {Id = 1, Name = "Substitution"},
                new QuestionType {Id = 2, Name = "TrueOrFalse"},
                new QuestionType {Id = 3, Name = "InterrogativeWord"},
            };

            foreach (var questionType in questionTypeList)
            {
                if (context.QuestionTypes.Find(questionType.Id) == null)
                {
                    context.QuestionTypes.Add(questionType);
                }
            }
        }
    }
}
