using System;

namespace TimeTrackerCQRS.ViewModel
{
    public interface IPersistentViewModelFactory
    {
        IPersistentViewModel GetPersitentViewModel();
    }
}