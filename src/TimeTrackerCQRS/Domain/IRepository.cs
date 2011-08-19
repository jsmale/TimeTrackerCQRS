using System;

namespace TimeTrackerCQRS.Domain
{
    public interface IRepository
    {
        void Save(AggregateRoot aggregate, int expectedVersion);
        T GetById<T>(Guid id) where T : AggregateRoot, new();
    }
}