using EventStore;
using EventStore.Persistence;
using EventStore.Persistence.RavenPersistence;
using EventStore.Serialization;
using Raven.Client;
using Raven.Client.Embedded;

namespace TimeTrackerCQRS.Messaging
{
    public class MyRavenPersistenceWireup : PersistenceWireup
    {
        public MyRavenPersistenceWireup(Wireup inner) : base(inner)
        {
            Container.Register(c => new MyRavenPersistenceFactory().Build());
        }
    }

    public static class MyRavenPersistenceWireupExtensions
    {
        public static MyRavenPersistenceWireup UsingMyRavenPersistence(this Wireup wireup)
        {
            return new MyRavenPersistenceWireup(wireup);
        }
    }

    public class MyRavenPersistenceFactory : IPersistenceFactory
    {
        static IDocumentStore documentStore;

        public IPersistStreams Build()
        {
            return new RavenPersistenceEngine(GetDocumentStore(), new DocumentObjectSerializer(), true);
        }

        IDocumentStore GetDocumentStore()
        {
            if (documentStore != null)
            {
                return documentStore;
            }

            documentStore = new EmbeddableDocumentStore
            {
                DataDirectory = "EventData"
            };
            documentStore.Initialize();
            return documentStore;
        }
    }
}