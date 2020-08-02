# XML ScreenSaver


### Overview

XML ScreenSaver is an open source Windows Screensaver that is designed for dual monitor computers.

It uses a simple XML file to specify the images that are displayed.


### Setup instructions

1.  Install a version of Visual Studio.
    If you will never be doing any coding, search for "Build Tools for Visual Studio 2019" and install it.
   
2.  Download the repository (zip file).

3.  Downloaded zip files are usually flagged due to coming from internet zone.
    Right click on the zip file -> Properties -> Unblock.

4.  Unzip the zip file.
    
5.  Open a command prompt and determine the location of msbuild.exe:

     C:
     cd "\Program Files (x86)"
     dir msbuild.exe /s

6.  In command prompt, change to the directory that has the file XMLScreenSaver.csproj

7.  Run msbuild.exe to build the project. The exact location of msbuild.exe can vary. For example:

     "C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\MSBuild\Current\Bin\msbuild.exe" XMLScreenSaver.csproj

8.  In Windows Explorer go to bin\Debug\ folder. Right click on XMLScreenSaver.scr and select Install.
    The Windows ScreenSaver dialog appears with XML ScreenSaver selected.

10. Click "Settings..." and download the sample XML file. Close the XML ScreenSaver settings window.

11. Modify the sample XML file so it references the image files you want displayed.

12. Set the environment variable XML_SCREENSAVER_CONFIG_FILENAME to the name of the XML file.

13. Close the Windows ScreenSaver dialog. Right click on XMLScreenSaver.scr a second time and select Install.

14. If the XML file is setup correctly, the left screen images will appear on the preview window.

15. Click OK and it will be installed as the default screen saver.


### Version History

| Date       | Version   | Description                                          |
|------------|-----------|------------------------------------------------------|
| 2020-08-02 | 1.0.0.0   | First public release.                                |


### License

This software is licensed under GPL Version 3.0 - https://www.gnu.org/licenses/gpl-3.0.txt

Copyright © 2020 Thomas Ochsenbein.

