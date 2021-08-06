using MailKit.Net.Smtp;
using MimeKit;
using System.Collections.Generic;

namespace BulkSenderSMTP.Services
{
    public static class SmtpSender
    {
        public static void BulkSend(string smtpServer, int smtpPort, string userName, string login, string password, string letterSubject, 
            IList<string> emailList, IList<string> messageList)
        {
            #region SmtpClient
            //declare the smtp object
            SmtpClient client = new SmtpClient();
            client.Timeout = 300000;
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            BodyBuilder builder = new BodyBuilder(); //Builds HTML body for sending
                                                     //Define the mail headers
            MimeMessage mail = new MimeMessage();
            mail.Subject = letterSubject;

            client.Connect(smtpServer, smtpPort); // cbSSL.Checked
            client.Authenticate(login, password);

            for (int i = 0; i < emailList.Count; i++)
            {
                builder.HtmlBody = messageList[i].ToString();

                mail.Body = builder.ToMessageBody();

                mail.From.Add(new MailboxAddress(userName, login));
                mail.To.Add(new MailboxAddress(emailList[i]));
                client.Send(mail);
            }

            client.Disconnect(true);
            #endregion
        }
    }
}
