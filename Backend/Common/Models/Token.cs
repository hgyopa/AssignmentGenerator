using System;

namespace Common.Models
{
    public class Token : ICloneable
    {
        public int Number { get; set; }
        public string Text { get; set; }
        public string Pos { get; set; }
        public string Lemma { get; set; }
        public string NamedEntityTag { get; set; }
        public object Clone()
        {
            return new Token
            {
                Number = this.Number,
                Text = this.Text,
                Pos = this.Pos,
                Lemma = this.Lemma,
                NamedEntityTag = this.NamedEntityTag
            };
        }
    }
}
