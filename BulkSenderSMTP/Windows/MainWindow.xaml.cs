using BulkSenderSMTP.Windows;
using MailKit.Net.Smtp;
using Microsoft.Win32;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;

namespace BulkSenderSMTP
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<string> emailList = new List<string>();
        List<string> messageList = new List<string>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_extractView_Click(object sender, RoutedEventArgs e)
        {
            AddressExtractor addressExtractor = new AddressExtractor();
            addressExtractor.Owner = this;
            addressExtractor.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            addressExtractor.Show();
        }

        private void btn_LoadCSV_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";

            if (ofd.ShowDialog() == true)
            {
                using (var sR = new StreamReader(ofd.FileName, Encoding.UTF8))
                {
                    while (!sR.EndOfStream)
                    {
                        string[] valueLine = sR.ReadLine().Split(';');
                        emailList.Add(valueLine[0]);
                        messageList.Add(valueLine[1]);
                    }

                    listVAddress.ItemsSource = emailList;
                    listVMessage.ItemsSource = messageList;
                }
            }
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            ImgSend.Visibility = Visibility.Visible;

            BulkSend();

            #region wo-BGw
            //CheckForIllegalCrossThreadCalls = false;

            //declare the smtp object
            SmtpClient client = new SmtpClient();
            client.Timeout = 300000;
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            BodyBuilder builder = new BodyBuilder(); //Builds HTML body for sending
                                                     //Define the mail headers
            MimeMessage mail = new MimeMessage();
            mail.Subject = tbSubject.Text;

            client.Connect(tbServer.Text, int.Parse(tbPort.Text)); // cbSSL.Checked
            client.Authenticate(tbLogin.Text, pbPassword.Password);

            for (int i = 0; i < emailList.Count; i++)
            {
                builder.HtmlBody = messageList[i].ToString();

                mail.Body = builder.ToMessageBody();

                mail.From.Add(new MailboxAddress(tbFrom.Text, tbLogin.Text));
                mail.To.Add(new MailboxAddress(emailList[i])); //listBoxEMails.Items[i].ToString()));
                client.Send(mail);

                //Thread.Sleep(1000);
            }

            #region BILO
            //for (int i = 0; i < emailList.Count; i++)
            //{
            //    BodyBuilder builder = new BodyBuilder(); //Builds HTML body for sending
            //    builder.HtmlBody = messageList[i].ToString();

            //    //Define the mail headers
            //    MimeMessage mail = new MimeMessage();
            //    mail.Subject = tbSubject.Text;
            //    mail.Body = builder.ToMessageBody();

            //    client.Connect(tbServer.Text, int.Parse(tbPort.Text)); // cbSSL.Checked
            //    client.Authenticate(tbLogin.Text, pbPassword.Password);

            //    mail.From.Add(new MailboxAddress(tbFrom.Text, tbLogin.Text));
            //    mail.To.Add(new MailboxAddress(emailList[i])); //listBoxEMails.Items[i].ToString()));
            //    client.Send(mail);

            //    Thread.Sleep(1000);
            //} 
            #endregion

            client.Disconnect(true);
            #endregion

            ImgSend.Visibility = Visibility.Hidden;
            MessageBox.Show("Done");
        }

        private void BulkSend()
        {
            throw new NotImplementedException();
        }
    }
}
