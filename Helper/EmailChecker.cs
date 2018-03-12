using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenPOP.POP3;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace GumTree.Helper
{
    class EmailChecker
    {
        private readonly POPClient popClient = new POPClient();
        string responce = string.Empty;

        //public static MyEvents loggerEvents = new MyEvents();

        //private void Log(string log)
        //{
        //    //EventsArgs args = new EventsArgs(log);
        //    //loggerEvents.LogText(args);       
        //}



        public string EmailVerification(string Email, string Password, out bool IsLogin)
        {
            string Host = string.Empty;
            string InviteUrl = string.Empty;
            int Port = 0;

            MailAddress address = new MailAddress(Uri.UnescapeDataString(Email));
            string EmailHostName = address.Host.ToLower();

            HostDetails HD = EmailHost.GetHostDetails(EmailHostName);

            Host = HD.Host;
            Port = HD.Port;

            if (!string.IsNullOrEmpty(Host))
            {
                if (popClient.Connected)
                    popClient.Disconnect();
                popClient.Connect(Host, Port, true);
                popClient.Authenticate(Uri.UnescapeDataString(Email), Password.Trim(), AuthenticationMethod.USERPASS);
                int Count = popClient.GetMessageCount();
                IsLogin = true;

                for (int i = Count; i >= 1; i--)
                {
                    OpenPOP.MIME.Message Message = popClient.GetMessage(i);
                    string subject = string.Empty;
                    subject = Message.Headers.Subject;
                    bool contains = subject.IndexOf("Please activate your new Gumtree account", StringComparison.OrdinalIgnoreCase) >= 0;

                    if (contains)
                    {
                        if (string.IsNullOrEmpty(InviteUrl))
                        {
                            InviteUrl = GetInviteUrlFromString(Message.RawMessageBody);
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
            }
            else
            {
                IsLogin = false;
            }

            return InviteUrl;
        }

        //public string GetRequiredEmail(string Email, string Password, out bool IsLogin,string Subject)
        //{
        //    string Host = string.Empty;
        //    string MessageBody = string.Empty;
        //    string Subject2 = "Verificar tu cuenta";
        //    string Subject3 = "التحقق من حسابك";
        //    string Subject4 = "Vérifier votre compte";
        //    int Port = 0;

        //    MailAddress address = new MailAddress(ConvertData.UnescapeData(Email));
        //    string EmailHostName = address.Host.ToLower();

        //    HostDetails HD = EmailHost.GetHostDetails(EmailHostName);

        //    Host = HD.Host;
        //    Port = HD.Port;

        //    if (!string.IsNullOrEmpty(Host))
        //    {
        //        if (popClient.Connected)
        //            popClient.Disconnect();
        //        popClient.Connect(Host, Port, true);
        //        popClient.Authenticate(ConvertData.UnescapeData(Email), Password.Trim());
        //        int Count = popClient.GetMessageCount();
        //        IsLogin = true;

        //        for (int i = Count; i >= 1; i--)
        //        {
        //            OpenPOP.MIME.Message Message = popClient.GetMessage(i);
        //            string subject = string.Empty;
        //            subject = Message.Headers.Subject;
        //            bool contains = subject.IndexOf(Subject, StringComparison.OrdinalIgnoreCase) >= 0;
        //            if (!contains)
        //            {
        //                contains = subject.IndexOf(Subject2, StringComparison.OrdinalIgnoreCase) >= 0;
        //            }
        //            if (!contains)
        //            {
        //                contains = subject.IndexOf(Subject3, StringComparison.OrdinalIgnoreCase) >= 0;
        //            }
        //            if (!contains)
        //            {
        //                contains = subject.IndexOf(Subject4, StringComparison.OrdinalIgnoreCase) >= 0;
        //            }
        //            if (contains)
        //            {
        //                //if (string.IsNullOrEmpty(MessageBody))
        //                //{
        //                MessageBody = Message.MessageBody[0];
        //                break;
        //                //}
        //                //else
        //                //{
        //                //    break;
        //                //}
        //            }
        //            //else
        //            //{
        //            //    InviteUrl = "Invite Url Not Found";
        //            //}
        //        }
        //    }
        //    else
        //    {
        //        IsLogin = false;
        //    }

        //    return MessageBody;
        //}

        public string GetInviteUrlFromString(string HtmlData)
        {
            System.Windows.Forms.Application.DoEvents();
            string InviteUrl = string.Empty;

            string strData = HtmlData;
            string[] arr = Regex.Split(strData, "<a href=");

            //Log("Fetching url for email verification ");

            foreach (string strhref in arr)
            {
                if (!strhref.Contains("<!DOCTYPE"))
                {
                    if (strhref.Contains("https://my.gumtree.com/activate-user"))
                    {
                        InviteUrl = strhref.Substring(0, strhref.IndexOf("style")).Replace("\"",string.Empty).Trim();
                        break;
                    }
                }
            }

            return InviteUrl;
        }

        /// <summary>
        /// This method is use for activation
        /// </summary>
        /// <param name="ActivationUrl">url of activate link</param>
        /// <returns>bool true/false</returns>
        //public bool ActivationProcess(string ActivationUrl)
        //{
        //    bool IsActivated = false;

        //    try
        //    {
        //        Chilkat.Http http = new Chilkat.Http();

        //        ///Chilkat Http Request to be used in Http Post...
        //        Chilkat.HttpRequest req = new Chilkat.HttpRequest();
        //        bool success;

        //        // Any string unlocks the component for the 1st 30-days.
        //        success = http.UnlockComponent("THEBACHttp_b3C9o9QvZQ06");
        //        if (success != true)
        //        {
        //            Console.WriteLine(http.LastErrorText);
        //        }
        //        http.CookieDir = "memory";
        //        http.SendCookies = true;
        //        http.SaveCookies = true;



        //        http.SetRequestHeader("Accept-Encoding", "gzip,deflate");


        //        //complete url with website and address
        //        string siteurl = "http://blog.com/wp-login.php";
        //        //siteurl = Website;

        //        //  Send the HTTP GET and return the content in a string.
        //        string html = null;
        //        //html = http.QuickGetStr(siteurl);
        //        string actvateUrl = ActivationUrl.Substring(ActivationUrl.IndexOf("http"));

        //        html = http.QuickGetStr(actvateUrl);

        //        if (responce.Contains("Your account is now active"))
        //        {
        //            Log("Account activated");
        //            IsActivated = true;
        //        }





        //        if (!string.IsNullOrEmpty(html))
        //        {
        //            if (html.Contains("Your account is now active") || html.Contains("Back to"))
        //            {
        //                //                   Log("blog account verified");

        //                return true;
        //            }
        //            else
        //            {
        //                //                  Log("blog account not verified");
        //                return false;
        //            }
        //        }
        //        else
        //        {

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        string ErrorMessage = "blogCom   [3] EmailVerification " + DateTime.Now.ToString() + " Error" + ex.Message;
        //        // ClsCsvWriter.AddLoggerFile(ErrorMessage);

        //    }


        //    return IsActivated;

        //}
    }
}
