using BulkSenderSMTP.Windows;
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
    }
}
