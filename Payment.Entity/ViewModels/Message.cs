using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Payment.Entity.ViewModels
{
    public class Message
    {
        //public List<MailboxAddress> To { get; set; }
        public string To { get; set;  }
        public string Subject { get; set; }
        public string Content { get; set; }

      //  public string Attachments { get; set; }
        public Message(string to, string subject, string content)
        {
            //To = new List<MailboxAddress>();
            //To.AddRange(to.Select(x => new MailboxAddress(x)));
            To = to;
            Subject = subject;
            Content = content;
          //  Attachments = attachments;
        }
    }
}
