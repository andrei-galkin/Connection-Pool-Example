using System;
using System.Collections.Generic;
using System.Threading;

namespace ConnectionPoolExample
{
    internal class Program
    {
        static List<IConnection> connections = new();

        static ConnectionPool connectionPool = new();

        static void Main(string[] args)
        {

            Console.WriteLine("Connection pool work simulation!");

            for (int i = 0; i < 5; i++)
            {
                connectionPool.Checkout();
            }

            for (int i = 0; i < 1000; i++)
            {
                Thread checkoutConnectionThread = new(CheckoutConnection);
                checkoutConnectionThread.Start();

                if (i % 2 == 0 || i % 7 == 0)
                {
                    Thread checkinConnectionThread = new(CheckinConnection);
                    checkinConnectionThread.Start();
                }
            }

            Console.WriteLine("Please press any key!");
            Console.ReadKey();
        }

        static void CheckoutConnection()
        {
            connections.Add(connectionPool.Checkout());
            Random r = new();
            int randomPeriod = r.Next(0, 5);

            Thread.Sleep(TimeSpan.FromSeconds(randomPeriod));
        }

        static void CheckinConnection()
        {
            Random r = new();
            int randomPeriod = r.Next(0, connections.Count - 1);

            connectionPool.Checkin(connections[randomPeriod]);

            Thread.Sleep(TimeSpan.FromSeconds(1));
        }
    }
}
