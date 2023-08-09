using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace Election_Saver
{
    /// <summary>
    /// Interaction logic for UpdateConfirmation.xaml
    /// </summary>
    public partial class UpdateConfirmation : Window
    {
        public UpdateConfirmation()
        {
            InitializeComponent();
        }

        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            //string msiDownloadLink = "https://github.com/NoahMoyer/Election-file-saver/releases/latest/download/Election.Saver.Setup.msi";
            //Process.Start("Explorer", msiDownloadLink);

            Process.Start(@"C:\ProgramData\Election Saver\updater.exe");
        }

        private void noButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
