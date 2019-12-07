namespace AssignmentGenerator.Api.Controllers
{
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using DataAccessLayer.Context;
    using DataAccessLayer.Entities;
    using Microsoft.AspNet.Identity;
    using Models;
    using TestGenerator;
    using TestGenerator.Generators.InterrogativeWordQuestion;
    using TestGenerator.Generators.TrueOrFalseQuestion;
    using TextAnalyzer;
    using TextAnalyzer.Models;

    public class TestGeneratorController : ApiController
    {
        private Analyzer analyzer;
        private Properties properties;
        private string jarRoot;
        private AssignmentGeneratorDbContext dbContext;

        public TestGeneratorController()
        {
            this.jarRoot = @"D:\PRIVATE\Projects.\stanford-corenlp-full-2018-10-05";
            //var jarRoot = @"..\..\..\..\stanford-corenlp-full-2018-10-05";
            var text = "Wife of Enrique Iglesias is Anna. Her house is big.";

            this.properties = new PropertyBuilder()
                .SetAnnotators(new[] { "tokenize", "ssplit", "pos", "lemma", "ner", "parse", "natlog", "dcoref", "truecase" })
                .SetKeyValuePairs("ner.useSUTime", "0")
                .SetKeyValuePairs("truecase.overwriteText", "true")
                .Build();

            this.dbContext = new AssignmentGeneratorDbContext();
        }

        [HttpPost]
        public async Task<IHttpActionResult> GetTest(AssignmentSource source)
        {
            return await Task.Run(() =>
            {
                var questionList = new List<TestGenerator.Models.Question>();
                this.analyzer = new Analyzer(this.properties, this.jarRoot, source.Text);

                var sentenceList = this.analyzer.GetListOfSentences();
                var coreferenceList = this.analyzer.GetCoreferenceList();

                var normalizer = new Normalizer();
                var normalizedSentences = normalizer.NormalizeText2(sentenceList, coreferenceList);

                var interrogativeGenerator = new InterrogativeWordQuestionGenerator();
                questionList.AddRange(interrogativeGenerator.Generate(normalizedSentences));

                var generator = new SubstitutionQuestionGenerator();
                questionList.AddRange(generator.Generate(normalizedSentences));

                var trueOrFalseGenerator = new TrueOrFalseQuestionGenerator();
                questionList.AddRange(trueOrFalseGenerator.Generate(normalizedSentences));

                return Ok(questionList);
            });
        }

        [HttpGet]
        [Route("GetAssignments")]
        public async Task<IHttpActionResult> GetAssignments()
        {
            var userId = User.Identity.GetUserId();
            var listOfAssignment = await this.dbContext.Assignments.Where(assignment => assignment.UserId == userId).ToListAsync();

            return Ok(listOfAssignment);
        }

        [HttpGet]
        [Route("GetAssignment/{id}")]
        public async Task<IHttpActionResult> GetAssignment(int id)
        {
            var assignment = await this.dbContext.Assignments.Include(e => e.Questions.Select(question => question.Answers)).Where(test => test.Id == id).ToListAsync();

            return Ok(assignment);
        }

        [HttpPost]
        public async Task<IHttpActionResult> SaveAssignment(Assignment assignment)
        {
            assignment.UserId = User.Identity.GetUserId();

            this.dbContext.Assignments.Add(assignment);
            await this.dbContext.SaveChangesAsync();

            return CreatedAtRoute("GetAssignment/{id}", new {id = assignment.Id}, assignment);
        }

        [HttpPost]
        public async Task<IHttpActionResult> UpdateAssignment(Assignment assignment)
        {
            this.dbContext.Assignments.Attach(assignment);
            this.dbContext.Entry(assignment).State = EntityState.Modified;
            await this.dbContext.SaveChangesAsync();

            return Ok(assignment);
        }
    }
}
