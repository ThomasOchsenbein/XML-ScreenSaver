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
 * Implemented Win32 API library calls to get more detailed information about the displays.
 * EnumDisplayDevices returns an Adapter RegKey value.
 * This is value is not the same for all screens connected to same adapter, so have to implement
 * clumsy code to try and do matching on Adapter.
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
        public byte[] EdidData { get; }     // Can be null.
        public string EdidDataString
        {
            get
            {
                if (EdidData == null) return "";
                return BitConverter.ToString(EdidData);
            }
        }
        public string DeviceModelName
        {
            get
            {
                if (EdidData == null) return "";

                const int EDID_DISPLAY_PRODUCT_NAME = 0xFC;

                int descriptorStringOffset = 0;
                string retValue = "";

                // Locate the display model name descriptor and extract the string.
                List<int> edidDescriptorOffsets = new List<int> { 0x48, 0x5a, 0x6c };
                foreach (int offset in edidDescriptorOffsets)
                {
                    if (
                       EdidData[offset]   == 0x00 &&
                       EdidData[offset+1] == 0x00 &&
                       EdidData[offset+2] == 0x00 && 
                       EdidData[offset+3] == EDID_DISPLAY_PRODUCT_NAME
                       )
                    {
                        descriptorStringOffset = offset + 5;
                    }
                }

                if (descriptorStringOffset > 0)
                {
                    retValue = System.Text.Encoding.ASCII.GetString(EdidData, descriptorStringOffset, 13);
                    if (retValue.IndexOf('\n') >= 0) retValue = retValue.Substring(0, retValue.IndexOf('\n'));
                }

                return retValue;
            }
        }
        public int WidthMM
        {
            get
            {
                if (EdidData == null) return 0;
                return ((EdidData[68] & 0xF0) << 4) + EdidData[66];
            }
        }
        public int HeightMM
        {
            get
            {
                if (EdidData == null) return 0;
                return ((EdidData[68] & 0x0F) << 8) + EdidData[67];
            }
        }
        public bool IsPrimary { get; }
        public bool IsDisabled { get; }


        public MonitorInformation
            (
            string adapterName, string adapterType, string adapterRegKey,
            string deviceName, string deviceType, string deviceRegKey, string deviceId,
            Rectangle bounds, int bitsPerPixel, int displayFrequency, byte[] edidData,
            bool isPrimary, bool isDisabled
            )
        {
            AdapterName = adapterName; AdapterType = adapterType; AdapterRegKey = adapterRegKey;
            DeviceName = deviceName; DeviceType = deviceType; DeviceRegKey = deviceRegKey; DeviceId = deviceId;
            Bounds = bounds; BitsPerPixel = bitsPerPixel; DisplayFrequency = displayFrequency;
            EdidData = edidData; IsPrimary = isPrimary; IsDisabled = isDisabled;
        }


        #region Win32Declarations

        const uint EDD_GET_DEVICE_INTERFACE_NAME = 0x00000001;

        const uint DISPLAY_DEVICE_ATTACHED_TO_DESKTOP = 0x00000001;
        const uint DISPLAY_DEVICE_PRIMARY_DEVICE = 0x00000004;
        const uint DISPLAY_DEVICE_ACTIVE = 0x00000001;
        const uint DISPLAY_DEVICE_MIRRORING_DRIVER = 0x00000008;

        const uint ENUM_CURRENT_SETTINGS = 0xFFFFFFFF;
        const uint ENUM_REGISTRY_SETTINGS = 0xFFFFFFFE;

        const int ERROR_SUCCESS = 0;
        const int ERROR_NO_MORE_ITEMS = 259;
        const Int64 INVALID_HANDLE_VALUE = -1;
        const int BUFFER_SIZE = 500;
        const int DIGCF_PRESENT = 0x00000002;
        const int DIGCF_DEVICEINTERFACE = 0x00000010;
        const int DICS_FLAG_GLOBAL = 0x00000001;
        const int DIREG_DEV = 0x00000001;
        const int KEY_READ = 0x20019;


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

        [StructLayout(LayoutKind.Sequential)]
        struct SP_DEVICE_INTERFACE_DATA
        {
            public UInt32 cbSize;
            public Guid interfaceClassGuid;
            public Int32 flags;
            private UIntPtr reserved;
        }

        [StructLayout(LayoutKind.Sequential)]
        struct SP_DEVINFO_DATA
        {
            public UInt32 cbSize;
            public Guid ClassGuid;
            public UInt32 DevInst;
            public IntPtr Reserved;
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        struct SP_DEVICE_INTERFACE_DETAIL_DATA
        {
            public int cbSize;
            [MarshalAs(UnmanagedType.ByValTStr, SizeConst = BUFFER_SIZE)]
            public string DevicePath;
        }


        [DllImport("user32.dll")]
        static extern bool EnumDisplayDevices
            (string lpDevice, uint iDevNum, ref DISPLAY_DEVICE lpDisplayDevice, uint dwFlags);

        [DllImport("user32.dll")]
        static extern bool EnumDisplaySettingsEx
            (string lpszDeviceName, uint iModeNum, ref DEVMODE lpDevMode, uint dwFlags);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        // 1st form using a ClassGUID only, with null Enumerator.
        static extern IntPtr SetupDiGetClassDevs(ref Guid ClassGuid, IntPtr Enumerator, IntPtr hwndParent, uint Flags);

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern Boolean SetupDiEnumDeviceInterfaces
            (
            IntPtr hDevInfo, IntPtr devInfo, ref Guid interfaceClassGuid, UInt32 memberIndex,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData
            );

        [DllImport("setupapi.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern Boolean SetupDiGetDeviceInterfaceDetail
            (
            IntPtr hDevInfo,
            ref SP_DEVICE_INTERFACE_DATA deviceInterfaceData,
            ref SP_DEVICE_INTERFACE_DETAIL_DATA deviceInterfaceDetailData,
            UInt32 deviceInterfaceDetailDataSize,
            ref UInt32 requiredSize,
            ref SP_DEVINFO_DATA deviceInfoData
            );

        [DllImport("setupapi.dll", SetLastError = true)]
        static extern bool SetupDiDestroyDeviceInfoList(IntPtr DeviceInfoSet);

        [DllImport("Setupapi", CharSet = CharSet.Auto, SetLastError = true)]
        static extern UIntPtr SetupDiOpenDevRegKey
            (
            IntPtr hDeviceInfoSet,
            ref SP_DEVINFO_DATA deviceInfoData,
            int scope,
            int hwProfile,
            int parameterRegistryValueKind,
            int samDesired
            );

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern int RegCloseKey(UIntPtr hKey);

        [DllImport("advapi32.dll", SetLastError = true)]
        static extern uint RegEnumValue
            (
            UIntPtr hKey,
            uint dwIndex,
            StringBuilder lpValueName,
            ref uint lpcValueName,
            IntPtr lpReserved,
            IntPtr lpType,
            IntPtr lpData,
            ref int lpcbData
            );


        #endregion


        /// <summary>
        /// Retrieves list of all monitors.
        /// </summary>
        public static List<MonitorInformation> EnumerateMonitors(bool includeDisabledMonitors = false)
        {

            List<MonitorInformation> newList = new List<MonitorInformation>();

            Dictionary<string, byte[]> allEdids = GetAllEdids();

            DISPLAY_DEVICE display = new DISPLAY_DEVICE();
            display.cb = Marshal.SizeOf(display);

            uint dev = 0;

            while (EnumDisplayDevices(null, dev, ref display, EDD_GET_DEVICE_INTERFACE_NAME))
            {

                DISPLAY_DEVICE monitor = new DISPLAY_DEVICE();
                monitor.cb = Marshal.SizeOf(monitor);

                // Ignore virtual mirror displays.
                if ((display.StateFlags & DISPLAY_DEVICE_MIRRORING_DRIVER) == 0)
                {

                    // Get info about the monitor attached to the display device.
                    uint monIdx = 0;
                    while (EnumDisplayDevices(display.DeviceName, monIdx, ref monitor, EDD_GET_DEVICE_INTERFACE_NAME))
                    {
                        if ((monitor.StateFlags & DISPLAY_DEVICE_ACTIVE) > 0) break;
                        monIdx++;
                    }

                    if (monitor.DeviceString == "") monitor.DeviceString = "Default Monitor";

                    // Get information about the display's position and the current display mode.
                    DEVMODE monitorSettings = new DEVMODE();
                    monitorSettings.dmSize = (short)Marshal.SizeOf(monitorSettings);

                    if (EnumDisplaySettingsEx(display.DeviceName, ENUM_CURRENT_SETTINGS, ref monitorSettings, 0) == false)
                    {
                        EnumDisplaySettingsEx(display.DeviceName, ENUM_REGISTRY_SETTINGS, ref monitorSettings, 0);
                    }

                    // All info retrieved - add this monitor to the list.
                    Rectangle tmpBounds = new Rectangle();
                    tmpBounds.X = monitorSettings.dmPositionX;
                    tmpBounds.Y = monitorSettings.dmPositionY;
                    tmpBounds.Width = monitorSettings.dmPelsWidth;
                    tmpBounds.Height = monitorSettings.dmPelsHeight;

                    Byte[] tmpEdid = null;
                    if (allEdids.ContainsKey(monitor.DeviceID.ToLower())) tmpEdid = allEdids[monitor.DeviceID.ToLower()];

                    if (((display.StateFlags & DISPLAY_DEVICE_ATTACHED_TO_DESKTOP) != 0) || includeDisabledMonitors == true)
                    {
                        newList.Add(
                            new MonitorInformation(
                                display.DeviceName, display.DeviceString, display.DeviceKey,
                                monitor.DeviceName, monitor.DeviceString, monitor.DeviceKey, monitor.DeviceID,
                                tmpBounds, monitorSettings.dmBitsPerPel, monitorSettings.dmDisplayFrequency,
                                tmpEdid,
                                ((display.StateFlags & DISPLAY_DEVICE_PRIMARY_DEVICE) != 0),
                                ((display.StateFlags & DISPLAY_DEVICE_ATTACHED_TO_DESKTOP) == 0)
                                )
                            );
                    }

                }

                // Move to next device.
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

            foreach (MonitorInformation mon in monitors)
            {
                if (mon.Bounds.X < x) x = mon.Bounds.X;
                if (mon.Bounds.Y < y) y = mon.Bounds.Y;
                if (mon.Bounds.X + mon.Bounds.Width > width) width = mon.Bounds.X + mon.Bounds.Width;
                if (mon.Bounds.Y + mon.Bounds.Height > height) height = mon.Bounds.Y + mon.Bounds.Height;
            }

            return new Rectangle(new Point(x, y), new Size(width, height));

        }               //public static Rectangle GetAllMonitorBounds(List<MonitorInformation> monitors)


        /// <summary>
        /// Helper function to get all Edids from Setup API.
        /// Dictionary Key is the device path of the monitor in lower case.
        /// Dictionary Value is the EDID byte array.
        /// </summary>
        private static Dictionary<string, byte[]> GetAllEdids()
        {

            Dictionary<string, byte[]> retEdids = new Dictionary<string, byte[]>();
            Guid monitorGuid = new Guid(0xe6f07b5f, 0xee97, 0x4a90, 0xb0, 0x76, 0x33, 0xf5, 0x7b, 0xf4, 0xea, 0xa7);
            string devicePath;
            bool retValue1 = true;

            // Retrieve all monitor devices.
            IntPtr hMonitorDevices =
                SetupDiGetClassDevs(ref monitorGuid, IntPtr.Zero, IntPtr.Zero, (uint)(DIGCF_PRESENT | DIGCF_DEVICEINTERFACE));
            if (hMonitorDevices.ToInt64() == INVALID_HANDLE_VALUE) return retEdids;

            for (uint i = 0; retValue1 == true; i++)
            {
                // Create a Device Interface Data structure.
                SP_DEVICE_INTERFACE_DATA devIntData = new SP_DEVICE_INTERFACE_DATA();
                devIntData.cbSize = (uint)Marshal.SizeOf(devIntData);

                // Enumerate the devices.
                retValue1 = SetupDiEnumDeviceInterfaces(hMonitorDevices, IntPtr.Zero, ref monitorGuid, i, ref devIntData);
                if (retValue1 == false) break;

                // Build a Device Info Data structure.
                SP_DEVINFO_DATA devData = new SP_DEVINFO_DATA();
                devData.cbSize = (uint)Marshal.SizeOf(devData);

                // Build a Device Interface Detail Data structure.
                SP_DEVICE_INTERFACE_DETAIL_DATA devIntDetail = new SP_DEVICE_INTERFACE_DETAIL_DATA();
                devIntDetail.cbSize = (int)(4 + Marshal.SystemDefaultCharSize); // from pinvoke.net

                // Get detailed information.
                uint nRequiredSize = 0;
                uint nBytes = BUFFER_SIZE;
                if (SetupDiGetDeviceInterfaceDetail(hMonitorDevices, ref devIntData, ref devIntDetail, nBytes, ref nRequiredSize, ref devData))
                {
                    devicePath = devIntDetail.DevicePath.ToLower();
                }
                else
                {
                    continue;   // Move to next monitor device.
                }

                UIntPtr hDevRegKey = SetupDiOpenDevRegKey
                    (hMonitorDevices, ref devData, DICS_FLAG_GLOBAL, 0, DIREG_DEV, KEY_READ);
                if (hDevRegKey == null) continue;   // Move to next monitor device.

                StringBuilder valueName = new StringBuilder(128);
                uint valueNameLength = 128;
                int edidDataSize = 1024;
                byte[] edidData = new byte[edidDataSize];
                IntPtr pEdidData = Marshal.AllocHGlobal(edidData.Length);
                Marshal.Copy(edidData, 0, pEdidData, edidData.Length);

                for (uint j = 0, retValue2 = ERROR_SUCCESS; retValue2 == ERROR_SUCCESS; j++)
                {
                    retValue2 = RegEnumValue
                        (hDevRegKey, j, valueName, ref valueNameLength, IntPtr.Zero, IntPtr.Zero, pEdidData, ref edidDataSize);

                    if (retValue2 != ERROR_SUCCESS || edidDataSize < 1) continue;

                    if (valueName.ToString().Contains("EDID"))
                    {
                        // Save the Edid.
                        byte[] returnEDID = new byte[edidDataSize];
                        Marshal.Copy(pEdidData, returnEDID, 0, edidDataSize);

                        retEdids.Add(devicePath, returnEDID);

                    }

                }

                Marshal.FreeHGlobal(pEdidData);
                RegCloseKey(hDevRegKey);

            }               //for (i = 0; retValue1 == true; i++)

            return retEdids;

        }               //private static Dictionary<string, byte[]> GetAllEdids()

    }

}
