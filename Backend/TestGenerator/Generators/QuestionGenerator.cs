namespace TestGenerator
{
    using System.Collections.Generic;
    using Common.Models;
    using Models;

    public abstract class QuestionGenerator
    {
        public abstract List<Question> Generate(List<Sentence> sentences);
    }
}
