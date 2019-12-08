namespace TestGenerator
{
    using System.Collections.Generic;
    using System.Linq;
    using Common.Models;
    using Extensions;
    using Models;

    public class SubstitutionQuestionGenerator : QuestionGenerator
    {
        private const int lineLength = 10;
        private string[] namedEntityTags = new string[] {"LOCATION", "COUNTRY", "CITY", "PERSON", "ORGANIZATION", "DATE"};

        public override List<Question> Generate(List<Sentence> sentences)
        {
            var questionList = new List<Question>();

            foreach (var sentence in sentences)
            {
                for(var i = 0; i < sentence.Tokens.Count; i++)
                {
                    if (!this.namedEntityTags.Contains(sentence.Tokens[i].NamedEntityTag.ToUpper())) continue;
                    var textToBeReplaced = string.Empty; //answer of the question
                    var tokenToBeReplaced = new int[sentence.Tokens.Count];
                    var arrayIndex = 0;
                    var questionText = string.Empty;
                    //some words true case, like unknown university (Obuda University) will be changed to Obuda university
                    //by only dot out with the help of Replace function, wont find the exact string in the original sentence (Obuda University <-> Obuda university)
                        
                    while (this.namedEntityTags.Contains(sentence.Tokens[i].NamedEntityTag.ToUpper()))
                    {
                        textToBeReplaced += $"{sentence.Tokens[i].Text} ";
                        tokenToBeReplaced[arrayIndex] = i;
                        i++;
                        arrayIndex++;
                    }

                    i--; //the counter is increased also after the last text to be replaced--> have to be decreased by 1
                    textToBeReplaced = textToBeReplaced.TrimEnd();

                    questionList.Add(new Question
                    {
                        Text = sentence.Text.CaseInsensitiveReplace(textToBeReplaced,new string('_', lineLength)),
                        Type = QuestionType.Substitution,
                        Answers = new List<Answer>
                        {
                            new Answer
                            {
                                Text = textToBeReplaced,
                                IsCorrect = true
                            }
                        }
                    });
                }
            }

            return questionList;
        }
    }
}
