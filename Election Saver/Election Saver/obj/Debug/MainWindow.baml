   M S B A M L   `   `   `  ����I  EElection Saver, Version=2.3.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:Election_SaverElection_Saver  5   	       Election_Saver.MainWindow   5   	   ^ ZWindowsFormsIntegration, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   .localclr-namespace:Election_Saver     5   	     � Title$  Election Saver��5   	   $	��500��6    $	��800��60   $	��500��5	   	   $	��800��6   .��5
      � -   5         �Name$ Election_Saver��5
      $��	0,0,0,-16q�6#   .+�5   
   �� .@�5      �� $
��Main��6   .��5      � ���	#FFE5E5E5 � ColumnDefinitions 5      �� 5      $	��37* �6+   �� 5      $
��240* �6+   �� 5      $
��120* �6+   .+�5      �� 
���   �$��1q�61   $��2��6E   $��22,239,262,56q�6Y   �� 5      -   5         ��Name$ CopyFilesButton��5      $��
Copy files��66   $
��Left=�6K   $��271,169,0,0q�6f   $	��Top=�6{   $��56��6�   $	��116��6�   {�.  $��1��6�   �� 5      -   5        $ printButton��5      $��Print files��62   $
��Left=�6H   $��414,169,0,0q�6c   $	��Top=�6x   $��56��6�   $	��116��6�   {�.  $��1��6�   $��2��6�   � 5      -   5         �Name$ progressBar��5   "   $
��Left=�67   $��40��6R   $��302,279,0,0q�6^   $	��Top=�6s   $	��200��6�   $y�Visible=�6�   $��1��6�   $��2��6�   �� 5      -   5         ��Name$ driveSelector��5      $
��Left=�66   $��
342,67,0,0q�6Q   $	��Top=�6e   $	��120��6}    J� RenderTransformOrigin$ 18934.85,111261.719&�6�   $��1��6�   �� 5      -   5         ��Name$ PrecinctTextBox��5      $
��Left=�67   $��343,117,0,0q�6R    �� TextWrapping$
 Wrap=�6g   $	��Top=�6{   $	��118��6�   $��1��6�   �� 5      -   5        	 ��Name$	 allowOverwriteCheckBox��5      $,��&Check to allow files to be overwritten��6?   $��Right=�6p   $��
0,118,20,0q�6�   $	��Top=�6�   $��2��6�   �� 5      -   5        $ timeBox��5      $
��Left=�6/   $��
154,52,0,0q�6J   $
 Wrap=�6^   $$��Enter time between prints here��6r   $	��Top=�6�   $	��127��6�   $��36��6�   $y�Hidden=�6�   {�.  $��1��6�   �� 5      -	   5        
 ��Name$
 driveSelectorLabel��5      $��Drive selection��68   $
��Left=�6R   $��
358,36,0,0q�6m   $	��Top=�6�   $��1��6�   �� 5      -
   5        $ refreshDrivesButton��5      $��Refresh Drives��6:   $
��Left=�6S   $��
475,42,0,0q�6n   $	��Top=�6�   $��1��6�   {�.  $y�Hidden=�6�   $��2��6�   �� 5      -   5        $
 precinctBoxLabel��5      $��Enter precinct below��66   $
��Left=�6U   $��
342,94,0,0q�6p   $	��Top=�6�    � Cursor$  ��6�   $	��120��6�   $��1��6�   �� 5      $��Button��6   $��35,78,205,344q�6.   $��2��6E   �� 5       -   5        $ unlockBitlockerButton��5       $��Unlock Drive��6<   $
��Left=�6S   $��
475,67,0,0q�6n   $	��Top=�6�   $��79��6�   $��1��6�   $��2��6�   �� 5!      -   5        $
 progressBarLabel��5!      $��Processing please wait...��66   $
��Left=�6Z   $��334,253,0,0q�6u   $	��Top=�6�   $��1��6�   �� 5"      $��22,239,262,56q�6#   $��2��6:   $
��Auto=�6N   $
��Auto=�6s   .��5#      ]� $��Center=�6$   $��Center=�6?   $
��Auto��6\   .+�5$      �� -   5         ��Name$ localFilesTextBlock��5$   (   $
��Wrap=�6E   $#��Please enter precinct number.��6Y   $	��121��6~   $	��221��6�   $��2��6�   5%      5&      �� 5'      -   5        $
 fileDataLabel��5'      $/��)Files available to copy from flash drive:��63   $
��Left=�6g   $��22,0,0,0q�6�   $	��Top=�6�   $��2��6�   $	��263��6�   �� 5(      -   5        $
 fileDataLabel_Copy��5(      $��Files available to print:��68   $
��Left=�6\   $��
22,208,0,0q�6w   $	��Top=�6�   $��2��6�   $	��142��6�   �� 5)      
���   �$��1q�61   $��2��6E   $��22,27,262,287q�6Y   �� 5*      $��22,26,262,287q�6#   $��2��6:   $
��Auto=�6N   $
��Auto=�6s   .��5+      ]� $��Center=�6$   $��Center=�6?   $
��Auto��6\   .+�5,      �� -   5        $ flashFilesTextBlock��5,   (   $
��Wrap=�6E   $#��Please enter precinct number.��6Y   $	��132��6~   $	��221��6�   $��2��6�   5-      5.      �� 5/      
���   �$��1q�61   $��2��6E   $��90,62,41,344q�6U   ._�50      �� -   5        $ driveLockStatusLable��50   I   $
��Wrap=�6$   $��1��68   $��Drive status��6g   $��0,-1,0,1q�6{   $��Center=�6�   51      52      �� 54      $��2��6   $��Drive lock status��6,   $
��Left=�6H   $��	86,31,0,0q�6c   $	��Top=�6v   $��99��6�   �� 55      -   5        $
 precintExistStatusLabel��55      $��1��6=   $��Precint already exists��6M   $
��Left=�6n   $��340,135,0,0q�6�   $	��Top=�6�   $	��124��6�   �� 56      -   5        $ 
updateLink��56      $��2��61   $
��Left=�6B   $��
22,409,0,0q�6]   $	��Top=�6q   
����  �$y�Visible=�6�   ��57      �� 58      $
 Hand��6(   $y�Hidden=�66   B� 59   "   .�6,   Update available6>   5:      ��5<      �� 5=       �� 
TargetType" ��6.   .R�5>   "   �� 5?      5A      �� 5B      -   5        $
 versionLabel��5B      $��2��62   $��Version: 0.0.0.0��6B   $
��Left=�6]   $��131,404,0,0q�6x   $	��Top=�6�   5D      5E      �� 5F      $��Settings��6   .��5G      � ���	#FFE5E5E5 5H      �� 5I      $
��Auto �6+   �� 5J      .+�5L      G� -   5         G�Name$ bitLockerPasswordTextBox��5L   "   $
��Left=�6D   $��	26,25,0,0q�6_   $	��Top=�6r   $	��120��6�   $��1��6�   �� 5M      -   5        $# updateBitLockerPasswordButton��5M      $��Set Bitlocker password��6D   $
��Left=�6e   $��
161,24,0,0q�6�   $	��Top=�6�   $��1��6�   �� 5N      -   5        $
 BitlockerLabel��5N      $(��"Enter new BitLocker password below��64   $
��Left=�6a   $��	26,-1,0,0q�6|   $	��Top=�6�   $��1��6�   �� 5O      -   5        $
 networkSaveLocation��5O      $#��Current network save location��69   $
��Left=�6a   $��	26,48,0,0q�6|   $	��Top=�6�   $��1��6�   $	��180��6�   $��26��6�   �� 5Q      -   5        $&  networkSaveLocationLabelDispalay��5Q       $%��Network Location will show here��6J   $
��Left=�6q   $��	26,74,0,0q�6�   $	��Top=�6�   $ 0.011,0.568&�6�   $��1��6�   $
��Wrap=�6�   $	��421��6�   $��31��6  6  �� 5S      -   5        $! changeNetworkLocationButton��5S      $��Change network location��6B   $
��Left=�6d   $��
26,105,0,0q�6   $	��Top=�6�   $��1��6�   �� 5T      -   5        $
 localSaveLocation��5T      $!��Current local save location��67   $
��Left=�6]   $��
26,130,0,0q�6x   $	��Top=�6�   $��1��6�   �� 5U      -   5        $$
 localSaveLocationLabelDispalay��5U      $%��Local location should show here��6D   $
��Left=�6n   $��
26,156,0,0q�6�   $	��Top=�6�   $��1��6�   �� 5V      -   5        $ changeLocalLocationButton��5V      $ ��Change local save location��6@   $
��Left=�6e   $��
26,187,0,0q�6�   $	��Top=�6�   $��1��6�   �� 5W      -   5        $$
 currentDefaultDriveLetterLabel��5W      $"��Current default drive letter��6D   $
��Left=�6k   $��
26,212,0,0q�6�   $	��Top=�6�   $��1��6�   �� 5X      -    5        $%
 currentDefaultDriveLetterLabel1��5X      $6��0Current default drive letter should display here��6E   $
��Left=�6�   $��
26,238,0,0q�6�   $	��Top=�6�   $��1��6�   �� 5Y      -!   5        $% changeDefaultDriveLetterButtton��5Y      $!��Change default drive letter��6F   $
��Left=�6l   $��
26,269,0,0q�6�   $	��Top=�6�   $��1��6�   �� 5Z      -"   5         ��Name$# listOfDriveLettersToExludeBox��5Z      $��527,30,35,347q�6F   $��1��6�   �� 5[      -#   5        $) #letterToAddToDrivesToExcludeTextBox��5[      $
��Left=�6K   $��525,153,0,0q�6f   $
 Wrap=�6{   $�� ��6�   $	��Top=�6�   $	��120��6�   $��1��6�   �� 5\      -$   5        $( "letterToAddToDrivesToExcludeButton��5\      $��Add drive letter��6I   $
��Left=�6d   $��659,153,0,0q�6   $	��Top=�6�   $ 0.105,0.354&�6�   $��1��6  �� 5]      -%   5        $ 
 driveLettersToExcludeLabel��5]      $&�� List of drive letters to exclude��6@   $
��Left=�6k   $��
540,-1,0,0q�6�   $	��Top=�6�   $��1��6�   �� 5^      -&   5        $% removeSelectedDriveLetterButton��5^      $��Remove selected��6F   $
��Left=�6`   $��659,177,0,0q�6{   $	��Top=�6�   $��1��6�   �� 5_      -'   5        $
 driveLetterExxclusionInfo��5_      $$��Enter drive letter to add here��6?   $
��Left=�6h   $��525,127,0,0q�6�   $	��Top=�6�   $��1��6�   �� 5`      -(   5        $# listOfFileExtensionsToCopyBox��5`      $��527,238,35,139q�6F   $��1��6�   �� 5a      -)   5        $ fileExtensionToAddTextBox��5a      $
��Left=�6A   $��519,359,0,0q�6\   $
 Wrap=�6q   $�� ��6�   $	��Top=�6�   $	��120��6�   $��1��6�   �� 5b      -*   5        $ fileExtensionToAddButton��5b      $��Add file extension��6?   $
��Left=�6\   $��653,359,0,0q�6w   $	��Top=�6�   $ 0.105,0.354&�6�   $��1��6�   �� 5c      -+   5        $
 fileExtensionsToCopyLabel��5c      $%��List of file extensions to copy��6?   $
��Left=�6i   $��540,207,0,0q�6�   $	��Top=�6�   $��1��6�   �� 5d      -,   5        $' !removeSelectedFileExtensionButton��5d      $��Remove selected��6H   $
��Left=�6b   $��653,384,0,0q�6}   $	��Top=�6�   $��1��6�   �� 5e      --   5        $
 fileExtensionInfoLabel��5e      $$��Enter drive letter to add here��6<   $
��Left=�6e   $��519,333,0,0q�6�   $	��Top=�6�   $��1��6�   �� 5f      -.   5        $
 bitLockerPermLabel��5f      $��1��68   $!��BitLocker Permission Status��6H   $
��Left=�6n   $��
26,306,0,0q�6�   $	��Top=�6�   $	��160��6�   �� 5g      -/   5        $
 bitLockerPermStatus��5g      $��1��69   $$��BitLocker status displays here��6I   $
��Left=�6r   $��
26,328,0,0q�6�   $	��Top=�6�   $ 0.002,-0.06&�6�   $	��180��6�   �� 5i      -0   5        $% BitLockerPermissionEnableButton��5i      $��1��6F   $
��Left=�6V   $��
26,357,0,0q�6q   $	��Top=�6�   $	��145��6�   $��20��6�   .��5j      � .+�5k      ��  �� Orientation$ 
Horizontal=�6)   $��65��6B   .+�5l   "   �� $��Enable��6(   $��25��69   $��Center=�6E   $��Center=�6`   �� 5m   "   $��UniformToFill=�6(   $��
shield.ico��6@   $��Right=�6T   $��Center=�6p   $��15��6�   $��15��6�   5n      5o      5p      �� 5q      -1   5        $&  BitLockerPermissionDisableButton��5q      $��1��6G   $
��Left=�6W   $��
26,383,0,0q�6r   $	��Top=�6�   $	��145��6�   $��20��6�   .��5r      � .+�5s      �� $ 
Horizontal=�6)   $��65��6B   .+�5t   "   �� $��Disable��6(   $��25��6:   $��Center=�6F   $��Center=�6a   �� 5u   "   $��UniformToFill=�6(   $��
shield.ico��6@   $��Right=�6T   $��Center=�6p   $��15��6�   $��15��6�   5v      5w      5x      5z      5{      5|      5~      5      