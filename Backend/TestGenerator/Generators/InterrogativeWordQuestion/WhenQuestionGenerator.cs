namespace TestGenerator.Generators.InterrogativeWordQuestion
{
    using Common.Models;
    using Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Eventing.Reader;
    using Extensions;
    using Microsoft.SqlServer.Server;

    public class WhenQuestionGenerator
    {
        //0     1   2    3  4
        //John was born in 2005.
        public Question Generate(Sentence sentence)
        {
            var position = 1;
            var isThereADateInTheSentence = false;
            var isPastPassive = false;
            var answerText = string.Empty;
            var subject = string.Empty;

            var tokenPositionOfVerbBe = 0;

            while (position < sentence.Tokens.Count)
            {
                if (sentence.Tokens[position - 1].Pos.ToUpper() == "IN"
                    && sentence.Tokens[position].NamedEntityTag == "DATE")
                {
                    isThereADateInTheSentence = true;
                    answerText = sentence.Tokens[position].Text;
                }

                if (sentence.Tokens[position - 1].Lemma == "be"
                    && sentence.Tokens[position - 1].Pos == "VBD"
                    && sentence.Tokens[position].Pos == "VBN")
                {
                    isPastPassive = true;
                    tokenPositionOfVerbBe = position - 1;
                }

                position++;
            }

            //If the sentence is not correct, return null
            if (!(isThereADateInTheSentence && isPastPassive)) return null;

            //search for the subject
            for (var index = 0; index < tokenPositionOfVerbBe; index++)
            {
                subject += sentence.Tokens[index].NamedEntityTag.StartsWith("NN") ? $"{sentence.Tokens[index].Text} " 
                                                                                    : $"{sentence.Tokens[index].Text.ToLower()} ";
            }

            subject = subject.Trim();

            //Change the word order:
            //Only past passive
            //Put the when word in the question:
            var questionText = $"When {sentence.Tokens[tokenPositionOfVerbBe].Text} {subject}";

            for (var index = tokenPositionOfVerbBe+1; index < sentence.Tokens.Count; index++)
            {
                questionText += $" {sentence.Tokens[index].Text}";
            }

            //Delete the date from the sentence,
            questionText = questionText.CaseInsensitiveReplace($"in {answerText}", "");

            //Exhange punctuation mark
            var punctuationMark = sentence.Tokens[sentence.Tokens.Count - 1].Text;
            questionText = questionText.Replace(punctuationMark, "?");

            return new Question
            {
                Type = QuestionType.InterrogativeWord,
                Text = questionText,
                Answers = new List<Answer> {new Answer {IsCorrect = true, Text = answerText}}
            };
        }
    }
}
