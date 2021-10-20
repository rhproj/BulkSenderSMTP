using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BulkSenderSMTP.Services
{
    public static class SmtpSender
    {
        internal static string SendBulk(string smtpServer, int smtpPort, string userName, string login, string password, string letterSubject,
            IList<string> emailList, IList<string> messageList)
        {
            //declare the smtp object
            SmtpClient client = new SmtpClient();
            client.Timeout = 300000;
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            BodyBuilder builder = new BodyBuilder(); //Builds HTML body for sending
                                                     //Define the mail headers
            MimeMessage mail = new MimeMessage();
            mail.Subject = letterSubject;

            try
            {
                client.Connect(smtpServer, smtpPort); // cbSSL.Checked
                client.Authenticate(login, password);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            for (int i = 0; i < emailList.Count; i++)
            {
                builder.HtmlBody = messageList[i].ToString();

                mail.Body = builder.ToMessageBody();

                mail.From.Add(new MailboxAddress(userName, login));
                mail.To.Add(new MailboxAddress(emailList[i]));

                try
                {
                    client.Send(mail);
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
            }

            client.Disconnect(true);
            return $"{emailList.Count} e-mails has been sent";
        }
    }
}
