using Core.ContextDatabase;
using MongoDB.Driver;

namespace CoreTests.InitTests.ContextTest
{
    public class ContextTest : BaseContext
    {
        public sealed override string ConnectionString { get; } = "mongodb://127.0.0.1:27017";
        public sealed override string DatabaseName { get; } = "DataBaseTest";

        public ContextTest()
        {
            Client = new MongoClient(ConnectionString);
            MongoDatabase = Client.GetDatabase(DatabaseName);
        }
    }
}
