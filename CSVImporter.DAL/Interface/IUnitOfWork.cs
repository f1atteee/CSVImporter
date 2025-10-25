using CSVImporter.DAL.Repositories.Interfaces;

namespace CSVImporter.DAL.Interface
{
    public interface IUnitOfWork
    {
        ITripRepository TripRepository { get; }

        void Dispose();
    }
}