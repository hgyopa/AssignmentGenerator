namespace TestGenerator.Models
{
    using System.Collections.Generic;

    public class Question
    {
        public string Text { get; set; }
        public QuestionType Type { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
