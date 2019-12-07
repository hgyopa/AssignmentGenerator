using System.Collections.Generic;
using System.Linq;
using Common.Models;
using TextAnalyzer.Models;

namespace TextAnalyzer
{
    public class Normalizer
    {
        public List<Sentence> NormalizeText(List<Sentence> sentences, List<Coreference> coreferences)
        {
            var normalizedSentences = sentences.Select(sentence => (Sentence)sentence.Clone()).ToList();

            foreach (var coreference in coreferences)
            {
                if (coreference.From == null)
                {
                    continue;
                }

                var fromSentence = normalizedSentences[coreference.From.SentenceNumber - 1];
                var fromStartTokenNumber = coreference.From.StartIndex - 1;
                var fromEndTokenNumber = coreference.From.EndIndex - 1;

                if (!coreference.To.Any())
                {
                    continue;
                }

                foreach (var toReference in coreference.To)
                {
                    var toSentence = normalizedSentences[toReference.SentenceNumber - 1];
                    var normalizedTokens = new List<Token>();
                    var newSentenceText = string.Empty;
                    var tokenIndex = 0;
                    var toStartIndex = toReference.StartIndex - 1;

                    if (toReference.MentionType == MentionType.PRONOMINAL)
                    {
                        for (int i = 0; i < toSentence.Tokens.Count; i++)
                        {
                            if (i == toStartIndex)
                            {
                                var newTokenList = new List<Token>();

                                for (int j = fromStartTokenNumber; j < fromEndTokenNumber; j++)
                                {
                                    var clonedToken = (Token)fromSentence.Tokens[j].Clone();
                                    newTokenList.Add(clonedToken);

                                    clonedToken.Number = tokenIndex;
                                    normalizedTokens.Add(clonedToken);
                                    tokenIndex++;
                                    newSentenceText += $"{clonedToken.Text} ";
                                }

                                if (new[] { "MY", "YOUR", "HIS", "HER", "ITS", "OUR", "THEIR" }.Contains(toReference.Text.ToUpper()))
                                {
                                    normalizedTokens[normalizedTokens.Count - 1].Text = $"{normalizedTokens[normalizedTokens.Count - 1].Text}'s";
                                    newTokenList[newTokenList.Count - 1].Text = $"{normalizedTokens[normalizedTokens.Count - 1].Text}'s";
                                    newSentenceText = $"{newSentenceText.TrimEnd()}'s ";
                                }

                                var exchangedPair = new TokenListPair(new List<Token>() { toSentence.Tokens[toReference.HeadIndex] }, newTokenList);
                            }
                            else
                            {
                                toSentence.Tokens[i].Number = tokenIndex;
                                normalizedTokens.Add(toSentence.Tokens[i]);
                                tokenIndex++;
                                newSentenceText += $"{toSentence.Tokens[i].Text} ";
                            }
                        }

                        normalizedSentences[toReference.SentenceNumber - 1].Text = newSentenceText.TrimEnd();
                        normalizedSentences[toReference.SentenceNumber - 1].Tokens = normalizedTokens;

                        //CheckTokenInCoreference(toSentence, normalizedSentences[toReference.SentenceNumber - 1],coreferences);
                    }
                }
            }

            return normalizedSentences;
        }

        public List<Sentence> NormalizeText2(List<Sentence> sentences, List<Coreference> coreferences)
        {
            var normalizedSentences = sentences.Select(sentence => (Sentence)sentence.Clone()).ToList();

            for (int sentenceCounter = normalizedSentences.Count - 1; sentenceCounter >= 0; sentenceCounter--)
            {
                //mondatokon végigmegyün hátulról

                for (int referenceCounter = coreferences.Count - 1; referenceCounter >= 0; referenceCounter--)
                {
                    var fromReference = coreferences[referenceCounter].From;

                    if (fromReference == null)
                    {
                        continue;
                    }

                    var fromStartTokenNumber = fromReference.StartIndex - 1;
                    var fromEndTokenNumber = fromReference.EndIndex - 1;
                    var fromSentence = normalizedSentences[fromReference.SentenceNumber - 1];

                    for (int toReferencesCount = coreferences[referenceCounter].To.Count-1; toReferencesCount >= 0; toReferencesCount--)
                    {
                        var toReference = coreferences[referenceCounter].To[toReferencesCount];

                        if (toReference.SentenceNumber - 1 == sentenceCounter)
                        {
                            var toSentence = normalizedSentences[sentenceCounter];
                            var toStartIndex = toReference.StartIndex-1;
                            var tokenIndex = 0;
                            var normalizedTokens = new List<Token>();
                            var newSentenceText = string.Empty;

                            //Last reference, last sentence
                            if (toReference.MentionType == MentionType.PRONOMINAL)
                            {
                                for (int i = 0; i < toSentence.Tokens.Count; i++)
                                {
                                    if (i == toStartIndex)
                                    {
                                        var newTokenList = new List<Token>();

                                        for (int j = fromStartTokenNumber; j < fromEndTokenNumber; j++)
                                        {
                                            var clonedToken = (Token)fromSentence.Tokens[j].Clone();
                                            newTokenList.Add(clonedToken);

                                            clonedToken.Number = tokenIndex;
                                            normalizedTokens.Add(clonedToken);
                                            tokenIndex++;
                                            newSentenceText += $"{clonedToken.Text} ";
                                        }

                                        if (new[] { "MY", "YOUR", "HIS", "HER", "ITS", "OUR", "THEIR" }.Contains(toReference.Text.ToUpper()))
                                        {
                                            normalizedTokens[normalizedTokens.Count - 1].Text = $"{normalizedTokens[normalizedTokens.Count - 1].Text}'s";
                                            newTokenList[newTokenList.Count - 1].Text = $"{normalizedTokens[normalizedTokens.Count - 1].Text}";
                                            newSentenceText = $"{newSentenceText.TrimEnd()}'s ";
                                        }
                                    }
                                    else
                                    {
                                        toSentence.Tokens[i].Number = tokenIndex;
                                        normalizedTokens.Add(toSentence.Tokens[i]);
                                        tokenIndex++;
                                        newSentenceText += $"{toSentence.Tokens[i].Text} ";
                                    }
                                }

                                normalizedSentences[toReference.SentenceNumber - 1].Text = newSentenceText.TrimEnd();
                                normalizedSentences[toReference.SentenceNumber - 1].Tokens = normalizedTokens;
                            }
                        }
                    }
                }
            }

            return normalizedSentences;
        }
    }
}
