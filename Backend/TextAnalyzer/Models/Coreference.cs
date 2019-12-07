using System.Collections.Generic;

namespace TextAnalyzer.Models
{
    public class Coreference
    {
        public int CorefClusterId { get; set; }
        public Reference From { get; set; }
        public List<Reference> To { get; set; }
    }
}
