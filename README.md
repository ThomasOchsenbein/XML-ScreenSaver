# XML ScreenSaver


### Overview

XML ScreenSaver is an open source Windows Screensaver that is designed for dual monitor computers.

XML ScreeSaver cycles through the images as specified by a simple XML file.

You can create images that span across 2 monitors, by splitting the images into left and right halves.

After it is installed, you can easily update the configuration by modifying the image files or the XML file.


### Setup instructions

Please compile the application and create XMLScreenSaver.scr.  
If you are familiar with Visual Studio, open the the solution and compile with Visual Studio.  
Alternatively, please follow the build instructions below.

Right click on XMLScreenSaver.scr and select Install (will be bin\Debug\ folder).  
The Windows ScreenSaver dialog will appear with XML ScreenSaver selected.  
Click "Settings..." and download the sample XML file. Close the XML ScreenSaver settings window.  
Modify the sample XML file so it references the image files you want displayed.  
Set the environment variable XML_SCREENSAVER_CONFIG_FILENAME to the name of the XML file.  
Close the Windows ScreenSaver dialog. Right click on XMLScreenSaver.scr a second time and select Install.  
If the XML file is setup correctly, the left screen images will appear on the preview window.  
Click OK and XML ScreenSaver will be installed as the default screen saver.


##### Build XMLScreenSaver without Visual Studio IDE

1.  Download the zip file.

2.  Downloaded zip files are usually flagged due to coming from internet zone.
    Right click on the zip file -> Properties -> Unblock.

3.  Unzip the zip file.

4.  Download and install "Build Tools for Visual Studio 2019".

    The installer will display screen with list of workloads to install.
    Choose .NET desktop build tools.
    Then on right side, select all options except "F# compiler".
    
6.  In command prompt, change to the directory that has the file XMLScreenSaver.csproj

7.  Run msbuild.exe to build the project.

~~~~
     "C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\msbuild.exe" XMLScreenSaver.csproj
~~~~


### Version History

| Date       | Version   | Description                                          |
|------------|-----------|------------------------------------------------------|
| 2020-08-02 | 1.0.0.0   | First public release.                                |


### License

This software is licensed under GPL Version 3.0 - https://www.gnu.org/licenses/gpl-3.0.txt

Copyright © 2020 Thomas Ochsenbein.
