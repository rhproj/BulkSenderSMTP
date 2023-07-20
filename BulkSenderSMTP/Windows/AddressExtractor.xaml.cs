using BulkSenderSMTP.Services;
using System;
using System.Collections.Generic;
using System.Windows;

namespace BulkSenderSMTP.Windows
{
    /// <summary>
    /// Extracts mail addresses striping up raw data source
    /// </summary>
    public partial class AddressExtractor : Window
    {
        public AddressExtractor()
        {
            InitializeComponent();
        }

        private void btnExtract_Click(object sender, RoutedEventArgs e)
        {
            IEnumerable<string> mailList = tbRawAddresses.Text.TakeStringInBetween(tbStartDelim.Text, tbEndDelim.Text);

            tbRawAddresses.Text = string.Join(Environment.NewLine,mailList);
        }
    }
}
