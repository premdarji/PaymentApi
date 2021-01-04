using System;
using Topshelf;

namespace CartReminder_topshelf
{
    class Program
    {
        static void Main(string[] args)
        {
            HostFactory.Run(x => x.Service<CheckoutReminder>());
        }
    }
}
