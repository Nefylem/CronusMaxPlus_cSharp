using System;
using System.IO;
using System.Runtime.InteropServices;

namespace CronusMaxPlusWrapper.CronusMaxPlus
{
    class CmPlus
    {
        public CmPlus(Define define) { _cm = define; }
        private readonly Define _cm;

        public bool Init()
        {
            const string api = "gcdapi.dll";

            if (!File.Exists(api)) return false;
            
            var ptrDll = Define.LoadLibrary(api);
            if (ptrDll == IntPtr.Zero) return false;

            var ptrLoad = LoadExternalFunction(ptrDll, "gcdapi_Load");
            if (ptrLoad == IntPtr.Zero) return false;

            var ptrIsConnected = LoadExternalFunction(ptrDll, "gcapi_IsConnected");
            if (ptrIsConnected == IntPtr.Zero) return false;

            var ptrUnload = LoadExternalFunction(ptrDll, "gcdapi_Unload");
            if (ptrUnload == IntPtr.Zero) return false;

            var ptrGetTimeVal = LoadExternalFunction(ptrDll, "gcapi_GetTimeVal");
            if (ptrGetTimeVal == IntPtr.Zero) return false;

            var ptrGetFwVer = LoadExternalFunction(ptrDll, "gcapi_GetFWVer");
            if (ptrGetFwVer == IntPtr.Zero) return false;

            var ptrWrite = LoadExternalFunction(ptrDll, "gcapi_Write");
            if (ptrWrite == IntPtr.Zero) return false;

            var ptrRead = LoadExternalFunction(ptrDll, "gcapi_Read");
            if (ptrRead == IntPtr.Zero) return false;

            var ptrWriteEx = IntPtr.Zero;     //Refer to the CM api for these
            var ptrReadEx = IntPtr.Zero;

            var ptrCalcPressTime = LoadExternalFunction(ptrDll, "gcapi_CalcPressTime");
            if (ptrCalcPressTime == IntPtr.Zero) return false;

            try
            {
                _cm.Load = (Define.GcapiLoad)Marshal.GetDelegateForFunctionPointer(ptrLoad, typeof(Define.GcapiLoad));
                _cm.IsConnected = (Define.GcapiIsConnected)Marshal.GetDelegateForFunctionPointer(ptrIsConnected, typeof(Define.GcapiIsConnected));
                _cm.Unload = (Define.GcapiUnload)Marshal.GetDelegateForFunctionPointer(ptrUnload, typeof(Define.GcapiUnload));
                _cm.GetTimeVal = (Define.GcapiGetTimeVal)Marshal.GetDelegateForFunctionPointer(ptrGetTimeVal, typeof(Define.GcapiGetTimeVal));
                _cm.GetFwVer = (Define.GcapiGetFwVer)Marshal.GetDelegateForFunctionPointer(ptrGetFwVer, typeof(Define.GcapiGetFwVer));
                _cm.Write = (Define.GcapiWrite)Marshal.GetDelegateForFunctionPointer(ptrWrite, typeof(Define.GcapiWrite));
                _cm.Read = (Define.GcapiRead)Marshal.GetDelegateForFunctionPointer(ptrRead, typeof(Define.GcapiRead));
                _cm.CalcPressTime = (Define.GcapiCalcPressTime)Marshal.GetDelegateForFunctionPointer(ptrCalcPressTime, typeof(Define.GcapiCalcPressTime));
            }
            catch (Exception ex)
            {
                //return ex.Message.ToString();
                return false;
            }

            return true;
        }

        private IntPtr LoadExternalFunction(IntPtr ptrDll, string strFunction)
        {
            var ptrFunction = Define.GetProcAddress(ptrDll, strFunction);
            
            if (ptrFunction == IntPtr.Zero)
                return IntPtr.Zero;                 //Error check this    

            return ptrFunction;
        }

    }
}
