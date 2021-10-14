﻿using BulkSenderSMTP.Services;
using BulkSenderSMTP.Windows;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace BulkSenderSMTP
{
    public partial class MainWindow : Window
    {
        private string letterSubject;
        private string smtpServer;
        private int smtpPort;
        private string userLogin;
        private string password;
        private string userName;
        private IList<string> emailList;
        private IList<string> messageList;

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
            if (MailFieldsInitializer() == false)
            {
                MessageBox.Show("Please fill in all the required fields");
                return;
            }
            else
            {
                Send();
            }
        }

        private bool MailFieldsInitializer()
        {
            if (!string.IsNullOrWhiteSpace(tbServer.Text) && !string.IsNullOrWhiteSpace(tbPort.Text) && !string.IsNullOrWhiteSpace(tbLogin.Text) && !string.IsNullOrWhiteSpace(pbPassword.Password))
            {
                smtpServer = tbServer.Text;
                smtpPort = int.Parse(tbPort.Text);
                userName = tbFrom.Text;
                userLogin = tbLogin.Text;
                password = pbPassword.Password;
                letterSubject = tbSubject.Text;

                return true;
            }
            else
            {
                return false;
            }
        }

        private void Send()
        {
            try
            {
                Task.Run(() => 
                {
                    Dispatcher.BeginInvoke(new Action(() =>
                    {
                        ImgSend.Visibility = Visibility.Visible;
                        Mouse.OverrideCursor = Cursors.Wait;
                    }));

                    SmtpSender.BulkSend(smtpServer, smtpPort, userName, userLogin, password, letterSubject, emailList, messageList);

                    Dispatcher.BeginInvoke((Action)(() =>
                    {
                        ImgSend.Visibility = Visibility.Hidden;
                        Mouse.OverrideCursor = null;
                    }));
                    MessageBox.Show($"{messageList.Count} e-mails has been sent");
                });

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);

                Dispatcher.BeginInvoke((Action)(() =>
                {
                    ImgSend.Visibility = Visibility.Hidden;
                    Mouse.OverrideCursor = null;
                }));
            }
        }
    }
}
