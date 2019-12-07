using TextAnalyzer.Models;

namespace TextAnalyzer
{
    public class PropertyBuilder : IPropertyBuilder
    {
        private readonly Properties properties;

        public PropertyBuilder()
        {
            properties = new Properties();
        }

        public IPropertyBuilder SetAnnotators(string[] annotations)
        {
            properties.SetProperty("annotators", string.Join(", ", annotations));
            return this;
        }

        public IPropertyBuilder SetKeyValuePairs(string key, string value)
        {
            properties.SetProperty(key,value);
            return this;
        }

        public Properties Build()
        {
            return properties;
        }
    }
}
