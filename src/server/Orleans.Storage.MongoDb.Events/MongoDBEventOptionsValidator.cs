namespace Orleans.Storage.MongoDb.Events
{
    public sealed class MongoDbEventOptionsValidator : IConfigurationValidator
    {
        private readonly MongoDBEventOptions options;
        private readonly string name;

        public MongoDbEventOptionsValidator(MongoDBEventOptions options, string name)
        {
            this.options = options;
            this.name = name;
        }

        public void ValidateConfiguration()
        {
            options.Validate(name);
        }
    }
}