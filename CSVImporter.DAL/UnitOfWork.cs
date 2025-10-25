using CSVImporter.DAL.Interface;
using CSVImporter.DAL.Repositories;
using CSVImporter.DAL.Repositories.Interfaces;

namespace CSVImporter.DAL
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ContextDBCSV _context;

        public ITripRepository TripRepository { get; private set; }

        private bool _disposed = false;

        public UnitOfWork(ContextDBCSV context)
        {
            _context = context;

            TripRepository = new TripRepository(context);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }

            this._disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}