using BulkSenderSMTP.Models;
using MailKit.Net.Smtp;
using MimeKit;
using System;

namespace BulkSenderSMTP.Services
{
    public class SmtpSender
    {
        private readonly BodyBuilder builder; 
        private readonly MimeMessage mimeMessage;
        private readonly SmtpClient client;

        public SmtpSender()
        {
            builder = new();
            mimeMessage = new();
            client = SetSmtpClient();
        }

        public string SendEmailsInBulk(SmtpModel smtp, UserModel user, MailModelCollections mailCollections, MailModelDetails mailDetails)
        {
            try
            {
                ConnectSmtpClient(smtp, user);

                mimeMessage.Subject = mailDetails.LetterSubject;

                for (int i = 0; i < mailCollections.EmailList.Count; i++)
                {
                    builder.HtmlBody = mailCollections.MessageList[i].ToString();

                    mimeMessage.Body = builder.ToMessageBody();

                    mimeMessage.From.Add(new MailboxAddress(user.UserName, user.UserLogin));
                    mimeMessage.To.Add(new MailboxAddress(mailCollections.EmailList[i]));

                    client.Send(mimeMessage);
                }

                client.Disconnect(true);
                return $"{mailCollections.EmailList.Count} e-mails has been sent";

            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void ConnectSmtpClient(SmtpModel smtp, UserModel user)
        {
            client.Connect(smtp.SmtpServer, smtp.SmtpPort);
            client.Authenticate(user.UserLogin, user.Password);
        }

        private static SmtpClient SetSmtpClient()
        {
            SmtpClient client = new SmtpClient();
            client.Timeout = 300000;
            client.AuthenticationMechanisms.Remove("XOAUTH2");
            return client;
        }
    }
}
