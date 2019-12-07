namespace TextAnalyzer.Models
{
    public class Properties
    {
        private readonly java.util.Properties underlyingModel;

        public Properties()
        {
            underlyingModel = new java.util.Properties();
        }

        public void SetProperty(string key, string value)
        {
            underlyingModel.setProperty(key, value);
        }

        public java.util.Properties GetUnderlyingModel()
        {
            return underlyingModel;
        }
    }
}
