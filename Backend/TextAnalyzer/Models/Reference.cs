using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TextAnalyzer.Models
{
    public class Reference
    {
        /// <summary>
        /// Number of the sentence
        /// </summary>
        public int SentenceNumber { get; set; }
        public int HeadIndex { get; set; }
        public int StartIndex { get; set; }
        public int EndIndex { get; set; }
        public string Text { get; set; }
        public Animacy Animacy { get; set; }
        public Gender Gender { get; set; }
        public MentionType MentionType { get; set; }
    }
}
