using System;

namespace ConnectionPoolExample
{
    public class ConnectionPool : IConnectionPool
    {
        private const int _maxPoolSize = 10;
        private int _connectionCount = 0;
        private IConnection[] _connections = new IConnection[_maxPoolSize];
        private readonly object _connectionLock = new object();
        private IConnection _connection;

        public void Checkin(IConnection connection)
        {
            if (connection == null) return;

            try
            {
                lock (_connectionLock)
                {
                    Console.WriteLine($"Checkin connection {connection.Index + 1}.");

                    _connections[connection.Index].IsAvailable = true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public IConnection Checkout()
        {
            try
            {
                if (_connection == null)
                {
                    lock (_connectionLock)
                    {
                        _connection = new Connection(_connectionCount)
                        {
                            IsAvailable = false
                        };
                        _connections[_connection.Index] = _connection;
                        _connectionCount++;
                        Console.WriteLine($"Checkout connection {_connection.Index + 1}.");
                    }

                    return _connection;
                }

                if (_connection.IsAvailable)
                {
                    _connection.IsAvailable = false;
                    return _connection;
                }
                else if (_connectionCount <= _maxPoolSize - 1)
                {
                    lock (_connectionLock)
                    {
                        if (_connectionCount > _maxPoolSize - 1)
                        {
                            _connectionCount = _maxPoolSize - 1;
                        }

                        _connection = new Connection(_connectionCount)
                        {
                            IsAvailable = false
                        };

                        _connections[_connection.Index] = _connection;

                        _connectionCount++;

                        Console.WriteLine($"Checkout connection {_connection.Index + 1}.");
                    }
                    return _connection;
                }

                int index = 0;
                IConnection connection = _connection;
                while (!connection.IsAvailable)
                {
                    connection = _connections[index];
                    index++;

                    if (index == _maxPoolSize)
                    {
                        index = 0;
                    }
                }

                _connection = connection;

                return _connection;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Log error : {ex}");
                throw;
            }
        }
    }
}
