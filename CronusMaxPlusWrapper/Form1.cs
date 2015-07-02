using System;
using System.Windows.Forms;

namespace CronusMaxPlusWrapper
{
    public partial class Form1 : Form
    {
        /*
         * CronusMax Pro api C# wrapper
         * 
         * Sample project using raw XInput for the gamepad then outputting to the CM api. I've tried to remove any non-essential stuff so it should be easy to follow.
         * Built in Visual Studio C# 2013 community
         * 
         * Beginners tip: click on the function then hit F12 to go to its definition. This will help you navigate the source code.
         * 
         * This is provided as is. If you have any questions, ask on the CM forums
         * 
         * If you make anything cool based on this code, feel free to go to the CM forums and let me know about it
         * 
         * Nefylem
         * 
         */
        private CronusMaxPlus.Define _cm;
        private CronusMaxPlus.Output _output;
        private XInput.Gamepad _gamepad;
        private bool _useRumble;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _useRumble = true;

            _cm = new CronusMaxPlus.Define();
            _output = new CronusMaxPlus.Output(_cm);
            _gamepad = new XInput.Gamepad();

            if (!new CronusMaxPlus.CmPlus(_cm).Init())
            {
                // Return an error string and see whats going on. 
                MessageBox.Show(@"Error opening API");       
                return;
            }
            _cm.Load();
            /*
             * I'm putting this in a timer for simplicity. If you're building a game loop, put it in an update thread. 
             */
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            UpdateCmPlus();
        }

        private void UpdateCmPlus()
        {
            var input = _gamepad.Read();
            var report = _output.Write(input);
            if (report.Rumble == null) return;

            if (!_useRumble) return;

            _gamepad.SetRumble(report.Rumble);

            /*
             * Quick demo to show you how to read what buttons are pressed on the origin gamepad.
             * This is a really lazy, messy way to do it.
             */

            listBox1.Items.Clear();
            foreach (var item in report.Input)
            {
                listBox1.Items.Add(item.Value);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            timer1.Enabled = false;
            _cm.Close();

        }
    }
}
