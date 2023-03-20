namespace ConnectionPoolExample
{
    public interface IConnectionPool
    {
        IConnection Checkout();
        void Checkin(IConnection connection);
    }
}
