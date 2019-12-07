using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.Models
{
    public class Sentence : ICloneable
    {
        /// <summary>
        /// Number of the sentence
        /// </summary>
        public int Number { get; set; }
        public string Text { get; set; }
        public List<Token> Tokens { get; set; }
        public Tree Tree { get; set; }
        public SemanticGraph SemanticGraph { get; set; }
        public object Clone()
        {
            return new Sentence
            {
                Number = this.Number,
                Text = this.Text,
                Tree = (Tree) this.Tree.Clone(),
                SemanticGraph = (SemanticGraph) this.SemanticGraph.Clone(),
                Tokens = this.Tokens.Select(token => (Token) token.Clone()).ToList()
            };
        }
    }
}
