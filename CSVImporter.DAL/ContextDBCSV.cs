using Microsoft.Data.SqlClient;
using System.Data.Common;
using System.Data;

namespace CSVImporter.DAL
{
    public class ContextDBCSV : IDisposable
    {
        private SqlConnection Connection { get; set; }
        private SqlCommand _command;
        private SqlDataReader _dataReader;
        private bool _disposed = false;

        public ContextDBCSV(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
            Connection.Open();
        }

        public async Task<DbDataReader> GetReader(string query)
        {
            _command = new SqlCommand(query, Connection);
            _dataReader = (SqlDataReader)await _command.ExecuteReaderAsync();

            return _dataReader;
        }

        public DbDataReader GetReaderNonAsync(string query)
        {
            _command = new SqlCommand(query, Connection);
            _dataReader = (SqlDataReader)_command.ExecuteReader();

            return _dataReader;
        }

        public async Task<int> ExecuteNonQueryAsync(string query)
        {
            _command = new SqlCommand(query, Connection);

            return await _command.ExecuteNonQueryAsync();
        }

        public int ExecuteNonQuery(string query)
        {
            _command = new SqlCommand(query, Connection);

            return _command.ExecuteNonQuery();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposed)
            {
                if (disposing)
                {
                    if (Connection?.State != ConnectionState.Closed)
                    {
                        Connection?.Close();
                    }
                    Connection?.Dispose();
                }
            }
            this._disposed = true;
        }

        public SqlConnection GetConnection()
        {
            return Connection;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}