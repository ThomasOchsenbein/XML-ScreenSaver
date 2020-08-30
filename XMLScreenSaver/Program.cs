/*
 * Copyright (c) 2020, Thomas Ochsenbein
 * All rights reserved.
 * 
 * GPL-3.0-only
 * 
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, version 3.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <https://www.gnu.org/licenses/>.
 *   
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS 
 * "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT 
 * LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS 
 * FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE 
 * COPYRIGHT OWNER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, 
 * BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; 
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER 
 * CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN 
 * ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE 
 * POSSIBILITY OF SUCH DAMAGE.
 * 
 */

/*
 * 
 * Thank you to Frank McCown for his excellent Windows ScreenSaver tutorial:
 * https://sites.harding.edu/fmccown/screensaver/screensaver.html
 * 
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Runtime.InteropServices;


namespace XMLScreenSaver
{
    static class Program
    {


        #region Win32 API functions

        [DllImport("Shcore.dll")]
        static extern int SetProcessDpiAwareness(int PROCESS_DPI_AWARENESS);

        [DllImport("user32.dll")]
        static extern bool SetForegroundWindow(IntPtr hWnd);

        #endregion


        /// <summary>
        /// The environment variable used to locate the XML Config File.
        /// </summary>
        public static string environmentVariableName = "XML_SCREENSAVER_CONFIG_FILENAME";


        /// <summary>
        /// XSD Schema for validating the config file.
        /// </summary>
        public static string xsdConfigFileSchema =
@"<xs:schema attributeFormDefault='unqualified' elementFormDefault='qualified' xmlns:xs='http://www.w3.org/2001/XMLSchema'>
  <xs:simpleType name='screenSaverDelayType'>
    <xs:restriction base='xs:integer'>
      <xs:minInclusive value='6'/>
      <xs:maxInclusive value='60'/>
    </xs:restriction> 
  </xs:simpleType>
  <xs:simpleType name='htmlColorType'>
    <xs:restriction base='xs:token'>
      <xs:pattern value='#[\dA-Fa-f]{6}'/>
    </xs:restriction>
  </xs:simpleType>
  <xs:simpleType name='multiDetectionType'>
    <xs:restriction base='xs:string'>
      <xs:enumeration value='MonitorModel'/>
      <xs:enumeration value='GraphicsAdapter'/>
    </xs:restriction>
  </xs:simpleType>
  <xs:element name='XMLScreenSaver'>
    <xs:complexType>
      <xs:sequence>
        <xs:element name='settings'>
          <xs:complexType>
            <xs:sequence>
              <xs:element type='screenSaverDelayType' name='displayTimeSeconds'/>
              <xs:element type='htmlColorType' name='backgroundColor' minOccurs='0'/>
              <xs:element type='multiDetectionType' name='multiDetectionMethod' minOccurs='0'/>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
        <xs:element name='frames'>
          <xs:complexType>
            <xs:sequence>
              <xs:element name='frame' minOccurs='2' maxOccurs='60'>
                 <xs:complexType>
                    <xs:complexContent>
                      <xs:restriction base='xs:anyType'>
                        <xs:attribute type = 'xs:int' name='index' use='required'/>
                        <xs:attribute type = 'xs:string' name='leftImage' use='required'/>
                        <xs:attribute type = 'xs:string' name='rightImage' use='required'/>
                      </xs:restriction>
                    </xs:complexContent>
                 </xs:complexType>
              </xs:element>
            </xs:sequence>
          </xs:complexType>
        </xs:element>
      </xs:sequence>
    </xs:complexType>
  </xs:element>
</xs:schema>
";


        /// <summary>
        /// Sample XML file.
        /// </summary>
        public static string xmlSampleFile =
@"<?xml version = '1.0' encoding = 'UTF-8' standalone = 'no' ?>
<!--
Set environment variable " + environmentVariableName + @" to the name of this file.
-->
<XMLScreenSaver>
    <settings>
        <!-- displayTimeSeconds is required. It must be between 6 and 60. -->
        <displayTimeSeconds>10</displayTimeSeconds>
        <!-- backgroundColor is optional. If not specified it defaults to #000000. -->
        <backgroundColor>#221122</backgroundColor>
        <!--
          multiDetectionMethod
          When there are more than 2 monitors, there is no simple method to always determine which
          2 monitors are left/right pairs.
          XML ScreenSaver has 2 methods to detect left/right monitors:
          - MonitorModel      Monitors that are same type are grouped together
          - GraphicsAdapter   Monitors connected to same graphics card in computer are grouped together
          This is optional. If not specified it defaults to MonitorModel.
        -->
        <multiDetectionMethod>MonitorModel</multiDetectionMethod>
    </settings>
    <!-- XML ScreenSaver requires a minimum of 2 frames and has a maximum of 60. -->
    <frames>
        <frame index='1' leftImage='C:\XMLScreenSaver\left_1.png' rightImage='C:\XMLScreenSaver\right_1.png'/>
        <frame index='2' leftImage='C:\XMLScreenSaver\left_2.png' rightImage='C:\XMLScreenSaver\right_2.png'/>
    </frames>
</XMLScreenSaver>
";


        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            // DpiAwareness.PerMonitorAware = 2
            SetProcessDpiAwareness(2);

            string firstArgument = "";
            string secondArgument = "";

            IntPtr parentWndHandle = new IntPtr(0);
            IntPtr configWndHandle = new IntPtr(0);

            XDocument xmlConfig = null;
            List<int> rightScreenXList = new List<int>();
            List<Win32.MonitorInformation> win32MonitorList;

            // Load the XML file.
            xmlConfig = LoadXmlConfigFile(environmentVariableName);

            // Screensaver can receive arugments with a space or column between name and value.
            // Examples: /c:1234567 or /P 1234567
            switch ( args.Length )
            {
                case 0:
                    firstArgument = "/c";   //If nothing provided default to configure form.
                    break;

                case 1:
                    firstArgument = args[0].ToLower().Trim();
                    if (firstArgument.Length > 3)
                    {
                        secondArgument = firstArgument.Substring(3).Trim();
                        firstArgument = firstArgument.Substring(0, 2);
                    }
                    break;

                case 2:
                    firstArgument = args[0].ToLower().Trim();
                    secondArgument = args[1];
                    break;

            }               //switch ( args.Length )


            // Select required action.
            switch ( firstArgument )
            {
                case "/c": // Configuration mode

                    if ( secondArgument != "" ) configWndHandle = new IntPtr(long.Parse(secondArgument));

                    Application.Run(new ScreenSaverSettingsForm());
                    break;

                case "/p": // Preview mode

                    if (secondArgument == "")
                    {
                        MessageBox.Show("Window handle not provided.",
                            "XmlScreenSaver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    parentWndHandle = new IntPtr(long.Parse(secondArgument));

                    // Launch screen saver in preview mode.
                    Application.Run(new ScreenSaverForm(parentWndHandle, xmlConfig));
                    break;

                case "/s": // Fullscreen mode

                    // Load information about the monitors.
                    win32MonitorList = Win32.MonitorInformation.EnumerateMonitors();

                    // Determine which screens are on the right hand side.
                    rightScreenXList = GetRightSideScreens(win32MonitorList, xmlConfig);

                    // Launch screen saver.
                    Application.Run(new ScreenSaverForm(xmlConfig, win32MonitorList, rightScreenXList));
                    break;

                default:
                    MessageBox.Show("Invalid command line argument: \"" + firstArgument + "\"",
                        "ScreenSaver", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    break;

            }               //switch ( firstArgument )

            // If launched from Windows ScreenSaver Settings Form, return focus to it.
            if (configWndHandle.ToInt64() > 0) SetForegroundWindow(configWndHandle);

        }               //static void Main()


        /// <summary>
        /// Loads the xml config file and validates it.
        /// Returns null on any error.
        /// </summary>
        static XDocument LoadXmlConfigFile(string environmentVariableName)
        {

            XDocument xmlConfig = null;
            XmlSchemaSet schemas = new XmlSchemaSet();
            bool isValidXml = true;

            // Read the XML Configuration file.
            try
            {
                string xmlConfigFileName = Environment.GetEnvironmentVariable(environmentVariableName);
                if (xmlConfigFileName == null) return null;
                xmlConfig = XDocument.Load(xmlConfigFileName);

            }
            catch
            {
                return null;
            }

            // Validate the XML against XSD.
            schemas.Add("", XmlReader.Create(new StringReader(xsdConfigFileSchema)));
            xmlConfig.Validate(schemas, (o, e) => {
                isValidXml = false;
            });
            if (!isValidXml) return null;

            return xmlConfig;

        }               //static XDocument LoadXmlConfigFile(string environmentVariableName)


        /// <summary>
        /// Attempts to determine which screens are located on right hand side of another screen.
        /// Returns list of X coordinates for monitors that are on right hand side.
        /// </summary>
        static List<int> GetRightSideScreens(List<Win32.MonitorInformation> win32Monitors, XDocument xmlConfig)
        {

            List<int> xList = new List<int>();
            Dictionary<string, int> groupNames = new Dictionary<string, int>();
            string detectionType = "";

            if (win32Monitors.Count == 1)
            {
                return xList;
            }

            if (win32Monitors.Count == 2)
            {
                if (win32Monitors[0].Bounds.X > win32Monitors[1].Bounds.X)
                    xList.Add(win32Monitors[0].Bounds.X);
                if (win32Monitors[1].Bounds.X > win32Monitors[0].Bounds.X)
                    xList.Add(win32Monitors[1].Bounds.X);
                return xList;
            }

            // If there are more than 2 monitors, have to try and find pairs.
            // There are 2 methods:
            // - monitor model name
            // - graphics adapter they are connected 
            if (xmlConfig != null )
            {
                XElement detectionTypeElement =
                    xmlConfig.Root.Element("settings").Element("multiDetectionMethod");
                if (detectionTypeElement != null)
                    detectionType = detectionTypeElement.Value;
            }

            // Create list of group names.
            foreach (Win32.MonitorInformation mon in win32Monitors)
            {
                string name;

                if (detectionType == "GraphicsAdapter") name = StringExtension.TrimIfFound(mon.AdapterRegKey, "\\");
                else name = mon.DeviceModelName;

                if (groupNames.ContainsKey(name)) groupNames[name] += 1;
                else groupNames.Add(name, 1);
            }

            //For each group, if there is more than 1 screen, select the right most screen.
            foreach (string groupItem in groupNames.Keys)
            {
                if (groupNames[groupItem] > 1)
                {
                    int rightScreenX = -(1 << 30);
                    foreach (Win32.MonitorInformation mon in win32Monitors)
                    {
                        string name;

                        if (detectionType == "GraphicsAdapter") name = StringExtension.TrimIfFound(mon.AdapterRegKey, "\\");
                        else name = mon.DeviceModelName;

                        if (groupItem == name && mon.Bounds.X > rightScreenX) rightScreenX = mon.Bounds.X;
                    }
                    xList.Add(rightScreenX);
                }
            }

            return xList;

        }               //static List<int> GetRightSideScreens()

    }
}
