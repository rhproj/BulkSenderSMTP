using BulkSenderSMTP.Windows;
using MailKit.Net.Smtp;
using Microsoft.Win32;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

            if (ofd.ShowDialog() == true)
            {
                using (var sR = new StreamReader(ofd.FileName))
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
            #region wo-BGw
            //CheckForIllegalCrossThreadCalls = false;

            //declare the smtp object
            SmtpClient client = new SmtpClient();
            client.Timeout = 300000;
            client.AuthenticationMechanisms.Remove("XOAUTH2");

            for (int i = 0; i < emailList.Count; i++)
            {
                BodyBuilder builder = new BodyBuilder(); //Builds HTML body for sending
                builder.HtmlBody = messageList[i].ToString();

                //Define the mail headers
                MimeMessage mail = new MimeMessage();
                mail.Subject = tbSubject.Text;
                mail.Body = builder.ToMessageBody();

                client.Connect(tbServer.Text, int.Parse(tbPort.Text)); // cbSSL.Checked
                client.Authenticate(tbLogin.Text, pbPassword.Password);

                mail.From.Add(new MailboxAddress(tbFrom.Text, tbLogin.Text));
                mail.To.Add(new MailboxAddress(emailList[i])); //listBoxEMails.Items[i].ToString()));
                client.Send(mail);

                client.Disconnect(true);
            }
            #endregion

            MessageBox.Show("Done");
        }
    }
}
