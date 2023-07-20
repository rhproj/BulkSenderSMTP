using BulkSenderSMTP.Models;
using BulkSenderSMTP.Services;
using BulkSenderSMTP.Windows;
using Microsoft.Win32;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BulkSenderSMTP
{
    public partial class MainWindow : Window
    {
        SmtpModel smtp;
        UserModel user;
        MailModelDetails mailDetails;
        MailModelCollections mailCollections;
        SmtpSender smtpSender;

        public MainWindow()
        {
            InitializeComponent();
            smtpSender = new SmtpSender();
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
            mailCollections = new MailModelCollections();
            OpenFileDialog ofd = SetOpenFileFilter();

            if (ofd.ShowDialog() == true)
            {
                using (var sR = new StreamReader(ofd.FileName, Encoding.UTF8))
                {
                    while (!sR.EndOfStream)
                    {
                        string[] valueLine = sR.ReadLine().Split(';');
                        mailCollections.EmailList.Add(valueLine[0]);
                        mailCollections.MessageList.Add(valueLine[1]);
                    }

                    listVAddress.ItemsSource = mailCollections.EmailList;
                    listVMessage.ItemsSource = mailCollections.MessageList;
                }
            }
        }

        private OpenFileDialog SetOpenFileFilter()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Multiselect = false;
            ofd.Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*";
            return ofd;
        }

        private async void Send_Click(object sender, RoutedEventArgs e)
        {
            if (AreMailModelsSet() == false)
            {
                MessageBox.Show("Please fill in all the required fields");
                return;
            }

            ImgSend.Visibility = Visibility.Visible;
            Mouse.OverrideCursor = Cursors.Wait;

            await Task.Run(() =>
            {
                string result = smtpSender.SendEmailsInBulk(smtp,user,mailCollections,mailDetails);
                MessageBox.Show(result);
            });

            ImgSend.Visibility = Visibility.Hidden;
            Mouse.OverrideCursor = null;
        }

        private bool AreMailModelsSet()
        {
            if (AreMailFieldsFilled())
            {
                smtp = new SmtpModel(tbServer.Text, int.Parse(tbPort.Text));
                user = new UserModel(tbFrom.Text, tbLogin.Text, pbPassword.Password);
                mailDetails = new MailModelDetails(tbSubject.Text);

                return true;
            }
            return false;
        }

        private bool AreMailFieldsFilled()
        {
            return !string.IsNullOrWhiteSpace(tbServer.Text) && 
                   !string.IsNullOrWhiteSpace(tbPort.Text) &&
                   !string.IsNullOrWhiteSpace(tbLogin.Text) && 
                   !string.IsNullOrWhiteSpace(pbPassword.Password) &&
                   mailCollections.EmailList != null && 
                   mailCollections.EmailList.Count == mailCollections.MessageList.Count;
        }
    }
}
