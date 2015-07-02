using System;
using System.Runtime.InteropServices;

namespace CronusMaxPlusWrapper.CronusMaxPlus
{
    class Define
    {
        [DllImport("kernel32.dll")]
        public static extern IntPtr LoadLibrary(string dllToLoad);

        [DllImport("kernel32.dll")]
        public static extern IntPtr GetProcAddress(IntPtr hModule, string procedureName);

        public struct GcapiConstants
        {
            public const int GcapiInputTotal = 30;
            public const int GcapiOutputTotal = 36;
        }
        public struct GcapiStatus
        {
            public byte Value; // Current value - Range: [-100 ~ 100] %
            public byte PrevValue; // Previous value - Range: [-100 ~ 100] %
            public int PressTv; // Time marker for the button press event
        }
        public struct GcapiReportControllermax
        {
            public byte Console; // Receives values established by the #defines CONSOLE_*
            public byte Controller; // Values from #defines CONTROLLER_* and EXTENSION_*

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
            public byte[] Led; // Four LED - #defines LED_*

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
            public byte[] Rumble; // Two rumbles - Range: [0 ~ 100] %
            public byte BatteryLevel; // Battery level - Range: [0 ~ 10] 0 = empty, 10 = full

            [MarshalAs(UnmanagedType.ByValArray, SizeConst = GcapiConstants.GcapiInputTotal, ArraySubType = UnmanagedType.Struct)]
            public GcapiStatus[] Input;
        }

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate byte GcapiLoad();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate byte GcapiIsConnected();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint GcapiGetTimeVal();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate uint GcapiGetFwVer();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate byte GcapiWrite(byte[] output);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate byte GcapiWriteEx(byte[] output);

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        public delegate byte GcapiWriteref(byte[] output);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate int GcapiCalcPressTime(byte time);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void GcapiUnload();

        [UnmanagedFunctionPointer(CallingConvention.StdCall)]
        internal delegate IntPtr GcapiRead([In, Out] ref GcapiReportControllermax gcapiReport);

        public GcapiLoad Load;
        public GcapiIsConnected IsConnected;
        public GcapiGetTimeVal GetTimeVal;
        public GcapiGetFwVer GetFwVer;
        public GcapiWrite Write;
        public GcapiWriteEx WriteEx;
        public GcapiWriteref WriteRef;
        public GcapiRead Read;
        public GcapiCalcPressTime CalcPressTime;
        public GcapiUnload Unload;

        public void Close()
        {
            if (Unload != null) Unload();

            Load = null;
            IsConnected = null;
            GetTimeVal = null;
            GetFwVer = null;
            Write = null;
            WriteEx = null;
            WriteRef = null;
            Read = null;
            CalcPressTime = null;
            Unload = null;
        }
    }
}
