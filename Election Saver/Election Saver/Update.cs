using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Diagnostics;

namespace Access_Time_Analyzer
{
    public class Update
    {
        /// <summary>
        /// This class contains functions and variables for checking if the application version is older than the version available on github
        /// </summary>
        public Update()
        {
            ///getting version on github
            System.Net.WebClient wc = new System.Net.WebClient();
            string webData = wc.DownloadString("https://raw.githubusercontent.com/NoahMoyer/Election-file-saver/main/Election%20Saver/Election%20Saver/Properties/AssemblyInfo.cs");

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

        public bool updateAvail { get; set; }
    }
}
