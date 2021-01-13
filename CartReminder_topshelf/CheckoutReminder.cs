using Microsoft.EntityFrameworkCore;
using Payment.Domain;
using Payment.Entity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;
using Topshelf;

namespace CartReminder_topshelf
{
    public class CheckoutReminder:ServiceControl
    {

        private static System.Timers.Timer _aTimer;
        private const string _logFileLocation = @"D:\CartReminder\servicelog.txt";

        public CheckoutReminder()
        {

            //_aTimer = new System.Timers.Timer();
            //_aTimer.Interval = 60000;
            //_aTimer.Enabled = true;
            //_aTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnElapsedTime);
            //_aTimer.Start();
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            Log("Service is recall at " + DateTime.Now);
            SendReminder();
        }
        private void Log(string logMessage)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(_logFileLocation));
            File.AppendAllText(_logFileLocation, DateTime.UtcNow.ToString() + " : " + logMessage + Environment.NewLine);
        }

        public bool Start(HostControl hostControl)
        {
            _aTimer = new System.Timers.Timer();
            _aTimer.Interval = 60000;
            _aTimer.Enabled = true;
            _aTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnElapsedTime);
            _aTimer.Start();
            Log("Starting");
            return true;
        }

        public void SendReminder()
        {
            var contextOptions = new DbContextOptionsBuilder<ApplicationContext>()
                 .UseSqlServer(@"Server=DSK-869\SQL2017;Database=PaymentApp;User Id=sa;Password=Password12$; MultipleActiveResultSets=True;Trusted_Connection=False;Connection Timeout=10000; persist security info=True;")
                         .Options;

            var context = new ApplicationContext(contextOptions);
            //IConfiguration Configuration = new Configuration();
            //var emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            EmailSender email = new EmailSender();
            CartDomain cart = new CartDomain(context, email);
            cart.reminder();

        }
        public bool Stop(HostControl hostControl)
        {
            Log("Stopping");
            return true;
        }
    }
}
