using edu.stanford.nlp.trees;

namespace TextAnalyzer
{
    public class TreeParser
    {
        public Common.Models.Tree Parse(edu.stanford.nlp.util.CoreMap sentence)
        {
            var tree = sentence.get(typeof(TreeCoreAnnotations.TreeAnnotation)) as Tree;
            return new Common.Models.Tree();
        }
    }
}
