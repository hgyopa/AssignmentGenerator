namespace TestGenerator.Generators.TrueOrFalseQuestion
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Common.Models;
    using Extensions;
    using Models;

    public class TrueOrFalseQuestionGenerator : QuestionGenerator
    {
        private SentenceNegator negator;
        private List<string> cityList = new List<string>() { "Budapest", "Rome", "Paris", "Bukarest", "Bratislava", "Berlin", "London" };
        private List<string> auxiliaryList = new List<string>() { "can", "dare", "do", "may", "might", "must", "need", "ought", "shall", "should", "will", "should", };
        private static Random rand = new Random();

        public TrueOrFalseQuestionGenerator()
        {
            this.negator = new SentenceNegator();
        }

        public override List<Question> Generate(List<Sentence> sentences)
        {
            var questionList = new List<Question>();

            foreach (var sentence in sentences)
            {
                //TO not only have false questions
                questionList.Add(new Question
                {
                    Type = QuestionType.TrueOrFalse,
                    Text = sentence.Text,
                    Answers = new List<Answer>
                    {
                        new Answer{IsCorrectAnswer = true, Text = "true"}
                    }
                });

                foreach (var token in sentence.Tokens)
                {
                    var negationResult = NegateSentence(sentence, token);
                    if (negationResult != null)
                    {
                        questionList.Add(negationResult);
                    }

                    var locationExchangeResult = ExchangeLocation(sentence, token);
                    if (locationExchangeResult != null)
                    {
                        questionList.Add(locationExchangeResult);
                    }
                }
            }

            return questionList;
        }
        private Question ExchangeLocation(Sentence sentence, Token token)
        {
            if (token.NamedEntityTag != "CITY") return null;

            var textToBeReplaced = token.Text;
            var nextPosition = token.Number;

            //Check whether the name of the city consists of multiple tokens
            while (sentence.Tokens[nextPosition].NamedEntityTag == "CITY" && nextPosition < sentence.Tokens.Count)
            {
                textToBeReplaced += $"{sentence.Tokens[nextPosition].Text} ";
                nextPosition++;
            }

            //Check whether we have different option than the original
            if (this.cityList.FirstOrDefault(city => city != textToBeReplaced) == null)
            {
                return null;
            }

            var randomIndex = rand.Next(this.cityList.Count);

            //Is is different from the original, if not, generate a new one
            while (textToBeReplaced == this.cityList[randomIndex])
            {
                randomIndex = rand.Next(this.cityList.Count);
            }

            var questionText = sentence.Text.CaseInsensitiveReplace(textToBeReplaced, this.cityList[randomIndex]);

            return new Question
            {
                Type = QuestionType.TrueOrFalse,
                Text = questionText,
                Answers = new List<Answer> { new Answer { IsCorrectAnswer = true, Text = textToBeReplaced } }
            };
        }

        private Question NegateSentence(Sentence sentence, Token token)
        {
            //the position of the token is always less than the number of the token. Position starts with 0, Token number starts with 1.
            // To get token before the current token --> -2 as (current token = sentence.Tokens[token.Number-1])

            switch (token.Lemma)
            {
                case "be":
                    {
                        //handle perfect tenses
                        //or if there is an auxiliary verb
                        if (sentence.Tokens[token.Number - 2].Lemma == "have" || this.auxiliaryList.Contains(sentence.Tokens[token.Number - 2].Lemma) || sentence.Tokens[token.Number-2].Lemma == "not")
                        {
                            break;
                        }

                        return HandleVerbBe(sentence, token);
                    }
                case "have":
                    {
                        //future perfect : I will have taken
                        //future perfect continuous: I will have been taking 
                        //Auxilaries are handled differently
                        if (sentence.Tokens[token.Number - 2].Lemma == "be" || this.auxiliaryList.Contains(sentence.Tokens[token.Number - 2].Lemma) || sentence.Tokens[token.Number - 2].Lemma == "not")
                        {
                            break;
                        }

                        return HandleVerbHave(sentence, token);
                    }
                default:
                    {
                        //to handle auxiliary verbs
                        if (this.auxiliaryList.Contains(token.Lemma))
                        {
                            return HandleAuxiliaryVerbs(sentence, token);
                        }

                        //to handle other verbs
                        if (token.Pos == "VBD" && token.Number > 1 && !(sentence.Tokens[token.Number - 2].Lemma == "be" || sentence.Tokens[token.Number - 2].Lemma == "have" || this.auxiliaryList.Contains(token.Lemma)))
                        {
                            //only case when the verb is in past simple form
                            return HandleOtherVerbs(sentence, token);
                        }

                        return null;
                    }
            }

            return null;
        }

        private Question HandleOtherVerbs(Sentence sentence, Token token)
        {
            if (sentence.Tokens[token.Number].Text != "not")
            //the position of the token is always less than the number of the token. Position starts with 0, Token number starts with 1.
            //sentence.Tokens[token.Number + 1] --> sentence.Tokens[token.Number]
            {
                return this.negator.NegateVerbInPastSimple(sentence, token.Number);
            }
            else
            {
                //A call of a conjugator would be nec.
                return null;
            }
        }
        private Question HandleVerbHave(Sentence sentence, Token token)
        {
            if (sentence.Tokens[token.Number].Text != "not")
            //the position of the token is always less than the number of the token. Position starts with 0, Token number starts with 1.
            //sentence.Tokens[token.Number + 1] --> sentence.Tokens[token.Number]
            {
                return this.negator.NegateHave(sentence, token.Number);
            }
            else
            {
                return this.negator.UnnegateHave(sentence, token.Number);
            }
        }
        public Question HandleVerbBe(Sentence sentence, Token token)
        {
            if (sentence.Tokens[token.Number].Text != "not")
            //the position of the token is always less than the number of the token. Position starts with 0, Token number starts with 1.
            //sentence.Tokens[token.Number + 1] --> sentence.Tokens[token.Number]
            {
                return this.negator.NegateBe(sentence, token.Number);
            }
            else
            {
                return this.negator.UnnegateBe(sentence, token.Number);
            }
        }
        private Question HandleAuxiliaryVerbs(Sentence sentence, Token token)
        {
            if (sentence.Tokens[token.Number].Text != "not")
                //the position of the token is always less than the number of the token. Position starts with 0, Token number starts with 1.
                //sentence.Tokens[token.Number + 1] --> sentence.Tokens[token.Number]
            {
                return this.negator.NegateAuxiliaryVerb(sentence, token.Number);
            }
            else
            {
                return this.negator.UnnegateAuxiliaryVerb(sentence, token.Number);
            }
        }
    }
}
