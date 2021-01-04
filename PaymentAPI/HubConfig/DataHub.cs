using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PaymentAPI.HubConfig
{
    public  class DataHub:Hub
    {
        string tempstring;
        public async Task AskServer(string message)
        {
            if (message == "hey")
            {
                tempstring = "Greetings from admin";
            }
            else
            {
                tempstring = "Greetings from User";
            }

            await  this.Clients.All.SendAsync("transferAdmindata", tempstring);
        }

    }
}
