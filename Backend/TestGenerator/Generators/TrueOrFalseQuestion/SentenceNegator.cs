namespace TestGenerator.Generators.TrueOrFalseQuestion
{
    using System;
    using System.Collections.Generic;
    using Common.Models;
    using Models;

    public class SentenceNegator
    {
        public Question NegateBe(Sentence sentence, int tokenNumber)
        {
            var questionText = string.Empty;

            //Special case: Future perfect
            //I will have taken ...
            if (sentence.Tokens[tokenNumber].Lemma == "have" && sentence.Tokens[tokenNumber].Pos == "VB" //have in base form
                    && sentence.Tokens[tokenNumber + 1].Pos == "VBN" && sentence.Tokens[tokenNumber + 1].Lemma != "be" && sentence.Tokens[tokenNumber + 1].Lemma != "have") //verb other than be or have, past participle
            {
                foreach (var token in sentence.Tokens)
                {
                    //eg: will from the example
                    if (token.Number == tokenNumber)
                    {
                        questionText += $"{token.Text} not ";
                    }
                    else
                    {
                        questionText += $"{token.Text} ";
                    }
                }
            }
            else
            {
                questionText = SimpleNegate(sentence, tokenNumber);
            }

            return new Question
            {
                Text = questionText,
                Type = QuestionType.TrueOrFalse,
                Answers = new List<Answer> { new Answer { IsCorrect = true, Text = "false" } }
            };
        }
        public Question UnnegateBe(Sentence sentence, int tokenNumber)
        {
            var questionText = string.Empty;

            //Special case: Future perfect
            //I will have taken ...
            if (sentence.Tokens[tokenNumber].Lemma == "have" && sentence.Tokens[tokenNumber].Pos == "VB" //have in base form
                && sentence.Tokens[tokenNumber+1].Pos == "VBN" && sentence.Tokens[tokenNumber + 1].Lemma != "be" && sentence.Tokens[tokenNumber + 1].Lemma != "have") //verb other than be or have, past participle
            {
                foreach (var token in sentence.Tokens)
                {
                    if (token.Number == tokenNumber + 1 || token.Number == tokenNumber +2)
                    {
                        continue;
                    }
                    else
                    {
                        questionText += $"{token.Text} ";
                    }
                }
            }
            else
            {
                questionText = SimpleUnnegate(sentence, tokenNumber);
            }

            return new Question
            {
                Text = questionText,
                Type = QuestionType.TrueOrFalse,
                Answers = new List<Answer> { new Answer { IsCorrect = true, Text = "false" } }
            };
        }

        public Question NegateHave(Sentence sentence, int tokenNumber)
        {
            var questionText = String.Empty;

            //the position of the token is always less than the number of the token. Position starts with 0, Token number starts with 1.
            //sentence.Tokens[tokenNumber].Pos] --> sentence.Tokens[tokenNumber-1].Pos
            switch (sentence.Tokens[tokenNumber-1].Pos)
            {
                case "VBP":
                case "VBZ":
                    {
                        questionText = SimpleNegate(sentence, tokenNumber);
                        break;
                    }
                case "VBD":
                    {
                        foreach (var token in sentence.Tokens)
                        {
                            if (token.Number == tokenNumber)
                            {
                                questionText += $"didn't {token.Lemma} ";
                            }
                            else
                            {
                                questionText += $"{token.Text} ";
                            }
                        }

                        break;
                    }
            }

            return new Question
            {
                Text = questionText,
                Type = QuestionType.TrueOrFalse,
                Answers = new List<Answer> { new Answer { IsCorrect = true, Text = "false" } }
            };
        }
        public Question UnnegateHave(Sentence sentence, int tokenNumber)
        {
            var questionText = String.Empty;

            //the position of the token is always less than the number of the token. Position starts with 0, Token number starts with 1.
            //sentence.Tokens[tokenNumber].Pos] --> sentence.Tokens[tokenNumber-1].Pos
            switch (sentence.Tokens[tokenNumber-1].Pos)
            {
                case "VBP":
                case "VBZ":
                case "VBD":
                    {
                        questionText = SimpleUnnegate(sentence, tokenNumber);
                        break;
                    }
            }

            if (questionText.Contains("any"))
            {
                questionText = questionText.Replace("any", "a");
            }

            return new Question
            {
                Text = questionText,
                Type = QuestionType.TrueOrFalse,
                Answers = new List<Answer> { new Answer { IsCorrect = true, Text = "false" } }
            };
        }

        private string SimpleNegate(Sentence sentence, int tokenNumber)
        {
            var questionText = String.Empty;

            foreach (var token in sentence.Tokens)
            {
                if (token.Number == tokenNumber)
                {
                    questionText += $"{token.Text} not ";
                }
                else
                {
                    questionText += $"{token.Text} ";
                }
            }

            return questionText;
        }
        private string SimpleUnnegate(Sentence sentence, int tokenNumber)
        {
            var questionText = String.Empty;

            foreach (var token in sentence.Tokens)
            {
                if (token.Number == tokenNumber + 1)
                {
                    continue;
                }
                else
                {
                    questionText += $"{token.Text} ";
                }
            }

            return questionText;
        }

        public Question NegateVerbInPastSimple(Sentence sentence, int tokenNumber)
        {
            var questionText = String.Empty;

            foreach (var token in sentence.Tokens)
            {
                if (token.Number == tokenNumber)
                {
                    questionText += $"did not {token.Lemma} ";
                }
                else
                {
                    questionText += $"{token.Text} ";
                }
            }

            return new Question
            {
                Text = questionText,
                Type = QuestionType.TrueOrFalse,
                Answers = new List<Answer> { new Answer { IsCorrect = true, Text = "false" } }
            };
        }

        public Question NegateAuxiliaryVerb(Sentence sentence, int tokenNumber)
        {
            var questionText = SimpleNegate(sentence, tokenNumber);

            return new Question
            {
                Text = questionText,
                Type = QuestionType.TrueOrFalse,
                Answers = new List<Answer>() { new Answer { IsCorrect = true, Text = "true"} }
            };
        }

        public Question UnnegateAuxiliaryVerb(Sentence sentence, int tokenNumber)
        {
            var questionText = SimpleUnnegate(sentence, tokenNumber);

            return new Question
            {
                Text = questionText,
                Type = QuestionType.TrueOrFalse,
                Answers = new List<Answer>() { new Answer { IsCorrect = true, Text = "true" } }
            };
        }
    }
}
