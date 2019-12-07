using edu.stanford.nlp.semgraph;

namespace TextAnalyzer
{
    public class SemanticGraphParser
    {
        public Common.Models.SemanticGraph Parse(edu.stanford.nlp.util.CoreMap sentence)
        {
            var dependencies = sentence.get(typeof(SemanticGraphCoreAnnotations.CollapsedCCProcessedDependenciesAnnotation)) as SemanticGraph;
            return new Common.Models.SemanticGraph();
        }
    }
}
