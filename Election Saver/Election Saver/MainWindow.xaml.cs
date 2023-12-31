﻿using System;
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
using System.IO;
using System.Threading;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Text.RegularExpressions;
using System.Timers;
using System.Diagnostics;
using Access_Time_Analyzer;
using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;

//new version lol
namespace Election_Saver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        //int fileCount;
        int waitTimeINSecondsBetweenPrints;
        int copyProgressBarWaitTime;
        double waitPeriodToPercentageInterval;
        FileCopier fileCopier; 
        string currentPrecinctWhenButtonPressed;
        string regexExpressionForPrecinctTextBox = "^(([0-9]+[-][0-9]+){1}|([0-9]+){1}){1}$";

        private static System.Timers.Timer aTimer;
        public MainWindow()
        {
            InitializeComponent();
            fileCopier = new FileCopier();
            waitTimeINSecondsBetweenPrints = 10;
            copyProgressBarWaitTime = 2;
            waitPeriodToPercentageInterval = 100 / copyProgressBarWaitTime;
            foreach (var drive in fileCopier.allDrives)
            {
                driveSelector.Items.Add(drive);
            }
            driveSelector.Text = fileCopier.getSourcePath();

            if(driveSelector.SelectedItem == null)
            {
                unlockBitlockerButton.IsEnabled = false;
            }
            else if(driveSelector.SelectedItem != null)
            {
                unlockBitlockerButton.IsEnabled = true;
            }
            progressBar.Visibility = Visibility.Hidden;
            progressBarLabel.Visibility = Visibility.Hidden;
            precintExistStatusLabel.Visibility = Visibility.Hidden;
            //Noah: comment what we are doing here
            networkSaveLocationLabelDispalay.Text = fileCopier.getNetworkDestinationPath();
            localSaveLocationLabelDispalay.Content = fileCopier.getLocalDestinationPath();
            currentDefaultDriveLetterLabel1.Content = fileCopier.getSourcePath();
            //Noah: comment what we are doing here
            foreach(var letter in fileCopier.getDriveLettersToExclude())
            {
                listOfDriveLettersToExludeBox.Items.Add(letter);
            }
            //Noah: comment what we are doing here
            foreach (var extension in fileCopier.getListOfFileExtensionsToCopy())
            {
                listOfFileExtensionsToCopyBox.Items.Add(extension);
            }

            if(Regex.IsMatch(PrecinctTextBox.Text, "^(([0-9]+[-][0-9]){1}|([0-9]+){1}|()){1}$")) //this will allow the preceinct to be nothing initailly. Only use this condition here.
            {
                //Poplulating the files available to copy text block
                string currentPrecinctFlashFiles = PrecinctTextBox.Text;
                currentPrecinctFlashFiles = fileCopier.getFlashAvailableFiles();
                flashFilesTextBlock.Text = currentPrecinctFlashFiles;

                //Pupulating the files available to print text block
                string currentPrecinctLocalFiles = PrecinctTextBox.Text;
                currentPrecinctLocalFiles = fileCopier.getAvailableFiles(currentPrecinctLocalFiles);
                localFilesTextBlock.Text = currentPrecinctLocalFiles;
            }
            else
            {
                MessageBox.Show("Precint must either be a number or dash encased by numbers. /n Examples: 1-1 or 2", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            if (fileCopier.testBitLockerPermission())
            {
                bitLockerPermStatus.Content = "Enabled";
                unlockBitlockerButton.IsEnabled = true;
                //Get drive lock status
                driveLockStatusLable.Text = fileCopier.getDriveLockStatus();


                refreshDrivesPeriodically();
            }
            else
            {
                bitLockerPermStatus.Content = "Disabled. Please contact system administrator.";
                unlockBitlockerButton.IsEnabled = false;
                driveLockStatusLable.Text = "ERROR";
            }


            //Setting version lable on GUI
            var tempVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            string version = tempVersion.ToString();
            this.versionLabel.Content = "Version: " + version;


            ///
            //checking for upate
            Update x = new Update();
            if (x.updateAvail)
            {
                this.updateLink.Visibility = Visibility.Visible;
            }
            ///

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CopyFilesButton_Click(object sender, RoutedEventArgs e)
        {
            progressBarLabel.Visibility = Visibility.Visible;
            progressBar.Visibility = Visibility.Visible;
            progressBar.Value = 0;
            if (Regex.IsMatch(PrecinctTextBox.Text, regexExpressionForPrecinctTextBox))
            {
                currentPrecinctWhenButtonPressed = PrecinctTextBox.Text;
                for (int i = 0; i < copyProgressBarWaitTime; i++)
                {
                    await Task.Delay(1000);
                    progressBar.Value = (i + 1) * waitPeriodToPercentageInterval;
                }

                progressBar.Value = 100;
                fileCopier.CopyFiles(currentPrecinctWhenButtonPressed, allowOverwriteCheckBox.IsChecked == true);
                await Task.Delay(copyProgressBarWaitTime * 1000);


                progressBar.Visibility = Visibility.Hidden;
                progressBarLabel.Visibility = Visibility.Hidden;

                //Pupulating the files available to copy text block
                string currentPrecinct = PrecinctTextBox.Text;
                currentPrecinct = fileCopier.getAvailableFiles(currentPrecinct);
                localFilesTextBlock.Text = currentPrecinct;
            }
            else
            {
                MessageBox.Show("Precinct must either be a number or dash encased by numbers. \nExamples: 1-1 or 2", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                progressBar.Value = 100;
                await Task.Delay(1 * 1000);
                progressBar.Visibility = Visibility.Hidden;
                progressBarLabel.Visibility = Visibility.Hidden;
            }


            //Check if precint file exists
            if (PrecinctTextBox.Text == "")
            {
                precintExistStatusLabel.Visibility = Visibility.Hidden;
            }
            else if (fileCopier.precintFolderExists(PrecinctTextBox.Text))
            {
                precintExistStatusLabel.Visibility = Visibility.Visible;
            }
            else
            {
                precintExistStatusLabel.Visibility = Visibility.Hidden;
            }

        }

        private void PreceintTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

            if (Regex.IsMatch(PrecinctTextBox.Text, regexExpressionForPrecinctTextBox))
            {
                CopyFilesButton.IsEnabled = true;
                printButton.IsEnabled = true;

                if (PrecinctTextBox.Text == "")
                {
                    CopyFilesButton.IsEnabled = false;
                    printButton.IsEnabled = false;
                }

                //Pupulating the files available to print text block
                string currentPrecinctLocalFiles = PrecinctTextBox.Text;
                currentPrecinctLocalFiles = fileCopier.getAvailableFiles(currentPrecinctLocalFiles);
                localFilesTextBlock.Text = currentPrecinctLocalFiles;
            }
            else if(Regex.IsMatch(PrecinctTextBox.Text, "^()$"))
            {
                localFilesTextBlock.Text = "Please enter a precinct to get information about available files to print.";
            }
            else
            {
                localFilesTextBlock.Text = "Precinct incorrect format. Must either be a number or dash encased by numbers. \nExamples: 1-1 or 2";
                //TODO: decide if we want to show an error or just wait until user clicks copy or print.
            }

            //Check if precint file exists
            if (PrecinctTextBox.Text == "")
            {
                precintExistStatusLabel.Visibility = Visibility.Hidden;
            }
            else if (fileCopier.precintFolderExists(PrecinctTextBox.Text))
            {
                precintExistStatusLabel.Visibility = Visibility.Visible;
            }
            else
            {
                precintExistStatusLabel.Visibility = Visibility.Hidden;
            }

            if (driveLockStatusLable.Text == "Locked")
            {
                CopyFilesButton.IsEnabled = false;
                printButton.IsEnabled = false;
            }

        }

        
        private async void printButton_Click(object sender, RoutedEventArgs e)
        {
            progressBarLabel.Visibility = Visibility.Visible;
            progressBar.Visibility = Visibility.Visible;
            progressBar.Value = 0;

            if (Regex.IsMatch(PrecinctTextBox.Text, regexExpressionForPrecinctTextBox))
            {
                currentPrecinctWhenButtonPressed = PrecinctTextBox.Text;
                fileCopier.PrintFiles(waitTimeINSecondsBetweenPrints, currentPrecinctWhenButtonPressed);
                await Task.Delay(2000);
                progressBar.Value = 25;
                await Task.Delay(2000);
                progressBar.Value = 50;
                await Task.Delay(2000);
                progressBar.Value = 75;
                await Task.Delay(2000);
                progressBar.Value = 100;
                await Task.Delay(5000);
                progressBar.Visibility = Visibility.Hidden;
                progressBarLabel.Visibility = Visibility.Hidden;
            }
            else 
            {
                MessageBox.Show("Precinct must either be a number or dash encased by numbers. \nExamples: 1-1 or 2", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                progressBar.Value = 100;
                await Task.Delay(1 * 1000);
                progressBar.Visibility = Visibility.Hidden;
                progressBarLabel.Visibility = Visibility.Hidden;
            }

        }

        private void timeBetweenPrintsSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            //waitTimeINSecondsBetweenPrints = Int32.Parse(timeBox.Text);
            
        }

        private void driveSelector_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            fileCopier.setSourcePath(driveSelector.SelectedItem);
            unlockBitlockerButton.IsEnabled = true;

            //Get drive lock status
            driveLockStatusLable.Text = fileCopier.getDriveLockStatus();

            if (Regex.IsMatch(PrecinctTextBox.Text, regexExpressionForPrecinctTextBox) || PrecinctTextBox.Text == "")
            {
                //Poplulating the files available to copy text block
                string currentPrecinctFlashFiles = PrecinctTextBox.Text;
                currentPrecinctFlashFiles = fileCopier.getFlashAvailableFiles();
                flashFilesTextBlock.Text = currentPrecinctFlashFiles;
            }
            else
            {
                //TODO: decide if we need to do anything in this case
            }

            if (driveLockStatusLable.Text == "Locked")
            {
                CopyFilesButton.IsEnabled = false;
                printButton.IsEnabled = false;
            }
        }

        //main function should be called when windows opens
        private void refreshDrivesPeriodically()
        {
            
            SetTimer();
            Debug.WriteLine("Periodic drive refresh started");
            
            //aTimer.Stop();
            //aTimer.Dispose();
            //Debug.WriteLine("Periodic calls ended");
        }
        //calls OnTimedEvent on a set time interval
        private void SetTimer()
        {
            // Create a timer with a half second interval.
            aTimer = new System.Timers.Timer(2000);
            // Hook up the Elapsed event for the timer. 
            aTimer.Elapsed += OnTimedEvent;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }
        //function called on set time interval by SetTimer()
        private void OnTimedEvent(Object source, ElapsedEventArgs e)
        {
            Debug.WriteLine("Drives refreshed at {0:HH:mm:ss.fff}",
                              e.SignalTime);
            refresh();

        }
        public void refresh()
        {
            fileCopier.updateDrivesPeriodically();
            
            Object selectedItem;
            this.Dispatcher.Invoke(() =>
            {
                //This if statement will handle when a drive is selected and then unplugged. In this case we want to check to make sure if the drive
                //is still in the list of drives currently plugged into the computer. If not reset bitmanager
                if (driveSelector.SelectedItem != null)
                {
                    DriveInfo n = driveSelector.SelectedItem as DriveInfo;
                    //check if drive is still connected.
                    if (!fileCopier.allDrives.Contains(n))
                    {
                        //if not clear the selection from bitManager so it is not checking it
                        Debug.WriteLine("Drive unplugged. Resetting bitmanager");
                        fileCopier.bitManager = null;

                    }
                }

                selectedItem = driveSelector.SelectedItem;
                driveSelector.Items.Clear();
                foreach (var drive in fileCopier.allDrives)
                {
                    driveSelector.Items.Add(drive);
                    if (selectedItem != null)
                    {
                        if (drive.Name == selectedItem.ToString())
                        {
                            driveSelector.SelectedItem = drive;

                        }
                    }
                    
                }

                if (driveSelector.SelectedItem == null)
                {
                    unlockBitlockerButton.IsEnabled = false;
                }
                else if (driveSelector.SelectedItem != null)
                {
                    unlockBitlockerButton.IsEnabled = true;
                }

                if (Regex.IsMatch(PrecinctTextBox.Text, regexExpressionForPrecinctTextBox))
                {
                    //Poplulating the files available to copy text block
                    string currentPrecinctFlashFiles = PrecinctTextBox.Text;
                    currentPrecinctFlashFiles = fileCopier.getFlashAvailableFiles();
                    flashFilesTextBlock.Text = currentPrecinctFlashFiles;
                }
                else
                {
                    //TODO: decide if we need to do anything in this case
                }

                if (driveSelector.Text == "")
                {
                    flashFilesTextBlock.Text = "No drive selected.";
                }

                
                

                //Get drive lock status
                driveLockStatusLable.Text = fileCopier.getDriveLockStatus();

                
                

                if (driveLockStatusLable.Text == "Locked")
                {
                    CopyFilesButton.IsEnabled = false;
                    printButton.IsEnabled = false;
                }

                if (driveSelector.Text == "" && fileCopier.allDrives.Count > 0 && driveSelector.SelectedItem == null)
                {
                    driveLockStatusLable.Text = "Select Drive";
                }
                else if(fileCopier.allDrives.Count == 0)
                {
                    driveLockStatusLable.Text = "No drives available";
                }
            });

        }

        private void refreshDrivesButton_Click(object sender, RoutedEventArgs e)
        {
            //refresh();
            fileCopier.updateDrives();
            driveSelector.Items.Clear();
            foreach (var drive in fileCopier.allDrives)
            {
                driveSelector.Items.Add(drive);
            }
            //driveSelector.Text = fileCopier.getSourcePath();

            if (driveSelector.SelectedItem == null)
            {
                unlockBitlockerButton.IsEnabled = false;
            }
            else if (driveSelector.SelectedItem != null)
            {
                unlockBitlockerButton.IsEnabled = true;
            }

            if (Regex.IsMatch(PrecinctTextBox.Text, regexExpressionForPrecinctTextBox))
            {
                //Poplulating the files available to copy text block
                string currentPrecinctFlashFiles = PrecinctTextBox.Text;
                currentPrecinctFlashFiles = fileCopier.getFlashAvailableFiles();
                flashFilesTextBlock.Text = currentPrecinctFlashFiles;
            }
            else
            {
                //TODO: decide if we need to do anything in this case
            }

            if (driveSelector.Text == "")
            {
                flashFilesTextBlock.Text = "No drive selected.";
            }

            //Get drive lock status
            driveLockStatusLable.Text = fileCopier.getDriveLockStatus();

            if (driveLockStatusLable.Text == "Locked")
            {
                CopyFilesButton.IsEnabled = false;
                printButton.IsEnabled = false;
            }

            if (driveSelector.Text == "" && fileCopier.allDrives.Count > 0)
            {
                driveLockStatusLable.Text = "Select Drive";
            }
            else
            {
                driveLockStatusLable.Text = "No drives available";
            }


        }

        private void unlockBitlockerButton_Click(object sender, RoutedEventArgs e)
        {
            if (driveSelector.SelectedItem != null)
            {
                fileCopier.unlockBitLocker();
            }
            else
            {
                MessageBox.Show("Please select a drive to unlock", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //Get drive lock status
            driveLockStatusLable.Text = fileCopier.getDriveLockStatus();

            if (Regex.IsMatch(PrecinctTextBox.Text, regexExpressionForPrecinctTextBox) || PrecinctTextBox.Text == "")
            {
                //Poplulating the files available to copy text block
                string currentPrecinctFlashFiles = PrecinctTextBox.Text;
                currentPrecinctFlashFiles = fileCopier.getFlashAvailableFiles();
                flashFilesTextBlock.Text = currentPrecinctFlashFiles;
            }
            else
            {
                //TODO: decide if we need to do anything in this case
            }
        }

        private void updateBitLockerPassword_Click(object sender, RoutedEventArgs e)
        {
            if(bitLockerPasswordTextBox.Password != null)
            {
                fileCopier.setBitLockerPassword(bitLockerPasswordTextBox.Password.ToString());
            }
            else
            {
                MessageBox.Show("No text entered, please input something", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            updateBitLockerPasswordButton.IsDefault = false;
        }

        private void changeNetworkLocationButton_Click(object sender, RoutedEventArgs e)
        {
            
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result.ToString() == "Ok")
            {
                fileCopier.setNetworkDestinationPath(dialog.FileName);
                networkSaveLocationLabelDispalay.Text = dialog.FileName;
            }
            

        }

        private void changeLocalLocationButton_Click_1(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result.ToString() == "Ok")
            {
                fileCopier.setLocalDestinationPath(dialog.FileName);
                localSaveLocationLabelDispalay.Content = dialog.FileName;
            }
        }

        private void changeDefaultDriveLetterButtton_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new CommonOpenFileDialog();
            dialog.IsFolderPicker = true;
            CommonFileDialogResult result = dialog.ShowDialog();
            if (result.ToString() == "Ok")
            {
                fileCopier.setSourcePath(dialog.FileName);
                currentDefaultDriveLetterLabel1.Content = dialog.FileName;
            }
        }

        private void letterToAddToDrivesToExcludeButton_Click(object sender, RoutedEventArgs e)
        {
            if(letterToAddToDrivesToExcludeTextBox.Text != null && letterToAddToDrivesToExcludeTextBox.Text != "")
            {
                if (!listOfDriveLettersToExludeBox.Items.Contains(letterToAddToDrivesToExcludeTextBox.Text))
                {
                    string text = letterToAddToDrivesToExcludeTextBox.Text.ToString();
                    if (Regex.IsMatch(text, "^[A-Z]{1}$"))
                    {
                        listOfDriveLettersToExludeBox.Items.Add(letterToAddToDrivesToExcludeTextBox.Text);
                        List<string> listOfNewLettersToExclude = new List<string>();
                        foreach (var letter in listOfDriveLettersToExludeBox.Items)
                        {
                            listOfNewLettersToExclude.Add(letter.ToString());
                        }
                        fileCopier.setDriveLettersToExclude(listOfNewLettersToExclude);
                    }
                    else
                    {
                        MessageBox.Show("Please only enter a single uppercase letter", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Drive is already excluded", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
            }
            else
            {
                MessageBox.Show("No text entered, please input something", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            letterToAddToDrivesToExcludeButton.IsDefault = false;
        }

        private void removeSelectedDriveLetterButton_Click(object sender, RoutedEventArgs e)
        {
            if(listOfDriveLettersToExludeBox.SelectedItem != null)
            {
                fileCopier.removeSepcicDriveLetterToExclude(listOfDriveLettersToExludeBox.SelectedItem.ToString());
                listOfDriveLettersToExludeBox.Items.Remove(listOfDriveLettersToExludeBox.SelectedItem);
            }
            else
            {
                MessageBox.Show("Please select an item to remove", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            removeSelectedDriveLetterButton.IsDefault = false;

        }

        private void removeSelectedFileExtension_Click(object sender, RoutedEventArgs e)
        {
            if(listOfFileExtensionsToCopyBox.SelectedItem != null)
            {
                fileCopier.removeSpecificFileExtensionToCopy(listOfFileExtensionsToCopyBox.SelectedItem.ToString());
                listOfFileExtensionsToCopyBox.Items.Remove(listOfFileExtensionsToCopyBox.SelectedItem);
            }
            else
            {
                MessageBox.Show("Please select an item to remove", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            removeSelectedFileExtensionButton.IsDefault = false;
        }

        private void fileExtensionToAddButton_Click(object sender, RoutedEventArgs e)
        {
            if(fileExtensionToAddTextBox.Text != null && fileExtensionToAddTextBox.Text != "")
            {
                if (!listOfFileExtensionsToCopyBox.Items.Contains("*." + fileExtensionToAddTextBox.Text))
                {
                    //TODO: make sure regular expression only allows lower case letters
                    if (Regex.IsMatch(fileExtensionToAddTextBox.Text.ToString(), "^[a-z]+$"))
                    {
                        listOfFileExtensionsToCopyBox.Items.Add("*." + fileExtensionToAddTextBox.Text);
                        fileCopier.addFileExtensionToCopy("*." + fileExtensionToAddTextBox.Text);
                    }
                    else
                    {
                        MessageBox.Show("Please enter lowercase letters only", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    MessageBox.Show("File extension is already included", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                
                
            }
            else
            {
                MessageBox.Show("No text entered, please input something", "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            fileExtensionToAddButton.IsDefault = false;
        }

        private void listOfDriveLettersToExludeBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateBitLockerPasswordButton.IsDefault = false;
            letterToAddToDrivesToExcludeButton.IsDefault = false;
            removeSelectedFileExtensionButton.IsDefault = false;
            fileExtensionToAddButton.IsDefault = false;
            removeSelectedDriveLetterButton.IsDefault = true;
        }

        private void letterToAddToDrivesToExcludeTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            updateBitLockerPasswordButton.IsDefault = false;
            removeSelectedDriveLetterButton.IsDefault = false;
            removeSelectedFileExtensionButton.IsDefault = false;
            fileExtensionToAddButton.IsDefault = false;
            letterToAddToDrivesToExcludeButton.IsDefault = true;
        }

        private void listOfFileExtensionsToCopyBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            updateBitLockerPasswordButton.IsDefault = false;
            letterToAddToDrivesToExcludeButton.IsDefault = false;
            removeSelectedDriveLetterButton.IsDefault = false;
            fileExtensionToAddButton.IsDefault = false;
            removeSelectedFileExtensionButton.IsDefault = true;
        }

        private void fileExtensionToAddTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            updateBitLockerPasswordButton.IsDefault = false;
            letterToAddToDrivesToExcludeButton.IsDefault = false;
            removeSelectedDriveLetterButton.IsDefault = false;
            removeSelectedFileExtensionButton.IsDefault = false;
            fileExtensionToAddButton.IsDefault = true;
        }

        private void bitlockerPasswordChanged(object sender, RoutedEventArgs e)
        {
            letterToAddToDrivesToExcludeButton.IsDefault = false;
            removeSelectedDriveLetterButton.IsDefault = false;
            removeSelectedFileExtensionButton.IsDefault = false;
            fileExtensionToAddButton.IsDefault = false;
            updateBitLockerPasswordButton.IsDefault = true;
        }

        private void BitLockerPermissionEnableButton_Click(object sender, RoutedEventArgs e)
        {
            fileCopier.giveCurrentUserBitlockerPermission();

            if (fileCopier.testBitLockerPermission())
            {
                bitLockerPermStatus.Content = "Enabled";

                //Get drive lock status
                driveLockStatusLable.Text = fileCopier.getDriveLockStatus();


                refreshDrivesPeriodically();
            }
            else
            {
                bitLockerPermStatus.Content = "Disabled. Please contact system administrator.";
                if(aTimer != null)
                {
                    aTimer.Stop();
                    //aTimer.AutoReset = false;
                    //aTimer.Enabled = false;
                }
                
                driveLockStatusLable.Text = "ERROR";
            }
        }

        private void BitLockerPermissionDisableButton_Click(object sender, RoutedEventArgs e)
        {
            fileCopier.deleteCurrentUserBitlockerPermission();

            if (fileCopier.testBitLockerPermission())
            {
                bitLockerPermStatus.Content = "Enabled";

                //Get drive lock status
                driveLockStatusLable.Text = fileCopier.getDriveLockStatus();


                refreshDrivesPeriodically();
            }
            else
            {
                bitLockerPermStatus.Content = "Disabled. Please contact system administrator.";
                if (aTimer != null)
                {
                    aTimer.Stop();
                    //aTimer.AutoReset = false;
                    //aTimer.Enabled = false;
                }
                driveLockStatusLable.Text = "ERROR";
            }
        }

        private void updateLinkButton_Click(object sender, RoutedEventArgs e)
        {
            //bring menu up to change default app groups
            //UpdateConfirmation popUp = new UpdateConfirmation();
            //DialogResult result = popUp.ShowDialog();

            //Process.Start(@"update\updater.exe");

            //open Update confirmation form
            UpdateConfirmation popUp = new UpdateConfirmation();
            popUp.ShowDialog();

            
        }
    }
}
