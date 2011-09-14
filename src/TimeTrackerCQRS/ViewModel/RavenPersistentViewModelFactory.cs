using Raven.Client;
using Raven.Client.Embedded;

namespace TimeTrackerCQRS.ViewModel
{
    public class RavenPersistentViewModelFactory : IPersistentViewModelFactory
    {
        static IDocumentStore documentStore;

        public IPersistentViewModel GetPersitentViewModel()
        {
            if (documentStore == null)
            {
                documentStore = new EmbeddableDocumentStore
                {
                    DataDirectory = "ViewModelData"
                };
                documentStore.Initialize();                
            }
            return new RavenPersistentViewModel(documentStore.OpenSession());
        }
    }
}