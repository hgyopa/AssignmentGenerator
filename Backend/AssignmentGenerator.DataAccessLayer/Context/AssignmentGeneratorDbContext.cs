namespace AssignmentGenerator.DataAccessLayer.Context
{
    using System.Data.Entity;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Data.Entity.Validation;
    using Entities;
    using System.Linq;

    public class AssignmentGeneratorDbContext : DbContext
    {
        public AssignmentGeneratorDbContext() : base("name=AssignmentGenerator")
        {
            Configuration.ProxyCreationEnabled = false;
            Configuration.LazyLoadingEnabled = false;
        }

        public DbSet<Assignment> Assignments { get; set; }

        public DbSet<Question> Questions { get; set; }

        public DbSet<QuestionType> QuestionTypes { get; set; }

        public DbSet<Answer> Answers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }

        public override int SaveChanges()
        {
            try
            {
                var savedChangeCount = base.SaveChanges();

                return savedChangeCount;
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                    .SelectMany(x => x.ValidationErrors)
                    .Select(x => x.ErrorMessage);
                var fullErrorMessage = string.Join("; ", errorMessages);
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);
                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }
        }
    }
}
