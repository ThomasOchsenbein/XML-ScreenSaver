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
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;

namespace XMLScreenSaver
{


    public partial class ScreenSaverSettingsForm : Form
    {

        /// <summary>
        /// Instructions.
        /// </summary>
        static string instructions =
@"Configure XML ScreenSaver in 3 easy steps:

1) Save your images to any location that is accessible from Windows File Explorer.

   XML ScreenSaver is optimized for dual monitor configurations.
   If you don't have left/right images, you may choose to use the same image for both left and right monitors.
   If there is only 1 monitor, XML ScreenSaver will use the left image.

2) Download the sample XML file and customize to meet your requirements.

3) Set the environment variable " + Program.environmentVariableName + @" to the name of the XML file.

Thats all! XML ScreenSaver will rotate through the images as specified in the XML file.

If you update the XML file or images, XML SreenSaver will automatically reload the new configuration each time it starts.
";
            
        public ScreenSaverSettingsForm()
        {
            InitializeComponent();
        }

        private void ScreenSaverSettingsForm_Load(object sender, EventArgs e)
        {
            lblInstructions.Text = instructions;
            lblVersion.Text = "XML ScreenSaver Version: " + Application.ProductVersion;
            btnOK.Select();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnAbout_Click(object sender, EventArgs e)
        {
            if ( btnAboutInstructions.Text == "About" )
            {
                tbcAboutInstructions.SelectTab("tabPage2");
                btnAboutInstructions.Text = "Instructions";
            }
            else
            {
                tbcAboutInstructions.SelectTab("tabPage1");
                btnAboutInstructions.Text = "About";
            }
        }

        private void btnDownloadSampleXML_Click(object sender, EventArgs e)
        {
            SaveFileDialog svfXMLFile = new SaveFileDialog();

            svfXMLFile.Filter = "XML Files (*.xml)|*.xml";
            svfXMLFile.Title = "Save XML File";
            svfXMLFile.FileName = "XML_ScreenSaver_Config.xml";
            svfXMLFile.ShowDialog();

            if ( svfXMLFile.FileName != "" )
            {
                File.WriteAllText(svfXMLFile.FileName, Program.xmlSampleFile);
            }
        }
    
    }
}
