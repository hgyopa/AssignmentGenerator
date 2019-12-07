using System;

namespace Common.Models
{
    public class Tree : ICloneable
    {
        public object Clone()
        {
            return new Tree();
        }
    }
}