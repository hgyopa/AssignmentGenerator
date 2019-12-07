using TextAnalyzer;

namespace Tester
{
    using TestGenerator;

    class Program
    {
        static void Main(string[] args)
        {
            var jarRoot = @"..\..\..\..\stanford-corenlp-full-2018-10-05";
            var text = "Wife of Enrique Iglesias is Anna. Her house is big.";

            var properties = new PropertyBuilder()
                .SetAnnotators(new [] {"tokenize", "ssplit", "pos", "lemma", "ner", "parse", "natlog", "dcoref"})
                .SetKeyValuePairs("ner.useSUTime", "0")
                .Build();

            var analyzer = new Analyzer(properties, jarRoot,text);
            var sentenceList = analyzer.GetListOfSentences();
            var coreferenceList = analyzer.GetCoreferenceList();

            var normalizer = new Normalizer();
            var normalizedSentences = normalizer.NormalizeText(sentenceList, coreferenceList);

            var generator = new SubstitutionQuestionGenerator();
            var questions = generator.Generate(normalizedSentences);
        }
    }
}
