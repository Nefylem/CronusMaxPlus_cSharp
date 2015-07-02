using System;

namespace CronusMaxPlusWrapper.XInput
{
    class Gamepad
    {
        private int _xboxCount;

        public byte[] Read()
        {
            var controls = Gamedef.GamePad.GetState(Gamedef.PlayerIndex.One);

            //I got bored of messing with definitions. If you want to take this out, just put var output = new byte[36];
            if (_xboxCount == 0) { _xboxCount = Enum.GetNames(typeof(Gamedef.Xbox)).Length; }
            var output = new byte[_xboxCount];

            /*
             * I added the definitions to the xbox class, but found them too slow for this. It's quicker just to run button by button
             */
            if (controls.DPad.Left) { output[(int) Gamedef.Xbox.Left] = Convert.ToByte(100); }
            if (controls.DPad.Right) { output[(int)Gamedef.Xbox.Right] = Convert.ToByte(100); }
            if (controls.DPad.Up) { output[(int)Gamedef.Xbox.Up] = Convert.ToByte(100); }
            if (controls.DPad.Down) { output[(int)Gamedef.Xbox.Down] = Convert.ToByte(100); }

            if (controls.Buttons.A) { output[(int)Gamedef.Xbox.A] = Convert.ToByte(100); }
            if (controls.Buttons.B) { output[(int)Gamedef.Xbox.B] = Convert.ToByte(100); }
            if (controls.Buttons.X) { output[(int)Gamedef.Xbox.X] = Convert.ToByte(100); }
            if (controls.Buttons.Y) { output[(int)Gamedef.Xbox.Y] = Convert.ToByte(100); }

            if (controls.Buttons.Start) { output[(int)Gamedef.Xbox.Start] = Convert.ToByte(100); }
            if (controls.Buttons.Guide) { output[(int)Gamedef.Xbox.Home] = Convert.ToByte(100); }
            if (controls.Buttons.Back) { output[(int)Gamedef.Xbox.Back] = Convert.ToByte(100); }

            if (controls.Buttons.LeftShoulder) { output[(int)Gamedef.Xbox.LeftShoulder] = Convert.ToByte(100); }
            if (controls.Buttons.RightShoulder) { output[(int)Gamedef.Xbox.RightShoulder] = Convert.ToByte(100); }
            if (controls.Buttons.LeftStick) { output[(int)Gamedef.Xbox.LeftStick] = Convert.ToByte(100); }
            if (controls.Buttons.RightStick) { output[(int)Gamedef.Xbox.RightStick] = Convert.ToByte(100); }

            if (controls.Triggers.Left > 0) { output[(int)Gamedef.Xbox.LeftTrigger] = Convert.ToByte(controls.Triggers.Left * 100); }
            if (controls.Triggers.Right > 0) { output[(int)Gamedef.Xbox.RightTrigger] = Convert.ToByte(controls.Triggers.Right * 100); }

            double dblLx = controls.ThumbSticks.Left.X * 100;
            double dblLy = controls.ThumbSticks.Left.Y * 100;
            double dblRx = controls.ThumbSticks.Right.X * 100;
            double dblRy = controls.ThumbSticks.Right.Y * 100;

            NormalGamepad(ref dblLx, ref dblLy);
            NormalGamepad(ref dblRx, ref dblRy);

            //Left and right sticks
            if (dblLx != 0) { output[(int)Gamedef.Xbox.LeftX] = (byte)Convert.ToSByte((int)(dblLx)); }
            if (dblLy != 0) { output[(int)Gamedef.Xbox.LeftY] = (byte)Convert.ToSByte((int)(dblLy)); }
            if (dblRx != 0) { output[(int)Gamedef.Xbox.RightX] = (byte)Convert.ToSByte((int)(dblRx)); }
            if (dblRy != 0) { output[(int)Gamedef.Xbox.RightY] = (byte)Convert.ToSByte((int)(dblRy)); }

            /*
             * There's no touch pad on a standard xinput gamepad. I use a mouse click to simulate it. There's also 
             * no options to swipe currently on the CM, so no need to fill in those details for you. I'm guessing you'd use
             * the same format as the left and right sticks with a -100 to +100 range sbyte.
             * If you want to implement all those features, I recommend using a mouse hook.
             */

            return output;
        }


        /*
         * The CM outputs -100 to +100 for both x & y.
         * The gamepad outputs circular motion, so at 45 degrees your sending roughly 75x 75y. 
         * This uses simple trig to convert the circular extremities to match the CM output
         * This was a messy hack I've never been bothered to fix. It gets the job done until someone does something better
         */
        private void NormalGamepad(ref double dblLx, ref double dblLy)
        {
            var dblNewX = dblLx;
            var dblNewY = dblLy;

            var dblLength = Math.Sqrt(Math.Pow(dblLx, 2) + Math.Pow(dblLy, 2));
            if (dblLength > 99.9)
            {
                var dblTheta = Math.Atan2(dblLy, dblLx);
                var dblAngle = (90 - ((dblTheta * 180) / Math.PI)) % 360;

                if ((dblAngle < 0) && (dblAngle >= -45)) { dblNewX = (int)(100 / Math.Tan(dblTheta)); dblNewY = -100; }
                if ((dblAngle >= 0) && (dblAngle <= 45)) { dblNewX = (int)(100 / Math.Tan(dblTheta)); dblNewY = -100; }
                if ((dblAngle > 45) && (dblAngle <= 135)) { dblNewY = -(int)(Math.Tan(dblTheta) * 100); dblNewX = 100; }
                if ((dblAngle > 135) && (dblAngle <= 225)) { dblNewX = -(int)(100 / Math.Tan(dblTheta)); dblNewY = 100; }
                if (dblAngle > 225) { dblNewY = (int)(Math.Tan(dblTheta) * 100); dblNewX = -100; }
                if (dblAngle < -45) { dblNewY = (int)(Math.Tan(dblTheta) * 100); dblNewX = -100; }
            }
            else
            {
                dblNewY = -dblNewY;
            }

            dblLx = dblNewX;
            dblLy = dblNewY;
        }


        /*
         * Do a foreach rumble here. You'll need to modify the Gamedef file to support the extra rumble motors, and look at the CM api to 
         * find out what order it reads rumble motors in
         */
        public void SetRumble(byte[] rumble)
        {
            Gamedef.GamePad.SetState(Gamedef.PlayerIndex.One, rumble[0], rumble[1]);
        }


    }
}
