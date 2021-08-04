using BulkSenderSMTP.Services;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace BulkSenderSMTP.Windows
{
    /// <summary>
    /// Interaction logic for AddressExtractor.xaml
    /// </summary>
    public partial class AddressExtractor : Window
    {
        public AddressExtractor()
        {
            InitializeComponent();
            //ComboDelimiterFiller();
        }

        //private void ComboDelimiterFiller()
        //{
        //    comboDelimiter.Items.Add("<>");
        //    comboDelimiter.Items.Add(@"""");
        //    comboDelimiter.Items.Add("{}");
        //    comboDelimiter.Items.Add("[]");
        //    comboDelimiter.Items.Add("()");
        //}



        private void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            List<string> mailList = tbRawAddresses.Text.EverythingBetween(tbStartDelim.Text, tbEndDelim.Text);

            tbRawAddresses.Text = string.Join(Environment.NewLine,mailList);

            //foreach (var adress in results)
            //{
            //    listBoxEMails.Items.Add(adress);
            //}
        }
    }
}
