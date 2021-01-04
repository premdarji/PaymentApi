using System;
using System.ServiceProcess;

namespace TestingService
{
    class Program
    {
        static void Main(string[] args)
        {
                ServiceBase.Run(new LogService());
        }
    }
}
