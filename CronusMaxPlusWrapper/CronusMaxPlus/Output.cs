namespace CronusMaxPlusWrapper.CronusMaxPlus
{
    class Output
    {
        public Output(Define cm) { _cm = cm; }
        private readonly Define _cm;

        public Define.GcapiReportControllermax Write(byte[] output)
        {
            var report = new Define.GcapiReportControllermax();

            if (_cm.IsConnected() != 1) return report;

            _cm.Write(output);
            _cm.Read(ref report);

            return report;
        }
    }
}
