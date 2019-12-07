using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Models;
using edu.stanford.nlp.ling;
using edu.stanford.nlp.pipeline;
using edu.stanford.nlp.util;
using java.util;
using TextAnalyzer.Models;
using Properties = TextAnalyzer.Models.Properties;

namespace TextAnalyzer
{
    public class Analyzer
    {
        private CoreDocument coreDocument;
        private Annotation document;
        private SemanticGraphParser semanticGraphParser;
        private TreeParser treeParser;
        private StanfordCoreNLP pipeline;
        private string text;

        public Analyzer(Properties properties, string jarRootPath, string text)
        {
            var curDir = Environment.CurrentDirectory;
            Directory.SetCurrentDirectory(jarRootPath);
            pipeline = new StanfordCoreNLP(properties.GetUnderlyingModel());
            Directory.SetCurrentDirectory(curDir);

            coreDocument = new CoreDocument(text);
            this.text = text;

            treeParser = new TreeParser();
            semanticGraphParser = new SemanticGraphParser();
        }


        public List<Coreference> GetCoreferenceList()
        {
            var coreferenceList = new List<Coreference>();
            Map graph = (Map)document.get(typeof(edu.stanford.nlp.coref.CorefCoreAnnotations.CorefChainAnnotation));
            var values = graph.values();

            var iterator = values.iterator();
            while (iterator.hasNext())
            {
                edu.stanford.nlp.coref.data.CorefChain element = (edu.stanford.nlp.coref.data.CorefChain)iterator.next();
                var chainId = element.getChainID();
                var mentions = (List)element.getMentionsInTextualOrder();

                var coreference = new TextAnalyzer.Models.Coreference();
                coreference.To = new List<Reference>();

                for (int j = 0; j < mentions.size(); j++)
                {
                    var mention = mentions.get(j) as edu.stanford.nlp.coref.data.CorefChain.CorefMention;

                    if (mention == null)
                    {
                        //TODO - HIBA
                    }

                    var reference = new Reference
                    {
                        Text = mention.mentionSpan,
                        SentenceNumber = mention.sentNum,
                        HeadIndex = mention.headIndex,
                        StartIndex = mention.startIndex,
                        EndIndex = mention.endIndex,
                        Gender = (Gender)Enum.Parse(typeof(Gender), mention.gender.toString(), true),
                        Animacy = (Animacy)Enum.Parse(typeof(Animacy), mention.animacy.toString(), true),
                        MentionType = (MentionType)Enum.Parse(typeof(MentionType), mention.mentionType.toString(), true)
                    };

                    if (reference.MentionType == MentionType.PROPER)
                    {
                        coreference.From = reference;
                    }
                    else
                    {
                        coreference.To.Add(reference);
                    }

                    coreference.CorefClusterId = mention.corefClusterID;
                }

                coreferenceList.Add(coreference);
            }

            return coreferenceList;
        }

        public List<Sentence> GetListOfSentences()
        {
            //ez a kettő átkerült
            document = new Annotation(text);
            pipeline.annotate(document);

            var listOfSentences = new List<Sentence>();

            if (document == null)
            {
                return null;
            }

            var sentenceList = GetSentences();
            var sentenceCounter = 1;

            foreach (CoreMap sentence in sentenceList)
            {
                if (sentence == null)
                {
                    throw new System.InvalidOperationException("Sentence is null in the sentence list");
                }

                var tokenList = sentence.get(typeof(CoreAnnotations.TokensAnnotation));

                if (!(tokenList is ArrayList))
                {
                    throw new System.Exception("TokenList is null in a sentence");
                }

                var dependecies =
                    sentence.get(
                        typeof(edu.stanford.nlp.semgraph.SemanticGraphCoreAnnotations.EnhancedDependenciesAnnotation));

                var tokenCounter = 1;
                var listOfTokens = new List<Token>();

                foreach (CoreLabel token in tokenList as ArrayList)
                {
                    if (token == null)
                    {
                        throw new System.Exception("Token is null");
                    }

                    CoreAnnotations.TrueCaseTextAnnotation truecase =
                        token.get(
                            typeof(CoreAnnotations.TrueCaseTextAnnotation)) as CoreAnnotations.TrueCaseTextAnnotation;


                    listOfTokens.Add(new Token
                    {
                        Number = tokenCounter,
                        Text = token.get(typeof(CoreAnnotations.TextAnnotation)).ToString(),
                        Pos = token.get(typeof(CoreAnnotations.PartOfSpeechAnnotation)).ToString(),
                        NamedEntityTag = token.get(typeof(CoreAnnotations.NamedEntityTagAnnotation)).ToString(),
                        Lemma = token.get(typeof(CoreAnnotations.LemmaAnnotation)).ToString()
                    });

                    tokenCounter++;
                }

                var tree = treeParser.Parse(sentence);
                var semanticGraph = semanticGraphParser.Parse(sentence);

                listOfSentences.Add(new Sentence { Number = sentenceCounter, Text = sentence.ToString(), Tokens = listOfTokens, SemanticGraph = semanticGraph, Tree = tree });
                sentenceCounter++;
            }

            return listOfSentences;
        }

        private List<CoreMap> GetSentences()
        {
            var sentences = document.get(typeof(CoreAnnotations.SentencesAnnotation));

            return (sentences as ArrayList)?.Cast<CoreMap>().ToList();
        }
    }
}
