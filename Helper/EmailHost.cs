using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GumTree.Helper
{
    class EmailHost
    {
        public static HostDetails GetHostDetails(string Domain)
        {
            HostDetails HD = new HostDetails();

            switch (Domain)
            {
                case "yahoo.com":
                    HD.Host = "pop.mail.yahoo.com";
                    HD.Port = 995;
                    break;
                case "gmail.com":
                    HD.Host = "pop.gmail.com";
                    HD.Port = 995;
                    break;
                default:
                    HD.Host = "rsb16.rhostbh.com";
                    HD.Port = 995;
                    break;
            }

            return HD;
        }
    }

    public class HostDetails
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
