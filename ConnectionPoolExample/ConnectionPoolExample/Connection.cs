
namespace ConnectionPoolExample
{
    public class Connection : IConnection
    {
        private int _index;

        public Connection(int index)
        {
            _index = index;
        }

        public int Index => _index;

        public bool IsAvailable { get; set; }

        public void Dispose()
        {
            _index = 0;
        }
    }
}
