using System;

namespace Common.Models
{
    public class SemanticGraph : ICloneable
    {
        public object Clone()
        {
            return new SemanticGraph();
        }
    }
}
