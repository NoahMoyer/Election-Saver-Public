using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Windows;

namespace Access_Time_Analyzer
{
    public class Update
    {
        /// <summary>
        /// This class contains functions and variables for checking if the application version is older than the version available on github
        /// </summary>
        public Update()
        {

            //Begin logging info
            string logFile = "C:\\ProgramData\\Election Saver\\log.txt";
            string userName = System.Security.Principal.WindowsIdentity.GetCurrent().Name;
            //End logging info


            /// <summary>
            /// Function to standardize logging data format.
            /// </summary>
            async void Log(string logData)
            {
                try
                {
                    File.AppendAllText(logFile, "[" + DateTime.Now + "] " + "[User name: " + userName + "] Log: " + logData + "\n");
                }
                catch (Exception Error)
                {
                    MessageBox.Show("Failed to write log file. Program should function like normal.\n\n" + Error, "\n\nInformation", MessageBoxButton.OK, MessageBoxImage.Information);
                }

            }

            try
            {
                ///getting version on github
                System.Net.WebClient wc = new System.Net.WebClient();
                string webData = wc.DownloadString("https://raw.githubusercontent.com/NoahMoyer/Election-Saver-Public/main/Election%20Saver/Election%20Saver/Properties/AssemblyInfo.cs");

                //parse webData for the following line:
                //[assembly: AssemblyVersion("1.2.2")]
                var versionLineRegex = Regex.Match(webData, @"AssemblyVersion\s*\(\s*""([0-9\.]*?)""\s*\)");
                string versionLineString = versionLineRegex.ToString();
                string[] versionArray = versionLineString.Split('\"');
                //string onlineVersion should be the following using the previous example:
                //1.2.2
                int[] onlineVersion = versionArray[1].Split('.').Select(n => Convert.ToInt32(n)).ToArray(); ;

                ///getting version from local system
                //This function is similar to the above. Only difference is we don't have to parse the file.
                //localVersionVar is simply returning the version number reported by the assembler
                var localVersionVar = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
                string localVersionString = localVersionVar.ToString();
                int[] localVersion = localVersionString.Split('.').Select(n => Convert.ToInt32(n)).ToArray();

                //compare version numbers
                for (int i = 0; onlineVersion.Length > i; i++)
                {
                    //if version on github is greater than the local version return true
                    if (onlineVersion[i] > localVersion[i])
                    {
                        updateAvail = true;
                    }
                }
            }
            catch (Exception Error) 
            {
                Log("Failed to check for application update. Program should function like normal. Cath error:\n " + Error);
                //MessageBox.Show("Failed to check for application update. Program should function like normal.\n\n" + Error, "\n\nInformation", MessageBoxButton.OK, MessageBoxImage.Information);
            }



        }

        public bool updateAvail { get; set; }
    }
}
