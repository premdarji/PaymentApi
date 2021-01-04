using MailKit.Net.Smtp;
using MimeKit;
using Payment.Entity.DbModels;
using Payment.Entity.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;

using System.Text;
using System.Threading.Tasks;

namespace Payment.Domain
{
    public class EmailSender : IEmailSender
    {

       // private readonly EmailConfiguration _emailConfig;
        public EmailSender()
        {
            EmailConfiguration _emailConfig = new EmailConfiguration()
            {
                From = "darjiprem91@gmail.com",
                SmtpServer = "smtp.gmail.com",
                Port = 465,
                UserName = "darjiprem91@gmail.com",
                Password = "8980923279"
            };
            //_emailConfig = emailConfig;
        }
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        public async Task ConfirmationEmail(Message message,int id)
        {
            var emailMessage =await CreateEmailMessageWithInvoice(message,id);
            Send(emailMessage);
        }


        private async Task<MimeMessage> CreateEmailMessageWithInvoice(Message message,int id)
        {
            EmailConfiguration _emailConfig = new EmailConfiguration()
            {
                From = "darjiprem91@gmail.com",
                SmtpServer = "smtp.gmail.com",
                Port = 465,
                UserName = "darjiprem91@gmail.com",
                Password = "8980923279"
            };


           

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.Add(new MailboxAddress(message.To));
            emailMessage.Subject = message.Subject;

            var emailbuilder =new BodyBuilder();
            emailbuilder.Attachments.Add("C:\\Users\\Premal.Darji\\Downloads\\Invoice_"+id+".pdf");
            emailbuilder.HtmlBody = "This email contains the invoice for your order";
            //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format("<table class=table table-bordered><tr><th>Name</th><th>Price</th> <th>Quantity</th><th>Amount</th></tr>" + 


            //       "<tr> <td>Example</td> <td>800</td> <td>2</td> <td>1600</td> </tr></table> ", message.Content) };
            emailMessage.Body = emailbuilder.ToMessageBody();

            return emailMessage;
        }

       

        private MimeMessage CreateEmailMessage(Message message)
        {
            EmailConfiguration _emailConfig = new EmailConfiguration()
            {
                From = "darjiprem91@gmail.com",
                SmtpServer = "smtp.gmail.com",
                Port = 465,
                UserName = "darjiprem91@gmail.com",
                Password = "8980923279"
            };
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfig.From));
            emailMessage.To.Add(new MailboxAddress(message.To));
            emailMessage.Subject = message.Subject;


            //var emailbuilder = new BodyBuilder();
            //emailbuilder.Attachments.Add("D:\\MYPdf (1).pdf");
            //emailbuilder.HtmlBody = "This email contains the invoice for your order";

            //emailMessage.Body = emailbuilder.ToMessageBody();

            //return emailMessage;

            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) {Text=string.Format(message.Content) };
            //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format("<h2 style='color:red;'>{0}</h2>", message.Content) };
            return emailMessage;


        }
        private void Send(MimeMessage mailMessage)
        {
            EmailConfiguration _emailConfig = new EmailConfiguration()
            {
                From = "darjiprem91@gmail.com",
                SmtpServer = "smtp.gmail.com",
                Port = 465,
                UserName = "darjiprem91@gmail.com",
                Password = "8980923279"
            };
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }

    public interface IEmailSender
    {
        void SendEmail(Message message);

        Task ConfirmationEmail(Message message,int id);
    }
}
