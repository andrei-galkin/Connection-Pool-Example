using System;

namespace ConnectionPoolExample
{
    public interface IConnection : IDisposable
    {
        int Index { get; }
        bool IsAvailable { get; set; }
    }
}
