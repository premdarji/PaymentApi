using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Payment.Domain;
using Payment.Entity;
using System;
using System.ServiceProcess;
using System.Threading;

namespace CartService
{
    class Program
    {
       


        static  void Main(string[] args)
        {


            //ServiceBase.Run(new CartCheckout());

            CartCheckout c = new CartCheckout();
            c.Start();
            Thread.Sleep(60000);
            c.Stope();


            //ServiceBase[] ServicesToRun;
            //ServicesToRun = new ServiceBase[]
            //{
            //    new CartCheckout()
            //};
            //ServiceBase.Run(ServicesToRun);
        }
    }
}
