using System.Collections.Generic;
using Common.Models;

namespace TextAnalyzer
{
    public class TokenListPair
    {
        public List<Token> OriginalTokens { get;  }
        public List<Token> NewTokens { get;  }

        public TokenListPair(List<Token> originalTokens, List<Token> newTokens)
        {
            this.OriginalTokens = originalTokens;
            this.NewTokens = newTokens;
        }
    }
}
