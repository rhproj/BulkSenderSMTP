using BulkSenderSMTP.Windows;
using Microsoft.Win32;
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
                    //string line;
                    //while ((line = sR.ReadLine()) != null)
                    //{
                    //    listBoxBody.Items.Add(line);
                    //}

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


            //_bodies = listBoxBody.Items.Count;
            //lblBody.Text = _bodies.ToString();

            //using (StreamReader streamReader = new StreamReader(path, Encoding.Default))
            //{
            //    while (!streamReader.EndOfStream)
            //    {
            //        pBoxId++;
            //        string[] valueLine = streamReader.ReadLine().Split(';');

            //        ipList.Add(valueLine[1]);
            //    }
            //    backgroundWorker1.RunWorkerAsync();
            //}
        }
    }
}
