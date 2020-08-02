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
 * The standard WinForms System.Windows.Forms Screen object provided by Microsoft is inadequate;
 * a poor effort from Microsoft.
 * It should also have following information:
 * - screen HighDpiMode (DpiUnaware, PerMonitor, PerMonitorV2 etc) 
 * - screen display scale factor (Windows HiDPI scaling e.g. 100%, 125%, 150% etc)
 * - screen physical width and height (in millimetres)
 * - screen model name from EDID (e.g. Sony CPD-E100)
 * - graphics adapter unique identifier
 * - graphics adapter model name (e.g. Nvidia TNT2)
 *  
 * If the standard WinForms Screen object provided graphics adapter unique id, it would be easy to
 * determine which screens are "paired".
 * 
 * Implemented Win32 API library call to get more detailed information about the displays.
 * EnumDisplayDevices returns an Adapter RegKey value.
 * This is value is not the same for all screens connected to same adapter, so have to implement
 * clumsy code to try and do the matching.
 * 
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Runtime.InteropServices;


namespace Win32
{


    public class MonitorInformation
    {

        public string AdapterName { get; }
        public string AdapterType { get; }
        public string AdapterRegKey { get; }
        public string DeviceName { get; }
        public string DeviceType { get; }
        public string DeviceRegKey { get; }
        public string DeviceId { get; }
        public Rectangle Bounds { get; }
        public int BitsPerPixel { get; }
        public int DisplayFrequency { get; }
        public bool IsPrimary { get; }
        public bool IsDisabled { get; }

        public MonitorInformation
            (
            string adapterName, string adapterType, string adapterRegKey,
            string deviceName, string deviceType, string deviceRegKey, string deviceId,
            Rectangle bounds, int bitsPerPixel, int displayFrequency, bool isPrimary, bool isDisabled
            )
        {
            AdapterName = adapterName; AdapterType = adapterType; AdapterRegKey = adapterRegKey;
            DeviceName = deviceName; DeviceType = deviceType; DeviceRegKey = deviceRegKey; DeviceId = deviceId;
            Bounds = bounds; BitsPerPixel = bitsPerPixel; DisplayFrequency = displayFrequency;
            IsPrimary = isPrimary; IsDisabled = isDisabled;
        }


        #region Win32Declarations

        const uint EDD_GET_DEVICE_INTERFACE_NAME = 0x00000001;

        const uint DISPLAY_DEVICE_ATTACHED_TO_DESKTOP = 0x00000001;
        const uint DISPLAY_DEVICE_PRIMARY_DEVICE = 0x00000004;
        const uint DISPLAY_DEVICE_ACTIVE = 0x00000001;
        const uint DISPLAY_DEVICE_MIRRORING_DRIVER = 0x00000008;

        const uint ENUM_CURRENT_SETTINGS = 0xFFFFFFFF;
        const uint ENUM_REGISTRY_SETTINGS = 0xFFFFFFFE;


        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        struct DISPLAY_DEVICE
        {
            [MarshalAs(UnmanagedType.U4)]
            public int cb;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string DeviceName;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceString;

            [MarshalAs(UnmanagedType.U4)]
            public uint StateFlags;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceID;

            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
            public string DeviceKey;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct DEVMODE
        {
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmDeviceName;
            public short dmSpecVersion;
            public short dmDriverVersion;
            public short dmSize;
            public short dmDriverExtra;
            public int dmFields;
            public int dmPositionX;
            public int dmPositionY;
            public int dmDisplayOrientation;
            public int dmDisplayFixedOutput;
            public short dmColor;
            public short dmDuplex;
            public short dmYResolution;
            public short dmTTOption;
            public short dmCollate;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
            public string dmFormName;
            public short dmLogPixels;
            public short dmBitsPerPel;
            public int dmPelsWidth;
            public int dmPelsHeight;
            public int dmDisplayFlags;
            public int dmDisplayFrequency;
            public int dmICMMethod;
            public int dmICMIntent;
            public int dmMediaType;
            public int dmDitherType;
            public int dmReserved1;
            public int dmReserved2;
            public int dmPanningWidth;
            public int dmPanningHeight;
        }


        [DllImport("user32.dll")]
        static extern bool EnumDisplayDevices
            (string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [DllImport("user32.dll")]
        static extern bool EnumDisplaySettingsEx
            (string lpszDeviceName, uint iModeNum, ref DEVMODE lpDevMode, uint dwFlags);


        #endregion


        /// <summary>
        /// Retrieves list of all monitors.
        /// </summary>
        public static List<MonitorInformation> EnumerateMonitors(bool includeDisabledMonitors = false)
        {

            List<MonitorInformation> newList = new List<MonitorInformation>();

            DISPLAY_DEVICE display = new DISPLAY_DEVICE();
            display.cb = Marshal.SizeOf(display);

            uint dev = 0;

            while (EnumDisplayDevices(null, dev, ref display, EDD_GET_DEVICE_INTERFACE_NAME))
            {

                DISPLAY_DEVICE monitor = new DISPLAY_DEVICE();
                monitor.cb = Marshal.SizeOf(monitor);

                // ignore virtual mirror displays
                if ((display.StateFlags & DISPLAY_DEVICE_MIRRORING_DRIVER) == 0)
                {

                    // get info about the monitor attached to the display device
                    uint monIdx = 0;
                    while (EnumDisplayDevices(display.DeviceName, monIdx, ref monitor, EDD_GET_DEVICE_INTERFACE_NAME))
                    {
                        if ((monitor.StateFlags & DISPLAY_DEVICE_ACTIVE) > 0) break;
                        monIdx++;
                    }

                    if (monitor.DeviceString == "") monitor.DeviceString = "Default Monitor";

                    // get information about the display's position and the current display mode
                    DEVMODE monitorSettings = new DEVMODE();
                    monitorSettings.dmSize = (short)Marshal.SizeOf(monitorSettings);

                    if (EnumDisplaySettingsEx(display.DeviceName, ENUM_CURRENT_SETTINGS, ref monitorSettings, 0) == false)
                    {
                        EnumDisplaySettingsEx(display.DeviceName, ENUM_REGISTRY_SETTINGS, ref monitorSettings, 0);
                    }

                    // all info retrieved - add this monitor to the list
                    Rectangle tmpBounds = new Rectangle();
                    tmpBounds.X = monitorSettings.dmPositionX;
                    tmpBounds.Y = monitorSettings.dmPositionY;
                    tmpBounds.Width = monitorSettings.dmPelsWidth;
                    tmpBounds.Height = monitorSettings.dmPelsHeight;

                    if (((display.StateFlags & DISPLAY_DEVICE_ATTACHED_TO_DESKTOP) != 0) || includeDisabledMonitors == true)
                    {
                        newList.Add(
                            new MonitorInformation(
                                display.DeviceName, display.DeviceString, display.DeviceKey,
                                monitor.DeviceName, monitor.DeviceString, monitor.DeviceKey, monitor.DeviceID,
                                tmpBounds, monitorSettings.dmBitsPerPel, monitorSettings.dmDisplayFrequency,
                                ((display.StateFlags & DISPLAY_DEVICE_PRIMARY_DEVICE) != 0),
                                ((display.StateFlags & DISPLAY_DEVICE_ATTACHED_TO_DESKTOP) == 0)
                                )
                            );
                    }

                }

                //move to next device
                dev++;

            }               //while (EnumDisplayDevices(null, dev, ref display, EDD_GET_DEVICE_INTERFACE_NAME))

            return newList;

        }               //public static List<Win32MonitorInformation> EnumerateMonitors()


        /// <summary>
        /// Returns a Rectangle that covers all monitors.
        /// </summary>
        public static Rectangle GetAllMonitorBounds(List<MonitorInformation> monitors)
        {
            int x = 0, y = 0, width = 0, height = 0;

            foreach(MonitorInformation mon in monitors)
            {
                if (mon.Bounds.X < x) x = mon.Bounds.X;
                if (mon.Bounds.Y < y) y = mon.Bounds.Y;
                if (mon.Bounds.X + mon.Bounds.Width > width) width = mon.Bounds.X + mon.Bounds.Width;
                if (mon.Bounds.Y + mon.Bounds.Height > height) height = mon.Bounds.Y + mon.Bounds.Height;
            }

            return new Rectangle(new Point(x,y), new Size(width, height));
        }

    }

}