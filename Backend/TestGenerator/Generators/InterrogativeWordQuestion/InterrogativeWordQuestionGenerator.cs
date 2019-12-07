namespace TestGenerator.Generators.InterrogativeWordQuestion
{
    using Common.Models;
    using Models;
    using System.Collections.Generic;

    public class InterrogativeWordQuestionGenerator : QuestionGenerator
    {
        private readonly WhoQuestionGenerator whoQuestionGenerator;
        private readonly WhenQuestionGenerator whenQuestionGenerator;
        private readonly WhereQuestionGenerator whereQuestionGenerator;
        public InterrogativeWordQuestionGenerator()
        {
            this.whoQuestionGenerator = new WhoQuestionGenerator();
            this.whenQuestionGenerator = new WhenQuestionGenerator();
            this.whereQuestionGenerator = new WhereQuestionGenerator();
        }
        
        public override List<Question> Generate(List<Sentence> sentences)
        {
            var listOfQuestion = new List<Question>();
            foreach (var sentence in sentences)
            {
                var whoResult = this.whoQuestionGenerator.Generate(sentence);
                if (whoResult != null)
                {
                    listOfQuestion.Add(whoResult);
                }

                var whenResult = this.whenQuestionGenerator.Generate(sentence);
                if (whenResult != null)
                {
                    listOfQuestion.Add(whenResult);
                }

                var whereResult = this.whereQuestionGenerator.Generate(sentence);
                if (whereResult != null)
                {
                    listOfQuestion.Add(whereResult);
                }
            }

            return listOfQuestion;
        }
    }
}
