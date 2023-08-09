using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
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

namespace Updater
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class UpdaterForm : Window
    {
        //Begin logging info
        string logFile = "log.txt";
        string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
        //End logging info

        //path to main exe. This is used to re-launch the main program after the update is comlete.
        string mainExe = @"C:\Program Files\Election Saver\Election Saver.exe";

        string installerPath = @"C:\ProgramData\Election Saver\";
        string installerName = "Election.Saver.Setup.msi";

        public UpdaterForm()
        {
            InitializeComponent();

            //Begin logging info
            File.WriteAllTextAsync(logFile, "");
            log("Initializing form");
            //End logging info

            

            //load form
            //Without this function the update process would not start.
            Loaded += MainWindow_Loaded;
        }

        /// <summary>
        /// Function to standardize logging data format.
        /// </summary>
        /// <param name="logData"></param>
        void log(string logData)
        {
            File.AppendAllTextAsync(logFile, "[" + DateTime.Now + "] " + "[User name: " + userName + "] Log: " + logData + "\n");
        }

        /// <summary>
        /// Reset UI back to a default state.
        /// </summary>
        /// <param name="lable"></param>
        void resetUI(string lable)
        {
            //set cursor back to default
            //Application.UseWaitCursor = false;
            //rest UI
            statusLabel.Content = lable;
            progressBar.Value = 0;
            retryButton.Visibility = Visibility.Visible;
            okButton.Visibility = Visibility.Visible;
            //releaseNotesLink.Visibility = Visibility.Visible;
        }
        void resetUI(string lable, int progressValue)
        {
            //set cursor back to default
            //Application.UseWaitCursor = false;
            //rest UI
            statusLabel.Content = lable;
            progressBar.Value = progressValue;
            retryButton.Visibility = Visibility.Visible;
            okButton.Visibility = Visibility.Visible;
            //releaseNotesLink.Visibility = Visibility.Visible;
        }
        // Event to track the progress
        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;

        }

        /// <summary>
        /// Function to launch install process automatically after the form loads.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            try
            {
                //Begin logging info
                log("Loading form and initializing variables");
                //End Logginng info

                //variables
                
                DirectoryInfo installerDirectory = Directory.CreateDirectory(installerPath);

                string msiDownloadLink = "https://github.com/NoahMoyer/Election-file-saver/releases/latest/download/Election.Saver.Setup.msi";
                okButton.Content = "OK";
                okButton.Visibility = Visibility.Hidden;
                //releaseNotesLink.Visibility = Visibility.Hidden;



                statusLabel.Content = "Killing main process";
                progressBar.Value = 10;

                try
                {
                    //Begin logging info
                    log("Killing main process");
                    //End logging info

                    //kill main exe
                    foreach (var mainProcess in Process.GetProcessesByName("Election Saver"))
                    {
                        mainProcess.Kill();
                    }
                    progressBar.Value = 15;
                }
                catch (Exception Error)
                {
                    //Begin logging info
                    log("Failed to stop main process. Exiting program. Cath error:\n" + Error);
                    //End logging info
                    MessageBox.Show("Failed to stop main process. Please try again after application closes.\n\n" + Error, "\n\nPlease contact your system administrator", MessageBoxButton.OK, MessageBoxImage.Error);
                    //Kill current process
                    Process.GetCurrentProcess().Kill();
                }

                //Begin logging info
                log("Main process succesfully stopped");
                //End logging info

                try
                {
                    //Begin logging info
                    log("Downloading installer");
                    //End logging info
                    statusLabel.Content = "Downloading insatller";

                    progressBar.Value = 25;
                    //launch download process
                    //Task download = Task.Run(() => downloadFiles(installerPath, installerName, msiDownloadLink));
                    //download.Wait();

                    using (var client = new WebClient())
                    {
                        client.DownloadFile(msiDownloadLink, installerPath + installerName);
                    }

                    //WebClient webClient = new WebClient();
                    //webClient.DownloadFile(msiDownloadLink, installerPath + installerName);
                    progressBar.Value = 50;
                }
                catch (Exception DownloadError)
                {
                    //Begin logging info
                    log("Failed to download update. Cath error:\n " + DownloadError);
                    //End logging info

                    MessageBox.Show("Failed to download update.\n\n" + DownloadError, "\n\nPlease contact your system administrator", MessageBoxButton.OK, MessageBoxImage.Error);

                    resetUI("Failed to download update");

                    return;
                }

                try
                {
                    //Begin logging info
                    log("Installing MSI file");
                    //End logging info

                    statusLabel.Content = "Installing update";

                    int installStatus = installProgram(installerPath, installerName);

                    if (installStatus != 0)
                    {
                        resetUI("Failed to install update");
                        return;
                    }

                    for (int i = 0; i < 10; i++)
                    {
                        progressBar.Value = progressBar.Value + 2;
                        System.Threading.Thread.Sleep(500);
                    }


                }
                catch (Exception Error)
                {
                    //Begin logging info
                    log("Failed to install update. Cath error:\n " + Error);
                    //End logging info

                    MessageBox.Show("Failed to install update.\n\n" + Error, "\n\nPlease contact your system administrator", MessageBoxButton.OK, MessageBoxImage.Error);
                    resetUI("Failed to install update");
                    return;
                }

                //Begin logging info
                log("Update installed successfully");
                //End logging info                

                System.Threading.Thread.Sleep(5000);

                resetUI("Update installed successfully", 100);
            }
            catch (Exception Error)
            {
                //Begin logging info
                log("Unhandeled exception occured. Cath error:\n " + Error);
                //End logging info

                MessageBox.Show("Unknown error. Please contact your system administrator.\n\n" + Error, "", MessageBoxButton.OK, MessageBoxImage.Error);
                resetUI("Unknown error");
                return;
            }
        }

        /// <summary>
        /// Function to download the installer file from github to a path passed into the function as a string.
        /// </summary>
        /// <param name="installerPath"></param>
        /// <param name="installerName"></param>
        void downloadFiles(string installerPath, string installerName, string msiDownloadLink)
        {
            using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFile(
                    // Param1 = Link of file
                    new System.Uri(msiDownloadLink),
                    // Param2 = Path to save
                    installerPath + installerName
                );
            }
        }

        /// <summary>
        /// Function to launch an installer based on a path passed into the function.
        /// Installer name is also passed into the function
        /// </summary>
        /// <param name="installerPath"></param>
        /// <param name="installerName"></param>
        int installProgram(string installerPath, string installerName)
        {
            try
            {
                Process installProcess = new Process();
                installProcess.StartInfo.FileName = "msiexec";
                installProcess.StartInfo.WorkingDirectory = installerPath;
                installProcess.StartInfo.Arguments = "/i " + installerName + " /q";
                installProcess.StartInfo.Verb = "runas";
                installProcess.Start();

                //string processStandardOutput = installProcess.StandardOutput.ReadToEnd();
                //string processStandardError = installProcess.StandardError.ReadToEnd();

                // Check both strings for !IsNullOrEmpty and log something of interest

                installProcess.WaitForExit();
                int exitCode = 0;
                exitCode = installProcess.ExitCode;

                // If ExitCode != 0, log the StandardError output...
                if (exitCode != 0)
                {
                    throw new InvalidOperationException("Install error. MSI installer returned error code: " + exitCode);
                }

                return exitCode;

            }
            catch (InvalidOperationException Error)
            {
                //Begin logging info
                log("Install function failed. Cath error:\n " + Error);
                //End logging info

                MessageBox.Show("Install function failed. Please contact your system administrator. Function returned the following error: \n\n" + Error, "\n\nInstall Function Failed", MessageBoxButton.OK, MessageBoxImage.Error);
                return -1;
            }

        }
        private void progressBar_Click(object sender, EventArgs e)
        {

        }
        /// <summary>
        /// Function to launch the main exe and close the current exe when the OK button is pressed.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            try
            {
                //Begin logging info
                log("User clicked OK button.");
                //End logging info

                //Begin logging info
                log("Deleting temp files");
                //End logging info
                //delete directory and installer
                File.Delete(installerPath + installerName);

                //start main exe
                Process.Start(mainExe);
                System.Threading.Thread.Sleep(3000);
                //kill current exe
                Process.GetCurrentProcess().Kill();
            }
            catch (Exception Error)
            {
                //Begin logging info
                log("Failed to start main process. Cath error:\n " + Error);
                //End logging info

                MessageBox.Show("Failed to start main process. Please contact your system administrator. Closing process.\n\n" + Error, "\n\nMain Process execution failed", MessageBoxButton.OK, MessageBoxImage.Error);
                Process.GetCurrentProcess().Kill();
            }

        }

        /// <summary>
        /// Rety button to restart update app. Will only become visible if the update fails.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void retryButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //Begin logging info
                log("User clicked retry button.");
                //End logging info

                //Process.Start(mainExe);
                Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
                return;
            }
            catch (Exception Error)
            {
                //Begin logging info
                log("Failed to restart update executable. Cath error:\n " + Error);
                //End logging info

                MessageBox.Show("Failed to restart process. Please contact your system administrator.\n\n" + Error, "\n\nRetry execution failed", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        /// <summary>
        /// Function to open the latest release notes on Github when the releaseNotesLink is clicked.
        /// This will open the link in the computers default browser.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void releaseNotesLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        //{
        //    try
        //    {
        //        //Begin logging info
        //        log("User clicked release notes link.");
        //        //End logging info

        //        string url = "https://github.com/NoahMoyer/AccessTimeAnalyzer/releases/latest";
        //        Process urlProcess = new Process();
        //        urlProcess.StartInfo.FileName = url;
        //        urlProcess.StartInfo.UseShellExecute = true;
        //        urlProcess.Start();
        //    }
        //    catch (Exception Error)
        //    {
        //        //Begin logging info
        //        log("Failed to open release notes. Cath error:\n " + Error);
        //        //End logging info

        //        MessageBox.Show("Failed to open link. Please contact your system administrator.\n\n" + Error, "\n\nRelease notes execution failed", MessageBoxButton.OK, MessageBoxImage.Error);
        //    }
    //}

}
}
