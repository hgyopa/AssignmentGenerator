namespace TestGenerator.Generators.InterrogativeWordQuestion
{
    using Common.Models;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;

    public class WhoQuestionGenerator
    {
        private string[] conjuctionPOS = new string[] {"CC",","};

        /// <summary>
        /// Examples:
        ///     Jane was born in 2005.
        ///     John and Jane were born is 2005.
        ///     John , Jack and Jane were born in 2005.
        /// </summary>
        /// <param name="sentence"></param>
        /// <returns></returns>
        public Question Generate(Sentence sentence)
        {
            var position = 0;
            var firstToken = sentence.Tokens[position];
            var isPerson = false;

            if (firstToken.Pos != "NNP")
            {
                return null;
            }

            var textToBeReplaced = firstToken.Text;

            if (firstToken.NamedEntityTag == "PERSON")
            {
                isPerson = true;
            }
            position++;

            while (this.conjuctionPOS.Contains(sentence.Tokens[position].Pos.ToUpper()) || sentence.Tokens[position].Pos == "NNP")
            {
                if (sentence.Tokens[position].NamedEntityTag == "PERSON") isPerson = true;
                textToBeReplaced += $" {sentence.Tokens[position].Text}";
                position++;
            }

            var questionText = isPerson
                ? $"{sentence.Text.CaseInsensitiveReplace(textToBeReplaced, "Who")}"
                : $"{sentence.Text.CaseInsensitiveReplace(textToBeReplaced, "What")}";

            var punctuationMark = sentence.Tokens[sentence.Tokens.Count - 1].Text;
            questionText = questionText.Replace(punctuationMark, " ?");

            return new Question
            {
                Type = QuestionType.InterrogativeWord,
                Text = questionText,
                Answers = new List<Answer> {new Answer {IsCorrect = true, Text = textToBeReplaced}}
            };
        }
    }
}
