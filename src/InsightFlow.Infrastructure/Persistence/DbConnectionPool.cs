using System.Collections.Concurrent;
using System.Data;
using InsightFlow.Common.Constants;
using InsightFlow.Infrastructure.Common.Configurations;
using InsightFlow.Infrastructure.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Options;

namespace InsightFlow.Infrastructure.Persistence;

public sealed class DbConnectionPool : IDbConnectionPool
{
    private static DbConnectionPool? _instance;

    private readonly ConcurrentBag<IDbConnection> _connections = new();
    private readonly string _connectionString;
    private readonly string _databaseProviderName;

    private DbConnectionPool(string connectionString, string databaseProviderName)
    {
        _connectionString = connectionString;
        _databaseProviderName = databaseProviderName;
    }

    public static void Initialize(string connectionString, string databaseProviderName) =>
        _instance ??= new DbConnectionPool(connectionString, databaseProviderName);

    public static DbConnectionPool Instance => _instance!;

    public IDbConnection GetConnection()
    {
        if (_connections.TryTake(out var connection) && connection.State == ConnectionState.Open)
            return connection;

        if (_databaseProviderName.Equals(StringConstants.SqlServer, StringComparison.InvariantCultureIgnoreCase))
        {
            var newConnection = new SqlConnection(_connectionString);

            newConnection.Open();

            return newConnection;
        }

        var sqliteConnection = new SqliteConnection(_connectionString);

        sqliteConnection.Open();

        return sqliteConnection;
    }

    public void ReturnConnection(IDbConnection connection)
    {
        if (connection.State == ConnectionState.Open)
            _connections.Add(connection);
        else
            connection.Dispose();
    }
}