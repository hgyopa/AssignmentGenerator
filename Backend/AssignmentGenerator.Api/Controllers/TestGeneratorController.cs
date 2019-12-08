namespace AssignmentGenerator.Api.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using DataAccessLayer.Context;
    using Extensions;
    using Microsoft.AspNet.Identity;
    using Models;
    using TestGenerator;
    using TestGenerator.Generators.InterrogativeWordQuestion;
    using TestGenerator.Generators.TrueOrFalseQuestion;
    using TextAnalyzer;
    using TextAnalyzer.Models;

    [RoutePrefix("api/TestGenerator")]
    public class TestGeneratorController : ApiController
    {
        private Analyzer analyzer;
        private Properties properties;
        private string jarRoot;
        private AssignmentGeneratorDbContext dbContext;

        public TestGeneratorController()
        {
            this.jarRoot = System.Configuration.ConfigurationManager.AppSettings["StanfordNLPCorePackagePath"];

            this.properties = new PropertyBuilder()
                .SetAnnotators(new[] { "tokenize", "ssplit", "pos", "lemma", "ner", "parse", "natlog", "dcoref", "truecase" })
                .SetKeyValuePairs("ner.useSUTime", "0")
                .SetKeyValuePairs("truecase.overwriteText", "true")
                .Build();

            this.dbContext = new AssignmentGeneratorDbContext();
        }

        [HttpPost]
        [Route("GenerateTest")]
        public async Task<IHttpActionResult> GenerateTest(AssignmentSource source)
        {
            return await Task.Run(() =>
            {
                var questionList = new List<QuestionDTO>();
                this.analyzer = new Analyzer(this.properties, this.jarRoot, source.Text);

                var sentenceList = this.analyzer.GetListOfSentences();
                var coreferenceList = this.analyzer.GetCoreferenceList();

                var normalizer = new Normalizer();
                var normalizedSentences = normalizer.NormalizeText2(sentenceList, coreferenceList);

                var interrogativeGenerator = new InterrogativeWordQuestionGenerator();
                questionList.AddRange(interrogativeGenerator.Generate(normalizedSentences).Select(question => Mappers.Mapper.MapFromTestGeneratorQuestionToDTO(question)).ToList());

                var substitutionQuestionGenerator = new SubstitutionQuestionGenerator();
                questionList.AddRange(substitutionQuestionGenerator.Generate(normalizedSentences).Select(question => Mappers.Mapper.MapFromTestGeneratorQuestionToDTO(question)).ToList());

                var trueOrFalseGenerator = new TrueOrFalseQuestionGenerator();
                questionList.AddRange(trueOrFalseGenerator.Generate(normalizedSentences).Select(question => Mappers.Mapper.MapFromTestGeneratorQuestionToDTO(question)).ToList());

                return Ok(new AssignmentDTO() {CreationDate = DateTime.Now, Id = 0, Questions = questionList, Title = source.Title, UserId = "1"});
            });
        }

        [HttpGet]
        [Route("GetAssignments")]
        public async Task<IHttpActionResult> GetAssignments()
        {
            var userId = User.Identity.GetUserId();
            var listOfAssignment = await this.dbContext.Assignments.Where(assignment => assignment.UserId == userId).ToListAsync();

            return Ok(listOfAssignment.Select(Mappers.Mapper.MapFromDALAssignmentToDTO).ToList());
        }

        [HttpGet]
        [Route("GetAssignment/{id}")]
        public async Task<IHttpActionResult> GetAssignment(int id)
        {
            var assignment = await this.dbContext.Assignments
                .Include(e => e.Questions.Select(question => question.Answers))
                .FirstOrDefaultAsync(test => test.Id == id);

            return Ok(Mappers.Mapper.MapFromDALAssignmentToDTO(assignment));
        }

        [HttpPost]
        [Route("CreateAssignment")]
        public async Task<IHttpActionResult> CreateAssignment(AssignmentDTO assignmentDto)
        {
            if (assignmentDto.Questions == null || !assignmentDto.Questions.Any())
            {
                throw new ArgumentException("Assignment must contain at least one question!");
            }

            var assignment = Mappers.Mapper.MapFromAssignmentDTOToDAL(assignmentDto);
            assignment.UserId = User.Identity.GetUserId();

            this.dbContext.Assignments.Add(assignment);
            await this.dbContext.SaveChangesAsync();

            assignmentDto = Mappers.Mapper.MapFromDALAssignmentToDTO(assignment);

            return Created("",assignmentDto);
        }

        [HttpPut]
        [Route("UpdateAssignment/{id}")]
        public async Task<IHttpActionResult> UpdateAssignment(int id, AssignmentDTO assignmentDto)
        {
            if (id != assignmentDto.Id)
            {
                throw new ArgumentException("Invalid id! Id in request url and id of the assignment don't match!");
            }

            if (assignmentDto.Questions == null || !assignmentDto.Questions.Any())
            {
                throw new ArgumentException("Assignment must contain at least one question!");
            }

            var assignment = Mappers.Mapper.MapFromAssignmentDTOToDAL(assignmentDto);

            var assignmentInDB = this.dbContext.Assignments
                .Include(e => e.Questions.Select(question => question.Answers))
                .FirstOrDefault(test => test.Id == assignmentDto.Id);

            var deletedQuestions = assignmentInDB.Questions.Except(assignment.Questions, a => a.Id).ToList();
            deletedQuestions.ForEach(question =>
            {
                this.dbContext.Answers.Remove(question.Answers.First());
                this.dbContext.Questions.Remove(question);
            });

            foreach (var question in assignment.Questions)
            {
                var existingQuestion = assignmentInDB.Questions.First(q => q.Id == question.Id);

                existingQuestion.Text = question.Text;
                existingQuestion.Answers.First().Text = question.Answers.First().Text;
            }

            await this.dbContext.SaveChangesAsync();

            assignmentInDB = this.dbContext.Assignments
                .Include(e => e.Questions.Select(question => question.Answers))
                .FirstOrDefault(test => test.Id == assignmentDto.Id);

            assignmentDto = Mappers.Mapper.MapFromDALAssignmentToDTO(assignmentInDB);

            return Ok(assignmentDto);
        }
    }
}
