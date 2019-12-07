using TextAnalyzer.Models;

namespace TextAnalyzer
{
    public interface IPropertyBuilder
    {
        IPropertyBuilder SetAnnotators(string[] annotations);
        IPropertyBuilder SetKeyValuePairs(string key, string value);
        Properties Build();
    }
}
