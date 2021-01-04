using Microsoft.EntityFrameworkCore;
using Payment.Domain;
using Payment.Entity;
using PaymentAPI.Controllers;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Payment.Entity.ViewModels;
using System.IO;

namespace CartService
{
    public partial class CartCheckout:ServiceBase
    {
       


        Timer timer = new Timer();
        private static System.Timers.Timer _aTimer;

        public CartCheckout()
        {
            _aTimer = new System.Timers.Timer(30000);
            _aTimer.Enabled = true;
            _aTimer.Elapsed += new System.Timers.ElapsedEventHandler(OnElapsedTime);
        }

        protected  override  void OnStart(string[] args)
        {
            WriteToFile("Service is started at " + DateTime.Now);
            // SendReminder();
            this.Start();
            timer.Elapsed += new ElapsedEventHandler(OnElapsedTime);
            timer.Interval = 5000; //number in milisecinds  
            timer.Enabled = true;
            
        }

        protected override void OnStop()
        {
            WriteToFile("Service is stopped at " + DateTime.Now);
            this.Stope();
        }

        private void OnElapsedTime(object source, ElapsedEventArgs e)
        {
            WriteToFile("Service is recall at " + DateTime.Now);
            //SendReminder();
           

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
            cart.Reminder();

        }


        public void WriteToFile(string Message)
        {
            string path = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string filepath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs\\ServiceLog_" + DateTime.Now.Date.ToShortDateString().Replace('/', '_') + ".txt";
            if (!File.Exists(filepath))
            {
                // Create a file to write to.   
                using (StreamWriter sw = File.CreateText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
            else
            {
                using (StreamWriter sw = File.AppendText(filepath))
                {
                    sw.WriteLine(Message);
                }
            }
        }

        public void Check()
        {
            OnStart(null);
            //var contextOptions = new DbContextOptionsBuilder<ApplicationContext>()
            //  .UseSqlServer(@"Server=DSK-869\SQL2017;Database=PaymentApp;User Id=sa;Password=Password12$; MultipleActiveResultSets=True;Trusted_Connection=False;Connection Timeout=10000; persist security info=True;")
            //          .Options;

            //var context = new ApplicationContext(contextOptions);
            ////IConfiguration Configuration = new Configuration();
            ////var emailConfig = Configuration.GetSection("EmailConfiguration").Get<EmailConfiguration>();
            //EmailSender email = new EmailSender();
            //CartDomain cart = new CartDomain(context,email);
            //cart.Reminder();

            
        }


        public void Start()
        {
            _aTimer.Start();
        }
        //Custom method to Stop the timer
        public void Stope()
        {
            _aTimer.Stop();
        }
    }
}
