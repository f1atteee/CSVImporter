namespace CSVImporter.DAL.Repositories
{
    public abstract class BaseRepository
    {
        protected ContextDBCSV _context;

        public BaseRepository(ContextDBCSV context)
        {
            _context = context;
        }
    }
}