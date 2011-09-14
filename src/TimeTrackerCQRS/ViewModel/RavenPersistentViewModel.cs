using System.Collections.Generic;
using Raven.Client;

namespace TimeTrackerCQRS.ViewModel
{
    public class RavenPersistentViewModel : IPersistentViewModel
    {
        readonly IDocumentSession session;

        public RavenPersistentViewModel(IDocumentSession session)
        {
            this.session = session;
        }

        public void Insert<T>(T viewModel)
        {
            session.Store(viewModel);
        }

        public IEnumerable<T> Query<T>()
        {
            return session.Query<T>();
        }

        public void Delete<T>(T viewModel)
        {
            session.Delete(viewModel);
        }

        public void SaveChanges()
        {
            session.SaveChanges();
        }

        public void Dispose()
        {
            session.Dispose();
        }
    }
}