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
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Runtime.InteropServices;


namespace XMLScreenSaver
{
    public partial class ScreenSaverForm : Form
    {

        #region Win32 API functions

        [DllImport("user32.dll")]
        static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);

        [DllImport("user32.dll")]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll")]
        static extern bool GetClientRect(IntPtr hWnd, out Rectangle lpRect);

        #endregion
        
        private XDocument _xmlConfig = null;
        private List<Win32.MonitorInformation> _win32MonitorsList;
        private List<int> _rightScreenXList = null;
        private Point _mouseLocation = new Point(0, 0);
        private bool _isPreviewMode = false;
        private string _errorMessages = "";
        private int _changeInterval = 10000;     //Milliseconds
        private int _changeCount = 4;
        private int _fadeTime = 3000;     //Milliseconds
        private List<Image> _leftBitmaps = new List<Image>();
        private List<Image> _rightBitmaps = new List<Image>();
        private Dictionary<string, List<Image>> _resizedBitmaps = new Dictionary<string, List<Image>>();

        private const string labelName = "lblMessage";
        private const string picName = "picImage";

        public ScreenSaverForm(XDocument xmlConfig, List<Win32.MonitorInformation> win32MonitorsList, List<int> rightScreenXList)
        {

            InitializeComponent();

            this.Bounds = Win32.MonitorInformation.GetAllMonitorBounds(win32MonitorsList);
            this._xmlConfig = xmlConfig;
            this._win32MonitorsList = win32MonitorsList;
            this._rightScreenXList = rightScreenXList;

            Constructor();

        }               //public ScreenSaverForm(Rectangle bounds, XDocument xmlConfig, List<Win32.MonitorInformation> win32MonitorsList, List<int> rightScreenXList)


        public ScreenSaverForm(IntPtr previewWndHandle, XDocument xmlConfig)
        {
            InitializeComponent();

            // Set the preview window as the parent of this window.
            SetParent(this.Handle, previewWndHandle);

            // Make this a child window so it will close when the parent dialog closes.
            // GWL_STYLE = -16, WS_CHILD = 0x40000000
            SetWindowLong(this.Handle, -16, new IntPtr(GetWindowLong(this.Handle, -16) | 0x40000000));

            // Place our window inside the parent.
            Rectangle ParentRect;
            GetClientRect(previewWndHandle, out ParentRect);
            Size = ParentRect.Size;
            Location = new Point(0, 0);

            // Required for handling key / mouse.
            _isPreviewMode = true;

            // Create "dummy" objects for common code to use.
            Rectangle monBounds = new Rectangle(0, 0, ParentRect.Size.Width, ParentRect.Size.Height);
            List <Win32.MonitorInformation> tmpMonitorList = new List<Win32.MonitorInformation>();
            tmpMonitorList.Add
                (
                new Win32.MonitorInformation
                    ("Preview", "Preview", "", "Preview", "Preview", "", "", monBounds, 0, 0, true, false)
                );
            List<int> tmpXList = new List<int>();

            // Set the values and call common code.
            this._xmlConfig = xmlConfig;
            this._win32MonitorsList = tmpMonitorList;
            this._rightScreenXList = tmpXList;

            Constructor();

        }               //public ScreenSaverForm(IntPtr PreviewWndHandle, XDocument xmlConfig)


        /// <summary>
        /// Code that is common for both constructors.
        /// </summary>
        private void Constructor()
        {

            if (_xmlConfig == null)
            {
                this._errorMessages = "Error loading the XML Configuration File.";
            }
            else
            {

                // Load the bitmaps from xmlConfig.
                XElement framesElement = _xmlConfig.Root.Element("frames");

                var frameTags = from frameTag in framesElement.Elements("frame")
                                   orderby Int32.Parse(frameTag.Attribute("index").Value)
                                   select frameTag;

                foreach (XElement frameTag in frameTags)
                {
                    try
                    {
                        _leftBitmaps.Add(new Bitmap(frameTag.Attribute("leftImage").Value));
                    }
                    catch
                    {
                        _errorMessages += "Error loading image file: " + frameTag.Attribute("leftImage").Value
                            + Environment.NewLine;
                    }
                    try
                    {
                        _rightBitmaps.Add(new Bitmap(frameTag.Attribute("rightImage").Value));
                    }
                    catch
                    {
                        _errorMessages += "Error loading image file: " + frameTag.Attribute("rightImage").Value
                            + Environment.NewLine;
                    }
                }               //foreach (XElement frameTag in frameTags)

                // Load information from xmlConfig.
                this._changeCount =
                    ((IEnumerable<XElement>)framesElement.Elements()).Count();
                this._changeInterval =
                    Int32.Parse(_xmlConfig.Root.Element("settings").Element("displayTimeSeconds").Value) * 1000;
                XElement backgroundColor =
                    _xmlConfig.Root.Element("settings").Element("backgroundColor");
                if (backgroundColor != null)
                    this.BackColor = ColorTranslator.FromHtml(backgroundColor.Value);

            }               //if ( _xmlConfig == null )

            // Resize the images as required for each screen.
            if (_errorMessages == "")
            {
                foreach (Win32.MonitorInformation mon in _win32MonitorsList)
                {
                    List<Image> tmpBitmaps = new List<Image>();
                    bool isRightSideMonitor = _rightScreenXList.Contains(mon.Bounds.X);
                    string resizeKey =
                        (isRightSideMonitor ? "right" : "left") + ":" +
                        mon.Bounds.Width + "," + mon.Bounds.Height;

                    if (!_resizedBitmaps.ContainsKey(resizeKey))
                    {
                        if (isRightSideMonitor)
                            foreach (Image bm in _rightBitmaps)
                                tmpBitmaps.Add(ImageExtension.Resize(bm, mon.Bounds.Size));
                        else
                            foreach (Image bm in _leftBitmaps)
                                tmpBitmaps.Add(ImageExtension.Resize(bm, mon.Bounds.Size));

                        _resizedBitmaps.Add(resizeKey, tmpBitmaps);
                    }
                }
            }               //if ( _errorMessages == "" )


            // Dynamically layout the controls on the form.
            if (_errorMessages != "")
            {
                if (_isPreviewMode) _errorMessages = "Please configure"+Environment.NewLine+"XMLScreenSaver";

                for (int i = 0; i < _win32MonitorsList.Count; i++)
                {
                    Label tmpLabel = new Label();
                    tmpLabel.Name = labelName + i;
                    tmpLabel.AutoSize = true;
                    tmpLabel.BackColor = Color.Transparent;
                    tmpLabel.Font = new Font("Courier New", (_isPreviewMode ? 8 : 12));
                    tmpLabel.ForeColor = Color.Lime;
                    tmpLabel.TabIndex = 0;
                    tmpLabel.Bounds = _win32MonitorsList[i].Bounds;
                    tmpLabel.Text = _errorMessages;
                    tmpLabel.Visible = false;
                    this.Controls.Add(tmpLabel);
                }
            }
            else
            {
                for (int i = 0; i < _win32MonitorsList.Count; i++)
                {
                    PictureBox tmpPictureBox = new PictureBox();
                    tmpPictureBox.Name = picName + i;
                    tmpPictureBox.BackColor = Color.Transparent;
                    tmpPictureBox.Anchor = AnchorStyles.None;
                    tmpPictureBox.Margin = new Padding(0);
                    tmpPictureBox.Padding = new Padding(0);
                    tmpPictureBox.Bounds = _win32MonitorsList[i].Bounds;
                    tmpPictureBox.Visible = false;
                    tmpPictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseMove);
                    tmpPictureBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.PictureBox_MouseClick);
                    this.Controls.Add(tmpPictureBox);
                }

            }

        }       //private void Constructor()


        private void ScreenSaverForm_Load(object sender, EventArgs e)
        {
            
            Cursor.Hide();
            TopMost = true;

            //The interval is recalcuated every time the timer fires.
            tmrAnimate.Interval = 10;
            tmrAnimate.Tick += new EventHandler(tmrAnimate_Tick);
            tmrAnimate.Start();

        }

        private void tmrAnimate_Tick(object sender, EventArgs e)
        {
            // Calculate index and time delays.
            DateTimeOffset now = DateTimeOffset.UtcNow;
            long nowNumber = now.ToUnixTimeMilliseconds();
            int currentIndex = (int)(nowNumber / _changeInterval) % _changeCount;
            int timeInCurrentFrame = (int)(nowNumber % _changeInterval);
            int delayToNextFrame = (int)(_changeInterval - (nowNumber % _changeInterval));
            int timerDelay = delayToNextFrame;
            
            // If Error Message exists, display it. Otherwise, display the images.
            if (_errorMessages != "")
            {
                foreach (Label lbl in Controls.OfType<Label>())
                {
                    int i = Int32.Parse(lbl.Name.Substring(labelName.Length));
                    lbl.Top = _win32MonitorsList[i].Bounds.Top + 20 + (_isPreviewMode ? 6 : 10) * currentIndex;
                    lbl.Visible = true;
                }
            }
            else
            {
                foreach (PictureBox pic in Controls.OfType<PictureBox>())
                {
                    string resizeKey =
                        (_rightScreenXList.Contains(pic.Bounds.X) ? "right" : "left") + ":" +
                        pic.Bounds.Width + "," + pic.Bounds.Height;

                    if ( timeInCurrentFrame < _fadeTime )
                    {
                        pic.Image = ImageExtension.PaintColor
                                    (
                                    _resizedBitmaps[resizeKey][currentIndex], this.BackColor,
                                    255 - (int)(255.0 * (float)timeInCurrentFrame / (float)_fadeTime)
                                    );
                        timerDelay = 33;    //30 frames per second
                    }
                    else if (delayToNextFrame < _fadeTime)
                    {
                        pic.Image = ImageExtension.PaintColor
                                    (
                                    _resizedBitmaps[resizeKey][currentIndex], this.BackColor,
                                    255 - (int)(255.0 * (float)delayToNextFrame / (float)_fadeTime)
                                    );
                        timerDelay = 33;    //30 frames per second
                    }
                    else
                    {
                        pic.Image = _resizedBitmaps[resizeKey][currentIndex];
                        timerDelay = delayToNextFrame - _fadeTime;
                    }
                    
                    pic.Visible = true;
                }
            }

            // Set delay to next tick event.
            if (timerDelay < 10) timerDelay = 10;     //Small possibility of calculated value = 0
            tmrAnimate.Interval = timerDelay;
        }

        private void ScreenSaverForm_MouseMove(object sender, MouseEventArgs e)
        {
            if (!_mouseLocation.IsEmpty)
            {
                // Only exit if mouse is moved a significant distance.
                if (Math.Abs(_mouseLocation.X - e.X) > 10 ||
                    Math.Abs(_mouseLocation.Y - e.Y) > 10)
                    if (!_isPreviewMode) Application.Exit();
            }

            // Update current mouse location.
            _mouseLocation = e.Location;
        }

        private void ScreenSaverForm_MouseClick(object sender, MouseEventArgs e)
        {
            if (!_isPreviewMode) Application.Exit();
        }

        private void ScreenSaverForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!_isPreviewMode) Application.Exit();
        }

        private void PictureBox_MouseMove(object sender, MouseEventArgs e)
        {
            ScreenSaverForm_MouseMove(sender, e);
        }

        private void PictureBox_MouseClick(object sender, MouseEventArgs e)
        {
            ScreenSaverForm_MouseClick(sender, e);
        }


    }
}
