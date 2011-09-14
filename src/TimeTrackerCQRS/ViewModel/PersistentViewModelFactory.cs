namespace TimeTrackerCQRS.ViewModel
{
    public class PersistentViewModelFactory : IPersistentViewModelFactory
    {
        static readonly IPersistentViewModel persistentViewModel = new PersistentViewModel();

        public IPersistentViewModel GetPersitentViewModel()
        {
            return persistentViewModel;
        }
    }
}