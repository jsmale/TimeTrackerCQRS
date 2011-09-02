using System;
using System.Collections.Generic;
using System.Linq;

namespace TimeTrackerCQRS.ViewModel
{
    public interface IPersistentViewModel
    {
        void Insert<T>(T viewModel);
        IEnumerable<T> Query<T>();
        void Delete<T>(T viewModel);
    }

    public class PersistentViewModel : IPersistentViewModel
    {
        private readonly IDictionary<Type, List<object>> data;

        public PersistentViewModel()
        {
            data = new Dictionary<Type, List<object>>();
        }

        private List<object> GetTable<T>()
        {
            var type = typeof (T);
            if (data.ContainsKey(type))
            {
                return data[type];
            }
            var list = new List<object>();
            data[type] = list;
            return list;
        }

        public void Insert<T>(T viewModel)
        {
            GetTable<T>().Add(viewModel);
        }

        public IEnumerable<T> Query<T>()
        {
            return GetTable<T>().Cast<T>();
        }

        public void Delete<T>(T viewModel)
        {
            GetTable<T>().Remove(viewModel);
        }
    }
}