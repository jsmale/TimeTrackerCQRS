using System;
using System.Collections.Generic;

namespace TimeTrackerCQRS.ViewModel
{
    public interface IPersistentViewModel : IDisposable
    {
        void Insert<T>(T viewModel);
        IEnumerable<T> Query<T>();
        void Delete<T>(T viewModel);
        void SaveChanges();
    }
}