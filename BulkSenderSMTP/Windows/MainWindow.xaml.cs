using BulkSenderSMTP.Windows;
using MailKit.Net.Smtp;
using Microsoft.Win32;
using MimeKit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        List<string> emailList; 
        List<string> messageList;
        BackgroundWorker backgroundWorker = new BackgroundWorker();

        public MainWindow()
        {
            InitializeComponent();

            backgroundWorker.DoWork += BackgroundWorker_DoWork;
            backgroundWorker.ProgressChanged += BackgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += BackgroundWorker_RunWorkerCompleted;

            backgroundWorker.WorkerReportsProgress = true;
        }

        private void BackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            ImgSend.Visibility = Visibility.Visible;
            btn_Send.IsEnabled = false;

            lblProgress.Content = e.ProgressPercentage;
        }

        private void BackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            Dispatcher.Invoke(
                new Action(() =>
                {
                    BulkSend();
                }));
        }

        private void BackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ImgSend.Visibility = Visibility.Hidden;
            btn_Send.IsEnabled = true;
            MessageBox.Show("Done");
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
                emailList = new List<string>();
                messageList = new List<string>();

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
            #region T T
            //new Task(BulkSend).Start();

            //Thread thread = new Thread(delegate ()
            //{
            //    BulkSend();
            //});
            //thread.IsBackground = true;
            //thread.Start(); 
            #endregion

            if (!backgroundWorker.IsBusy) //если bgW не занят:
            {
                backgroundWorker.RunWorkerAsync();
            }
        }


        private void BulkSend()
        {
            #region wo-BGw
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

                backgroundWorker.ReportProgress(i);

                Thread.Sleep(1000); ///TEST
            }

            client.Disconnect(true);
            #endregion
        }

        //private void BulkSend()
        //{
        //    //Dispatcher.BeginInvoke(new Action(() =>
        //    //{
        //    //    ImgSend.Visibility = Visibility.Visible;
        //    //}));

        //    #region wo-BGw
        //    //declare the smtp object
        //    SmtpClient client = new SmtpClient();
        //    client.Timeout = 300000;
        //    client.AuthenticationMechanisms.Remove("XOAUTH2");

        //    BodyBuilder builder = new BodyBuilder(); //Builds HTML body for sending
        //                                             //Define the mail headers
        //    MimeMessage mail = new MimeMessage();
        //    mail.Subject = tbSubject.Text;

        //    client.Connect(tbServer.Text, int.Parse(tbPort.Text)); // cbSSL.Checked
        //    client.Authenticate(tbLogin.Text, pbPassword.Password);

        //    for (int i = 0; i < emailList.Count; i++)
        //    {
        //        builder.HtmlBody = messageList[i].ToString();

        //        mail.Body = builder.ToMessageBody();

        //        mail.From.Add(new MailboxAddress(tbFrom.Text, tbLogin.Text));
        //        mail.To.Add(new MailboxAddress(emailList[i])); //listBoxEMails.Items[i].ToString()));
        //        client.Send(mail);

        //        Thread.Sleep(1000); ///TEST
        //    }

        //    client.Disconnect(true);
        //    #endregion

        //    //Dispatcher.BeginInvoke((Action)(() =>
        //    //{
        //    //    ImgSend.Visibility = Visibility.Hidden;
        //    //}));
        //}
    }
}
