namespace TestGenerator.Generators.InterrogativeWordQuestion
{
    using System;
    using Common.Models;
    using Models;
    using System.Collections.Generic;
    using System.Linq;
    using Extensions;

    public enum Tense
    {
        None,
        SimplePresent,
        SimplePast,
        PastPassive
    }

    public class WhereQuestionGenerator
    {
        private string[] locationTypeList = new string[] {"CITY", "COUNTRY"};

        public Question Generate(Sentence sentence)
        {
            var position = 1;
            var isThereALocationInTheSentence = false;
            var tokenPositionOfVerb = 0;
            var answerText = string.Empty;
            var subject = string.Empty;
            var questionText = string.Empty;
            var tense = Tense.None;

            while (position < sentence.Tokens.Count)
            {
                if (sentence.Tokens[position - 1].Pos.ToUpper() == "IN"
                    && this.locationTypeList.Contains(sentence.Tokens[position].NamedEntityTag))
                {
                    isThereALocationInTheSentence = true;
                    answerText = sentence.Tokens[position].Text;
                }

                //0  1     2  3    
                //He lives in Hungary. SIMPLE PRESENT
                if ((sentence.Tokens[position - 1].Pos == "VBP" || sentence.Tokens[position - 1].Pos == "VBZ")
                    && !sentence.Tokens[position].Pos.StartsWith("VB"))
                {
                    tense = Tense.SimplePresent;
                    tokenPositionOfVerb = position - 1;
                }

                //0  1   2  3
                //He was in Hungary. SIMPLE PAST
                if (sentence.Tokens[position - 1].Pos == "VBD"
                    && !sentence.Tokens[position].Pos.StartsWith("VB"))
                {
                    tense = Tense.SimplePast;
                    tokenPositionOfVerb = position - 1;
                }

                //0   1   2   3     4  5  
                //The key was found in Hungary.
                if (sentence.Tokens[position - 1].Lemma == "be"
                    && sentence.Tokens[position - 1].Pos == "VBD"
                    && sentence.Tokens[position].Pos == "VBN")
                {
                    tense = Tense.PastPassive;
                    tokenPositionOfVerb = position - 1;
                }

                position++;
            }

            if (!(isThereALocationInTheSentence && tense == Tense.None))
            {
                return null;
            }

            //search for the subject
            for (var index = 0; index < tokenPositionOfVerb; index++)
            {
                subject += (sentence.Tokens[index].Pos == "NNP" && sentence.Tokens[index].Pos == "NNPS") ? $"{sentence.Tokens[index].Text} "
                    : $"{sentence.Tokens[index].Text.ToLower()} ";
            }

            subject = subject.Trim();

            //Change order
            switch (tense)
            {
                case Tense.SimplePresent:
                    questionText = CreateQuestionForSimplePresent(sentence,subject,tokenPositionOfVerb);
                    break;
                case Tense.SimplePast:
                    questionText = CreateQuestionForSimplePast(sentence, subject, tokenPositionOfVerb);
                    break;
                case Tense.PastPassive:
                    questionText = CreateQuestionForPastPassive(sentence, subject, tokenPositionOfVerb);
                    break;
                case Tense.None:
                    return null;
                default:
                    throw new InvalidOperationException("Invalid tense value.");
            }

            //Delete the location from the sentence,
            questionText = questionText.CaseInsensitiveReplace($"in {answerText}", "");

            //Exhange punctuation mark
            var punctuationMark = sentence.Tokens[sentence.Tokens.Count - 1].Text;
            questionText = questionText.Replace(punctuationMark, "?");

            return new Question
            {
                Type = QuestionType.InterrogativeWord,
                Text = questionText,
                Answers = new List<Answer> { new Answer { IsCorrect = true, Text = answerText } }
            };
        }

        private string CreateQuestionForSimplePast(Sentence sentence, string subject, int tokenPositionOfVerb)
        {
            var result = string.Empty;

            result = sentence.Tokens[tokenPositionOfVerb].Lemma == "be" ?
                $"Where {sentence.Tokens[tokenPositionOfVerb].Text} {subject}"
                : $"Where did {subject} {sentence.Tokens[tokenPositionOfVerb].Lemma}";

            for (var index = tokenPositionOfVerb + 1; index < sentence.Tokens.Count; index++)
            {
                result += $" {sentence.Tokens[index].Text}";
            }

            return result;
        }

        private string CreateQuestionForPastPassive(Sentence sentence, string subject, int tokenPositionOfVerb)
        {
            var result  = $"Where {sentence.Tokens[tokenPositionOfVerb].Text} {subject}";

            for (var index = tokenPositionOfVerb + 1; index < sentence.Tokens.Count; index++)
            {
                result += $" {sentence.Tokens[index].Text}";
            }

            return result;
        }

        private string CreateQuestionForSimplePresent(Sentence sentence, string subject, int tokenPositionOfVerb)
        {
            var result = string.Empty;

            if (sentence.Tokens[tokenPositionOfVerb].Lemma == "be")
            {
                result = $"Where {sentence.Tokens[tokenPositionOfVerb].Text} {subject}";
            }
            else
            {
                result = sentence.Tokens[tokenPositionOfVerb].Pos == "VBZ" ?
                    $"Where does {subject} {sentence.Tokens[tokenPositionOfVerb].Lemma}"
                    : $"Where do {subject} {sentence.Tokens[tokenPositionOfVerb].Lemma}";
            }

            for (var index = tokenPositionOfVerb + 1; index < sentence.Tokens.Count; index++)
            {
                result += $" {sentence.Tokens[index].Text}";
            }

            return result;
        }
    }
}
